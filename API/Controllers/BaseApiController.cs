using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Tinder_lvl10.Data;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public class BaseApiController : ControllerBase
    {

        protected DataContext _context;
        public BaseApiController(DataContext context) {

            this._context = context;
        }
       
    }
}
