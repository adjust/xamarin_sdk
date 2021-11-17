using System;
using Foundation;
using TestLib;

namespace TestApp
{
    public class CommandListener : AdjustCommandDelegate
    {
        private readonly AdjustCommandExecutor _adjustCommandExecutor;

        public CommandListener()
        {
            _adjustCommandExecutor = new AdjustCommandExecutor();
        }

        public override void ExecuteCommand(string className, string methodName, NSDictionary parameters)
        {
            Command command = new Command(className, methodName, parameters);
            switch (className.ToLower())
            {
                case "adjust":
                    _adjustCommandExecutor.ExecuteCommand(command);
                    break;
                default:
                    Console.WriteLine("Could not find {0} class to execute", className);
                    break;
            }
        }
    }
}
