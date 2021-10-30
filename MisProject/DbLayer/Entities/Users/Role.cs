﻿using System.ComponentModel.DataAnnotations;
using DbLayer.Entities.Permissions;

namespace DbLayer.Entities.Users;

public class Role
{
    public Role()
    {
    }

    public Role(string roleName) : this()
    {
        RoleName = roleName;
    }

    [Key] public int RoleId { get; set; }

    [Display(Name = "مقام")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    public string RoleName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    #region Relations

    public ICollection<UserRole> UserRoles { get; set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; set; } = null!;

    #endregion
}