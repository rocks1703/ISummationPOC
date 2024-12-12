using ISummationPOC.DBContext;
using ISummationPOC.Request;
using ISummationPOC.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ISummationPOC.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserService UserService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ISummationDbContext _context;
        public UserController(IMediator mediator, IUserService _userService, IFileUploadService fileUploadService , ISummationDbContext context)
        {
            _mediator = mediator;
            UserService = _userService;
            _fileUploadService = fileUploadService;
            _context = context;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await UserService.GetAllUserAsync();
            return View("GetUsers", user);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await UserService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        //CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request,  IFormFile? image)
        {
            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");
            if (image != null)
            {
                var allowedExtensions = new[] { ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp" };
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProfileImage", "Only image files (JPEG, PNG, JPG, GIF, BMP, WebP) are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                await UserService.CreateUserAsync(request.User, image);

                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction("GetUsers");
            }
           
            return View(request);
        }


        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {          

            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");          
            return View("CreateUser");
        }

        //UpdateUser
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");            
            UpdateUserRequest userData = new UpdateUserRequest
            {
                User = user 
            };

            return View("UpdateUser", userData);
        }

        //UpdateUser
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request, IFormFile? image)
        {
            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");
            if (image != null)
            {
                var allowedExtensions = new[] { ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp" };
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                { 
                    ModelState.AddModelError("ProfileImage", "Only image files (JPEG, PNG, JPG, GIF, BMP, WebP) are allowed.");
                }
               
            }

            if (ModelState.IsValid)
            {
                await UserService.UpdateUser(request.User, image);
                return RedirectToAction("GetUsers");
            } 


            return View(request);
        }

        //Delete

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            

            var user = await UserService.DeleteUserAsync(id);
            if (user > 0) 
            {
                TempData["SuccessDeleteMessage"] = "User Deleted Successfully.";
            }
            if (user == null) return NotFound();
            return RedirectToAction("GetUsers", User);
        }

    }
}
