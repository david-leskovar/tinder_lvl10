using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;

namespace API.Controllers
{
    
    public class BuggyController : BaseApiController
    {
        public BuggyController(DataContext context) : base(context)
        {
        }


        
        [HttpGet("auth")]

        public ActionResult<string> GetSecret() {

            return "secret text";

        }


        [AllowAnonymous]
        [HttpGet("not-found")]

        public ActionResult<AppUser> GetNotFound()
        {

            var thing = _context.Users.Find(Guid.NewGuid());
            ModelState.AddModelError("notFound", "User you are trying to get was not found");

            if (thing == null) return NotFound(ModelState);
            

            
            return Ok(thing);

        }

        [AllowAnonymous]
        [HttpGet("server-error")]

        public ActionResult<string> GetServerError()
        {

                var thing = _context.Users.Find(Guid.NewGuid());
                var thingToReturn = thing.ToString();
                return thingToReturn;
         


        }
        [AllowAnonymous]
        [HttpGet("bad-request")]

        public ActionResult<string> GetBadRequest()
        {

            return BadRequest("This was not a good request");

        }
        


    }
}
