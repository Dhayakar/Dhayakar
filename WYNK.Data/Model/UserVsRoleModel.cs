using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class User_Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int CMPID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; } 
        public string RoleDescription { get; set; }
        public DateTime ?UpdatedUTC { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string ReferenceTag { get; set; }
        
    }


}
