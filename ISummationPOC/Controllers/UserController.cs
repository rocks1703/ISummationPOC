using ISummationPOC.DBContext;
using ISummationPOC.Entity;
using ISummationPOC.Handler;
using ISummationPOC.Models;
using ISummationPOC.Request;
using ISummationPOC.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateUser(CreateUserRequest request,  IFormFile image)
        {
            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");

            if (ModelState.IsValid)
            {
                await UserService.CreateUserAsync(request.User, image);
                return RedirectToAction("GetUsers");
            }

            // return RedirectToAction("CreateUser");
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user, IFormFile image)
        {
            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");
            if (ModelState.IsValid)
            {
                await UserService.UpdateUser(user, image);
                return RedirectToAction("GetUsers");
            }
            return RedirectToAction("UpdateUser");
        }


        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {          

            var userTypes = _context.userTypes.Select(ut => new { ut.Id, ut.UserType }).ToList();
            ViewBag.UserTypes = new SelectList(userTypes, "Id", "UserType");

            return View("CreateUser");
        }

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

            return View("UpdateUser", user);
        }


      
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            

            var user = await UserService.DeleteUserAsync(id);
            if (user > 0) 
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            if (user == null) return NotFound();
            return RedirectToAction("GetUsers", User);
        }

    }
}
