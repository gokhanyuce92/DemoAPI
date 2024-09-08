using Demo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MyDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<Calisan> Calisanlar { get; set; }
    public DbSet<ControllerActionRole> ControllerActionRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        });
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
        });

        modelBuilder.Entity<Calisan>().HasKey(x => x.Id);

        modelBuilder.Entity<ControllerActionRole>()
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}