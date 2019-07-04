using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Entities
{
    [Table("UserRoles")]
    public class UserRole : AuditableEntity
    {
        [MaxLength(1000)]
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public virtual ICollection<UserRolePermission> RolePermissionCollection { get; set; }
        public virtual ICollection<User> UserCollection { get; set; }
    }
}
