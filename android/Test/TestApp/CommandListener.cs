using System;
using System.Collections.Generic;
using Android.Content;
using Com.Adjust.Test;

namespace TestApp
{
    public class CommandListener : Java.Lang.Object, ICommandListener
    {
        private readonly AdjustCommandExecutor _adjustCommandExecutor;
              
        public CommandListener(Context context)
        {
            _adjustCommandExecutor = new AdjustCommandExecutor(context);
        }
        
        public void SetTestLibrary(TestLibrary testLibrary)
        {
            _adjustCommandExecutor?.SetTestLibrary(testLibrary);
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
