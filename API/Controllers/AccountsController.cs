using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public AccountsController(DataContext context,ITokenService tokenService) : base(context)
        {
            this._tokenService = tokenService;
        }

        [HttpPost("login")]
        
        public async Task<ActionResult<UserDTO>> Login(RegisterDTO loginRequest) {

            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

           

            
            var user = await _context.Users.SingleOrDefaultAsync(x=>x.Username==loginRequest.username);
            if (user == null) {
                return BadRequest("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.password));

            for (int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDTO { Username = user.Username, Token = _tokenService.CreateToken(user) };



        }





        [HttpPost("register")]
   
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerRequest) {


            if (!ModelState.IsValid) {
                return BadRequest(ModelState.ValidationState);
            }




#pragma warning disable CS8604 // Possible null reference argument. Nope
            if (await UserExists(registerRequest.username)) {

                return BadRequest("Username is taken");
            }
#pragma warning restore CS8604 // Possible null reference argument.

            using var hmac = new HMACSHA512();

#pragma warning disable CS8604 // Possible null reference argument.
            var user = new AppUser { Id = Guid.NewGuid(), Username = registerRequest.username.ToLower(), PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRequest.password)), PasswordSalt = hmac.Key };
#pragma warning restore CS8604 // Possible null reference argument.
            var addedUser = await _context.Users.AddAsync(user);

            if (addedUser != null) {
                await _context.SaveChangesAsync();
                return new UserDTO { Username = user.Username, Token = _tokenService.CreateToken(user) };
            }

            return BadRequest();

           

           
            
            
          
            
        }

        private async Task<bool> UserExists(string username) {

            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());

        }


    }
}
