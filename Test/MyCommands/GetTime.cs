using System;
using System.Threading.Tasks;
using VoDA.FtpServer.Attributes;
using VoDA.FtpServer.Commands;
using VoDA.FtpServer.Interfaces;
using VoDA.FtpServer.Models;

namespace Test.MyCommands
{
    [FtpCommand("TIME")]
    internal class GetTime : BaseCommand
    {
        public override Task<IFtpResult> Invoke(FtpClient client, FtpClientParameters configParameters, string arguments)
        {
            return Task.FromResult(CustomResponse(200, $"Current time: {DateTime.Now.Ticks}"));
        }
    }
}
