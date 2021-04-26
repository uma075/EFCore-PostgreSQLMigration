using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sql.EFCoreMigration
{
    [Table("CustomEvent")]
    public class CustomEventEntity
    {
        [Key]
        public int CustomEventSqlId { get; set; }

        public int UserSqlId { get; set; }
    

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CustomEventDate { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }

        public string Source { get; set; }

        public UserEntity User { get; set; }

    }

    [Table("User")]
    public class UserEntity
    {
        [Key]
        [Column("UserSqlId")]
        public int UserSqlId { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        public ICollection<CustomEventEntity> CustomEvents { get; set; }
    }

    [SqlServerEntityConfig]
    [PostgresEntityConfig]
    public class CustomEventEntityConfiguration : IEntityTypeConfiguration<CustomEventEntity>
    {
        public void Configure(EntityTypeBuilder<CustomEventEntity> builder)
        {
            builder.HasOne(d => d.User)
                .WithMany(p => p.CustomEvents)
                .HasForeignKey(d => d.UserSqlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReadEvent_Users");
        }
    }

}
