using System.Text;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.System
{
    public class Help : Handler<Help.Options>
    {
        [Flag("help")]
        public class Options {}

        public IHipChatService HipChatService { get; set; }

        public override void Receive(Message message, Room room, Options options)
        {
            var commands = new ParameterPrinter().Print(string.Empty, typeof (Help).Assembly);
            var sb = new StringBuilder();

            foreach(var command in commands)
            {
                sb.AppendLine(command);
            }

            HipChatService.Say(room, sb.ToString());
        }
    }
}
