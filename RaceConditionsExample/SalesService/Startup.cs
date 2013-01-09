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
            int i = 0;
            while (true)
            {
                Console.WriteLine("Enter P to place and C to cancel an order...");
                var line = Console.ReadLine();
                switch (line)
                {
                    case "P":
                        i++;
                        Console.WriteLine("Your order number is: " + i);
                        Bus.SendLocal(new PlaceOrder(i));
                        break;
                    case "C":
                        Console.Write("Enter order to cancel: ");
                        var order = Console.ReadLine();
                        Bus.SendLocal(new CancelOrder(Convert.ToInt32(order)));
                        break;
                    default:
                        break;
                }
            }
        }

        public void Stop()
        {
        }
    }
}