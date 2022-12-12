using API.DTOs;
using API.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public AccountsController(DataContext context,ITokenService tokenService,IMapper mapper) : base(context)
        {
            this._tokenService = tokenService;
            this._mapper = mapper;
        }

        [HttpPost("login")]
        
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginRequest) {

            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

           

            
            var user = await _context.Users.SingleOrDefaultAsync(x=>x.Username==loginRequest.Username);
            if (user == null) {
                return BadRequest("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.password));

            for (int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDTO { Username = user.Username, Token = _tokenService.CreateToken(user),KnownAs=user.KnownAs,Gender=user.Gender };



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



            using var hmac = new HMACSHA512();


            user.Id = Guid.NewGuid().GetHashCode();
            user.Username = registerRequest.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRequest.password));
            user.PasswordSalt = hmac.Key;

            var addedUser = await _context.Users.AddAsync(user);

            if (addedUser != null) {
                await _context.SaveChangesAsync();
                return new UserDTO { Username = user.Username, Token = _tokenService.CreateToken(user),KnownAs=user.KnownAs,Gender=user.Gender };
            }

            return BadRequest();

           

           
            
            
          
            
        }

        private async Task<bool> UserExists(string username) {

            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());

        }


    }
}
