using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class User_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public int CMPID { get; set; }
        public string UserType { get; set; }
        public int ?ReferenceID { get; set; }
        public string ReferenceTag { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserAccess { get; set; }
        public string EmailID { get; set; }
        public string Reasons { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime ?InActiveDate { get; set; }
        //public DateTime ?CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }


}
