using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EcommerceDev.Infrastructure.SignalR;

/// <summary>
/// SignalR Hub for sending real-time payment notifications to clients
/// </summary>
public class PaymentNotificationHub : Hub
{
    private readonly ILogger<PaymentNotificationHub> _logger;

    public PaymentNotificationHub(ILogger<PaymentNotificationHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Allows a client to join a group for a specific order to receive payment notifications
    /// </summary>
    /// <param name="orderId">The order ID to subscribe to</param>
    public async Task JoinOrderGroup(string orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");

        _logger.LogInformation(
            "Client {ConnectionId} joined order group: {OrderId}",
            Context.ConnectionId,
            orderId);
    }

    /// <summary>
    /// Allows a client to leave an order group
    /// </summary>
    /// <param name="orderId">The order ID to unsubscribe from</param>
    public async Task LeaveOrderGroup(string orderId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order-{orderId}");

        _logger.LogInformation(
            "Client {ConnectionId} left order group: {OrderId}",
            Context.ConnectionId,
            orderId);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            _logger.LogWarning(
                exception,
                "Client disconnected with error: {ConnectionId}",
                Context.ConnectionId);
        }
        else
        {
            _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
