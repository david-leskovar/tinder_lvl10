using Tinder_lvl10.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tinder_lvl10.Data
{
    public class DataContext:DbContext
    {

        private readonly IConfiguration _config;

        public DataContext(DbContextOptions options,IConfiguration config) : base(options) {

            this._config = config;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<AppUser> Users { get; set; }

    }
}
