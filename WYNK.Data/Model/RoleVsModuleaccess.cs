using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class RoleVsModuleaccess
    {
        [Key]

        public Int64 RoleModuleID { get; set; }
        public int ModuleID { get; set; }
        public int UserRoleID { get; set; }
        public int RoleID { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Query { get; set; }
        public bool Print { get; set; }
        public bool All { get; set; }
        public int MenuOrder { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public int ParentModuleID { get; set; }
        public int CmpID { get; set; }


    }


}