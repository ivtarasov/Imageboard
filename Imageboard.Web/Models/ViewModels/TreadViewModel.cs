using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class TreadViewModel
    {
        private int _numberOfOmittedPosts;
        public Tread Tread { get; init; }
        public bool IsShortcut { get; init; }

        public int NumberOfOmittedPosts
        {
            get => _numberOfOmittedPosts;
            init
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
