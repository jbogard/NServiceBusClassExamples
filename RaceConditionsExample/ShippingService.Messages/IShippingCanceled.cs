using NServiceBus;

namespace ShippingService.Messages
{
    public interface IShippingCanceled : IEvent
    {
        int OrderId { get; set; }
         
    }
}