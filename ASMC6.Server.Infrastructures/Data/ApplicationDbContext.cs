using ASMC6.Shared.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics.CodeAnalysis;

namespace ASMC6.Server.Infrastructures.Data;

public class ApplicationDbContext : IdentityDbContext<Users, Roles, Guid>
{
    public ApplicationDbContext([NotNull] DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Roles>().HasData(new Roles()
        {
            Id = Guid.Parse("ab251560-a455-40fd-adfd-54f9e150f874"),
            Name = "Administrator",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = "Administrator"
        }, new Roles()
        {
            Id = Guid.Parse("8d4b836e-d9fa-4fa9-88c0-9a875d2b7d5c"),
            Name = "Employee",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = "Employee"
        }, new Roles()
        {
            Id = Guid.Parse("f91ec0e5-d768-42e2-8926-de7d3162430f"),
            Name = "Customer",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = "Customer"
        });



        builder.Entity<Users>().HasData(new Users()
        {
            Id = Guid.Parse("fb1eab16-920e-4480-b3ee-01f6e9c15ab5"),
            UserName = "vankieu0409@gmail.com",
            NormalizedUserName = "vankieiu0409@gmail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "vankieu0409@gmail.com",
            EmailConfirmed = true,
            NormalizedEmail = "vankieu0409@gmail.com",
            PhoneNumber = "",
            PhoneNumberConfirmed = false,
            LockoutEnabled = false,
            LockoutEnd = DateTimeOffset.MinValue,
            AccessFailedCount = 0,
            IsDeleted = false,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            TwoFactorEnabled = false,
            Image = "https://media-cdn-v2.laodong.vn/Storage/NewsPortal/2022/8/18/1082204/Leesuk.jpg",
            Decriptions = "",
            DisplayName = "Bậu"
        }, new Users()
        {
            Id = Guid.Parse("6aa93c41-f21f-44e3-8f46-7d76b03574c5"),
            UserName = "kieunvph14806@fpt.edu.vn",
            NormalizedUserName = "kieunvph14806@fpt.edu.vn",
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = "kieunvph14806@fpt.edu.vn",
            EmailConfirmed = true,
            NormalizedEmail = "kieunvph14806@fpt.edu.vn",
            PhoneNumber = "",
            PhoneNumberConfirmed = false,
            LockoutEnabled = false,
            LockoutEnd = DateTimeOffset.MinValue,
            AccessFailedCount = 0,
            IsDeleted = false,
            Image = "https://media-cdn-v2.laodong.vn/Storage/NewsPortal/2022/8/18/1082204/Leesuk.jpg",
            Decriptions = " Chị rất nóng tính",
            DisplayName = "Chị Nhà Cục Súc",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            TwoFactorEnabled = false
        });
        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>()
        {
            UserId = Guid.Parse("fb1eab16-920e-4480-b3ee-01f6e9c15ab5"),
            RoleId = Guid.Parse("ab251560-a455-40fd-adfd-54f9e150f874")
        }, new IdentityUserRole<Guid>()
        {
            UserId = Guid.Parse("fb1eab16-920e-4480-b3ee-01f6e9c15ab5"),
            RoleId = Guid.Parse("8d4b836e-d9fa-4fa9-88c0-9a875d2b7d5c")
        }, new IdentityUserRole<Guid>()
        {
            UserId = Guid.Parse("6aa93c41-f21f-44e3-8f46-7d76b03574c5"),
            RoleId = Guid.Parse("f91ec0e5-d768-42e2-8926-de7d3162430f"),
        });
    }
}