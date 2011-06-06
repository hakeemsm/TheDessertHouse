using System;
using System.Collections;
using System.Collections.Generic;

namespace TheDessertHouse.Web
{

    public class Pagination<T> : IPagination<T>
    {
        private IEnumerable<T> _dataSource;

        //public Pagination(int dataSource, int currPage)
        //{
        //    _dataSource = dataSource;
        //    _currPage = currPage;
        //    _pageSize = DessertHouseConfigurationSection.Current.Articles.PageSize;
        //    TotalPages = (int)Math.Ceiling((double)_dataSource / _pageSize);
        //}

        public Pagination(IEnumerable<T> dataSource, int pageNumber, int pageSize, int totalItems)
        {
            _dataSource = dataSource;
            PageSize = pageSize; //No. of items to display per page
            PageNumber = pageNumber;
            TotalPageCount = (int)Math.Ceiling((double)(totalItems) / pageSize);
            TotalItemCount = totalItems;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dataSource.GetEnumerator();
        }

        public int TotalPageCount { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasPrevious
        {
            get { return PageNumber > 1; }
            
        }

        public bool HasNext
        {
            get { return PageNumber < TotalPageCount; }
            
        }

        public int PageNumber { get; set; }


        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }
    }
}