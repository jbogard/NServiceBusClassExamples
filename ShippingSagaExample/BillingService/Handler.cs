using System;
using BillingService.Messages;
using NServiceBus;
using SalesService.Messages;

namespace BillingService
{
    public class Handler : IHandleMessages<IOrderAccepted>
    {
        public IBus Bus { get; set; }

        public void Handle(IOrderAccepted message)
        {
            Console.WriteLine("Order billed. " + message.OrderId);
        }
    }

    public class Startup : IWantToRunAtStartup
    {
        public IBus Bus { get; set; }

        public void Run()
        {
            while (true)
            {
                Console.Write("Enter order ID:");
                int orderId = Convert.ToInt32(Console.ReadLine());

                Bus.Publish<IOrderBilled>(m =>
                {
                    m.OrderId = orderId;
                });

                Console.WriteLine("Order billed. " + orderId);
            }
        }

        public void Stop()
        {
        }
    }

}
