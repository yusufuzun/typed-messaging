namespace TypedMessagingExample.TypedMessaging.Functions
{
    public class PingCommand : IMessageFunction
    {
        private readonly IMessagingFacade messagingFacade;

        public PingCommand(IMessagingFacade messagingFacade)
        {
            this.messagingFacade = messagingFacade;
        }

        public readonly string Name = "PING";

        public string GetName()
        {
            return Name;
        }

        public bool AllowRun(string channel, string message) => message == "p";

        public void Run(string publishingChannel, string incomingMessage)
        {
            messagingFacade.Publish(publishingChannel, Name);
        }
    }
}
