using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Tinder_lvl10.Data;
using Tinder_lvl10.Entities;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper mapper;

        public UserRepository(DataContext context,IMapper mapper) {
            this._context = context;
            this.mapper = mapper;
        }

      

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p=>p.Photos)
                
                
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)

        {
         
            _context.Entry(user).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
            
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
            return await _context.Users.Where(x => x.Username == username).ProjectTo<MemberDTO>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();


            query = query.Where(u => u.Username != userParams.CurrentUsername);
            query = query.Where(u => u.Gender.ToLower() == userParams.Gender.ToLower());

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge-1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);


            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            Console.WriteLine("Loggam order:");
            Console.WriteLine(userParams.OrderBy);

            query = userParams.OrderBy switch
            {

                "created" => query.OrderByDescending(u => u.LastActive),
                _ => query.OrderBy(u => u.Username)

            };

            



      


            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(mapper.ConfigurationProvider).AsNoTracking(),userParams.PageNumber, userParams.PageSize);

            

        }

      
    }
}
 