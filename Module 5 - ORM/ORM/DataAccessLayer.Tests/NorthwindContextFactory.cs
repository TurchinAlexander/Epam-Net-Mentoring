using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer.Tests
{
    public class NorthwindContextFactory : IDesignTimeDbContextFactory<NorthwindContext>
    {
        public NorthwindContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-J66IAL7\SQLEXPRESS;Database=test;Trusted_Connection=True");
            
            return new NorthwindContext(optionsBuilder.Options);
        }
    }
}