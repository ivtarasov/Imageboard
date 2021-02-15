using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; private set; }
        public bool IsFromShortcutTread { get; private set; }
        public bool IsOp 
        {
            get => Post.NumberInTread == 0;
        }

        public PostViewModel(Post post, bool isFromShrtcutTread)
        {
            Post = post;
            IsFromShortcutTread = isFromShrtcutTread;
        }
    }
}
