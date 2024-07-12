namespace ReadFile_Mini.Controllers
{
    public class PaginatedData<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public List<T> Data { get; set; }
    }
}
