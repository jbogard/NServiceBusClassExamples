using NServiceBus;

namespace SalesService.Messages
{
    public interface IOrderAccepted : IEvent
    {
        int OrderId { get; set; }
    }
}