using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace HipBot.Services
{
    public class TfsService : ITfsService
    {
        public IConfigService ConfigService { get; set; }

        public WorkItem RetrieveWorkItem(string user, string tfsUri, string collectionName, int workItemId)
        {
            var uri = new Uri(string.Format("{0}{1}", tfsUri, collectionName));
            var tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
            tpc.Authenticate();
            
            var workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));
            var wi = workItemStore.GetWorkItem(workItemId);
            return wi;
        }

        public WorkItem CreateTask(string user, string tfsUri, string collectionName, string project, string title, string body)
        {
            var uri = new Uri(string.Format("{0}{1}", tfsUri, collectionName));
            var tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(uri);
            tpc.Authenticate();

            var workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));
            var workItemTypes = workItemStore.Projects[project].WorkItemTypes;

            var storyType = workItemTypes["User Story"];
            var wi = new WorkItem(storyType);
            
            wi.Title = title;
            wi.Description = body;
            
            workItemStore.BatchSave(new [] {wi});

            return wi;
        }

        public void SetUserConfig(string @from, string tfsUri, string collection)
        {
            var config = ConfigService.GetConfig();

            config.SetValue("Tfs", @from + "-tfsuri", tfsUri);
            config.SetValue("Tfs", @from + "-collection", collection);

            ConfigService.SetConfig(config);
        }

        public void GetUserConfig(string user, out string tfsUri, out string collection)
        {
            var config = ConfigService.GetConfig();

            tfsUri = config.GetValue("Tfs", user + "-tfsuri", null);
            collection = config.GetValue("Tfs", user + "-collection", "");
        }

        private IEnumerable<CatalogNode> GetTeamProjectsUsingCatalog(TfsTeamProjectCollection tpc)
        {
            var projectNodes = tpc.CatalogNode.QueryChildren(
                        new[] { CatalogResourceTypes.TeamProject },
                        false, CatalogQueryOptions.None);

            return projectNodes;
        }
    }
}