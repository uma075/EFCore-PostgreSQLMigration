using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sql.EFCoreMigration;

namespace Sql.EFCoreMigration
{
    // This class is only here for migrations
    public class UserEntitiesFactory : IDesignTimeDbContextFactory<UserEntities>
    {
        public UserEntities CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserEntities>();
            if (args != null && args[0].Equals("Npgsql", System.StringComparison.InvariantCultureIgnoreCase))
            {
                optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=123456;");
            }
            else
            {
                optionsBuilder.UseSqlServer("Data Source =.; Initial Catalog = MigrationDB; Integrated Security = True");
            }

            return new UserEntities(optionsBuilder.Options);
        }
    }
}

