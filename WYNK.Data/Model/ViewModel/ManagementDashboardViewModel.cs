using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class ManagementDashboardViewModel
    {
        public ICollection<QuarterlyFromList> QuarterlyList { get; set; }
        public ICollection<QuarterlytoList> QuarterlytoList { get; set; }
        public ICollection<Revenuefortheday> revenuefortheday { get; set; }
        public ICollection<TOTALMERGEDRevenue> TOTALMERGEDRevenue { get; set; }
        public ICollection<RevenuefortheMonth> RevenuefortheMonth { get; set; }
        
        public ICollection<RevenueAMT> RevenueAMT { get; set; }
        public ICollection<Revenuedesc> Revenuedesc { get; set; }
        public ICollection<RevenueAMount> RevenueAMount { get; set; }
        public int newcount {get;set;}
        public int Reviewcount { get; set; }
        public int SurgeryReviewcount { get; set; }
        public int TOnewcount { get; set; }
        public int TOReviewcount { get; set; }
        public int TOSurgeryReviewcount { get; set; }
        public decimal? Totalrevenuesum { get; set; }
        public DateTime RFromdate { get; set; }
        public DateTime RTodate { get; set; }


        public string RevenueFromdate { get; set; }
        public string RevenueTodate { get; set; }
        public string RevenueAllFrom { get; set; }
        public string RevenueAllTo { get; set; }
        public decimal? TotalrevenuesumFortheDay { get; set; }
        public decimal? TotalrevenuesumFoirtheMonth { get; set; }


        public decimal? TotalSurgsumFortheDay { get; set; }
        public decimal? TotalSurgsumFoirtheMonth { get; set; }
        public int? patientcountforday { get; set; }
        public int? patientcountformonth { get; set; }
        public string Monthname { get; set; }

        public ICollection<SurgeryBreakUP> SurgeryBreakUPs { get; set; }
        public ICollection<SurgeryBreakUPTOtalDetails> SurgeryBreakUPTOtalDetailss { get; set; }
        public ICollection<SurgeryBreakUPfromdate> SurgeryBreakUPfromdate { get; set; }
        

        public ICollection<RevenuespecificBreakUPTOtalDetails> RevenuespecificBreakUPTOtalDetailsss { get; set; }
        public ICollection<toRevenuespecificBreakUPTOtalDetails> toRevenuespecificBreakUPTOtalDetailss { get; set; }
        public ICollection<Surgerydoctorbreakupdetails> Surgerydoctorbreakupdetails { get; set; }

        public ICollection<Revenuebreakupdetails> Revenuebreakupdetails { get; set; }
    }


    public class Revenuebreakupdetails
    {
        public string Description { get; set; }
        public string Doctorname { get; set; }
        public DateTime Date { get; set; }
        public decimal? Amount { get; set; }

    }
    public class Surgerydoctorbreakupdetails
    {
        public string fDoctorname { get; set; }
        public string mDoctorname { get; set; }
        public string lDoctorname { get; set; }
        public int? patientcount { get; set; }
        public string specialitydescription { get; set; }
        public string companybranch { get; set; }
        public decimal? amount { get; set; }

    }



    public class toRevenuespecificBreakUPTOtalDetails
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Phone { get; set; }
        public string Specilaitydescription { get; set; }
        public decimal? Specilatyamount { get; set; }

    }


    public class RevenuespecificBreakUPTOtalDetails
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Phone { get; set; }
        public string Specilaitydescription { get; set; }
        public decimal? Specilatyamount { get; set; }

    }


    public class SurgeryBreakUPTOtalDetails
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Phone { get; set; }
        public string Specilaitydescription { get; set; }
        public string ICDdescription { get; set; }

    }
    public class SurgeryBreakUPfromdate
    {
        public string Revenuedesc { get; set; }
        public decimal? CountForday { get; set; }
        public decimal? RevenueAmount { get; set; }
        public int? patientcountforday { get; set; }
        public int? patientcountformonth { get; set; }

    }
    public class SurgeryBreakUP
    {
        public string SpecialityDescription { get; set; }
        public decimal? CountForday { get; set; }
        public decimal? CountForMonth { get; set; }
        public int? patientcountforday { get; set; }
        public int? patientcountformonth { get; set; }

    }


    public class TOTALMERGEDRevenue
    {
        public decimal? NewRevenueAmount { get; set; }
        public decimal? OLDRevenueAmount { get; set; }
        public string Revenuedesc { get; set; }

        public int? serviceid { get; set; }

        

    }

    public class RevenueAMount
    {
        public decimal? RevenueAmount { get; set; }
    }
    public class Revenuedesc
    {
        public string Revenuedescription { get; set; }
    }
    public class Revenuefortheday
    {
        public decimal? RevenueAmount { get; set; }
        public decimal? RevenueMonthAmount { get; set; }
        public string Revenuedesc { get; set; }
        public int? serviceid { get; set; }
    }
    public class RevenuefortheMonth
    {
        public decimal? RevenueAmount { get; set; }
        public decimal? RevenueMonthAmount { get; set; }
        public string Revenuedesc { get; set; }
        public int? serviceid { get; set; }
    }
    public class RevenueAMT
    {
        public decimal? RevenueAmount { get; set; }
    }

    public class QuarterlyFromList
    {
        public string Name { get; set; }
        public string UIN { get; set; }
        public string gender { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string email { get; set; }
    }
    public class QuarterlytoList
    {
        public string Name { get; set; }
        public string UIN { get; set; }
        public string gender { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string email { get; set; }
    }
}
