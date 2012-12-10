using System.Linq;
using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Tfs
{
    public class WorkItem : Handler<WorkItem.Options>
    {
        [Flag("workitem")]
        public class Options
        {
            [Parameter("collection", Required = false, Default = "")]
            public string Collection { get; set; }

            [Parameter("item", Required = true)]
            public int Item { get; set; }
        }

        public IHipChatService HipChatService { get; set; }

        public ITfsService TfsService { get; set; }
        
        public IRoomService RoomService { get; set; }

        public override void Receive(Message message, Room room, Options options)
        {
            string tfsUri, collectionName;
            TfsService.GetUserConfig(message.From, out tfsUri, out collectionName);

            var result = TfsService.RetrieveWorkItem(message.From, tfsUri, options.Collection ?? collectionName, options.Item);

            if (result == null)
            {
                HipChatService.Say(room, "Could not find work item");
            }
            else
            {
                var wiResourceUri = result.Project.Uri;
                var uri = string.Format("{0}/web/UI/Pages/WorkItems/WorkItemEdit.aspx?id={1}&pguid={2}", tfsUri, result.Id, wiResourceUri.Segments.Last());
                var info = string.Format("<a href=\"{5}\">{1} ({0})</a><br/><br/>Type: {2}<br/>State: {3}<br/>Assigned To: {4}", result.Id, result.Title, result.Type.Name, result.State, result.Fields["System.AssignedTo"].Value, uri);
                room = RoomService.GetRoomByJabberId(room.JabberId);
                
                HipChatService.SayHtml(room, info);
            }
        }
    }
}