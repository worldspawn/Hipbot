using System.Threading.Tasks;

namespace HipBot.Services
{
    public interface IIntranetService
    {
        /// <summary>
        /// Adds the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        void Add(int id, string name);

        void Remove(int id, string name);
        int? GetIdFor(string name);
        Task<bool> Afk(string name, string location, string duration);
        Task<bool> NotAfk(string name);
    }
}