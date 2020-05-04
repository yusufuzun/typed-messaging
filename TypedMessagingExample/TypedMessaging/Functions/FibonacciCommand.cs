namespace TypedMessagingExample.TypedMessaging.Functions
{
    public class FibonacciCommand : IMessageFunction
    {
        private readonly IMessagingFacade messagingFacade;

        public FibonacciCommand(IMessagingFacade messagingFacade)
        {
            this.messagingFacade = messagingFacade;
        }

        public string Name { get => "FIBONACCI"; }

        public string GetName()
        {
            return Name;
        }

        public bool AllowRun(string channel, string message) => message.StartsWith("f ");

        public void Run(string publishingChannel, string incomingMessage)
        {
            messagingFacade.Publish(publishingChannel, $"{Name}_QUESTION={incomingMessage.Remove(0, 2)}");
        }
    }
}
