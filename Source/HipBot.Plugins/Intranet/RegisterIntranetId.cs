using HipBot.Domain;
using HipBot.Handlers;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Plugins.Intranet
{
    public class RegisterIntranetId : Handler<RegisterIntranetId.Options>
    {
        [Flag("registerintranetid")]
        public class Options
        {
            [Parameter("id", Required = true)]
            public int Id { get; set; }
        }

        public IHipChatService HipChatService { get; set; }

        public IIntranetService IntranetService { get; set; }

        public IRoomService RoomService { get; set; }
        
        public IConfigService ConfigService { get; set; }

        public override void Receive(Message message, Room room, Options options)
        {
            var id = options.Id;

            IntranetService.Add(id, message.From);

            room = RoomService.GetRoomByJabberId(room.JabberId);

            HipChatService.SayHtml(room, "<b>{0}</b> is registered with the id <i>{1}</i>", message.From, id);
        }
    }
}
