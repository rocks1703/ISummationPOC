using ISummationPOC.DBContext;
using ISummationPOC.Entity;
using ISummationPOC.Request;
using ISummationPOC.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISummationPOC.Handler
{
    public class UserHandler :IRequestHandler<CreateUserRequest, User>,
                                   IRequestHandler<DeleteUserRequest, int>,
                                    IRequestHandler<UpdateUserRequest, User>,
                                    IRequestHandler<GetUserByIdRequest, User>
                        
    {
        private readonly IUserService _usersService;
       // private readonly IUserService _userService;
        public readonly UserHandler _userHandler;
        private readonly ISummationDbContext summationDbContext;



        public UserHandler(IUserService usersService, ISummationDbContext summationDbContext)
        {
            _usersService = usersService;
            summationDbContext = summationDbContext;


        }


        public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken )
        {
            try
            {
                await _usersService.AddAsync(request.User);
                return request.User;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existinguser = await _usersService.GetUserByIdAsync(request.User.Id);
                if (existinguser == null)
                {
                    throw new Exception("user not found");
                }


                existinguser.Id = request.User.Id;
                existinguser.UserTypeId = request.User.UserTypeId;
                existinguser.FirstName = request.User.FirstName;
                existinguser.LastName = request.User.LastName;
                existinguser.Email = request.User.Email;
                existinguser.Mobile = request.User.Mobile;
                

                await _usersService.UpdateAsync(existinguser);

                return existinguser;
            }
            catch (DbUpdateException dbEx)
            {

                throw new Exception("Database error occurred while updating the User", dbEx);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while updating the user", ex);
            }
        }

        public async Task<User> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var usr = await _usersService.GetUserByIdAsync(request.id);
                if (usr == null)
                {
                    throw new Exception("User not found");

                }

                return usr;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            int deleted = 0;
            try
            {

                await _usersService.DeleteUserAsync(request.id);
                deleted = request.id;
            }
            catch
            {
                deleted = 0;
            }
            return deleted;
        }

    }
}
