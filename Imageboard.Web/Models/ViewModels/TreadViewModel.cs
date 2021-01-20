using Imageboard.Data.Enteties;

namespace Imageboard.Web.Models.ViewModels
{
    public class TreadViewModel
    {
        public Tread Tread { get; set; }
        public string AboutOmittedPosts { get; set; }
        public bool IsShortcut { get; set; }
        public int BoardId { get; set; }
        public TreadViewModel(Tread tread, string aboutOmittedPosts, bool isShortcut)
        {
            Tread = tread;
            AboutOmittedPosts = aboutOmittedPosts;
            IsShortcut = isShortcut;
        }
    }
}
