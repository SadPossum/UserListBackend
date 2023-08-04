using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using UserListBackend.Models.ApiModels;
using UserListBackend.Models.DataModels;
using UserListBackend.Repositories;
using UserListBackend.Validators;

namespace UserListBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;
        private readonly UserValidator _userValidator;

        public UserController(IUserRepository repository, ILogger<UserController> logger, UserValidator userValidator)
        {
            _repository = repository;
            _logger = logger;
            _userValidator = userValidator;
        }

        // GET user
        [HttpGet]
        public async Task<ListResponse<User>> Get([FromQuery] ListUserRequest request)
        {
            _logger.LogInformation($"Retrieving list of ${nameof(User)} items with parameters");

            (IEnumerable<User> entries, int count, int totalCount) = await _repository.GetUsersAsync(
                request.FullName,
                request.DateOfBirth,
                request.Pagination,
                request.Sort);

            return new()
            {
                items = entries,
                ItemsCount = count,
                TotalItemsCount = totalCount,
            };
        }

        // GET user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Retrieving ${nameof(User)} with id {id}");

            User? user = await _repository.GetUserAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"{nameof(Models.DataModels.User)} with id {id} not found");
            }

            return Ok(user);
        }

        // POST user
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            _logger.LogInformation($"Adding ${nameof(User)}");

            ValidationResult validation = _userValidator.Validate(user);

            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            int userId = await _repository.AddUserAsync(user);

            return CreatedAtAction(nameof(Post), new { id = userId }, userId);
        }

        // PUT user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            _logger.LogInformation($"Updating ${nameof(User)} with id {id}");

            User? existingUser = await _repository.GetUserAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"{nameof(Models.DataModels.User)} with id {id} not found");
            }

            user.Id = id;
            int userId = await _repository.UpdateUserAsync(user);

            return NoContent();
        }

        // DELETE user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting ${nameof(User)} with id {id}");

            await _repository.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
