using System.Collections.Generic;
using Netaba.Data.Enteties;

namespace Netaba.Services.Repository
{
    public interface IRepository
    {
        public Post FindPost(int postId);

        public void Delete(IEnumerable<int> postIds, string ip, string password);

        public void AddNewTread(Tread tread, int boardId);
        public void AddNewPost(Post post, int treadId);

        public Board LoadBoard(int boardId);
        public Tread LoadTread(int treadId);
    }
}
