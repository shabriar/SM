using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Entities
{
    [Table("UserRolePermissions")]
    public class UserRolePermission : Entity
    {
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual UserRole Role { get; set; }
        [MaxLength(1000)]
        public string Permission { get; set; }
    }
}
