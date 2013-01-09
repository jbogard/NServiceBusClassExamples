using NServiceBus;

namespace ShippingService.Messages
{
    public interface IProductReturned : IEvent
    {
        int OrderId { get; set; }
         
    }
}