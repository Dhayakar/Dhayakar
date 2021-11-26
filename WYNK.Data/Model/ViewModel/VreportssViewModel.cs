using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Vreportss
    {
        public ICollection<Mreportss2> Mreportss2 { get; set; }
        public ICollection<Mreportssbranch> Mreportssbranch { get; set; }
        
        public ICollection<Mreportsdetails> Mreportsdetailss { get; set; }
        public ICollection<UINdetails> UINdetailss { get; set; }
        
        public string Date { get; set; }


        public string FromdateDate { get; set; }
        public string TodateDate { get; set; }
        public decimal? totalReveuewAmountwhole { get; set; }

        public decimal? NewReveuewAmountwhole { get; set; }

        public decimal? RevenueReveuewAmountwhole { get; set; }

        public int Patientcumulativetotal { get; set; }

        public string COmpanyName { get; set; }







        public int? NewGeneralNormal { get; set; }
        public int? NewGeneralOcular { get; set; }
        public int? NewPediatricNormal { get; set; }
        public int? NewPediatricOcular { get; set; }


        public int? ReviewGeneralNormal { get; set; }
        public int? ReviewGeneralOcular { get; set; }
        public int? ReviewPediatricNormal { get; set; }
        public int? ReviewPediatricOcular { get; set; }


        public int? SReviewGeneralNormal { get; set; }
        public int? SReviewGeneralOcular { get; set; }
        public int? SReviewPediatricNormal { get; set; }
        public int? SReviewPediatricOcular { get; set; }



        public int? TotalNew { get; set; }
        public int? TotalReview { get; set; }
        public int? TotalSurgery { get; set; }

    }

     public class Mreportssbranch
    {
        public DateTime? Datess { get; set; }        
        public string Companybranch { get; set; }

        public string CompanyName { get; set; }
        public int NewPatients { get; set; }
        public int ReviewPatients { get; set; }
        public int Surgeryreviewpatients { get; set; }



        public decimal? RevenueNewPatients { get; set; }
        public decimal? revenueReviewPatients { get; set; }
        public decimal? RevenueCumulativetotal { get; set; }
        
        public int Cumulativetotal { get; set; }

        public int? NewGeneralNormal { get; set; }
        public int? NewGeneralOcular { get; set; }
        public int? NewPediatricNormal { get; set; }
        public int? NewPediatricOcular { get; set; }


        public int? ReviewGeneralNormal { get; set; }
        public int? ReviewGeneralOcular { get; set; }
        public int? ReviewPediatricNormal { get; set; }
        public int? ReviewPediatricOcular { get; set; }


        public int? SReviewGeneralNormal { get; set; }
        public int? SReviewGeneralOcular { get; set; }
        public int? SReviewPediatricNormal { get; set; }
        public int? SReviewPediatricOcular { get; set; }

        

    }


    public class Mreportss2
    {
        public DateTime? Datess { get; set; }
        public int NewPatients { get; set; }
        public int ReviewPatients { get; set; }
        public int Surgeryreviewpatients { get; set; }



        public decimal? RevenueNewPatients { get; set; }
        public decimal? revenueReviewPatients { get; set; }
        public decimal? RevenueCumulativetotal { get; set; }
        
        public int Cumulativetotal { get; set; }

        public int? NewGeneralNormal { get; set; }
        public int? NewGeneralOcular { get; set; }
        public int? NewPediatricNormal { get; set; }
        public int? NewPediatricOcular { get; set; }


        public int? ReviewGeneralNormal { get; set; }
        public int? ReviewGeneralOcular { get; set; }
        public int? ReviewPediatricNormal { get; set; }
        public int? ReviewPediatricOcular { get; set; }


        public int? SReviewGeneralNormal { get; set; }
        public int? SReviewGeneralOcular { get; set; }
        public int? SReviewPediatricNormal { get; set; }
        public int? SReviewPediatricOcular { get; set; }

        public string Maindates { get; set; }

        

    }

    public class UINdetails
    {
        public string UIN { get; set; }
        public string DB { get; set; }

    }
        public class Mreportsdetails
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PatientType { get; set; }
        public string EYE { get; set; }

        public decimal? ConsultationAmount { get; set; }


    }
}
