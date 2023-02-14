using UserListBackend.Models.DataModels;
using UserListBackend.Models.LogicModels;

namespace UserListBackend.Repositories
{
    public interface IUserRepository
    {
        Task<(IEnumerable<User> Entries, int Count, int TotalCount)> GetUsersAsync(
            string? fullNameSearch = null,
            DateTime? dateOfBirthSearch = null,
            PaginationFilter? paginationFilter = null,
            List<SortCriteria>? sortCriterias = null);

        Task<User?> GetUserAsync(int id);

        Task<int> AddUserAsync(User user);

        Task<int> UpdateUserAsync(User user);

        Task DeleteUserAsync(int id);
    }
}
