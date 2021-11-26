using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{




    public class SURGDETAILS
    {
        public ICollection<Surgeondetails> surgeondetails { get; set; }
        public ICollection<Anesthetistdetails> anesthetistdetails { get; set; }

    }




    public class savingCounsellingdetails
    {
        public ICollection<ChecklistDetails> Checklistdetailsss { get; set; }
        public ICollection<PaymentdetailsAdvance> PaymentdetailsAdvance { get; set; }
        public PatientCounsellingDetails PatientCounsellingDtls { get; set; }
        //public ICollection<Surgeondetails> Surgeondetail { get; set; }
        public ICollection<Mergeddetails> Mergedetail { get; set; }
        public string compnayid { get; set; }
        public string Userid { get; set; }
        public string Transactionid { get; set; }
        public string Reasons { get; set; }
        public string UINSS { get; set; }

    }

    public class Counselling_Master
    {
        public string cmed { get; set; }
        public string all { get; set; }
        public string fhis { get; set; }
        public ICollection<PatientHistoryDetailss> PatientHistorys { get; set; }
        public ICollection<ComplaintsDetailss> ComplaintsDetails { get; set; }
        public string[] TOtalLines { get; set; }
        public ICollection<ConslgDetails> ConslgDetails { get; set; }
        public ICollection<SurgeonDoctordetails> SurgeonDoctordetails { get; set; }
        public ICollection<Ane3sthetistDoctordetails> AnesthetistDoctordetails { get; set; }
        public ICollection<Mergeddetails> Mergeddetails { get; set; }
        public ICollection<ConslgDetails> CounsellingchecklistDetails { get; set; }
        public ICollection<UINDETAILS> UINDETAILS { get; set; }
        public ICollection<Lensdetails> Lensdetails { get; set; }
        public ICollection<CounsellingHistorydetails> CounsellingHistorydetails { get; set; }
        public CounsellingChecklist Cps { get; set; }
        public PatientCounsellingDetails PatientCounsellingDtls { get; set; }
        public ICollection<PaymentdetailsAdvance> PaymentdetailsAdvance { get; set; }
        public ICollection<PatientAssignStatus> PatientAssignStatus { get; set; }
        public ICollection<Textdetails> Textdetails { get; set; }
        public string ErrorMessaGE { get; set; }
        //public ICollection<PatientCounsellingDetails> PatientCounsellingDtls { get; set; }

    }

    public class Mergeddetails
    {
        public string Doctornames { get; set; }
        public string Designation { get; set; }
    }
    public class Textdetails
    {
        public string Textname { get; set; }
        public DateTime Datetime { get; set; }
    }

    public class SurgeonDoctordetails
    {
        public string Doctornames { get; set; }
        public string Designation { get; set; }
    }

    public class Ane3sthetistDoctordetails
    {
        public string Doctornames { get; set; }
        public string Designation { get; set; }
    }


    public class Surgeondetails
    {
        public string IDS { get; set; }
    }
    public class Anesthetistdetails
    {
        public string IDS { get; set; }
    }
    public class ChecklistDetails
    {
        public string ItemDescription { get; set; }
        public Boolean? Select { get; set; }

    }

    public class PatientCounsellingDetails
    {

        public DateTime? Scheduleddate { get; set; }
        public DateTime? Fromtime { get; set; }
        public DateTime? ToTime { get; set; }
        //public string Surgeon { get; set; }
        //public string Anesthiest { get; set; }
        public string RoomID { get; set; }
        public string Patienttype { get; set; }
        public string Typeoflens { get; set; }
        public string Pgognosis { get; set; }
        public string Roomstatus { get; set; }
        public string Remarks { get; set; }
        public string UIN { get; set; }
    }


    public class PatientHistoryDetailss
    {
        public int? ID { get; set; }
        public string Description { get; set; }
        public DateTime? FromUTC { get; set; }
        public string Duration { get; set; }

    }

    public class ComplaintsDetailss
    {
        public int? ID { get; set; }
        public string Description { get; set; }
        public bool? ISOD { get; set; }
        public bool? ISOS { get; set; }
        public bool? ISOU { get; set; }
    }


    public class Roomdetails
    {
        public string RoomDescription { get; set; }
        public string Roomcost { get; set; }
        public string Noofbeds { get; set; }
        public string NoofRooms { get; set; }
    }


    public class PaymentdetailsAdvance
    {

        public string PaymentFor { get; set; }
        public int? PaymentAgainstID { get; set; }
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime? Expirydate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AdvAmount { get; set; }

    }


    public class CounsellingHistorydetails
    {
        public string Description { get; set; }
        public DateTime AffectedFrom { get; set; }
        public string TotalMonths { get; set; }

    }
    public class Lensdetails
    {
        public string Lensdesc { get; set; }
    }

    public class UINDETAILS
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string UIN { get; set; }
        public string SurgeryAdvicedby { get; set; }
        public string Gender { get; set; }
        public string SpecialityDescriptiobn { get; set; }
        public DateTime? SurgeryAdviceddate { get; set; }
    }
    public class ConslgDetails
    {
        public string Description { get; set; }
        public string Type1 { get; set; }
        public int ID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
    }


}

