using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Phone).HasMaxLength(20);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Owned entity for Name
        builder.OwnsOne(u => u.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Firstname).HasColumnName("FirstName").HasMaxLength(100);
            nameBuilder.Property(n => n.Lastname).HasColumnName("LastName").HasMaxLength(100);
        });

        // Owned entity for Address
        builder.OwnsOne(u => u.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
            addressBuilder.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
            addressBuilder.Property(a => a.Number).HasColumnName("Number");
            addressBuilder.Property(a => a.Zipcode).HasColumnName("Zipcode").HasMaxLength(20);
            
            // Owned entity for Geolocation within Address
            addressBuilder.OwnsOne(a => a.Geolocation, geoBuilder =>
            {
                geoBuilder.Property(g => g.Lat).HasColumnName("Latitude").HasMaxLength(50);
                geoBuilder.Property(g => g.Long).HasColumnName("Longitude").HasMaxLength(50);
            });
        });
    }
}
