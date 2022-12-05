using API.DTOs;
using Tinder_lvl10.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(Guid id);

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<MemberDTO> GetMemberAsync(string username);

        Task<IEnumerable<MemberDTO>> GetMembersAsync();


    }
}
