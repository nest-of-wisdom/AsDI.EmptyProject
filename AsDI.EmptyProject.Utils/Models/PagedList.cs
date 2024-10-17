namespace AsDI.EmptyProject.Utils.Models
{
    public class PagedList<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int TotalPages
        {
            get
            {
                return Total / PageSize + (Total % PageSize > 0 ? 1 : 0);
            }
        }

        public List<T> Data { get; set; } = null;


    }


}
