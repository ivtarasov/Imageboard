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

        public Task<Board> FindAndLoadBoardAsync(string boardName);
        public Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId);

        public Task<bool> TryDeleteAsync(IEnumerable<int> postIds, string ip, string password);
    }
}