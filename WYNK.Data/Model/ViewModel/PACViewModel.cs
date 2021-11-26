using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PACViewM
    {
        public PACHistoryModel PACHistoryModel { get; set; }
        public PACHistoryTranModel PACHistoryTranModel { get; set; }
        public PACExamination PACExamination { get; set; }
        public PACInvestigation PACInvestigation { get; set; }
        public PACPreOperativeInstruction PACPreOperativeInstruction { get; set; }
        public ICollection<getPDT> getPDT { get; set; }
        public ICollection<AddPAHDet> AddPAHDet { get; set; }
        public ICollection<pushData> pushData { get; set; }
        public ICollection<getPACHistoryDet> getPACHistoryDet { get; set; }
        public ICollection<getPACHistoryTranDet> getPACHistoryTranDet { get; set; }
        public ICollection<getPACExamDet> getPACExamDet { get; set; }
        public ICollection<getPACInv> getPACInv { get; set; }
        public ICollection<getPACPreOperative> getPACPreOperative { get; set; }
        public ICollection<getBMI> getBMI { get; set; }
        public ICollection<Beforeinductionoana> Beforeinductionoana { get; set; }
        public ICollection<SscResponse> SscResponse { get; set; }
        public int Cmpid { get; set; }
        public int SAID { get; set; }
        public int CreatedBy { get; set; }
        public int DoctorID { get; set; }
        public ICollection<Beforeinductionoanaprint> Beforeinductionoanaprint { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string phone { get; set; }
        public string web { get; set; }
        public string Compnayname { get; set; }
        public SscResponseTran SscResponseTran { get; set; }
        public ICollection<getPACHistoryDetprint> getPACHistoryDetprint { get; set; }
        public ICollection<getPACHistoryTranDetprint> getPACHistoryTranDetprint { get; set; }
        public ICollection<getPACPreOperativeprint> getPACPreOperativeprint { get; set; }
        public ICollection<getPACExamDetprint> getPACExamDetprint { get; set; }
        public ICollection<getPACInvprint> getPACInvprint { get; set; }


    }







    public class Beforeinductionoanaprint
    {
        public string Description { get; set; }
        public string Question { get; set; }
        public string Questiontowhom { get; set; }
        public string Yesprint { get; set; }
        public string Noprint { get; set; }
        public string NAprint { get; set; }
        public DateTime CreateUTC { get; set; }
        public string Doctor { get; set; }

    }
    public class Beforeinductionoana
    {
        public int SSCID { get; set; }
        public string Description { get; set; }
        public string Question { get; set; }
        public string Questiontowhom { get; set; }
        public Boolean Yes { get; set; }
        public Boolean No { get; set; }
        public Boolean NA { get; set; }
        public Boolean chkYes { get; set; }
        public Boolean chkNo { get; set; }
        public Boolean chkNA { get; set; }
        public Boolean checkStatusYes { get; set; }
        public Boolean checkStatusNo { get; set; }
        public Boolean checkStatusNA { get; set; }
        public int SSCRID { get; set; }
        public Boolean Yess { get; set; }
        public Boolean Noo { get; set; }
        public Boolean NAA { get; set; }
        public int OLMID { get; set; }
        public int ID { get; set; }


    }
    public class getPACPreOperative
    {
        public int PACPreOperativeID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public string NilOralyAfterTime { get; set; }
        public DateTime? NilOralyAfterDate { get; set; }
        public string ArrangeFor { get; set; }
        public string BloodProduct { get; set; }
        public string ShiftedOTTime { get; set; }
        public DateTime? ShiftedOTDate { get; set; }
        public string SpecialInstructions { get; set; }
        public string PreMedications { get; set; }
        public string DoctorID { get; set; }
    }
    public class getBMI
    {
        public string BMICharacter { get; set; }
        public int BMIID { get; set; }

    }
    public class getPDT
    {
        public string GenericName { get; set; }
        public string DrugName { get; set; }
        public int DrugID { get; set; }
        public int GenericID { get; set; }

        public string Dosage { get; set; }
        public string frequency { get; set; }

    }
    public class AddPAHDet
    {
        public string PACHDosage { get; set; }
        public string PACHFrequency { get; set; }
        public string PACHGenericName { get; set; }
        public string PACHDrugName { get; set; }
        public string PACHDesc { get; set; }
    }
    public class pushData
    {
        public string DrugName { get; set; }
        public string Dosage { get; set; }
        public string DrugDescription { get; set; }
        public string frequency { get; set; }
        public int DrugID { get; set; }
    }
    public class getPACHistoryDet
    {
        public int PACHistoryID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public bool Previous_Operation { get; set; }
        public bool ColdCoughFever { get; set; }
        public bool Orthopnoea_PND { get; set; }
        public bool CPD_Asthma { get; set; }
        public bool Unconsciousness { get; set; }
        public bool Psychiatric_illness { get; set; }
        public bool PalpitationsBreathless { get; set; }
        public bool Renal_Disorders { get; set; }
        public bool Jaundice { get; set; }
        public bool HTN_IHD_CHD_DM { get; set; }
        public bool Bleeding_Disorders { get; set; }
        public string Exercise_Tolerance { get; set; }
        public string Family_Ho { get; set; }
        public string Menstrual { get; set; }
        public string Last_Meal_Time { get; set; }
        public bool Smoking_Tobacco { get; set; }
        public bool Alcohol_habituation { get; set; }
        public string Drug_Addictions { get; set; }
        public string Remarks { get; set; }
        public string DoctorID { get; set; }
    }
    public class getPACHistoryTranDet
    {
        public int PACHistoryTranID { get; set; }
        public int PACHistoryID { get; set; }
        public string DrugDescription { get; set; }
        public string DrugName { get; set; }
        public string GenericName { get; set; }
        public string Dosage { get; set; }
        public string frequency { get; set; }
        public int DrugID { get; set; }
    }
    public class getPACExamDet
    {

        public int PACExamID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public decimal PulseHeartRate { get; set; }
        public decimal Respiration { get; set; }
        public string BloodPressure { get; set; }
        public decimal temperature { get; set; }
        public string GC { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal BMI { get; set; }
        public string NStatus { get; set; }
        public bool Pallor { get; set; }
        public bool Icterus { get; set; }
        public bool Cyanosis { get; set; }
        public string Clubbing { get; set; }
        public bool LAP { get; set; }
        public bool Oedema { get; set; }
        public string JVP { get; set; }
        public string Veins { get; set; }
        public string Spine { get; set; }
        public Int16 BHT { get; set; }
        public string TMJoint { get; set; }
        public Int16 MouthOpening { get; set; }
        public bool LooseMissedTeeth { get; set; }
        public string MallampattiClass { get; set; }
        public Int16 Thyromentaldist { get; set; }
        public bool ArtificialTeeth { get; set; }
        public string NasalPatency { get; set; }
        public string OralHygiene { get; set; }
        public string UpperRespiratory { get; set; }
        public string LowerRespiratory { get; set; }
        public string CVS { get; set; }
        public string CNS { get; set; }
        public string PerAbdomen { get; set; }
        public string Neck { get; set; }
        public string DoctorID { get; set; }

    }
    public class getPACInv
    {
        public int PACInvestigationID { get; set; }
        public int CMPID { get; set; }
        public int ADMID { get; set; }
        public string UIN { get; set; }
        public string Hb_PCV { get; set; }
        public decimal PLCount { get; set; }
        public string BLGlu { get; set; }
        public Int16 SUricAcid { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string TSH { get; set; }
        public string SElectNAPlus { get; set; }
        public string SElectKPlus { get; set; }
        public string SElectCAPlus { get; set; }
        public string UrineRMALB { get; set; }
        public string UrineRMSugar { get; set; }
        public string UrineRMKET { get; set; }
        public string UrineRMPlusCells { get; set; }
        public string UrineRMWBC { get; set; }
        public string UrineRMRBC { get; set; }
        public string XRay { get; set; }
        public string ECG { get; set; }
        public string ECHO { get; set; }
        public string ABG { get; set; }
        public string SpecialInvestigation { get; set; }
        public string TLC { get; set; }
        public string BlGroup { get; set; }
        public decimal BUrea { get; set; }
        public decimal Creatinine { get; set; }
        public string HBSAG { get; set; }
        public string HIV { get; set; }

        public string DLC { get; set; }
        public string PT { get; set; }
        public string PTTK { get; set; }
        public string SBil { get; set; }
        public string SGOTSGPT { get; set; }
        public string ALKPOGGT { get; set; }
        public string SProtein { get; set; }

        public string UnfitReasons { get; set; }
        public string Reviewedby1 { get; set; }
        public string Reviewedby2 { get; set; }
        public string Reviewedby3 { get; set; }
        public DateTime? Reviewedon1 { get; set; }
        public DateTime? Reviewedon2 { get; set; }
        public DateTime? Reviewedon3 { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }
        public string PlanGA { get; set; }
        public string DoctorID { get; set; }
    }
    public class getPACHistoryDetprint
    {
        public string Previous_Operation { get; set; }
        public string ColdCoughFever { get; set; }
        public string Orthopnoea_PND { get; set; }
        public string CPD_Asthma { get; set; }
        public string Unconsciousness { get; set; }
        public string Psychiatric_illness { get; set; }
        public string PalpitationsBreathless { get; set; }
        public string Renal_Disorders { get; set; }
        public string Jaundice { get; set; }
        public string HTN_IHD_CHD_DM { get; set; }
        public string Bleeding_Disorders { get; set; }
        public string Exercise_Tolerance { get; set; }
        public string Family_Ho { get; set; }
        public string Menstrual { get; set; }
        public string Last_Meal_Time { get; set; }
        public string Smoking_Tobacco { get; set; }
        public string Alcohol_habituation { get; set; }
        public string Drug_Addictions { get; set; }
        public string Remarks { get; set; }
        public string DoctorID { get; set; }
        public string Knownallergies { get; set; }
        public DateTime Create { get; set; }

    }
    public class getPACHistoryTranDetprint
    {
        public string DrugDescription { get; set; }
        public string DrugName { get; set; }
        public string GenericName { get; set; }
        public string Dosage { get; set; }
        public string frequency { get; set; }
    }
    public class getPACPreOperativeprint
    {
        public string NilOralyAfterTime { get; set; }
        public DateTime? NilOralyAfterDate { get; set; }
        public string ArrangeFor { get; set; }
        public string BloodProduct { get; set; }
        public string ShiftedOTTime { get; set; }
        public DateTime? ShiftedOTDate { get; set; }
        public string SpecialInstructions { get; set; }
        public string PreMedications { get; set; }
        public string DoctorID { get; set; }
        public DateTime createutc { get; set; }
    }
    public class getPACExamDetprint
    {

        public decimal PulseHeartRate { get; set; }
        public decimal Respiration { get; set; }
        public string BloodPressure { get; set; }
        public decimal temperature { get; set; }
        public string GC { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal BMI { get; set; }
        public string NStatus { get; set; }
        public string Pallor { get; set; }
        public string Icterus { get; set; }
        public string Cyanosis { get; set; }
        public string Clubbing { get; set; }
        public string LAP { get; set; }
        public string Oedema { get; set; }
        public string JVP { get; set; }
        public string Veins { get; set; }
        public string Spine { get; set; }
        public Int16 BHT { get; set; }
        public string TMJoint { get; set; }
        public Int16 MouthOpening { get; set; }
        public string LooseMissedTeeth { get; set; }
        public string MallampattiClass { get; set; }
        public Int16 Thyromentaldist { get; set; }
        public string ArtificialTeeth { get; set; }
        public string NasalPatency { get; set; }
        public string OralHygiene { get; set; }
        public string UpperRespiratory { get; set; }
        public string LowerRespiratory { get; set; }
        public string CVS { get; set; }
        public string CNS { get; set; }
        public string PerAbdomen { get; set; }
        public string Neck { get; set; }
        public string DoctorID { get; set; }
        public DateTime CREATE { get; set; }

    }

    public class getPACInvprint
    {

        public string Hb_PCV { get; set; }
        public decimal PLCount { get; set; }
        public string BLGlu { get; set; }
        public Int16 SUricAcid { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string TSH { get; set; }
        public string SElectNAPlus { get; set; }
        public string SElectKPlus { get; set; }
        public string SElectCAPlus { get; set; }
        public string UrineRMALB { get; set; }
        public string UrineRMSugar { get; set; }
        public string UrineRMKET { get; set; }
        public string UrineRMPlusCells { get; set; }
        public string UrineRMWBC { get; set; }
        public string UrineRMRBC { get; set; }
        public string XRay { get; set; }
        public string ECG { get; set; }
        public string ECHO { get; set; }
        public string ABG { get; set; }
        public string SpecialInvestigation { get; set; }
        public string TLC { get; set; }
        public string BlGroup { get; set; }
        public decimal BUrea { get; set; }
        public decimal Creatinine { get; set; }
        public string HBSAG { get; set; }
        public string HIV { get; set; }
        public string DLC { get; set; }
        public string PT { get; set; }
        public string PTTK { get; set; }
        public string SBil { get; set; }
        public string SGOTSGPT { get; set; }
        public string ALKPOGGT { get; set; }
        public string SProtein { get; set; }
        public string UnfitReasons { get; set; }
        public string Reviewedby1 { get; set; }
        public string Reviewedby2 { get; set; }
        public string Reviewedby3 { get; set; }
        public DateTime? Reviewedon1 { get; set; }
        public DateTime? Reviewedon2 { get; set; }
        public DateTime? Reviewedon3 { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }
        public string PlanGA { get; set; }
        public string DoctorID { get; set; }
        public DateTime create { get; set; }
    }

}
