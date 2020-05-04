using System;
using System.Linq;
using TypedMessagingExample.TypedMessaging;
using TypedMessagingExample.TypedMessaging.Functions;
using TypedMessagingExample.TypedMessaging.MessagingFacade;

namespace TypedMessagingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string channelName = "ChannelPC";

            IMessagingFacade messagingFacade = new RedisMessagingFacade("localhost:7001");

            var commands = new IMessageFunction[] { new PingCommand(messagingFacade), new FibonacciCommand(messagingFacade) };

            var actions = new IMessageFunction[] { new PongFunction(messagingFacade), new FibonacciFunction(messagingFacade) };

            foreach (var function in Enumerable.Concat(commands, actions))
            {
                messagingFacade.StoreFunction(function);
            }

            Console.WriteLine("Enter p for producer , else it is consumer : ");

            var isProducer = Console.ReadLine() == "p";

            if (isProducer)
            {
                Console.WriteLine("Producer Mode Selected");

                Console.WriteLine("Enter p to ping , enter f {number} to fibonacci calculation , else it will quit: ");

                messagingFacade.SubscribeFunction(channelName, new ConsoleWriteFunction());

                string command;

                while ((command = Console.ReadLine()) != "")
                {
                    messagingFacade.Send(channelName, command);
                }
            }
            else
            {
                Console.WriteLine("Consumer Mode Selected");

                messagingFacade.SubscribeStore(channelName, channelName);
            }

            Console.WriteLine("Press anything to exit");
            Console.ReadLine();
        }
    }

    public class ConsoleWriteFunction : IMessageFunction
    {
        private readonly string Name = "ConsoleLog";

        public bool AllowRun(string channel, string message) => true;

        public string GetName()
        {
            return Name;
        }

        public void Run(string publishingChannel, string incomingMessage)
        {
            Console.WriteLine($"{publishingChannel}: {incomingMessage}");
        }
    }
}
