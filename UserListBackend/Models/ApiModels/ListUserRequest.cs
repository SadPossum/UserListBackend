using UserListBackend.Models.LogicModels;

namespace UserListBackend.Models.ApiModels
{
    public class ListUserRequest
    {
        public string? FullName { get; set; } = null;
        public DateTime? DateOfBirth { get; set; } = null;
        public PaginationFilter? Pagination { get; set; } = null;
        public List<SortCriteria>? Sort { get; set; } = null;
    }
}
