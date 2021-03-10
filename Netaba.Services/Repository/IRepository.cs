using System.Collections.Generic;
using Netaba.Data.Models;

namespace Netaba.Services.Repository
{
    public interface IRepository
    {
        public bool TryGetPostLocation(int postId, string boardName, out int treadId);

        public bool TryAddNewTreadToBoard(Tread tread, string boardName, out int treadId);
        public bool TryAddNewPostToTread(Post post, string boardName, int treadId, out int postId);

        public Board FindAndLoadBoard(string boardName, int page, out int count);
        public Tread FindAndLoadTread(string boardName, int treadId);

        public void Delete(IEnumerable<int> postIds, string ip, string password);
    }
}
