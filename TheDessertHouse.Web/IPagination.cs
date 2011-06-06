using System.Collections.Generic;

namespace TheDessertHouse.Web
{
    public interface IPagination
    {
        bool HasPrevious { get; }
        bool HasNext { get; }
        int TotalPageCount { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
    }

    public interface IPagination<T>: IPagination, IEnumerable<T>
    {
        
    }


}