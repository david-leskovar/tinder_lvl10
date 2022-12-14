using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;

namespace API.Controllers
{

    
    public class AdminController : BaseApiController
    {
        private readonly DataContext context;
        private readonly UserManager<AppUser> userManager;

        public AdminController(DataContext context, UserManager<AppUser> userManager) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]

        public async Task<ActionResult> GetUsersWithRoles() 
        {

            var users = await userManager.Users.Include(r => r.UserRoles).ThenInclude(r => r.Role).OrderBy(u => u.UserName).Select(u => new {u.Id,Username=u.UserName,Roles=u.UserRoles.Select(r=>r.Role.Name).ToList() }).ToListAsync();

            return Ok(users);


        





        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]

        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles) {

            var selectedRoles = roles.Split(",").ToArray();


            var user = await userManager.FindByNameAsync(username);

            if (user == null) return NotFound("User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest(result.Errors);

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));



        
        }    



        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photos-to-moderate")]

        public ActionResult GetPhotosForModeration()
        {

            return Ok("Admins or moderators can see this");


        }

        [AllowAnonymous]
        [HttpGet("GetUsers")]

        public ActionResult GetUsers()
        {

            string sqlstring = "SELECT * FROM AspNetUsers";


            var users = _context.Users.FromSqlRaw(sqlstring);



            return Ok(users);

        }






    }
}
