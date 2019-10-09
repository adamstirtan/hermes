using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hermes.Database
{
    public sealed class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlite("Data Source=bot.sqlite");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}