using System;
using System.Collections.Generic;
using Foundation;

namespace TestApp
{
    public class Command
    {
        internal string ClassName;
        internal string MethodName;
        internal Dictionary<string, List<string>> Parameters;

        public Command(string className, string methodName, Dictionary<string, List<string>> parameters)
        {
            ClassName = className;
            MethodName = methodName;
            Parameters = parameters;
        }

        public Command(string className, string methodName, NSDictionary parameters)
        {
            ClassName = className;
            MethodName = methodName;
            Parameters = new Dictionary<string, List<string>>();

            foreach(NSObject nsKey in parameters.Keys)
            {
                string key = nsKey.ToString();
                List<string> value = new List<string>();
                NSArray valueArray = (NSArray)parameters[key];

                for (nuint i = 0; i < valueArray.Count; i++)
                {
                    NSString stringVal = valueArray.GetItem<NSString>(i);
                    value.Add(stringVal);
                }

                Parameters.Add(key, value);
            }
        }

        public string GetFirstParameterValue(string parameterKey)
        {
            if (Parameters == null || !Parameters.ContainsKey(parameterKey))
            {
                return null;
            }

            var parameterValues = Parameters[parameterKey];
            return parameterValues.Count == 0 ? null : parameterValues[0];
        }

        public bool ContainsParameter(string parameterKey)
        {
            if (Parameters == null || string.IsNullOrEmpty(parameterKey))
            {
                return false;
            }
            if (!Parameters.ContainsKey(parameterKey))
            {
                return false;
            }

            var parameterValues = Parameters[parameterKey];
            if (parameterValues == null || parameterValues.Count == 0)
            {
                return false;
            }

            return parameterValues[0] != null;
        }
    }
}
