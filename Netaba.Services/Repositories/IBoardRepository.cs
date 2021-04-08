using System.Collections.Generic;
using Netaba.Models;
using System.Threading.Tasks;

namespace Netaba.Services.Repository
{
    public interface IBoardRepository
    {
        public Task<(bool, int)> TryGetPostLocationAsync(int postId, string boardName);
        public Task<Board> FindBoardAsync(string boardName);
        public Task<string> GetBoardDescriptionAsync(string boardName);
        public Task<List<string>> GetBoardNamesAsync();
        public Task<int> CountTreadsAsync(string boardName);

        public Task<bool> TryAddBoardAsync(Board board);
        public Task<(bool, int)> TryAddTreadToBoardAsync(Tread tread, string boardName);
        public Task<(bool, int)> TryAddPostToTreadAsync(Post post, string boardName, int treadId);

        public Task<Board> FindAndLoadBoardAsync(string boardName, int page, int pageSize);
        public Task<Tread> FindAndLoadTreadAsync(string boardName, int treadId);

        public Task<bool> TryDeleteBoardAsync(Board board);
        public Task<bool> TryDeletePostsAndTreadsAsync(IEnumerable<int> postIds, string ip, string password, bool isTreadDeletionAllowed);
    }
}