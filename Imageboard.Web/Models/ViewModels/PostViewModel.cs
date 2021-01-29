using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public bool IsFromShortcutTread { get; set; }

        public PostViewModel(Post post, bool isFromShrtcutTread)
        {
            Post = post;
            IsFromShortcutTread = isFromShrtcutTread;
        }
    }
}
