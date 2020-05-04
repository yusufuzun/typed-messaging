namespace TypedMessagingExample.TypedMessaging
{
    public interface IMessagingFacade
    {
        void StoreFunction(IMessageFunction runningFunction);

        void Send(string channel, string message);

        void Publish(string channel, string message);

        void SubscribeStore(string channelToSubscribe, string channelToPublish);

        void SubscribeFunction(string channel, IMessageFunction runningFunction);
    }
}
