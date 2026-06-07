using Microsoft.EntityFrameworkCore;

namespace UniNest.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ 

        }
    }
}
