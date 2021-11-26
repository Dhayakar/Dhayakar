using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PatientHistoryViewModel
    {
          public ICollection<AllergyTran> AllergyTran { get; set; }
        public ICollection<getallergyDetails> getallergyDetails { get; set; }
        public ICollection<getPACExamhistoryDetailsbh> getPACExamhistoryDetailsbh { get; set; }
        public ICollection<getbhDetails> getbhDetails { get; set; }
        public PACExamination PACExamination { get; set; }
        public ICollection<AllergyTranrecent> AllergyTranrecent { get; set; }
        public ICollection<AllergyTranhistory> AllergyTranhistory { get; set; }
        public ICollection<PatientHistoryWithAction> listofHistory { get; set; }
        public PatientGeneralDatamodel Patientgeneral { get; set; }
        public RegistrationTran_Master Tran_Master { get; set; }
        public BirthHistory BirthHistory { get; set; }
        public ICollection<Oculartotalhistory> Oculartotalhistory { get; set; }
        public ICollection<systematictotalhistory> systematictotalhistory { get; set; }
        public ICollection<CurrentMedicationtotalhistory> CurrentMedicationtotalhistory { get; set; }
        public ICollection<Allergytotalhistory> Allergytotalhistory { get; set; }
        public ICollection<FamilyHistorytotalhistory> FamilyHistorytotalhistory { get; set; }
        public ICollection<history> history { get; set; }
        public ICollection<Totalhistory> Totalhistory { get; set; }
        public ICollection<ITotalhistory> ITotalhistory { get; set; }
        public ICollection<Idatyes> Idatyes { get; set; }
        public ICollection<MedicalPrescriptionhistory> MedicalPrescriptionhistory { get; set; }
        public ICollection<Medicalhistory> Medicalhistory { get; set; }
        public ICollection<InvestigationPrescriptionhistory> InvestigationPrescriptionhistory { get; set; }
        public ICollection<Investigationhistory> Investigationhistory { get; set; }
        public ICollection<refractiondetails1> refractiondetails1 { get; set; }
        public ICollection<refractionHistorycv> refractionHistorycv { get; set; }
        public ICollection<refractionHistoryiop> refractionHistoryiop { get; set; }
        public ICollection<refractionVisualacuity> refractionVisualacuity { get; set; }
        public ICollection<refractionHistoryrefraction> refractionHistoryrefraction { get; set; }
        public ICollection<refractionHisacceptance> refractionHisacceptance { get; set; }
        public ICollection<OpticalpreHisfnalpre> OpticalpreHisfnalpre { get; set; }
        public ICollection<refractionHisamsler> refractionHisamsler { get; set; }
        public ICollection<SquintTraninfo1> SquintTraninfo1 { get; set; }
        public ICollection<refractiondetails> refractiondetails { get; set; }
        public ICollection<opticalhistory1> opticalhistory1 { get; set; }
        public ICollection<getPACExamDetails> getPACExamDetails { get; set; }
        public ICollection<getPACExamhistoryDetails> getPACExamhistoryDetails { get; set; }
        public ICollection<FDDTDescriptionDetail> FDDTDescriptionDetails { get; set; }
        public ICollection<FDDTDescriptionDetail> SYRINGINGDescriptionDetails { get; set; }
        public string MPNO { get; set; }
        public string imgdt { get; set; }
        public string USERID { get; set; }
    }







    public class getPACExamDetails
    {

        public int PACExamID { get; set; }
        public string UIN { get; set; }
        public decimal PulseHeartRate { get; set; }
        public decimal Respiration { get; set; }
        public string BloodPressure { get; set; }
        public decimal temperature { get; set; }
        public string tempunit { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string Weightunit { get; set; }
        public string Heightunit { get; set; }
        public decimal BMI { get; set; }


    }

    public class getbhDetails
    {
        public string delivery { get; set; }
        public decimal weight { get; set; }
        public string transplant { get; set; }
        public string therapy { get; set; }
        public string illness { get; set; }
        public string vac { get; set; }
        public string gad { get; set; }
        public string unit { get; set; }


    }
    public class getPACExamhistoryDetailsbh
    {

        public string delivery { get; set; }
        public decimal weight { get; set; }
        public string transplant { get; set; }
        public string therapy { get; set; }
        public string illness { get; set; }
        public DateTime Dates { get; set; }
        public string vac { get; set; }
        public string gad { get; set; }
        public string unit { get; set; }


    }

    public class getallergyDetails
    {

        public DateTime date { get; set; }
        public string desc { get; set; }
        

    }
    public class getPACExamhistoryDetails
    {

        public string UIN { get; set; }
        public decimal PulseHeartRate { get; set; }
        public decimal Respiration { get; set; }
        public string BloodPressure { get; set; }
        public string temperature { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public decimal BMI { get; set; }
        public DateTime Dates { get; set; }


    }
    public class AllergyTranrecent
    {
        public int ID { get; set; }
        public int RegID { get; set; }
        public string UIN { get; set; }
        public int Description { get; set; }
        public string Descriptionname { get; set; }
        public int Type { get; set; }
        public string Typename { get; set; }
        public DateTime FromUTC { get; set; }
        public Boolean IsActive { get; set; }
        public int Since { get; set; }
        public string Period { get; set; }
        public int CmpID { get; set; }


    }

    public class AllergyTranhistory
    {
        public string UIN { get; set; }
        public string Descriptionname { get; set; }
        public string Typename { get; set; }
        public DateTime FromUTC { get; set; }

    }

    public class opticalhistory1
    {
        public string Prescribedby1 { get; set; }
        public DateTime prescribeddate1 { get; set; }
        public int? regid1 { get; set; }
    }

    public class CurrentMedication
    {
        public string GenericDescription { get; set; }
        public string Eye { get; set; }
        public string Frequency { get; set; }
        public int FrequencyID { get; set; }
        public string Since { get; set; }
        public string YearMonths { get; set; }
        public string PrescribedBy { get; set; }
        public string PrescribedDoctor { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public Boolean? FromDB { get; set; }
        public Boolean? Disabled { get; set; }
    }
    public class PatientCurrentMed
    {
        public ICollection<CurrentMedication> CurrentMedications { get; set; }
    }
    public class refractiondetails1
    {
        public string Description { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string DistSph1 { get; set; }
        public string NearCyl1 { get; set; }
        public string PinAxis1 { get; set; }
        public string Add1 { get; set; }
        public string Details { get; set; }

        public DateTime Date { get; set; }
    }



    public class refractionHistorycv
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public string CV_normal { get; set; }
        public string CV_defective { get; set; }
        public string Desc_Text { get; set; }
        public string CV_normal1 { get; set; }
        public string CV_defective1 { get; set; }
        public string Desc_Text1 { get; set; }
    }
    public class refractionHistoryiop
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Iolnct { get; set; }
        public string Iolat { get; set; }
        public string Iolnct1 { get; set; }
        public string Iolat1 { get; set; }
        public string Time { get; set; }
    }
    public class refractionVisualacuity
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public string SubCategory { get; set; }
        public string Ocular { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PowerGlass { get; set; }
        public string N_V_DESC { get; set; }
        public string PinAxis { get; set; }
        public string DistSphh { get; set; }
        public string NearCyll { get; set; }
        public string PowerGlasss { get; set; }
        public string N_V_DESCC { get; set; }
        public string PinAxiss { get; set; }
    }
    public class refractionHistoryrefraction
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public string SubCategory { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string DistSph1 { get; set; }
        public string NearCyl1 { get; set; }
        public string PinAxis1 { get; set; }
        public string DistSph2 { get; set; }
        public string NearCyl2 { get; set; }
        public string PinAxis2 { get; set; }
        public string DistSph3 { get; set; }
        public string NearCyl3 { get; set; }
        public string PinAxis3 { get; set; }


    }
    public class refractionHisacceptance
    {
        public string Description { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string DistSph1 { get; set; }
        public string NearCyl1 { get; set; }
        public string PinAxis1 { get; set; }
        public string Add1 { get; set; }
        public string DistSph2 { get; set; }
        public string Add2 { get; set; }
        public string DistSph3 { get; set; }
        public string Add3 { get; set; }
        public string Remarks { get; set; }
        public DateTime Date { get; set; }

        public string PD { get; set; }


    }
    public class refractionHisamsler
    {
        public string Description { get; set; }
        public Boolean A_n_OD { get; set; }
        public Boolean A_abn_OD { get; set; }
        public Boolean A_n_OS { get; set; }
        public Boolean A_abn_OS { get; set; }
        public string Desc_Text { get; set; }
        public string Desc_Text1 { get; set; }
        public DateTime Date { get; set; }


    }
    public class SquintTraninfo1
    {
        public Boolean IsDVOD { get; set; }
        public Boolean IsDVOS { get; set; }
        public Boolean IsDVOU { get; set; }
        public Boolean IsNVOD { get; set; }
        public Boolean IsNVOS { get; set; }
        public Boolean IsNVOU { get; set; }
        public DateTime Date { get; set; }
        public string SquintTypename { get; set; }
        public int SquintType { get; set; }
        public string SquintDiagnosisDescription { get; set; }
        public string Squintname { get; set; }
        public int ICDSpecID { get; set; }
        public int SquintID { get; set; }
        public int RefractionID { get; set; }
        public int ID { get; set; }

    }

    public class OpticalpreHisfnalpre
    {
        public string Description { get; set; }
        public string DistSph { get; set; }
        public string NearCyl { get; set; }
        public string PinAxis { get; set; }
        public string Add { get; set; }
        public string DistSph1 { get; set; }
        public string NearCyl1 { get; set; }
        public string PinAxis1 { get; set; }
        public string Add1 { get; set; }
        public string DistSph2 { get; set; }
        public string Add2 { get; set; }
        public string DistSph3 { get; set; }
        public string Add3 { get; set; }
        public string Remarks { get; set; }

        public string PD { get; set; }
        public string MPD { get; set; }
        public string MPD1 { get; set; }
        public DateTime Date { get; set; }


    }

    public class refractiondetails
    {
        public DateTime visitDate { get; set; }
        public int Rid { get; set; }
    }

    public class ListCompanyIds
    {
        public int CompanyIDS { get; set; }
    }











    public class Investigationhistory
    {
        public DateTime visistdate { get; set; }
        public string prescribedby { get; set; }
        public decimal invamount { get; set; }
        public string Invdesc { get; set; }
        public int ID { get; set; }
        public string exterinternal { get; set; }
    }


    public class InvestigationPrescriptionhistory
    {
        public DateTime date { get; set; }
        public string Prescribedby { get; set; }
        public decimal Amount { get; set; }
        public string Investigationdesc { get; set; }
    }


    public class Medicalhistory
    {
        public string Drugname { get; set; }
        public string Generic { get; set; }
        public string UOM { get; set; }
        public string Frequency { get; set; }
        public short Days { get; set; }
        public int? Quantity { get; set; }
        public string Eye { get; set; }
        public string Food { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Remarks { get; set; }
    }

    public class MedicalPrescriptionhistory
    {
        public DateTime Visitdate { get; set; }
        public DateTime PrescribeDate { get; set; }
        public string PrescribedBY { get; set; }
        public string MedicalPrescriptionNo { get; set; }
    }
    public class Idatyes
    {
        public DateTime FromDate { get; set; }
    }
    public class ITotalhistory
    {
        public string OcularDescription { get; set; }
        public string systematicDescription { get; set; }
        public string currentDescription { get; set; }
        public string allergyDescription { get; set; }
        public string familyhistoryDescription { get; set; }
        public DateTime FromDate { get; set; }
        public string Duration { get; set; }
        public DateTime? SufferedFromDate { get; set; }


    }
    public class Totalhistory
    {
        public string OcularDescription { get; set; }
        public string systematicDescription { get; set; }
        public string currentDescription { get; set; }
        public string allergyDescription { get; set; }
        public string familyhistoryDescription { get; set; }
        public DateTime FromDate { get; set; }
        public string Duration { get; set; }
        public DateTime SufferedFromDate { get; set; }


    }

    public class history
    {
        public string OcularDescription { get; set; }
        public string systematicDescription { get; set; }
        public string currentDescription { get; set; }
        public string allergyDescription { get; set; }
        public string familyhistoryDescription { get; set; }
        public DateTime FromDate { get; set; }
        public string Duration { get; set; }
        public DateTime SufferedFromDate { get; set; }

    }

    public class FamilyHistorytotalhistory
    {
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
    }
    public class Allergytotalhistory
    {
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
    }

    public class CurrentMedicationtotalhistory
    {
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
    }

    public class systematictotalhistory
    {
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime SufferFromDate { get; set; }
        public string Duration { get; set; }
    }

    public class Oculartotalhistory
    {
        public string Description { get; set; }
        public DateTime FromDate { get; set; }

    }

    public class PatientHistoryWithAction
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public string TotalMonths { get; set; }
        public string Actions { get; set; }
        public string UIN { get; set; }
    }


    public class SurgeryHistoryViewModel
    {
        public IList<SurgeryHistoryDetail> SurgeryHistoryDetails { get; set; }
        public List<ListCompanyIds> ListCmpid { get; set; }
        public int CreatedBy { get; set; }
        public int Cmpid { get; set; }
        public string RemovedReasons { get; set; }
    }


    public class SurgeryHistoryDetail
    {
        public DateTime? DateOfSurgery { get; set; }
        public string TypeofSurgery { get; set; }
        public string SurgeonName { get; set; }
        public string HospitalClinic { get; set; }
        public string Eye { get; set; }
        public string SurgeryDone { get; set; }
        public string Status { get; set; }
        public int Cmpid { get; set; }
        public string UIN { get; set; }
        public string RemovedReason { get; set; }
        public string Remarks { get; set; }
    }

}
