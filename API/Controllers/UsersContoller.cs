using API.Controllers;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public Users(DataContext context,IUserRepository userRepository,IMapper mapper) : base(context)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("Ka daj")]

        public async Task<ActionResult> SeedUsers()
        {

            

            using var hmac = new HMACSHA512();

            var user1 = new AppUser
            {
                Username = "Lisa",
                Gender = "Female",
                DateOfBirth = DateTime.Parse("1956-07-22"),
                KnownAs = "Lisa",
                Created = DateTime.Parse("2020-06-24"),
                LastActive = DateTime.Parse("2020-06-21"),
                Introduction = "Lisa introduction basic af",
                LookingFor = "The boys",
                Interests = "Basic af interests",
                City = "Maribor",
                Country = "Slovenija",
                Photos = new Photo[] { new Photo { Url = "https://randomuser.me/api/portraits/women/54.jpg", IsMain = true } },
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))
            };

            var user2 = new AppUser
            {
                Username = "Karen",
                Gender = "Female",
                DateOfBirth = DateTime.Parse("1995-10-12"),
                KnownAs = "Karen",
                Created = DateTime.Parse("2019-06-24"),
                LastActive = DateTime.Parse("2020-04-23"),
                Introduction = "Karen introduction basic af",
                LookingFor = "The boys",
                Interests = "Basic af interests",
                City = "Ljubljana",
                Country = "Slovenija",
                Photos = new Photo[] { new Photo { Url = "https://randomuser.me/api/portraits/women/51.jpg", IsMain = true } },
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))
            };

            var user3 = new AppUser
            {
                Username = "Kevin",
                Gender = "Male",
                DateOfBirth = DateTime.Parse("1991-03-12"),
                KnownAs = "Karen",
                Created = DateTime.Parse("2019-06-24"),
                LastActive = DateTime.Parse("2020-04-23"),
                Introduction = "Kevin out here lesgoo",
                LookingFor = "The girls",
                Interests = "Basic af interests",
                City = "Ljubljana",
                Country = "Slovenija",
                Photos = new Photo[] { new Photo { Url = "https://randomuser.me/api/portraits/men/33.jpg", IsMain = true } },
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))
            };

            //_context.Users.Add(user1);
            //_context.Users.Add(user2);
            //_context.Users.Add(user3);

            //_context.SaveChanges();


            return Ok("Done");
        }


       


        [AllowAnonymous]
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


        [AllowAnonymous]
        [HttpGet("{username}")]

        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {

            /*Prva verzija
           var user = await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<MemberDTO>(user);

            */

            return await _userRepository.GetMemberAsync(username);

        }


    }
}
