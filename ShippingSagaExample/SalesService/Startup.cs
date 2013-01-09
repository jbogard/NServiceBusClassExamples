using System;
using NServiceBus;
using SalesService.Messages;

namespace SalesService
{
    public class Startup : IWantToRunAtStartup
    {
        public IBus Bus { get; set; }

        public void Run()
        {            
            while(true)
            {
                Console.Write("Enter order ID:");
                int orderId = Convert.ToInt32(Console.ReadLine());

                Bus.Publish<IOrderAccepted>(m =>
                {
                    m.OrderId = orderId;
                });

                Console.WriteLine("Order accepted. " + orderId);
            }
        }

        public void Stop()
        {
        }
    }
}