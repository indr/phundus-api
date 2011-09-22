using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace phiNdus.fundus.Core.CliTools
{
    internal class Program
    {
        private static readonly Commands Commands = new Commands();

        private static void Main(string[] args)
        {
            if (args.Length > 0)
                ExecuteCommandString(args);
            else
            {
                Console.WriteLine("fundus CliTools");
                Console.WriteLine();
                Console.WriteLine("Type help to see available commands.");
                Console.WriteLine("Type exit to quit.");
                Console.WriteLine();

                while(true)
                {
                    Console.Write("> ");
                    var inputLine = Console.ReadLine();
                    if ((inputLine == null) || String.IsNullOrWhiteSpace(inputLine))
                        continue;

                    // Problem Leerzeichen: show user="Vorname Nachname"
                    var command = inputLine.Split(' ');

                    if (command[0].ToLowerInvariant() == "exit")
                        break;

                    ExecuteCommandString(command);
                }
            }
            
        }

        private static void ExecuteCommandString(string[] args)
        {
            var method = Commands.GetType().GetMethod(args[0],
                                                              BindingFlags.Instance | BindingFlags.IgnoreCase |
                                                              BindingFlags.Public);
            if (method == null)
                ShowAvailableCommands(args[0]);
            else {
                method.Invoke(Commands, new object[] { Console.Out, args });
                Console.WriteLine();
            }
        }

        private static void ShowAvailableCommands(string input)
        {
            Console.WriteLine("Command \"" + input + "\" not found.");
            Console.WriteLine("");

            var methods =
                Commands.GetType().GetMethods(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public).ToList();

            methods.RemoveAll(m => (m.Name == "ToString") || (m.Name == "Equals") || (m.Name == "GetType") || (m.Name == "GetHashCode"));
            
            Console.WriteLine("Available commands are:");
            Console.WriteLine();
            foreach (var each in methods)
                Console.WriteLine(each.Name.ToLowerInvariant());
            Console.WriteLine();
            Console.WriteLine("Try [command] help");
            Console.WriteLine();
        }
    }
}