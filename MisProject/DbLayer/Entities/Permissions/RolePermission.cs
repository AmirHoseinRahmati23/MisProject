using System.ComponentModel.DataAnnotations;
using DbLayer.Entities.Users;

namespace DbLayer.Entities.Permissions;

public class RolePermission
{
    public RolePermission()
    {

    }

    public RolePermission(int roleId, int permissionId) : this()
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    [Key] public int RolePermissionId { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    #region Relations

    public Permission Permission { get; set; }
    public Role Role { get; set; }

    #endregion
}