using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WYNK.Data.Model
{
   public class Users
    {
        [Key]
        public int Userid { get; set; }
        public string Usertype { get; set; }
        public int CMPID { get; set; }
       // public int Roleid { get; set; }
        public string ReferenceTag { get; set; }
        public int? ReferenceID { get; set; }
        //public string Employeename { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Useraccess { get; set; }
        public string Emailid { get; set; }
        public bool Isactive { get; set; }
        public DateTime? Inactivedate { get; set; }
        public string Reasons { get; set; }
      //  public bool Isdeleted { get; set; }
        public DateTime? Createdutc { get; set; }
        public DateTime ?Updatedutc { get; set; }
        public int Createdby { get; set; }
        public int ?Updatedby { get; set; }






    }
}
