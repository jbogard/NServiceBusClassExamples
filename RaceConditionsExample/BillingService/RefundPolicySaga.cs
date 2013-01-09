using System;
using NServiceBus;
using NServiceBus.Saga;
using SalesService.Messages;
using ShippingService.Messages;

namespace BillingService
{
    public class RefundPolicyData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public int OrderId { get; set; }

        public bool OrderCancelled { get; set; }

        public bool ShippingCanceled { get; set; }

        public bool ProductReturned { get; set; }
    }

    public class RefundPolicySaga
        : Saga<RefundPolicyData>,
        IAmStartedByMessages<IOrderCancelled>,
        IAmStartedByMessages<IShippingCanceled>,
        IAmStartedByMessages<IProductReturned>
    {

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<IOrderCancelled>(s => s.OrderId, m => m.OrderId);
            ConfigureMapping<IShippingCanceled>(s => s.OrderId, m => m.OrderId);
            ConfigureMapping<IProductReturned>(s => s.OrderId, m => m.OrderId);
        }

        public void Handle(IOrderCancelled message)
        {
            Data.OrderId = message.OrderId;
            Data.OrderCancelled = true;
            CheckRefund();
        }

        public void Handle(IShippingCanceled message)
        {
            Data.OrderId = message.OrderId;
            Data.ShippingCanceled = true;
            CheckRefund();
        }

        public void Handle(IProductReturned message)
        {
            Data.OrderId = message.OrderId;
            Data.ProductReturned = true;
            CheckRefund();
        }

        private void CheckRefund()
        {
            // Can also implement partial refunds here
            if (Data.OrderCancelled && (Data.ShippingCanceled || Data.ProductReturned))
            {
                Console.WriteLine("Refund issued for order " + Data.OrderId);
                MarkAsComplete();
            }
        }
    }
}