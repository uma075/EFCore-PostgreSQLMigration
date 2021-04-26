using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sql.EFCoreMigration
{
    [Table("Profile")]
    public class ProfileEntity
    {
        [Key]
        [Column("ProfileID")]
        public Guid ProfileId { get; set; }

        [MaxLength(30)]
        public string Username { get; set; }

        [MaxLength(8)]
        public string SerialNumber { get; set; }

        public decimal TotalTime { get; set; }

        [MaxLength(255)]
        public string ErrorMessage { get; set; }

    }

    [SqlServerEntityConfig]
    public class ProfileEntityConfigurationSqlServer : IEntityTypeConfiguration<ProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.Property(d => d.ProfileId)
                .HasDefaultValueSql("NEWID()");

            builder.Property(p => p.TotalTime)
                .HasColumnType("decimal(6,2)");

        }
    }

    [PostgresEntityConfig]
    public class ProfileEntityConfigurationPostgres : IEntityTypeConfiguration<ProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.Property(d => d.ProfileId)
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.TotalTime)
                .HasColumnType("numeric(6,2)");

        }
    }
}
