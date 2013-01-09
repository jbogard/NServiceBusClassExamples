using NServiceBus;

namespace SalesService.Messages
{
    public class CancelOrder : ICommand
    {
        public int OrderId { get; private set; }

        public CancelOrder(int orderId)
        {
            OrderId = orderId;
        }
    }
}