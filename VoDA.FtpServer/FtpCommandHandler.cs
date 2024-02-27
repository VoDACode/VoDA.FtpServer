using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Commands;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;
using VoDA.FtpServer.Responses;

namespace VoDA.FtpServer
{
    internal class FtpCommandHandler
    {
        private static FtpCommandHandler? _instance;
        public static FtpCommandHandler Instance => _instance ??= new FtpCommandHandler();

        private readonly Dictionary<string, BaseCommandDetails> _commands = new();
        public IReadOnlyDictionary<string, BaseCommandDetails> Commands => _commands;

        private FtpCommandHandler()
        {
            var commands = Assembly.GetCallingAssembly().GetTypes()
              .Where(t => string.Equals(t.Namespace, "VoDA.FtpServer.Commands", StringComparison.Ordinal) 
                          && t.GetCustomAttribute(typeof(FtpCommandAttribute)) != null)
              .ToArray();
            foreach (var command in commands)
            {
                var obj = Activator.CreateInstance(command) as BaseCommand;
                if(obj == null)
                    continue;
                var key = command.GetCustomAttribute<FtpCommandAttribute>();
                if (key == null)
                    throw new ArgumentNullException($"FtpCommandAttribute\n Type: {command.FullName}");
                var auth = command.GetCustomAttribute<AuthorizeAttribute>();
                _commands.Add(key.Command, new BaseCommandDetails(obj, auth is not null));
            }
        }

        public async Task<IFtpResult> HandleCommand(FtpCommand command, FtpClient client, FtpClientParameters configParameters)
        {
            if (!Commands.ContainsKey(command.Command))
                return new UnknownCommand502Response();
            var commandDetails = Commands[command.Command];
            if (configParameters.AuthorizationOptions.UseAuthorization && !client.IsAuthorized && !commandDetails.NeedVerification)
                return new NotLoggedIn530Response();
            return await Commands[command.Command].Command.Invoke(client, configParameters, command.Arguments);
        }

        public void Add<T>() where T : BaseCommand
        {
            var obj = Activator.CreateInstance(typeof(T)) as BaseCommand;
            if(obj == null)
                throw new Exception($"Can't create instance of {typeof(T).FullName}");
            var key = typeof(T).GetCustomAttribute<FtpCommandAttribute>();
            if(key is null)
                throw new ArgumentNullException($"FtpCommandAttribute\n Type: {typeof(T).FullName}");
            if (Commands.ContainsKey(key.Command))
                throw new ArgumentException($"Command '{key.Command}' already exists");
            var auth = typeof(T).GetCustomAttribute<AuthorizeAttribute>();
            _commands.Add(key.Command, new BaseCommandDetails(obj, auth is not null, true));
        }
    }
    
    internal class BaseCommandDetails
    {
        public BaseCommand Command { get; }
        public bool IsCustom { get; }
        public bool NeedVerification { get; }
        public BaseCommandDetails(BaseCommand command, bool needVerification, bool isCustom = false)
        {
            Command = command;
            NeedVerification = needVerification;
            IsCustom = isCustom;
        }
    }
}
