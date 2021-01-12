using Imageboard.Data.Enteties;

namespace Imageboard.Web.ViewModels
{
    public class TreadModel
    {
        public Tread Tread { get; set; }
        public string AboutOmittedPosts { get; set; }
        public int FirstDisplayedPost { get; set; }
        public bool IsShortcut { get; set; }
    }
}
