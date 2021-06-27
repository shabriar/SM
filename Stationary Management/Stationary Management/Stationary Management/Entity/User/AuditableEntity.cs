using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Entities
{
    public abstract class AuditableEntity : Entity
    {
        public byte? Status { get; set; }
        [Display(Name = "Created At")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Updated At")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }

        public AuditableEntity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
