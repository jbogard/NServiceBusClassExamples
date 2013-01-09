using NServiceBus;

namespace ShippingService.Messages
{
    public class ReturnProduct : ICommand
    {
        public ReturnProduct(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}