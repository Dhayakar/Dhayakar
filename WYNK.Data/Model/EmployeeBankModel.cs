using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class EmployeeBank
    {
        [Key]


        public int ID { get; set; }
        public int CmpID { get; set; }
        public int EmpID { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountHoldersName { get; set; }
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string IFSCCode { get; set; }

    }
}