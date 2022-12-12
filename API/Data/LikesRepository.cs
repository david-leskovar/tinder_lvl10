using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {

        private readonly DataContext _context;
        public LikesRepository(DataContext context) {

            this._context = context;
        
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, int userId)



        {

            var cleanName = predicate.Trim();


            var users = _context.Users.OrderBy(u => u.Username).AsQueryable();
            var likes = _context.Likes.AsQueryable();


            Console.WriteLine("Predicate length:" + predicate.Length);
            Console.WriteLine("Comparison length: " + "likedBy".Length);
           

            if (cleanName=="liked") {

                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.LikedUser);
            }

            if (cleanName.Equals("likedBy")) {

                Console.WriteLine("Sem tu predicate je likedBy");

                likes = likes.Where(like => like.LikedUserId == userId);
                users = likes.Select(like => like.SourceUser);
            }

            return await users.Select(user => new LikeDTO {
            
                Username=user.Username,
                KnownAs=user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoURL = user.Photos.FirstOrDefault(p=>p.IsMain).Url,
                City = user.City,
                Id = user.Id
            
            }).ToListAsync();

        }

        public async  Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
