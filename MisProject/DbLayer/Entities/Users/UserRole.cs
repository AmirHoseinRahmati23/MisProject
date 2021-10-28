using System.ComponentModel.DataAnnotations;

namespace DbLayer.Entities.Users;

public class UserRole
{
    public UserRole()
    {

    }

    public UserRole(int userId, int roleId) : this()
    {
        UserId = userId;
        RoleId = roleId;
    }

    [Key] public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    #region Relations

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;

    #endregion
}