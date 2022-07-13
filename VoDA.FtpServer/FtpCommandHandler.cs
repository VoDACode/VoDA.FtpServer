using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private static FtpCommandHandler _instance;
        public static FtpCommandHandler Instance => _instance ?? (_instance = new FtpCommandHandler());

        private IReadOnlyDictionary<string, BaseCommand> Commands { get; }
        private IReadOnlyList<string> VerificationCommands { get; }

        private FtpCommandHandler()
        {
            var commands = Assembly.GetCallingAssembly().GetTypes()
              .Where(t => string.Equals(t.Namespace, "VoDA.FtpServer.Commands", StringComparison.Ordinal) 
                          && t.GetCustomAttribute(typeof(FtpCommandAttribute)) != null)
              .ToArray();
            var _commands = new Dictionary<string, BaseCommand>();
            var _verificationCommands = new List<string>();
            foreach (var command in commands)
            {
                var obj = Activator.CreateInstance(command) as BaseCommand;
                if(obj == null)
                    continue;
                var key = command.GetCustomAttribute<FtpCommandAttribute>();
                _commands.Add(key.Command, obj);
                var auth = command.GetCustomAttribute<AuthorizeAttribute>();
                if (!(auth is null))
                    _verificationCommands.Add(key.Command);
            }
            Commands = _commands;
            VerificationCommands = _verificationCommands;
        }

        public async Task<IFtpResult> HandleCommand(FtpCommand command, FtpClient client, FtpServerAuthorizationOptions authorization, FtpServerFileSystemOptions fileSystem, FtpServerOptions serverOptions)
        {
            if (!Commands.ContainsKey(command.Command))
                return new UnknownCommand502Response();
            if (authorization.UseAuthorization && !client.IsAuthorized && !VerificationCommands.Any(p => p == command.Command))
                return new NotLoggedIn530Response();
            return await Commands[command.Command].Invoke(client, authorization, fileSystem, serverOptions, command.Arguments);
        }
    }
}
