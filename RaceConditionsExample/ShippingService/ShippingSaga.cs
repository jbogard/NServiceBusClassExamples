using System;
using BillingService.Messages;
using NServiceBus;
using NServiceBus.Saga;
using SalesService.Messages;
using ShippingService.Messages;

namespace ShippingService
{
    public class ShippingSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public int OrderId { get; set; }
        public bool OrderAccepted { get; set; }
        public bool OrderBilled { get; set; }

        public bool OrderShipped { get; set; }

        public bool OrderCancelled { get; set; }

        public bool ProductReturned { get; set; }
    }

    public class ShippingSaga : Saga<ShippingSagaData>,
        IAmStartedByMessages<IOrderAccepted>,
        IAmStartedByMessages<IOrderBilled>,
        IAmStartedByMessages<IOrderCancelled>,
        IHandleMessages<ReturnProduct>
    {

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<IOrderAccepted>(s => s.OrderId, m => m.OrderId);
            ConfigureMapping<IOrderBilled>(s => s.OrderId, m => m.OrderId);
            ConfigureMapping<IOrderCancelled>(s => s.OrderId, m => m.OrderId);
            ConfigureMapping<ReturnProduct>(s => s.OrderId, m => m.OrderId);
        }

        public void Handle(IOrderAccepted message)
        {
            Data.OrderId = message.OrderId;
            Data.OrderAccepted = true;
            CheckIfComplete();
        }

        public void Handle(IOrderBilled message)
        {
            Data.OrderId = message.OrderId;
            Data.OrderBilled = true;
            CheckIfComplete();
        }

        public void Handle(IOrderCancelled message)
        {
            Data.OrderId = message.OrderId;
            Data.OrderCancelled = true;

            Console.WriteLine("Shipping canceled. " + Data.OrderId);
            CheckIfComplete();
        }

        public void Handle(ReturnProduct message)
        {
            Data.ProductReturned = true;
            Console.WriteLine("Product returned. " + Data.OrderId);
            CheckIfComplete();
        }

        private void CheckIfComplete()
        {
            if (Data.OrderCancelled)
            {
                if (Data.OrderShipped && Data.ProductReturned)
                {
                    Bus.Publish<IProductReturned>(msg => msg.OrderId = Data.OrderId);
                }
                else if (!Data.OrderShipped)
                {
                    Bus.Publish<IShippingCanceled>(msg => msg.OrderId = Data.OrderId);
                }
                return;
            }

            if (Data.OrderBilled && Data.OrderAccepted)
            {
                Console.WriteLine("Shipping order. " + Data.OrderId);
                Data.OrderShipped = true;
            }
        }
    }
}