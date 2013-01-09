using NServiceBus;

namespace SalesService.Messages
{
    public interface IOrderCancelled : IEvent
    {
        int OrderId { get; set; }
    }
}