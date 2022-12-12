using API.DTOs;
using API.Helpers;
using Tinder_lvl10.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<MemberDTO> GetMemberAsync(string username);

        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);


    }
}
