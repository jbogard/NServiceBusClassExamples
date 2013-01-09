using NServiceBus;

namespace SalesService.Messages
{
    public class PlaceOrder : ICommand
    {
        public int OrderId { get; private set; }

        public PlaceOrder(int orderId)
        {
            OrderId = orderId;
        }
    }
}