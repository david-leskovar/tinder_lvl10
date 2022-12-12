using API.DTOs;
using API.Entities;
using Tinder_lvl10.Entities;

namespace API.Interfaces
{
    public interface ILikesRepository
    {

        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate,int userId);






    }
}
