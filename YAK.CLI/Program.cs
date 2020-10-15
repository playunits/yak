using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using YAK.CLI.Attributes;
using static YAK.CLI.UI.Utils;

namespace YAK.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Process(args);
        }

        public static void Process(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine(GenerateUsage());
                    return;
                }

                foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                {
                    CliContainerAttribute containerAttribute = t.GetCustomAttribute<CliContainerAttribute>();
                    if (containerAttribute != null)
                    {
                        if (containerAttribute.Name == args[0])
                        {
                            string[] newArgs = new string[args.Length - 1];
                            Array.Copy(args, 1, newArgs, 0, newArgs.Length);
                            ProcessMethods(t, newArgs);
                            return;
                        }
                    }
                }
                Console.WriteLine(GenerateUsage());
            }
            catch (Exception e)
            {
                if (Constants.Spinner.IsRunning())
                {
                    Constants.Spinner.Stop(IOStatus.ERROR, $"Error: {Constants.Spinner.Message}!");
                }
                //Console.Write(GenerateExceptionUsage(e));
                throw;
            }
        }

        private static string GenerateExceptionUsage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"An Unhandled {ex.GetType().ToString()} has occured in {ex.Source}");
            sb.AppendLine("");
            sb.AppendLine("The full Stacktrace is: ");
            sb.AppendLine(ex.StackTrace ?? "Not given");

            return sb.ToString();
        }

        private static void ProcessMethods(Type type, string[] args)
        {
            if (args.Length < 1 || args[0] == "help")
            {
                Console.WriteLine(GenerateContainerUsage(type));
            }
            else
            {
                foreach (MethodInfo info in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    CliCommandAttribute commandAttribute = info.GetCustomAttribute<CliCommandAttribute>();
                    if (commandAttribute != null)
                    {
                        if (commandAttribute.Name == args[0])
                        {
                            string[] newArgs = new string[args.Length - 1];
                            Array.Copy(args, 1, newArgs, 0, newArgs.Length);
                            ProcessMethodExecution(type, info, newArgs);
                        }
                    }
                }
            }
        }

        public static List<ParameterInfo> GetRequiredArguments(MethodInfo info)
        {
            List<ParameterInfo> required = new List<ParameterInfo>();
            foreach (ParameterInfo pInfo in info.GetParameters())
            {
                if (DBNull.Value.Equals(pInfo.DefaultValue))
                    required.Add(pInfo);
            }
            return required;
        }

        public static List<ParameterInfo> GetOptionalArguments(MethodInfo info)
        {
            List<ParameterInfo> optional = new List<ParameterInfo>();
            foreach (ParameterInfo pInfo in info.GetParameters())
            {
                if (!DBNull.Value.Equals(pInfo.DefaultValue))
                    optional.Add(pInfo);
            }
            return optional;
        }

        private static void ProcessMethodExecution(Type type, MethodInfo info, string[] args)
        {
            List<ParameterInfo> parameters = info.GetParameters().ToList();

            int rCount = GetRequiredArguments(info).Count;
            int oCount = GetOptionalArguments(info).Count;


            if (args.Length < rCount || args.Length > (rCount + oCount) || (args.Length > 0 && args[0] == "help"))
            {
                Console.WriteLine(GenerateCommandUsage(type, info));
            }
            else
            {
                object[] values = new object[parameters.Count];


                for (int i = 0; i < args.Length; i++)
                {
                    ParameterInfo pInfo = parameters[i];

                    Type paramType = pInfo.ParameterType;
                    if (Is(paramType, args[i]))
                    {
                        values[i] = Convert(paramType, args[i]);
                    }
                }

                info.Invoke(null, values);

            }


        }

        private static string GenerateCommandUsage(Type type, MethodInfo info)
        {
            StringBuilder sb = new StringBuilder();

            CliContainerAttribute containerAttribute = type.GetCustomAttribute<CliContainerAttribute>();
            CliCommandAttribute commandAttribute = info.GetCustomAttribute<CliCommandAttribute>();

            sb.AppendLine($"Help for {GlobalConfig.CmdName} {containerAttribute.Name} {commandAttribute.Name}");
            sb.AppendLine("");
            sb.AppendLine("Usage (<param> is required, [param] is optional):");
            sb.AppendLine("");


            sb.Append($"{GlobalConfig.CmdName} ");
            sb.Append($"{containerAttribute.Name} ");


            sb.Append($"{commandAttribute.Name} ");

            foreach (ParameterInfo pInfo in info.GetParameters())
            {
                if (DBNull.Value.Equals(pInfo.DefaultValue))
                {
                    sb.Append($"<{pInfo.ParameterType.ToString().Replace(pInfo.ParameterType.Namespace.ToString() + ".", "")} {pInfo.Name}> ");
                }
                else
                {
                    sb.Append($"[{pInfo.ParameterType.ToString().Replace(pInfo.ParameterType.Namespace.ToString() + ".", "")} {pInfo.Name}] ");
                }
            }

            return sb.ToString();
        }

        private static string GenerateContainerUsage(Type type)
        {
            StringBuilder sb = new StringBuilder();
            CliContainerAttribute containerAttribute = type.GetCustomAttribute<CliContainerAttribute>();

            sb.AppendLine($"Help for: {GlobalConfig.CmdName} {containerAttribute.Name}");
            sb.AppendLine("");
            sb.AppendLine("Available Subcommands:");
            sb.AppendLine("");

            foreach (MethodInfo info in type.GetMethods())
            {
                CliCommandAttribute commandAttribute = info.GetCustomAttribute<CliCommandAttribute>();
                if (commandAttribute != null)
                {
                    sb.AppendLine($"\t-{commandAttribute.Name}");
                }
            }

            sb.AppendLine("");
            sb.AppendLine("In order to retrieve help for a command enter:");
            sb.AppendLine($"{GlobalConfig.CmdName} {containerAttribute.Name} <command> help");

            return sb.ToString();
        }

        private static string GenerateUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"General usage for {GlobalConfig.CmdName}:");
            sb.AppendLine("");
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                CliContainerAttribute containerAttribute = t.GetCustomAttribute<CliContainerAttribute>();
                if (containerAttribute != null)
                {
                    sb.AppendLine($"\t-{containerAttribute.Name}");
                }
            }

            sb.AppendLine("");
            sb.AppendLine($"Enter \"{GlobalConfig.CmdName} <cmd> help\" to receive help");
            return sb.ToString();
        }

        private static bool Is(Type type, string input)
        {
            try
            {
                TypeDescriptor.GetConverter(type).ConvertFromString(input);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static object Convert(Type type, string input)
        {
            return TypeDescriptor.GetConverter(type).ConvertFromString(input);
        }
    }
}
