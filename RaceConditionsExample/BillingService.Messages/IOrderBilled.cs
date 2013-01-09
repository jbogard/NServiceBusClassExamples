using NServiceBus;

namespace BillingService.Messages
{
    public interface IOrderBilled : IEvent
    {
        int OrderId { get; set; }
    }
}