namespace UserListBackend.Models.ApiModels
{
    public class ListResponse<T>
    {
        public IEnumerable<T>? items { get; set; }
        public int ItemsCount { get; set; }
        public int TotalItemsCount { get; set; }
        public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now;
    }
}
