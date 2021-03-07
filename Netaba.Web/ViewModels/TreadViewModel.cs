using System.Collections.Generic;

namespace Netaba.Web.ViewModels
{
    public class TreadViewModel
    {
        public int NumberOfOmittedPosts { get; set; }
        public List<PostViewModel> PostViewModels { get; private set; }
        public bool IsShortcut { get; private set; }
        public int TreadId { get; private set; }

        public TreadViewModel(List<PostViewModel> postViewModels, int treadId)
        {
            PostViewModels = postViewModels;
            TreadId = treadId;
        }

        public TreadViewModel(List<PostViewModel> postViewModels, int numberOfDisplayedPosts, int treadId)
        {
            IsShortcut = true;
            TreadId = treadId;

            var numberOfOmittedPosts = postViewModels.Count - numberOfDisplayedPosts;
            if (numberOfOmittedPosts > 0)
            {
                NumberOfOmittedPosts = numberOfOmittedPosts;
                postViewModels.RemoveRange(1, numberOfOmittedPosts);
            }

            PostViewModels = postViewModels;
        }
    }
}