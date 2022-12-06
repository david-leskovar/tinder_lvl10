using API.Controllers;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;

namespace Tinder_lvl10.Controllers
{

    public class Users : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoservice;

        public Users(DataContext context,IUserRepository userRepository,IMapper mapper,IPhotoService photoService) : base(context)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._photoservice = photoService;
        }

        [AllowAnonymous]
        [HttpGet("Ka daj")]

        public async Task<ActionResult> SeedUsers()
        {

            

            using var hmac = new HMACSHA512();

            var user1 = new AppUser
            {
                Username = "Maia",
                Gender = "Female",
                DateOfBirth = DateTime.Parse("1956-07-22"),
                KnownAs = "Maia",
                Created = DateTime.Parse("2020-06-24"),
                LastActive = DateTime.Parse("2021-06-21"),
                Introduction = "Maia im here hehe xd introduction basic af",
                LookingFor = "The boys",
                Interests = "Basic af interests",
                City = "Slovenj Gradec",
                Country = "Slovenija",
                Photos = new Photo[] { new Photo { Url = "https://randomuser.me/api/portraits/women/54.jpg", IsMain = true } },
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))
            };

            var user2 = new AppUser
            {
                Username = "Mia",
                Gender = "Female",
                DateOfBirth = DateTime.Parse("1995-10-12"),
                KnownAs = "Mia",
                Created = DateTime.Parse("2019-06-24"),
                LastActive = DateTime.Parse("2020-04-23"),
                Introduction = "Mia ka daj introduction basic af",
                LookingFor = "The boys",
                Interests = "Basic af interests",
                City = "Novo mesto",
                Country = "Slovenija",
                Photos = new Photo[] { new Photo { Url = "https://randomuser.me/api/portraits/women/51.jpg", IsMain = true } },
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))
            };

            var user3 = new AppUser
            {
                Username = "Marko",
                Gender = "Male",
                DateOfBirth = DateTime.Parse("1992-03-12"),
                KnownAs = "Marko",
                Created = DateTime.Parse("2019-06-24"),
                LastActive = DateTime.Parse("2020-04-23"),
                Introduction = "Marko out here lesgoo",
                LookingFor = "The girls",
                Interests = "Basic af interests",
                City = "Domžale",
                Country = "Slovenija",
                Photos = new Photo[] { new Photo { Url = "https://randomuser.me/api/portraits/men/33.jpg", IsMain = true } },
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))
            };

            _context.Users.Add(user1);
            _context.Users.Add(user2);
            _context.Users.Add(user3);

            _context.SaveChanges();


            return Ok("Done");
        }


       



        [HttpGet]
        

        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers() {

            /*Prva verzija
            var users = await _userRepository.GetUsersAsync();
            var usersToReturn = _mapper.Map<IEnumerable<MemberDTO>>(users);

            return Ok(usersToReturn);
        
            */
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);


        }


        [HttpGet("{username}",Name ="GetUser")]

        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {

            /*Prva verzija
           var user = await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<MemberDTO>(user);

            */

            return await _userRepository.GetMemberAsync(username);

        }

        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDTO) {

            

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDTO, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        
        }

        [HttpPost("add-photo")]

        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file) {

            var username = User.GetUsername();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            var result = await _photoservice.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUri.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0) {

                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync()) {
                
                return CreatedAtRoute("GetUser",new { UserName=user.Username} ,_mapper.Map<PhotoDTO>(photo));
               

            }
            return BadRequest("Problem adding photo");

        }


        [HttpPut("set-main-photo/{photoId}")]

        public async Task<ActionResult> SetMainPhoto(int photoId) {

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");

        
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId) {


            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You cannot delete  your main photo");

            if (photo.PublicId != null) {
                var result = await _photoservice.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("There was an error deleting the photo");

        }




    }
}
