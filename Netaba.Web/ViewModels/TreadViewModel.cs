using System.Collections.Generic;

namespace Netaba.Web.ViewModels
{
    public class TreadViewModel
    {
        public int NumberOfOmittedPosts { get; }
        public List<PostViewModel> PostViewModels { get; }
        public bool IsShortcut { get; }
        public int TreadId { get; }

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