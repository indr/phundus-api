using System;
using System.Reflection;

namespace phiNdus.fundus.Core.CliTools
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No command specified.");
                Console.ReadKey();
                return;
            }
            
            var cmds = new Commands();
            var method = cmds.GetType().GetMethod(args[0], BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
            if (method == null)
            {
                Console.WriteLine("Command \"" + args[0] +"\" not found.");
                Console.WriteLine("");
                
                var methods = cmds.GetType().GetMethods(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                Console.WriteLine("Available commands are:");
                foreach(var each in methods)
                    Console.WriteLine(each.Name);

                Console.WriteLine("Try [command] help");
                Console.ReadKey();
                return;
            }
            method.Invoke(cmds, new object[] {Console.Out, args});
            Console.ReadKey();
        }
    }
}
