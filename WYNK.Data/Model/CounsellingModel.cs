using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
   public class CounsellingModel
    {
        [Key]

        public int ID { get; set; }
        public int CmpID { get; set; }
        public int TCID { get; set; }
        public DateTime? Schedule { get; set; }
        public string UIN { get; set; }
        public string LensID { get; set; }
        //public int SurgeonID { get; set; }
        //public int AnaesthetistID { get; set; }
        public string Progonosis { get; set; }
        public string OtherRequirements { get; set; }
        public string Reasons { get; set; }
        public DateTime? DateOfConfirmation { get; set; }
        //public string PatientType { get; set; }
        public DateTime? DateOfArrival { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public decimal PaymentDetails { get; set; }
        public int? RoomID { get; set; }

        public DateTime? UpdatedUTC { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
