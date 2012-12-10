using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace HipBot.Services
{
    public interface ITfsService
    {
        void GetUserConfig(string user, out string tfsUri, out string collection);
        WorkItem RetrieveWorkItem(string user, string tfsUri, string collectionName, int workItemId);
        void SetUserConfig(string @from, string tfsUri, string collection);
    }
}