using DbLayer.Entities.Permissions;
using DbLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DbLayer.Contexts;

public class MisDbContext : DbContext
{
    public MisDbContext(DbContextOptions<MisDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User

        modelBuilder.Entity<User>(p => { p.HasQueryFilter(u => !u.IsDeleted); });

        modelBuilder.Entity<Role>(p => { p.HasQueryFilter(u => !u.IsDeleted); });

        modelBuilder.Entity<UserRole>(p => { p.HasQueryFilter(u => !u.User.IsDeleted && !u.Role.IsDeleted); });

        #endregion

        #region Permissions

        modelBuilder.Entity<Permission>(p =>
        {
            p.HasData(
                new Permission(1, "پنل مدیریت"),
                //----------------------------------------------------------
                new Permission(2, "مدیریت کاربران", 1),
                new Permission(3, "افزودن کاربر", 2),
                new Permission(4, "ویرایش کاربر", 2),
                new Permission(5, "حذف کاربر", 4),
                new Permission(6, "تغییر رمز عبور کاربر", 4),
                //----------------------------------------------------------
                new Permission(7, "مدیریت نقش ها", 1),
                new Permission(8, "افزودن نقش جدید", 7),
                new Permission(9, "ویرایش نقش", 7),
                new Permission(10, "حذف نقش", 9)
            );

            //p.HasMany(u => u.SubPermissions)
            //    .WithOne(u => u.ParentPermission)
            //    .HasForeignKey(u => u.ParentPermissionId);

            p.HasOne(x => x.ParentPermission)
                .WithMany(u => u.SubPermissions)
                .HasForeignKey(u => u.ParentPermissionId);
        });

        modelBuilder.Entity<RolePermission>(p => { p.HasQueryFilter(u => !u.Role.IsDeleted); });

        #endregion
    }
}