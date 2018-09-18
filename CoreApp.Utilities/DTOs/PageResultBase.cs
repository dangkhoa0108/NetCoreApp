using System;

namespace CoreApp.Utilities.DTOs
{
    /// <summary>
    /// Abstract class to define Result Page
    /// </summary>
    public abstract class PageResultBase
    {
        //Current Page
        public int CurrentPage { get; set; }

        //Number Row of Page
        public int PageSize { get; set; }

        //Total Row Data
        public int RowCount { get; set; }

        //First Row on Page
        public int FirstRow => (CurrentPage - 1) * PageSize + 1;

        //Last Row on Page
        public int LastRow => Math.Min(CurrentPage * PageSize, RowCount);

        //Total Page to Show Data
        public int PageCount
        {
            get
            {
                var pageCount = (double) RowCount / PageSize;
                return (int) Math.Ceiling(pageCount);
            }
            set
            {
                PageCount = value;
            }
        }

    }
}
