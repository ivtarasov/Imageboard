using Netaba.Data.Enteties;

namespace Netaba.Web.Models.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; init; }
        public int NumberInTread { get; init; }
        public bool IsFromShortcutTread { get; init; }

        public PostViewModel(Post post, int numberInTread, bool isFromShrtcutTread)
        {
            Post = post;
            NumberInTread = numberInTread;
            IsFromShortcutTread = isFromShrtcutTread;
        }
    }
}
