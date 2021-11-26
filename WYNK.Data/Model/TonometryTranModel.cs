using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class TonometryTran
    {
        [Key]
        public int ID { get; set; }
        //public string FindingsID { get; set; }
        public int CmpID { get; set; }
        public int RegistrationtranID { get; set; }
        public string UIN { get; set; }
        public string VisitDatetime { get; set; }
        public int TonometryID { get; set; }
        public string BOD { get; set; }
        public string BOS { get; set; }
        public string AOD { get; set; }
        public string AOS { get; set; }
        public int Dilation { get; set; }
        public string Time { get; set; }
        public int StaffID { get; set; }
        public bool IsDeleted { get; set; }
        public string RemovedReasons { get; set; }
        public int RemovedBy { get; set; }
        public DateTime? RemovedDatetime { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Dropsname { get; set; }

    }
}
