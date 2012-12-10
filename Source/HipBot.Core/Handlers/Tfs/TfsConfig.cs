using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.Tfs
{
    public class TfsConfig : Handler<TfsConfig.Options>
    {
        [Flag("tfsconfig")]
        public class Options
        {
            [Parameter("tfsuri", Required = false, Default = "")]
            public string TfsUri { get; set; }

            [Parameter("collection", Required = false, Default = "")]
            public string Collection { get; set; }
        }

        public IHipChatService HipChatService { get; set; }
        
        public ITfsService TfsService { get; set; }

        public override void Receive(Message message, Room room, Options options)
        {
            TfsService.SetUserConfig(message.From, options.TfsUri, options.Collection);

            HipChatService.Say(room, string.Format("{0} tfs settings updated!", message.From));
        }
    }
}