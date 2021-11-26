using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
   public  class ManagementMaster
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int NewPatients { get; set; }
        public int ReviewPatients { get; set; }
        public decimal? ConsultationNew { get; set; }
        public decimal? ConsultationReview { get; set; }

        //public int paediatricNewPatient { get; set; }
        //public int paediatricReviewpatient { get; set; }
        //public int OcularNewPatient { get; set; }
        //public int OcularReviewPatient { get; set; }


        public int? NewGeneralNormal { get; set; }
        public int? NewGeneralOcular { get; set; }
        public int? NewPediatricNormal { get; set; }
        public int? NewPediatricOcular { get; set; }


        public int? ReviewGeneralNormal { get; set; }
        public int? ReviewGeneralOcular { get; set; }
        public int? ReviewPediatricNormal { get; set; }
        public int? ReviewPediatricOcular { get; set; }




        public int SurgeryReviewPatient { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }


    }
}

  