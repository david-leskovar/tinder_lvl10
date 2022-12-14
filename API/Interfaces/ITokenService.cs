using Tinder_lvl10.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {

        Task<string> CreateToken(AppUser user);
        

    }
}
