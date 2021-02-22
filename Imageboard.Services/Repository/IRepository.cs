using System.Collections.Generic;
using Imageboard.Data.Enteties;

namespace Imageboard.Services.Repository
{
    public interface IRepository
    {
        public void Delete(IEnumerable<int> postIds);

        public void AddNewTread(Tread tread, int boardId);
        public void AddNewPost(Post post, int treadId);

        public Board LoadBoard(int boardId);
        public Tread LoadTread(int treadId);
    }
}
