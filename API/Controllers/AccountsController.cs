using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;

namespace API.Controllers
{

    [AllowAnonymous]
    public class AccountsController : BaseApiController
    {

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AccountsController(DataContext context, ITokenService tokenService, SignInManager<AppUser> signInManager, IMapper mapper, UserManager<AppUser> userManager,RoleManager<AppRole> roleManager) : base(context)
        {
            this._tokenService = tokenService;
            this._mapper = mapper;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginRequest) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }




            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginRequest.Username);
            if (user == null) {
                return BadRequest("Invalid username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.password, false);

            if (!result.Succeeded) return Unauthorized();





            return new UserDTO { Username = user.UserName, Token = await _tokenService.CreateToken(user), KnownAs = user.KnownAs, Gender = user.Gender };



        }





        [HttpPost("register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerRequest) {


            if (!ModelState.IsValid) {
                return BadRequest(ModelState.ValidationState);
            }




#pragma warning disable CS8604 // Possible null reference argument. Nope
            if (await UserExists(registerRequest.Username)) {

                return BadRequest("Username is taken");
            }
#pragma warning restore CS8604 // Possible null reference argument.


            var user = _mapper.Map<AppUser>(registerRequest);






            user.Id = Guid.NewGuid().GetHashCode();
            user.UserName = registerRequest.Username.ToLower();


            var result = await _userManager.CreateAsync(user, registerRequest.password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if(!roleResult.Succeeded ) return BadRequest(roleResult.Errors);    


            return new UserDTO { Username = user.UserName, Token = await _tokenService.CreateToken(user), KnownAs = user.KnownAs, Gender = user.Gender };











        }


        [HttpGet("kadaj")]
        public async Task<ActionResult> SeedRoles()
        {

            var roles = new List<AppRole> {

                new AppRole{Name="Member"},
                new AppRole{Name="Admin"},
                new AppRole{Name="Moderator"},

            };


            try
            {
                foreach (var role in roles)
                {

                    await _roleManager.CreateAsync(role);


                }


                var admin = new AppUser
                {
                    UserName = "admin1"
                };

                await _userManager.CreateAsync(admin, "Pa$$w0rd");
                await _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });  


                return Ok();
            }

            catch (Exception ex) { 
            
            
                return BadRequest(ex.Message);
            }


        }

        private async Task<bool> UserExists(string username) {

            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());

        }


        

    }
}
