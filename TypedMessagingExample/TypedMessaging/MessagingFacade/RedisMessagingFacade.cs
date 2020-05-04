using StackExchange.Redis;
using System.Collections.Generic;

namespace TypedMessagingExample.TypedMessaging.MessagingFacade
{
    public class RedisMessagingFacade : IMessagingFacade
    {
        private readonly string connectionString;

        private readonly List<IMessageFunction> functionStore = new List<IMessageFunction>();

        private ConnectionMultiplexer connectionMultiplexer;

        public RedisMessagingFacade(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void StoreFunction(IMessageFunction runningFunction)
        {
            functionStore.Add(runningFunction);
        }

        public void Send(string channel, string message)
        {
            foreach (var command in functionStore)
            {
                if (command.AllowRun(channel, message))
                {
                    command.Run(channel, message);
                }
            }
        }

        public void Publish(string channel, string message)
        {
            ISubscriber subscriber = GetSubscriber();

            subscriber.Publish(new RedisChannel(channel, RedisChannel.PatternMode.Auto), message);
        }

        public void SubscribeStore(string channelToSubscribe, string channelToPublish)
        {
            ISubscriber subscriber = GetSubscriber();

            subscriber.Subscribe(channelToSubscribe, (channel, value) =>
            {
                foreach (var function in functionStore)
                {
                    if (function.AllowRun(channel, value))
                    {
                        function.Run(channelToPublish, value);
                    }
                }
            });
        }

        public void SubscribeFunction(string channel, IMessageFunction runningFunction)
        {
            ISubscriber subscriber = GetSubscriber();

            subscriber.Subscribe(channel, (channel, value) =>
            {
                runningFunction.Run(channel, value);
            });
        }

        private ISubscriber GetSubscriber()
        {
            if (connectionMultiplexer == null)
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            }

            var subscriber = connectionMultiplexer.GetSubscriber();

            return subscriber;
        }
    }
}
