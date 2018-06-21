using System;
using System.Collections.Generic;
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
        
        public void ExecuteCommand(string className, string methodName, IDictionary<string, IList<string>> parameters)
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
