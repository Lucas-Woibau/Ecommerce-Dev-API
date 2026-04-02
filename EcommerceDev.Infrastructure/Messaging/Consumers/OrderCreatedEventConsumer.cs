using EcommerceDev.Core.Events;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Payment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EcommerceDev.Infrastructure.Messaging.Consumers
{
    public class OrderCreatedEventConsumer : BackgroundService
    {
        private readonly RabbitMqSettings _settings;
        private readonly IServiceProvider _provider;
        private IConnection _connection;
        private IChannel _channel;

        public OrderCreatedEventConsumer(RabbitMqSettings settings, IServiceProvider provider)
        {
            _settings = settings;
            _provider = provider;
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

                    var scope = _provider.CreateScope();

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

                    var paymentResult = await paymentService.CreateOrderAsync(orderPaymentModel);

                    order.MarkAsPaymentPending();
                    order.IdExternalOrder = paymentResult.Id;
                    order.PaymentUrl = paymentResult.Url;
                    await orderRepository.UpdateAsync(order);

                    Console.WriteLine($"[Consumer] Order with Id {@event.IdOrder} updated");

                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Consumer] Excepetion: {ex.Message}");

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

            Console.WriteLine($"[Consumer] RabbitMQ initialized - " +
                $"Exchange: {_settings.ExchangeName}," +
                $" Queue: {_settings.QueueName}");
        }
    }
}
