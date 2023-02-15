using Microsoft.EntityFrameworkCore;
using UserListBackend.Data;
using UserListBackend.Extensions;
using UserListBackend.Models.DataModels;
using UserListBackend.Models.LogicModels;

namespace UserListBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(IEnumerable<User> Entries, int Count, int TotalCount)> GetUsersAsync(
            string? fullNameSearch = null,
            DateTime? dateOfBirthSearch = null,
            PaginationFilter? paginationFilter = null,
            List<SortCriteria>? sortCriterias = null)
        {
            _logger.LogInformation("Retrieving all users");

            IQueryable<User> query = _context.Users.AsQueryable();
            int totalCount = await _context.Users.CountAsync();

            if (fullNameSearch != null)
            {
                query = query.Where(a => a.FullName.Contains(fullNameSearch));
            }

            if (dateOfBirthSearch != null)
            {
                query = query.Where(a => a.DateOfBirth == dateOfBirthSearch);
            }

            if (sortCriterias != null)
            {
                query = query.SortBy(sortCriterias);
            }

            if (paginationFilter != null)
            {
                query = query
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize);
            }

            int count = await query.CountAsync();

            IEnumerable<User> entries = await query.ToListAsync();
            return (entries, count, totalCount);
        }

        public async Task<User?> GetUserAsync(int id)
        {
            _logger.LogInformation($"Retrieving user with id {id}");
            return await _context.Users.FindAsync(id);
        }

        public async Task<int> AddUserAsync(User user)
        {
            _logger.LogInformation("Adding user");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            _logger.LogInformation($"Updating user with id {user.Id}");
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger.LogInformation($"Deleting user with id {id}");
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
