using System;
using TypedMessagingExample.TypedMessaging;
using TypedMessagingExample.TypedMessaging.Functions;
using TypedMessagingExample.TypedMessaging.MessagingFacade;

namespace ConsumerApp
{
    class Program
    {
        static void Main()
        {
            const string channelName = "ChannelPC";

            IMessagingFacade messagingFacade = new RedisMessagingFacade("localhost:7001");

            var actions = new IMessageFunction[] { new PongFunction(messagingFacade), new FibonacciFunction(messagingFacade) };

            foreach (var function in actions)
            {
                messagingFacade.StoreFunction(function);
            }

            Console.WriteLine("Consumer Mode");

            messagingFacade.SubscribeStore(channelName, channelName);

            Console.WriteLine("Press anything to exit");

            Console.ReadLine();
        }
    }
}
