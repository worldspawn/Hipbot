using HipBot.Domain;
using HipBot.Handlers;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Plugins.Intranet
{
    public class Afk : Handler<Afk.Options>
    {
        [Flag("afk")]
        public class Options
        {
            [Parameter("location", Required = false, Default = "Lunch")]
            public string Location { get; set; }

            [Parameter("duration", Required = false, Default = "15 mins")]
            public string Duration { get; set; }
        }

        public IHipChatService HipChatService { get; set; }

        public IIntranetService IntranetService { get; set; }

        public IRoomService RoomService { get; set; }

        public IConfigService ConfigService { get; set; }

        public override void Receive(Message message, Room room, Options options)
        {
            var result = IntranetService.Afk(message.From, options.Location, options.Duration);

            result.Wait();

            if (result.Result)
            {
                room = RoomService.GetRoomByJabberId(room.JabberId);
                HipChatService.SayHtml(room, "<b>Checked out!</b>");
            }
            else
            {
                room = RoomService.GetRoomByJabberId(room.JabberId);
                HipChatService.SayHtml(room, "<b>Failed to checked out!</b>");
            }
        }
    }
}