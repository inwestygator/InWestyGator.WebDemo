using InWestyGator.WebDemo.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InWestyGator.WebDemo.DataAccess.Configurations
{
    public class PackConfiguration : IEntityTypeConfiguration<Pack>
    {
        public void Configure(EntityTypeBuilder<Pack> builder)
        {
            // Primary key
            builder.HasKey(p => p.Number);

            // Set the required fields
            builder.Property(p => p.Id)
                .IsRequired();
            builder.Property(p => p.PackName)
                .IsRequired();
            builder.Property(p => p.Active)
                .IsRequired();
            builder.Property(p => p.Price)
                .IsRequired();

            // Handling the self-referencing relationship
            builder.HasOne(p => p.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(c => c.ParentNumber)
                .OnDelete(DeleteBehavior.Restrict);

            // Ignore the ChildPackIds collection for EF Core mapping
            builder.Ignore(p => p.ChildPackIds);
            builder.Ignore(p => p.ParentPackId);
        }
    }
}
