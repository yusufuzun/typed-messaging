namespace TypedMessagingExample.TypedMessaging.Functions
{
    public class FibonacciFunction : IMessageFunction
    {
        private readonly IMessagingFacade messagingFacade;

        public FibonacciFunction(IMessagingFacade messagingFacade)
        {
            this.messagingFacade = messagingFacade;
        }

        public string Name { get => "FIBONACCI_ANSWER"; }

        public string GetName()
        {
            return Name;
        }

        public bool AllowRun(string channel, string message) => message.StartsWith("FIBONACCI_QUESTION=");

        public void Run(string publishingChannel, string incomingMessage)
        {
            int number;

            bool hasNumber = int.TryParse(incomingMessage.Remove(0, "FIBONACCI_QUESTION=".Length), out number);

            if (hasNumber)
            {
                var res = CalculateFibonacci(number);

                messagingFacade.Publish(publishingChannel, $"{Name}({number})={res}");
            }
            else
            {
                messagingFacade.Publish(publishingChannel, $"{Name} has incorrect argument");
            }
        }

        private int CalculateFibonacci(int number)
        {            
            if(number > 1)
            {
                return CalculateFibonacci(number - 1) + CalculateFibonacci(number - 2);
            }
            else if(number < 0)
            {
                return 0;
            }
            else
            {
                return number;
            }
        }
    }
}
