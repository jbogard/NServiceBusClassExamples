using System;
using BillingService.Messages;
using NServiceBus;
using NServiceBus.Saga;
using SalesService.Messages;

namespace ShippingService
{
    public class ShippingSaga 
        : Saga<ShippingSagaData>,
        IAmStartedByMessages<IOrderAccepted>,
        IAmStartedByMessages<IOrderBilled>
    {
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<IOrderAccepted>(s => s.OrderId, m => m.OrderId);
            ConfigureMapping<IOrderBilled>(s => s.OrderId, m => m.OrderId);
        }

        public void Handle(IOrderAccepted message)
        {
            Console.WriteLine("Received Order Accepted: " + message.OrderId);

            Data.OrderId = message.OrderId;
            Data.OrderAccepted = true;
            CheckIfComplete();
        }

        public void Handle(IOrderBilled message)
        {
            Console.WriteLine("Received Order Billed: " + message.OrderId);

            Data.OrderId = message.OrderId;
            Data.OrderBilled = true;
            CheckIfComplete();
        }

        private void CheckIfComplete()
        {
            if (Data.OrderBilled && Data.OrderAccepted)
            {
                Console.WriteLine("Shipping order. " + Data.OrderId);
                MarkAsComplete();
            }
        }
    }
}
