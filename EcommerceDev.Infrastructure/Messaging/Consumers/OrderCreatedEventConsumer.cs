using EcommerceDev.Core.Events;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Payment;
using EcommerceDev.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EcommerceDev.Infrastructure.Messaging.Consumers
{
    public class OrderCreatedEventConsumer : BackgroundService
    {
        private readonly RabbitMqSettings _settings;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<PaymentNotificationHub> _hubContext;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private IConnection _connection;
        private IChannel _channel;

        public OrderCreatedEventConsumer(
            IServiceProvider serviceProvider,
            RabbitMqSettings settings,
            IHubContext<PaymentNotificationHub> hubContext,
            ILogger<OrderCreatedEventConsumer> logger)
        {
            _settings = settings;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMqAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                    Console.WriteLine($"[Consumer] Received OrderCreatedEvent with Id {@event.IdOrder}");

                    var scope = _serviceProvider.CreateScope();

                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                    var order = await orderRepository.GetByIdAsync(@event.IdOrder);

                    if (order is null)
                    {
                        Console.WriteLine($"[Consumer] Oder with Id {@event.IdOrder} does not exist");

                        return;
                    }

                    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                    var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

                    var customer = await customerRepository.GetById(order.IdCustomer);

                    var customerPaymentModel = new PaymentCustomerModel
                    {
                        Email = customer.Email,
                        FullName = customer.FullName,
                        PhoneNumber = customer.PhoneNumer
                    };

                    string? customerPaymentId;

                    if (customer.IdExternalPayment != null)
                    {
                        customerPaymentId = customer.IdExternalPayment;
                    }
                    else
                    {
                        customerPaymentId = await paymentService.CreateCustomerAsync(customerPaymentModel);

                        customer.IdExternalPayment = customerPaymentId;

                        await customerRepository.Update(customer);
                    }

                    var orderPaymentModel = new PaymentOrderModel
                    {
                        IdExternalCustomer = customerPaymentId,
                        Items = order.Items.Select(i => new PaymentOrderItemModel
                        {
                            Name = i.Product.Title,
                            Price = i.Product.Price,
                            Quantity = i.Quantity
                        }).ToList()
                    };

                    var shippingPaymentOrderItemModel = new PaymentOrderItemModel
                    {
                        Name = "Shipping Cost",
                        Price = order.ShippingPrice,
                        Quantity = 1
                    };

                    orderPaymentModel.Items.Add(shippingPaymentOrderItemModel);

                    var paymentResult = await paymentService.CreateOrderAsync(orderPaymentModel);

                    order.MarkAsPaymentPending();
                    order.IdExternalOrder = paymentResult.Id;
                    order.PaymentUrl = paymentResult.Url;
                    await orderRepository.UpdateAsync(order);

                    _logger.LogInformation(
                    "[Consumer] Order {OrderId} updated with payment URL: {PaymentUrl}",
                    @event.IdOrder,
                    paymentResult.Url);

                    await _hubContext.Clients
                        .Group($"order-{order.Id}")
                        .SendAsync("ReceivePaymentUrl", new
                        {
                            orderId = order.Id.ToString(),
                            paymentUrl = order.PaymentUrl,
                            externalOrderId = order.IdExternalOrder,
                            totalAmount = order.TotalProductsPrice + order.ShippingPrice,
                            status = order.Status.ToString()
                        }, stoppingToken);

                    _logger.LogInformation(
                        "[Consumer] Sent payment URL via SignalR for Order {OrderId}",
                        order.Id);

                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Consumer] Exception processing order event");

                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, true, cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _settings.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }

        private async Task InitializeRabbitMqAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
            };

            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;

            // Declare Exchange
            await _channel.ExchangeDeclareAsync(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
                );

            // Declare Queue
            await _channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange with routing key
            await _channel.QueueBindAsync(
                queue: _settings.QueueName,
                exchange: _settings.ExchangeName,
                routingKey: "ordercreated");

            _logger.LogInformation(
            "[Consumer] RabbitMQ initialized - Exchange: {ExchangeName}, Queue: {QueueName}",
            _settings.ExchangeName,
            _settings.QueueName);
        }
    }
}
