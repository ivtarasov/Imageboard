using Netaba.Models;

namespace Netaba.Web.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; }
        public int NumberInTread { get; }
        public bool IsFromShortcutTread { get; }
        
        public PostViewModel(Post post, int numberInTread, bool isFromShrtcutTread)
        {
            Post = post;
            NumberInTread = numberInTread;
            IsFromShortcutTread = isFromShrtcutTread;
        }
    }
}
