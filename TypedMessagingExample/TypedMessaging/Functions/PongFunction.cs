namespace TypedMessagingExample.TypedMessaging.Functions
{
    public class PongFunction : IMessageFunction
    {
        private readonly IMessagingFacade messagingFacade;

        public PongFunction(IMessagingFacade messagingFacade)
        {
            this.messagingFacade = messagingFacade;
        }

        public readonly string Name = "PONG";

        public string GetName()
        {
            return Name;
        }

        public bool AllowRun(string channel, string message) => message == "PING";

        public void Run(string publishingChannel, string incomingMessage)
        {
            messagingFacade.Publish(publishingChannel, Name);
        }
    }
}
