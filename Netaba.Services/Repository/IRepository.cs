using System.Collections.Generic;
using Netaba.Data.Models;
using System.Threading.Tasks;

namespace Netaba.Services.Repository
{
    public interface IRepository
    {
        public Task<(bool, int)> TryGetPostLocationAsync(int postId, string boardName);

        public Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName);
        public Task<(bool, int)> TryAddPostToTreadAsync(Post post, string boardName, int treadId);

        public Task<(Board, int)> FindAndLoadBoardAsync(string boardName, int page);
        public Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId);

        public Task DeleteAsync(IEnumerable<int> postIds, string ip, string password);
    }
}