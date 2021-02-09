using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; private set; }
        public bool IsFromShortcutTread { get; private set; }
        public bool IsOp {get ; private set; }

        public PostViewModel(Post post, bool isFromShrtcutTread, bool isOp)
        {
            Post = post;
            IsFromShortcutTread = isFromShrtcutTread;
            IsOp = isOp;
        }
    }
}
