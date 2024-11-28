using Azure.Core;
using Azure.Storage.Blobs;
using ISummationPOC.DBContext;
using ISummationPOC.Entity;
using ISummationPOC.Models;
using ISummationPOC.Repository;
using ISummationPOC.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ISummationPOC.Service
{
    public class UserService : Repository<User>, IUserService
    {

        private readonly ISummationDbContext _context;

        private readonly BlobContainerClient _blobContainerClient;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMediator _mediator;

        public UserService(ISummationDbContext context, IConfiguration configuration, IFileUploadService fileUploadService, IMediator mediator) : base(context)
        {
            _context = context;
            var connectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
            var containerName = configuration.GetValue<string>("AzureBlobStorage:ContainerName");
            var blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            _fileUploadService = fileUploadService;
            _mediator = mediator;
        }  
        //UpdateUser

        public async Task<User> UpdateUser(User user,IFormFile ProfileImage)
        {
            string profileImage = user.ProfileImage;

            if (ProfileImage != null) 
            {
                //string imageName = ProfileImage.FileName;
                string imageName = ProfileImage.FileName + " "+ user.FirstName + " " + user.LastName;
               // string imageName = ;


                using (var stream = ProfileImage.OpenReadStream())
                {
                    await _fileUploadService.UploadFileAsync(stream, imageName);
                }

               
                user.ProfileImage = imageName;
            }
            else
            {
               
                if (string.IsNullOrEmpty(user.ProfileImage))
                {
                    user.ProfileImage = "noimage.jpg"; 
                }
               
            }           
            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
        //CreateUser
        public async Task<User> CreateUserAsync(User user, IFormFile ProfileImage)
        {


            if (ProfileImage != null)
            {
                //var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };


                //string imageName = ProfileImage.FileName;
                string imageName = ProfileImage.FileName + " " + user.FirstName + " " + user.LastName;

                using (var stream = ProfileImage.OpenReadStream())
                {
                    await _fileUploadService.UploadFileAsync(stream, imageName);
                }


                user.ProfileImage = imageName;
            }
            else if (string.IsNullOrEmpty(user.ProfileImage))
            {
                user.ProfileImage = "noimage.jpg";
            }
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        //UserList
        public async Task<IEnumerable<UserViewModel>> GetAllUserAsync()
        {

            var usrlst = (from c in _context.users
                          orderby c.Id descending
                          select new UserViewModel
                          {
                              Id = c.Id,
                              UserName = c.UserName.ToLower(),
                              FirstName = c.FirstName,
                              LastName = c.LastName,
                              Email = c.Email.ToLower(),                             
                              UserType = _context.userTypes.Where(f => f.Id == c.UserTypeId).Select(f => f.UserType).FirstOrDefault(),
                              Mobile = c.Mobile,
                              UserDateOfBirth = c.UserDateOfBirth.ToString("dd/MM/yyyy"),
                              ProfileImage = !string.IsNullOrEmpty(c.ProfileImage) ? "http://127.0.0.1:10000/devstoreaccount1/userprofile/" + c.ProfileImage
                                            : "http://127.0.0.1:10000/devstoreaccount1/userprofile/noimage.jpg"
                          });
            return await usrlst.ToListAsync();
        }

        //DeleteUser 
        public async Task<int> DeleteUserAsync(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _context.users.Remove(user);
            return await _context.SaveChangesAsync();

        }

        //GetUserById       
        public async Task<User> GetUserByIdAsync(int id)
        {
            //return await _context.users.Include(u => u.UserType).FirstOrDefaultAsync(u => u.Id == id);
            return await _context.users.FirstOrDefaultAsync();
        }


    }
}
