using System;
using System.Linq;

namespace Sql.EFCoreMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            // SQL server

            var sqlDbContext = new UserEntitiesFactory().CreateDbContext(new[] { "SqlServer" });

            sqlDbContext.Profilles.Add(new ProfileEntity()
            {
                SerialNumber = "1",
                Username="TestUser"

            });

            sqlDbContext.SaveChanges();

            var data = sqlDbContext.Profilles.ToList();


            // Postgres

            var postgresDbContext = new UserEntitiesFactory().CreateDbContext(new[] { "Npgsql" });
            postgresDbContext.Profilles.Add(new ProfileEntity()
            {
                SerialNumber = "1",
                Username = "TestUser"

            });

            postgresDbContext.SaveChanges();

            var pgdata = sqlDbContext.Profilles.ToList();
        }
    }
}
