using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace Sql.EFCoreMigration
{
    public partial class UserEntities : DbContext
    {       
        public UserEntities(DbContextOptions<UserEntities> options) : base(options)
        {
        }
             

        public virtual DbSet<ProfileEntity> Profilles { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<CustomEventEntity> CustomEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsNpgsql())
            {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntities).Assembly, x => x.GetCustomAttributes(typeof(PostgresEntityConfigAttribute)).Any());
            }
            else
            {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntities).Assembly, x => x.GetCustomAttributes(typeof(SqlServerEntityConfigAttribute)).Any());
            }
            
        }
    }
}
