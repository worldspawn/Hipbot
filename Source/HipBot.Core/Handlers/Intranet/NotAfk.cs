using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Intranet
{
    public class NotAfk : Handler<NotAfk.Options>
    {
        [Flag("notafk")]
        public class Options
        {
        }

        public IHipChatService HipChatService { get; set; }

        public IIntranetService IntranetService { get; set; }

        public IRoomService RoomService { get; set; }

        public IConfigService ConfigService { get; set; }

        public override void Receive(Message message, Room room, Options options)
        {
            var result = IntranetService.NotAfk(message.From);

            result.Wait();
            
            if (result.Result)
            {
                room = RoomService.GetRoomByJabberId(room.JabberId);
                HipChatService.SayHtml(room, "<b>Back at work!</b>");
            }
            else
            {
                room = RoomService.GetRoomByJabberId(room.JabberId);
                HipChatService.SayHtml(room, "<b>Failed to check you back in!</b>");
            }
        }
    }
}