using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHM.Entities 
{
    [Table("Users")]
    public class User : Entity
    {
        [MaxLength(1000)]
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }
        [MaxLength(20)]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string SpouseName { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [MaxLength(1000)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Mobile")]
        public string MobileNumber { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Emergency Contact Number")]
        public string EmergencyContNumber { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Phone Extension")]
        public string PhoneExt { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Presen Address")]
        public string PresentAddress { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }
        [MaxLength(1000)]
        public string Gender { get; set; }
        public bool SupUser { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Image Link")]
        public string ImageFile { get; set; }
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual UserRole UserRole { get; set; }
        //[Display(Name = "Department")]
        //public int? DepartmentId { get; set; }
        //[ForeignKey("DepartmentId")]
        //public virtual Department Department { get; set; }
        //[Display(Name = "Designation")]
        //public int? DesignationId { get; set; }
        //[ForeignKey("DesignationId")]
        //public virtual Designation Designation { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoiningDate { get; set; }
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [MaxLength(1000)]
        [DataType(DataType.Password)]
        [Display(Name = "Last Password")]
        public string LastPassword { get; set; }

        [Display(Name = "Last Password Change Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastPassChangeDate { get; set; }

        public int? PasswordChangedCount { get; set; }
        [MaxLength(1000)]
        public virtual string SecurityStamp { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }

        [MaxLength(1000)]
        [Display(Name="User Type")]
        public string UserType { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Employee Type")]
        public string EmployeeType { get; set; }

        public DateTime? ExpireDate { get; set; }

       
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int? AccessFailedCount { get; set; }
      

        public byte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }



    }
}
