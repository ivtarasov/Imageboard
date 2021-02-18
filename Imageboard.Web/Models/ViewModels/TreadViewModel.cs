using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class TreadViewModel
    {
        private int _numberOfOmittedPosts;
        public Tread Tread { get; private set; }
        public bool IsShortcut { get; private set; }
        //public int BoardId { get; private set; }

        public int NumberOfOmittedPosts
        {
            get => _numberOfOmittedPosts;
            private set
            {
                if (value < 0) _numberOfOmittedPosts = 0;
                else _numberOfOmittedPosts = value;
            }
        }

        public TreadViewModel(Tread tread) => Tread = tread;

        public TreadViewModel(Tread tread, int numberOfOmittedPosts)
        {
            Tread = tread;
            NumberOfOmittedPosts = numberOfOmittedPosts;
            IsShortcut = true;
        }
    }
}
