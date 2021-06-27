using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Entities
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public Guid AuditLogId { get; set; }
        public string EventType { get; set; }
        [Required]
        public string TableName { get; set; }
      
        public string PrimaryKeyName { get; set; }
        public string PrimaryKeyValue { get; set; }
     
        public string ColumnName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public int? CreatedUser { get; set; }
        [ForeignKey("CreatedUser")]
        public virtual User User { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
