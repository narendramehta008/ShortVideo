using Microsoft.EntityFrameworkCore;
using ShortVideo.API.Data.DbModels;

namespace ShortVideo.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}