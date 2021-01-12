using Imageboard.Data.Enteties;

namespace Imageboard.Web.ViewModels
{
    public class PostModel
    {
        public Post Post { get; set; }
        public int PostNumber { get; set; }
        public bool IsFromShortcutTread { get; set; }
    }
}
