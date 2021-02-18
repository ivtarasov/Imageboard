using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; private set; }
        public int NumberInTread { get; private set; }
        public bool IsFromShortcutTread { get; private set; }

        public PostViewModel(Post post, int numberInTread, bool isFromShrtcutTread)
        {
            Post = post;
            NumberInTread = numberInTread;
            IsFromShortcutTread = isFromShrtcutTread;
        }
    }
}
