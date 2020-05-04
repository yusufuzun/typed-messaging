namespace TypedMessagingExample.TypedMessaging
{
    public interface IMessageFunction
    {
        string GetName();

        bool AllowRun(string channel, string message);

        void Run(string publishingChannel, string incomingMessage);
    }
}
