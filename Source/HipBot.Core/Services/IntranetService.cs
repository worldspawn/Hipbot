using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HipBot.Domain;

namespace HipBot.Services
{
    /// <summary>
    /// Service to manage this bots nicknames.
    /// </summary>
    public class IntranetService : IIntranetService
    {
        #region Dependencies

        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        #endregion

        /// <summary>
        /// Adds the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void Add(int id, string name)
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

            config.SetValue("IntranetIds", name, id.ToString());

            // Save to disk
            ConfigService.SetConfig(config);
        }

        public void Remove(int id, string name)
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

            config.Delete("IntranetIds", name);

            // Save to disk
            ConfigService.SetConfig(config);
        }
        
        public int? GetIdFor(string name)
        {
            var config = ConfigService.GetConfig();

            var id = config.GetValue("IntranetIds", name, null);

            if (id == null)
                return null;

            return Convert.ToInt32(id);
        }

        public Task<bool> Afk(string name, string location, string duration)
        {
            var result = new Task<bool>(() =>
                {
                    int? intranetId = GetIdFor(name);
                    if (intranetId == null)
                        return false;

                    var clientHandler = new HttpClientHandler();
                    clientHandler.UseDefaultCredentials = true;

                    var client = new HttpClient(clientHandler);
                    client.BaseAddress = new Uri("http://intranet");
                    var form = new Dictionary<string, string>();

                    var timeRegex = new Regex(@"^\d{1,3} mins$");
                    var usedTimeString = timeRegex.IsMatch(duration);

                    form["txtUser"] = name;
                    form["txtLocation"] = location;
                    form["txtDepart"] = DateTime.Now.ToString("HH:mm");
                    form["txtReturn"] = duration;
                    form["usedTimeString"] = usedTimeString ? "true" : string.Empty;
                    form["txtHidden"] = usedTimeString ? "OutSmartLocation" : "Out";
                    form["txtID"] = intranetId.ToString();
                    form["txtUserOld"] = name;
                    var content = new FormUrlEncodedContent(form);

                    var task = client.PostAsync("/asp/where.asp", content);

                    task.Wait();
                    var response = task.Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }

                    return false;
                });

            result.Start();

            return result;
        }

        public Task<bool> NotAfk(string name)
        {
            var result = new Task<bool>(() =>
            {
                int? intranetId = GetIdFor(name);
                if (intranetId == null)
                    return false;

                var clientHandler = new HttpClientHandler();
                clientHandler.UseDefaultCredentials = true;

                var client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri("http://intranet");
                var form = new Dictionary<string, string>();
                form["txtUser"] = name;
                form["txtLocation"] = "";
                form["txtDepart"] = DateTime.Now.ToString("HH:mm");
                form["txtReturn"] = "";
                form["usedTimeString"] = "false";
                form["txtHidden"] = "In";
                form["txtID"] = intranetId.ToString();
                form["txtUserOld"] = name;
                var content = new FormUrlEncodedContent(form);

                var task = client.PostAsync("/asp/where.asp", content);
                task.Wait();

                var response = task.Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            });

            result.Start();

            return result;
        }
    }
}