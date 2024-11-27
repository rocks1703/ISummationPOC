using ISummationPOC.Entity;
using ISummationPOC.Models;
using ISummationPOC.Repository;

namespace ISummationPOC.Service
{
    public interface IUserService  :IRepository<User>
    {       
        Task<IEnumerable<UserViewModel>> GetAllUserAsync();
        Task<User> GetUserByIdAsync(int id);       
        Task <User> CreateUserAsync(User user, IFormFile ProfileImage);        
        Task <User> UpdateUser(User user ,IFormFile ProfileImage);
        Task<int> DeleteUserAsync(int id);
    }
}