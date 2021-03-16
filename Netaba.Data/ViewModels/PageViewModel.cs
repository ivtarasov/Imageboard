using System;

namespace Netaba.Data.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public string BoardName { get; }

        public PageViewModel(int count, int pageNumber, int pageSize, string boardName)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            BoardName = boardName;
        }
    }
}
