using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class EmployeeCommunication
    {
        [Key]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int EmpID { get; set; }
        public string PresentAddress1 { get; set; }
        public string PresentAddress2 { get; set; }
        public decimal PresentLocationID { get; set; }
        public string PresentLandmark { get; set; }
        public string PermanentAddress1 { get; set; }
        public string PermanentAddress2 { get; set; }
        public decimal PermanentLocationID { get; set; }
        public string PermanentLandmark { get; set; }
        public string Phone { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string EmailID { get; set; }
        public string AlternateEmailID { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? UpdatedUTC { get; set; }

        public DateTime CreatedUTC { get; set; }

        
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}


