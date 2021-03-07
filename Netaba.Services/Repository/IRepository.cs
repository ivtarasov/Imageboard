using System.Collections.Generic;
using Netaba.Data.Models;

namespace Netaba.Services.Repository
{
    public interface IRepository
    {
        public bool IsThereBoard(int boardId);
        public bool TryGetPostLocation(int postId, out (int BoardId, int TreadId) postPlace);

        public void Delete(IEnumerable<int> postIds, string ip, string password);

        public bool TryAddNewTreadToBoard(Tread tread, int boardId, out int treadId);
        public bool TryAddNewPostToTread(Post post, int treadId, out int boardId);

        public Board LoadBoard(int boardId);
        public Tread LoadTread(int treadId);
    }
}
