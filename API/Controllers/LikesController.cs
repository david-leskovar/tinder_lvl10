using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tinder_lvl10.Data;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly DataContext context;
        private readonly IUserRepository userRepository;
        private readonly ILikesRepository likesRepository;

        public LikesController(DataContext context,IUserRepository userRepository,ILikesRepository likesRepository) : base(context)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.likesRepository = likesRepository;
        }


        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username) {

            var sourceUserId = User.GetUserId();
            var likedUser = await userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await likesRepository.GetUserWithLikes(sourceUserId);


            if (likedUser == null) return NotFound();
            if (sourceUser.Username == username) return BadRequest("You cannot like yourself bozo");

            var userLike = await likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike { SourceUserId = sourceUserId, LikedUserId = likedUser.Id };

            sourceUser.LikedUsers.Add(userLike);
            if (await userRepository.SaveAllAsync()) return Ok();


            return BadRequest("Failed to like user");
           

        
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes(string predicate) {

            var users = await likesRepository.GetUserLikes(predicate, User.GetUserId());
            return Ok(users);
        
        }



    }
}
