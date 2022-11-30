using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;

namespace Tinder_lvl10.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersContoller : ControllerBase
    {
        private readonly DataContext _context;
        public UsersContoller(DataContext context) {
        
            this._context = context;

        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {


            AppUser appUser = new AppUser { Username = "deyf", Id = Guid.NewGuid() };

            var query  = _context.Users.ToListAsync();
           
            return Ok(await query);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<AppUser>> GetUser(Guid id)
        {
            var query = _context.Users.FindAsync(id);

            var user = await query;

            if (user == null) {
                return NotFound();
            }

            return Ok(user);

         
            

            

        }


    }
}
