using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Npgsql.NameTranslation;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sql.EFCoreMigration
{
    public partial class UserEntitiesContext : DbContext
    {

        private static readonly Regex _keysRegex = new Regex("^(PK|FK|IX)_", RegexOptions.Compiled);

        public UserEntitiesContext(DbContextOptions<UserEntitiesContext> options) : base(options)
        {
        }
             

        public virtual DbSet<ProfileEntity> Profilles { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<CustomEventEntity> CustomEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsNpgsql())
            {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntitiesContext).Assembly, x => x.GetCustomAttributes(typeof(PostgresEntityConfigAttribute)).Any());
            }
            else
            {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntitiesContext).Assembly, x => x.GetCustomAttributes(typeof(SqlServerEntityConfigAttribute)).Any());
            }
            
        if (Database.IsNpgsql())
            {
                FixLowerCaseNames(modelBuilder);
            }

        }

        private void FixLowerCaseNames(ModelBuilder modelBuilder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var table in modelBuilder.Model.GetEntityTypes())
            {
                ConvertToLowerCase(mapper, table);
                foreach (var property in table.GetProperties())
                {
                    ConvertToLowerCase(mapper, property);
                }

                foreach (var primaryKey in table.GetKeys())
                {
                    ConvertToLowerCase(mapper, primaryKey);
                }

                foreach (var foreignKey in table.GetForeignKeys())
                {
                    ConvertToLowerCase(mapper, foreignKey);
                }

                foreach (var indexKey in table.GetIndexes())
                {
                    ConvertToLowerCase(mapper, indexKey);
                }
            }
        }

        private void ConvertToLowerCase(INpgsqlNameTranslator mapper, object entity)
        {
            switch (entity)
            {
                case IMutableEntityType table:
                    var relationalTable = table.Relational();
                    relationalTable.TableName = ConvertGeneralToLowerCase(mapper, relationalTable.TableName);
                    break;
                case IMutableProperty property:
                    property.Relational().ColumnName = ConvertGeneralToLowerCase(mapper, property.Relational().ColumnName);
                    break;
                case IMutableKey primaryKey:
                    primaryKey.Relational().Name = ConvertKeyToLowerCase(mapper, primaryKey.Relational().Name);
                    break;
                case IMutableForeignKey foreignKey:
                    foreignKey.Relational().Name = ConvertKeyToLowerCase(mapper, foreignKey.Relational().Name);
                    break;
                case IMutableIndex indexKey:
                    indexKey.Relational().Name = ConvertKeyToLowerCase(mapper, indexKey.Relational().Name);
                    break;
                default:
                    throw new NotImplementedException("Unexpected type was provided to lower case converter");
            }
        }

        private string ConvertKeyToLowerCase(INpgsqlNameTranslator mapper, string keyName) =>
           ConvertGeneralToLowerCase(mapper, _keysRegex.Replace(keyName, match => match.Value.ToLower()));

        private string ConvertGeneralToLowerCase(INpgsqlNameTranslator mapper, string entityName) =>
            mapper.TranslateMemberName(ModifyNameBeforeConvertion(mapper, entityName));

        protected virtual string ModifyNameBeforeConvertion(INpgsqlNameTranslator mapper, string entityName) => entityName.ToLower();
    }
}
