using Netaba.Data.Enteties;
using System;

namespace Netaba.Web.Models.ViewModels
{
    public class TreadViewModel
    {
        public int NumberOfOmittedPosts { get; set; }
        public Tread Tread { get; private set; }
        public bool IsShortcut { get; private set; }

        public TreadViewModel(Tread tread) => Tread = tread;

        public TreadViewModel(Tread tread, int numberOfDisplayedPosts)
        {
            IsShortcut = true;

            var numberOfOmittedPosts = tread.Posts.Count - numberOfDisplayedPosts;
            if (numberOfOmittedPosts > 0)
            {
                NumberOfOmittedPosts = numberOfOmittedPosts;
                tread.Posts.RemoveRange(1, numberOfOmittedPosts);
            }

            Tread = tread;
        }
    }
}