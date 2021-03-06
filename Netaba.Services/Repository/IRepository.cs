using System.Collections.Generic;
using Netaba.Data.Models;

namespace Netaba.Services.Repository
{
    public interface IRepository
    {
        public bool IsThereBoard(int boardId);
        public bool TryFindPost(int postId, out (int BoardId, int TreadId) postPlace);

        public void DeletePosts(IEnumerable<int> postIds, string ip, string password);

        public void AddNewTreadToBoard(Tread tread, int boardId);
        public void AddNewPostToTread(Post post, int treadId);

        public Board LoadBoard(int boardId);
        public Tread LoadTread(int treadId);
    }
}
