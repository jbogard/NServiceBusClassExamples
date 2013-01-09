using System;
using BillingService.Messages;
using NServiceBus;
using SalesService.Messages;

namespace BillingService
{
    public class OrderAcceptedHandler
        : IHandleMessages<IOrderAccepted>
    {
        public IBus Bus { get; set; }

        public void Handle(IOrderAccepted message)
        {
            Console.WriteLine("Order received: " + message.OrderId);
            Console.ReadLine();
            Bus.Publish<IOrderBilled>(m => m.OrderId = message.OrderId);
        }
    }
}