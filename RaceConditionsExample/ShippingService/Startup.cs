using System;
using NServiceBus;
using ShippingService.Messages;

namespace ShippingService
{
    public class Startup : IWantToRunAtStartup
    {
        public IBus Bus { get; set; }

        public void Run()
        {
            while (true)
            {
                Console.Write("Enter shipping product returned: ");
                int orderId = Convert.ToInt32(Console.ReadLine());

                Bus.SendLocal(new ReturnProduct(orderId));
            }
        }

        public void Stop()
        {
        }
    }
}