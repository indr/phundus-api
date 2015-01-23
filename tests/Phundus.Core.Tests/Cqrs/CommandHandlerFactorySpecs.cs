namespace Phundus.Core.Tests.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common.Cqrs;
    using Core.Cqrs;
    using Machine.Specifications;

    public class all_commands_should_have_extact_one_handler
    {
        private Establish ctx = () =>
        {
            
        };

        private Because of = () =>
        {
            var allTypes = Assembly.GetAssembly(typeof (CoreInstaller)).GetTypes();

            _commands = allTypes.Where(p => p.GetInterfaces().Contains(typeof(ICommand))).ToList();
            _handlers = allTypes.Where(p => p.IsClass && p.GetInterfaces().Contains(typeof(IHandleCommand))).ToList();
        };

        public It should_not_have_more_handlers_than_commands = () =>
            _handlers.Count.ShouldBeLessThanOrEqualTo(_commands.Count);

        public It should_have_closed_ihandlecommand_implementation = () =>
        {
            foreach (var command in _commands)
            {
                var commandHandlerType =typeof (IHandleCommand<>).MakeGenericType(command);
                var handler = _handlers.SingleOrDefault(p => p.GetInterfaces().Contains(commandHandlerType));
                handler.ShouldNotBeNull();
                handler.IsClass.ShouldBeTrue();
                handler.IsAbstract.ShouldBeFalse();

            }
        };        

        private static List<Type> _commands;
        private static List<Type> _handlers;
    }
}