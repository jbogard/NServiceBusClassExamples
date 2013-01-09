using System;
using NServiceBus;
using NServiceBus.Saga;
using SalesService.Messages;

namespace SalesService
{
    public class OrderPlacementData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public int OrderId { get; set; }

        public bool OrderCancelled { get; set; }

        public bool OrderAccepted { get; set; }
    }

    public class OrderReadyToBePlaced : ITimeoutState
    {
        
    }

    public class OrderPlacementSaga
        : Saga<OrderPlacementData>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>,
        IHandleTimeouts<OrderReadyToBePlaced>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<CancelOrder>(s => s.OrderId, m => m.OrderId);
        }

        public void Handle(PlaceOrder message)
        {
            Data.OrderId = message.OrderId;
            RequestUtcTimeout<OrderReadyToBePlaced>(TimeSpan.FromSeconds(30));
            Console.WriteLine("Place Order request received " + Data.OrderId);
        }

        public void Handle(CancelOrder message)
        {
            if (Data.OrderCancelled)
                return;

            Data.OrderCancelled = true;
            if (Data.OrderAccepted)
            {
                Bus.Publish<IOrderCancelled>(msg => msg.OrderId = message.OrderId);
            }
            Console.WriteLine("Order Cancelled " + Data.OrderId);
        }

        public void Timeout(OrderReadyToBePlaced state)
        {
            if (!Data.OrderCancelled)
            {
                Bus.Publish<IOrderAccepted>(msg => msg.OrderId = Data.OrderId);
                Data.OrderAccepted = true;
                Console.WriteLine("Order Placed " + Data.OrderId);
            }
        }
    }
}