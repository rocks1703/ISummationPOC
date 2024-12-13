using Azure.Core;
using Azure.Storage.Blobs;
using ISummationPOC.DBContext;
using ISummationPOC.Entity;
using ISummationPOC.Models;
using ISummationPOC.Repository;
using ISummationPOC.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace ISummationPOC.Service
{
    public class UserService : Repository<User>, IUserService
    {

        private readonly ISummationDbContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMediator _mediator;
        private readonly BlobContainerClient _blobContainerClient;

       
        public UserService(ISummationDbContext context,IFileUploadService fileUploadService,IMediator mediator,BlobServiceClient blobServiceClient, IOptions<AzureBlobStorageSettings> blobStorageSettings): base(context)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _mediator = mediator;
            var containerName = blobStorageSettings.Value.ContainerName;                  
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }
        //UpdateUser  

        public async Task<User> UpdateUser(User user, IFormFile? ProfileImage)
        {
           
            string profileImage = user.ProfileImage;

            if (ProfileImage != null)
            {
                
                string folderPath = $"{user.Id}/";
                string imageName = folderPath + ProfileImage.FileName ;

                using (var stream = ProfileImage.OpenReadStream())
                {
                    await _fileUploadService.UploadFileAsync(stream, imageName); 
                }

                profileImage = imageName; 
            }

            user.ProfileImage = profileImage;

            
            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }


        //CreateUser
        public async Task<User> CreateUserAsync(User user, IFormFile ProfileImage)
        {

            if (ProfileImage != null)
            {
                _context.users.Add(user);
                await _context.SaveChangesAsync();

                string folderPath = $"{user.Id}/";

                string imageName = folderPath + ProfileImage.FileName ;               

                using (var stream = ProfileImage.OpenReadStream())
                {
                    await _fileUploadService.UploadFileAsync(stream, imageName);
                }

                user.ProfileImage = imageName;
            }           
            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        //UserList
        public async Task<IEnumerable<UserViewModel>> GetAllUserAsync()
        {

            var userslist = (from c in _context.users                         
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
                              ProfileImage = !string.IsNullOrEmpty(c.ProfileImage)
                                                ? $"http://127.0.0.1:10000/devstoreaccount1/userprofile/{c.ProfileImage}"
                                                 : null,

                          }).OrderByDescending(x=> x.Id);
            return await userslist.ToListAsync();
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
            return await _context.users.FirstOrDefaultAsync();
        }

        public class AzureBlobStorageSettings
        {
           
            public string ContainerName { get; set; }
        }


    }
}
