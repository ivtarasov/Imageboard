using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public bool IsFromShortcutTread { get; set; }
        public bool IsOp {get ; set; }

        public PostViewModel(Post post, bool isFromShrtcutTread, bool isOp)
        {
            Post = post;
            IsFromShortcutTread = isFromShrtcutTread;
            IsOp = isOp;
        }
    }
}
