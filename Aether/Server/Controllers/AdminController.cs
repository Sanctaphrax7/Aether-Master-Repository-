using Aether.Server.Data;
using Aether.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        public AdminController(DataContext context)
        {
            _context = context;

        }

        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet("roles")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost("create")]
        public async Task<ActionResult<List<User>>> CreateUser(User user)
        {
            //if (_context.Users.Any(u => u.UserName == user.UserName))
            //{
            //    return BadRequest("User Already Exists");
            //}
            if (!CheckUser(user.UserName))
            {
                user.Enabled = true;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                return Ok("User Added");
            }

            return BadRequest("User Already Exists!");

        }


        [HttpPut("update")]
        public async Task<ActionResult<List<User>>> UpdateUser(User user)
        {
            var editUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (editUser == null)
                return NotFound("Sorry, User Doesn't Exist");

            editUser.UserName = user.UserName;
            editUser.Role = user.Role;
            editUser.Enabled = user.Enabled;

            await _context.SaveChangesAsync();


            return Ok(user);
        }


        private bool CheckUser(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }
    }
}
