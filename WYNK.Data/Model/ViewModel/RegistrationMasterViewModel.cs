using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{

    public class COnsultationview
    {
        public ICollection<ConPayment_Master> ConsultationPaymentMastermas { get; set; }
    }


    public class ConPayment_Master
    {
        public int? ID { get; set; }
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public string Instrumentdate { get; set; }
        public string Expirydate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public int Amount { get; set; }        
    }


    public class Consentdata
    {
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string Consentobtainedby { get; set; }
        public string cmpid { get; set; }
        public ICollection<GetConsentdetails> Totalvoiceconsentdata { get; set; }

    }
    public class Consultaionpaymentdetails
    {
        public ICollection<Consultationpaymentdetails> Consultationpaymentdetails { get; set; }
    }
    public class Consultationpaymentdetails
    {
        public int ID { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public DateTime? Expirydate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public decimal Amount { get; set; }
        public bool Disabled { get; set; }
    }

    public class GetConsentdetails
    {

        public string Name { get; set; }
        public string UIN { get; set; }
        public string Voice { get; set; }
        public string video { get; set; }
        public DateTime Consentcreated { get; set; }
    }

    public class RegistrationMasterUpdate
    {

        public Registration_Master RegistrationMaster { get; set; }

    }
    public class RegistrationMasterI
    {
        public ICollection<Payment_Master> PaymentMaster { get; set; }
        public ICollection<Payment_Master> ConsultationPaymentMastermas { get; set; }
        public Registration_Master RegistrationMaster { get; set; }
        public RegistrationTran_Master RegistrationTranMaster { get; set; }
        public RegistrationExtension RegistrationExtension { get; set; }
        public ICollection<PatientKINDetails> KinNDetails { get; set; }
        public ICollection<Patientlists> Patientlist { get; set; }
        public ICollection<AlternativeDrug> AlternativeDrug { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
        public int? ParentDescriptioncountryID { get; set; }
        public int? ParentDescriptionPinCode { get; set; }

        public int Cmpid { get; set; }
        public int billtranid { get; set; }
        public int DoctorID { get; set; }
        public decimal? ConsultationFee { get; set; }
        public string Companyname { get; set; }
        public string CampUIN { get; set; }
        public string AppUIN { get; set; }
        public decimal M_Consultationcharges { get; set; }
        public decimal Regcharges { get; set; }

        public string ContraTransactionid { get; set; }
        public ICollection<BusinessRuleAct> BRuleActBR { get; set; }
        public ICollection<Business_Rule> BRule { get; set; }
        public ICollection<Business_Rule1> BRule1 { get; set; }
        public ICollection<PatientAssignStatus> PatientAssignStatus { get; set; }

        public ICollection<PatientVSInsurance> PatientVSInsurance { get; set; }

        public ICollection<ForNationaldetails> ForNationaldetails { get; set; }
        public ICollection<GETRegExtension> GETRegExtension { get; set; }

        public ICollection<paymentReMode> paymentReMode { get; set; }
        public ICollection<GetAvccessDetails> GetAvccessDetails { get; set; }
        public decimal TOpayMode { get; set; }
        public string surgeryID { get; set; }
        public string Message { get; set; }
        public DateTime EFMdate { get; set; }
        public decimal? Concharges { get; set; }
        public decimal? Registrationcharges { get; set; }
    }

    public class GetAvccessDetails
    {

        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool All { get; set; }
        public bool Export { get; set; }
        public bool Print { get; set; }
    }

    public class GETRegExtension
    {
        public string UIN { get; set; }
        public int CMPID { get; set; }
        public bool Artificialeye { get; set; }
        public bool OD { get; set; }
        public bool OU { get; set; }
        public bool OS { get; set; }
    }
    public class Roledetails
    {
        public string Roles { get; set; }


    }
    public class paymentReMode
    {
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime? Expirydate { get; set; }
        public decimal Amount { get; set; }

    }
    public class NewSummaryImages
    {
        public string Imagedesc { get; set; }
        public string ImageName { get; set; }
        public string Imagecategory { get; set; }
    }

    public class NewSummaryImagesgorup
    {
        public object Imagedesc { get; set; }
        public object ImageName { get; set; }
        public string Imagecategory { get; set; }
    }
    public class NewSummaryPDF
    {
        public string Imagedesc { get; set; }
        public string ImageName { get; set; }
        public string Imagecategory { get; set; }
    }
    
    public class ForNationaldetails
    {
        public int ID { get; set; }
        public bool IsForeignNational { get; set; }
        public decimal? NormalFee { get; set; }
        public decimal? SuperSpecialityFee { get; set; }

    }


    public class LoginMasterViewModel
    {
        public Users Users { get; set; }
        public int USERSCompanyID { get; set; }
        public string Referrencetag { get; set; }
        public int? ReferrenceIC { get; set; }
        public int Userid { get; set; }
        public int ParentID { get; set; }
        public string CompanyName { get; set; }
        public string RoleDescription { get; set; }
        public string doctorname { get; set; }
        public ICollection<UserCompany> UserCompany { get; set; }
        public string status { get; set; }
    }


    public class COnsultationmembersdata
    {        
        public string Name { get; set; }
        public string LName { get; set; }
        public DateTime dov { get; set; }
        public string uin { get; set; }
        public string consultant { get; set; }
        public decimal? fees { get; set; }
    }


    public class RegistrationMasterViewModel
    {
        public ICollection<COnsultationmembersdata> COnsultationmembersdata { get; set; }
        public string InVoiceNumber { get; set; }
        public decimal? totalamountconsultation { get; set; }
        public string UIN { get; set; }
        public int PTotalAmountcon1 { get; set; }
        public string userroleID { get; set; }
        public string ContraTransactionid { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime APpointmentSceduleDate { get; set; }
        public string AppointmentCancellationReasons { get; set; }
        public string Appointmentmemberd { get; set; }

        public string Appointmentuserid { get; set; }

        public string Messagestatus { get; set; }
        public string appointmentdate { get; set; }
        public string appointmenttime { get; set; }
        public string Eyedoctor { get; set; }
        public string Hour { get; set; }
        public string Mnute { get; set; }



        public string Cancellatioreasons { get; set; }
        public string Stringdata { get; set; }
        public string CancellatioreasonsSeparation { get; set; }
        public string PAtientstatus { get; set; }
        public string UserRole { get; set; }
        public string UserRolecount { get; set; }
        public ICollection<Roledetails> Roledetailss { get; set; }
        public ICollection<Excelimportregistrationmaster> Excelimport { get; set; }
        public ICollection<dashboardpaymentReMode> dashboardpaymentReMode { get; set; }
        
        public int TypeofVisit { get; set; }
        public ICollection<NewSummaryImages> NewSummaryImages { get; set; }
        public ICollection<NewSummaryImagesgorup> NewSummaryImagesgorup { get; set; }
        public ICollection<NewSummaryPDF> NewSummaryPDF { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int ALLMenuaccessdata { get; set; }
        public string ResetMessage { get; set; }
        public string Token { get; set; }
        public string doctorname { get; set; }
        public string Errortoken { get; set; }

        public string Tokenusername { get; set; }
        public string Tokenpassword { get; set; }


        public string MenuDoctorname { get; set; }
        public string MenuDoctorTitle { get; set; }
        public string MenecompanyBranch { get; set; }
        public string Menulogindateandtime { get; set; }
        public string Compnayflag { get; set; }

        public string AppointmentPtaientname { get; set; }
        public string Appointmentage { get; set; }
        public string Appointmentgender { get; set; }
        public string Appointmentaddress { get; set; }
        public string Appointmentphone { get; set; }
        public string Appointmenttime { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string AppointmentReasons { get; set; }
        public string PreferredDoctor { get; set; }
        public string Appointmentbookedby { get; set; }
        public string Appointmentcreatedby { get; set; }





        public string Registrationnumber { get; set; }
        public string ProductImage { get; set; }
        public string ProductImages { get; set; }
        public string ThumbImage { get; set; }
        public int TOTALPatientcount { get; set; }
        public int TOTALREVIEWPatientcount { get; set; }
        public string Roleid { get; set; }

        public string RoleLogMessagge { get; set; }

        public int RoleidValue { get; set; }
        public int RoleRoleid { get; set; }
        public int? Adminid { get; set; }
        public string password { get; set; }
        public string confirmpasswword { get; set; }
        public string oldpassword { get; set; }
        public int Userid { get; set; }
        public int USERSCompanyID { get; set; }
        public int CompanyID { get; set; }
        public string FullRoleAccess { get; set; }
        public string SELECTALLCompany { get; set; }
        public object FYearDetails { get; set; }
        public string AlertNotification { get; set; }
        public string countryname { get; set; }
        public string BillingType { get; set; }
        public bool? RegistrationDateManage { get; set; }

        public int ParentID { get; set; }
        public string CompanyName { get; set; }
        public int DoctoID { get; set; }
        public string Referrencetag { get; set; }

        public string GMTUTCTime { get; set; }
        public int? ReferrenceIC { get; set; }
        public string RoleDescription { get; set; }

        public int NEWFORTHEDAY { get; set; }
        public int NEWMONTHTILLDATE { get; set; }
        public int REVIEWFORTHEDAY { get; set; }
        public int REVIEWMONTHTILLDATE { get; set; }
        public int SURGERYREVIEWFORTHEDAY { get; set; }
        public int SURGERYREVIEWMONTHTILLDATE { get; set; }

        public int KPINEWFORTHEDAY { get; set; }
        public int KPINEWMONTHTILLDATE { get; set; }
        public int KPISURGERYREVIEWFORTHEDAY { get; set; }
        public string KPIDFDATE { get; set; }
        public string KPITDDAY { get; set; }
        public int KPIDATAPOPULATIONTOTAL { get; set; }


        public int MonthNewFromDate { get; set; }
        public int MonthNewTODate { get; set; }
        public int MonthReviewFromdate { get; set; }
        public int MonthReviewToDate { get; set; }
        public int MonthSurgeryFromDate { get; set; }
        public int MonthSurgeryToDate { get; set; }
        public string MonthKPIFromDate { get; set; }
        public string MonthKPIToDate { get; set; }

        public Registration_Master RegistrationMaster { get; set; }
        public RegistrationTran_Master RegistrationTranMaster { get; set; }
        public RegistrationExtension RegistrationExtension { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public ICollection<Patientlists> Patientlist { get; set; }
        public ICollection<RevPatientlist> RevPatientlist { get; set; }
        public Number_Control NumberControl { get; set; }
        public Users Users { get; set; }
        public int Cmpid { get; set; }


        public string Companymastername { get; set; }

        public string NumberControlname { get; set; }

        public string Accessname { get; set; }
        public string Prenmae { get; set; }
        public string postname { get; set; }
        public string Mdashname { get; set; }
        public int Userroleidmoduleid { get; set; }

        public string regapplicablevalues { get; set; }
        public ICollection<PatientAssignStatus> PatientAssignStatus { get; set; }
        public ICollection<Payment_Master> PaymentMaster { get; set; }
        public Payment_Master PaymentMastermas { get; set; }
        public int fndid { get; set; }
        public string Mastername { get; set; }
        public string OneLionMastername { get; set; }
        public string Drugpropertymaster { get; set; }
        public string reports { get; set; }
        public string InventoryMaster { get; set; }
        public string InventoryTransactionsname { get; set; }

        public string CompanyImnage { get; set; }

        public ICollection<useraccesscontrol> useraccesscontrol { get; set; }
        public ICollection<usersCreation> usersCreation { get; set; }

        public ICollection<Workflowaccerss> Workflowaccerss { get; set; }


        public ICollection<Workflowaccerss> Premasters { get; set; }
        public ICollection<Workflowaccerss> Intramasters { get; set; }
        public ICollection<Workflowaccerss> Postmasters { get; set; }


        public ICollection<Workflowaccerss> RefractionMasters { get; set; }
        public ICollection<Workflowaccerss> FindingsMasters { get; set; }
        public ICollection<Workflowaccerss> Hrmspayrollmaster { get; set; }
        public ICollection<Workflowaccerss> AdmissionSurgerymenu { get; set; }
        public ICollection<Workflowaccerss> Utilitiesmenu { get; set; }
        public ICollection<Workflowaccerss> AdminMenu { get; set; }
        public ICollection<Workflowaccerss> operationMenu { get; set; }

        public ICollection<Workflowaccerss> OpticalTransactions { get; set; }
        public ICollection<Workflowaccerss> OpticalReports { get; set; }
        public ICollection<Workflowaccerss> Discharge { get; set; }

        public ICollection<Workflowaccerss> AdmissionRoomInformation { get; set; }

        public ICollection<Workflowaccerss> ClinicalProcedureTransactions { get; set; }

        public ICollection<Workflowaccerss> OutpatientsTransactions { get; set; }

        public ICollection<Workflowaccerss> InventoryReports { get; set; }
        public ICollection<Workflowaccerss> OutpatientsReports { get; set; }
        public ICollection<Workflowaccerss> PharmacyMenu { get; set; }



        public ICollection<Workflowaccerss> outpatientsmenu { get; set; }
        public ICollection<Workflowaccerss> opticalsmenu { get; set; }
        public ICollection<Workflowaccerss> Counsellingmenu { get; set; }
        public ICollection<DrugPropertyMaster> DrugPropertyMaster { get; set; }
        public ICollection<ReportsMaster> ReportsMasters { get; set; }
        public ICollection<InventoryMaster> inventoryMasters { get; set; }

        public ICollection<Workflowaccerss> AdmissionMasters { get; set; }

        public ICollection<Workflowaccerss> Admissiontransactions { get; set; }
        public ICollection<Billings> Billings { get; set; }

        public ICollection<InventoryTransactions> inventoryTransactions { get; set; }
        public ICollection<Patientpopulationdetails> patientpopulationdetails { get; set; }
        public ICollection<PatientMonthtilldatepopulationdetails> patientMonthtilldatepopulationdetails { get; set; }
        public ICollection<REVIEWPatientpopulationdetails> rEVIEWPatientpopulationdetails { get; set; }
        public ICollection<REVIEWPatientMonthtilldatepopulationdetails> rEVIEWPatientMonthtilldatepopulationdetails { get; set; }
        public ICollection<SURGERYREVIEWPatientpopulationdetails> SURREVIEWPOPULATION { get; set; }
        public ICollection<SURGERYREVIEWPatientMonthtilldatepopulationdetails> SURREVIEWPOPULATIONMONTHTILLDATE { get; set; }
        public ICollection<POPUPPatientpopulationdetails> POPUPPatientpopulationdetails { get; set; }
        public ICollection<REVIEWPOPUPPatientpopulationdetails> REVIEWPOPUPPatientpopulationdetails { get; set; }
        public ICollection<SURGERYPOPUPPatientpopulationdetails> SURGERYPOPUPPatientpopulationdetails { get; set; }

        public ICollection<MonthPOPUPPatientpopulationdetails> MonthFromPOPUPPatientpopulationdetails { get; set; }
        public ICollection<MonthREVIEWPOPUPPatientpopulationdetails> MonthFromREVIEWPOPUPPatientpopulationdetails { get; set; }
        public ICollection<MonthSURGERYPOPUPPatientpopulationdetails> MonthFromSURGERYPOPUPPatientpopulationdetails { get; set; }

        public ICollection<MonthTOPOPUPPatientpopulationdetails> MonthTOPOPUPPatientpopulationdetails { get; set; }
        public ICollection<MonthTOREVIEWPOPUPPatientpopulationdetails> MonthTOREVIEWPOPUPPatientpopulationdetails { get; set; }
        public ICollection<MonthTOSURGERYPOPUPPatientpopulationdetails> MonthTOSURGERYPOPUPPatientpopulationdetails { get; set; }

        public ICollection<MyPatientlists> MyPatientlistss { get; set; }
        public ICollection<UserCompany> UserCompany { get; set; }
        public ICollection<APpointmentdwtails> APpointmentdwtailss { get; set; }
        public ICollection<APpointmentdwtailscalender> APpointmentdwtailscalender { get; set; }

        public ICollection<Companyarray> Companyarray { get; set; }

        public ICollection<DataMainModulevsrole> DataMainModulevsrole { get; set; }

        public ICollection<FgformsModule> FgformsModule { get; set; }



        public List<DataMainModule> DataMainModules = new List<DataMainModule>();

        public List<OriginalModule> OriginalModule = new List<OriginalModule>();

        public List<DataOriginalModule> DataOriginalModule = new List<DataOriginalModule>();
        public ICollection<DashboardsectionModule> DashboardsectionModule { get; set; }
        public List<WorkflowDataOriginalmainModule> WorkflowDataOriginalmainModule = new List<WorkflowDataOriginalmainModule>();
        public string Deafaultdashboard { get; set; }

        public string cmpname { get; set; }
        public string cmpaddress { get; set; }
        public string cmpaddress2 { get; set; }
        public string cmpaddress3 { get; set; }
        public string cmpphone { get; set; }
        public string patientuin { get; set; }
        public string name { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string age { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public DateTime dateofregistration { get; set; }
        public decimal amount { get; set; }
        public string phone { get; set; }
        public string country { get; set; }

        public string receiptnumber { get; set; }
        public DateTime? receiptdate { get; set; }
        public ICollection<GetAvccessDetails> GetAvccessDetails { get; set; }

    }

    public class dashboardpaymentReMode
    {
        public string PaymentMode { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime? Expirydate { get; set; }
        public decimal Amount { get; set; }

    }
    public class DataMainModulevsrole
    {
        public string Moduletype { get; set; }
        public int moduleid { get; set; }

        public int parentmoduleid { get; set; }
        public string moduledescription { get; set; }


    }


    public class DashboardsectionModule
    {
        public string MainDescription { get; set; }
        public string DescriptionPath { get; set; }
    }


    public class WorkflowDataOriginalmainModule
    {
        public string MainDescription { get; set; }
        public List<workflowDataOriginalsubModule> workflowDataOriginalsubModule = new List<workflowDataOriginalsubModule>();

    }

    public class workflowDataOriginalModule
    {
        public string subDescription { get; set; }
        public string Moduletypes { get; set; }
        public List<workflowDataOriginalsubModule> workflowDataOriginalsubModule = new List<workflowDataOriginalsubModule>();
    }

    public class workflowDataOriginalsubModule
    {
        public string subDescription { get; set; }
        public string Moduletypes { get; set; }
        public List<workflowFgformsModule> workflowFgformsModule = new List<workflowFgformsModule>();
        public string formDescription { get; set; }

        public string Parentmoduledescription { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool All { get; set; }        
        public bool Update { get; set; }

        public bool Export { get; set; }
        public bool Delete { get; set; }

        public int? TransactionID { get; set; }
    }

    public class workflowFgformsModule
    {
        public string formDescription { get; set; }

        public string Parentmoduledescription { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool All { get; set; }
        public string Moduletypes { get; set; }
        public bool Update { get; set; }

        public bool Export { get; set; }
        public bool Delete { get; set; }

        public int? TransactionID { get; set; }
    }

    public class DataOriginalModule
    {
        public bool? Formvisible { get; set; }
        public string MainDescription { get; set; }
        public List<DataOriginalsubModule> DataOriginalsubModule = new List<DataOriginalsubModule>();

    }

    public class DataOriginalsubModule
    {
        public string subDescription { get; set; }
        public bool? Formvisible { get; set; }      
        public string Moduletypes { get; set; }
        public List<DataOriginalformsModule> DataOriginalformsModule = new List<DataOriginalformsModule>();
    }

    public class FgformsModule
    {
        public string formDescription { get; set; }
        public bool? Formvisible { get; set; }
        public string Parentmoduledescription { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Update { get; set; }

        public bool Export { get; set; }
        public bool Delete { get; set; }

        public int? TransactionID { get; set; }
    }

    public class DataOriginalformsModule
    {
		     public bool? Formvisible { get; set; }
        public string formDescription { get; set; }
        public string Moduletypes { get; set; }
        public string Parentmoduledescription { get; set; }
        public bool Add { get; set; }
        public bool All { get; set; }
        public bool Edit { get; set; }
        public bool Update { get; set; }

        public bool Export { get; set; }
        public bool Delete { get; set; }

        public int? TransactionID { get; set; }
    }

    public class OriginalModule
    {
        public List<orgFormsModules> orgFormsModules = new List<orgFormsModules>();
    }

    public class DataMainModule
    {
        public int id { get; set; }
        public string MainModuleName { get; set; }
    }

    public class DataSubModule
    {
        public int id { get; set; }

        public int Modid { get; set; }
        public string SubModuleName { get; set; }

        public string Subformnames { get; set; }
    }

    public class orgFormsModules
    {
        public int id { get; set; }
        public string orgFormNames { get; set; }

    }


    public class FormsModules
    {
        public int id { get; set; }

        public int parentid { get; set; }
        public string FormNames { get; set; }

    }

    public class APpointmentdwtailscalender
    {
        public string text { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime endDate { get; set; }
    }


    public class APpointmentdwtails
    {
        public int? Doctoruserid { get; set; }
        public string APPID { get; set; }
        public int appointmnentranid { get; set; }
        
        public string Name { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Doctorid { get; set; }
        public string APpointmentreasons { get; set; }

        public string Appointmentbookedby { get; set; }

        public string Appoinytstatus { get; set; }
        public string Apptime { get; set; }
        public DateTime? Appointmentdate { get; set; }
        public DateTime? Cancelleddatetime { get; set; }

        public DateTime Appointmentcreateddatedate { get; set; }



    }

}


public class Companyarray
{
    public int CompanyIDS { get; set; }
    public int ParentID { get; set; } 
}

    public class BusinessRuleAct
{
    public DateTime WEF { get; set; }
    public int BRID { get; set; }
    public int? From { get; set; }
    public int? TO { get; set; }
    public decimal Amount { get; set; }
    public int NoofDays { get; set; }
    public int TotalNoofDays { get; set; }
    public int CountOfRegTran { get; set; }
    public bool IsActive { get; set; }
    public DateTime? EffectiveDate { get; set; }

}
public class Business_Rule1
{
    public DateTime WEF { get; set; }
    public int BRID { get; set; }
    public int? From { get; set; }
    public int? TO { get; set; }
    public decimal Amount { get; set; }
    public int NoofDays { get; set; }
    public int TotalNoofDays { get; set; }
    public int CountOfRegTran { get; set; }
    public bool IsActive { get; set; }
    public DateTime? EffectiveDate { get; set; }

}

public class Business_Rule
{
    public DateTime WEF { get; set; }
    public int BRID { get; set; }
    public int? From { get; set; }
    public int? TO { get; set; }
    public decimal Amount { get; set; }
    public int NoofDays { get; set; }
    public int TotalNoofDays { get; set; }
    public int CountOfRegTran { get; set; }
    public bool IsActive { get; set; }
    public DateTime? EffectiveDate { get; set; }

}


public class Excelimportregistrationmaster
{
    public string UIN { get; set; }
    public int CMPID { get; set; }
    public DateTime DateofRegistration { get; set; }
    public string Name { get; set; }
    public DateTime DateofBirth { get; set; }
    public string Gender { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Address3 { get; set; }
    public int LocationID { get; set; }
    public string FatherHusbandName { get; set; }
    public string Occupation { get; set; }
    public string Phone { get; set; }
    public string AadharNumber { get; set; }
    public string EmailID { get; set; }
    public int SourceofReferralID { get; set; }
    public string ReferralName { get; set; }
    public Boolean IsDeleted { get; set; }
    public DateTime CreatedUTC { get; set; }
    public DateTime? UpdatedUTC { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public string PhotoPath { get; set; }
    public string BiometricPath { get; set; }
}




public class UserCompany
{
    public string CompanyNames { get; set; }
    public int CompanyIDS { get; set; }

}

public class MonthTOREVIEWPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}
public class MonthTOSURGERYPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}
public class MonthTOPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}


public class MonthREVIEWPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}
public class MonthSURGERYPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}
public class MonthPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}










public class REVIEWPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}
public class SURGERYPOPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}
public class POPUPPatientpopulationdetails
{
    public string Name { get; set; }
    public string UIN { get; set; }
    public string gender { get; set; }
    public string Age { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string email { get; set; }
}

public class SURGERYREVIEWPatientpopulationdetails
{
    public string Name { get; set; }
    public int UIN { get; set; }
    public string Gender { get; set; }
    public string Age { get; set; }
    public string Phonenumber { get; set; }
    public string Address { get; set; }
}

public class SURGERYREVIEWPatientMonthtilldatepopulationdetails
{
    public string UINCount { get; set; }
}

public class REVIEWPatientpopulationdetails
{
    public string Name { get; set; }
    public int UIN { get; set; }
    public string Gender { get; set; }
    public string Age { get; set; }
    public string Phonenumber { get; set; }
    public string Address { get; set; }
}

public class REVIEWPatientMonthtilldatepopulationdetails
{
    public int UINCount { get; set; }
}

public class Patientpopulationdetails
{
    public string Name { get; set; }
    public int UIN { get; set; }
    public string Gender { get; set; }
    public string Age { get; set; }
    public string Phonenumber { get; set; }
    public string Address { get; set; }
}

public class PatientMonthtilldatepopulationdetails
{
    public int UINCount { get; set; }
}
public class useraccesscontrol
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
    public int? TransactionID { get; set; }
}
public class Workflowaccerss
{
    public string formDescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public string Moduletypes { get; set; }
    public bool Update { get; set; }  
    public bool Delete { get; set; }
    public bool Export { get; set; }
    public bool All { get; set; }
    public int? TransactionID { get; set; }
}
public class usersCreation
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
    public int? TransactionID { get; set; }
}
public class DrugPropertyMaster
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public int? TransactionID { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
}
public class ReportsMaster
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public int? TransactionID { get; set; }
    public bool Delete { get; set; }
}
public class InventoryMaster
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
    public int? TransactionID { get; set; }
}
public class InventoryTransactions
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
    public int? TransactionID { get; set; }
}
public class Billings
{
    public string moduledescription { get; set; }
    public string Parentmoduledescription { get; set; }
    public bool Add { get; set; }
    public int? TransactionID { get; set; }
    public bool Edit { get; set; }
    public bool Update { get; set; }
    public bool Delete { get; set; }
}
public class PatientAssignStatus
{
    public string RegistrationTranID { get; set; }
    public string StatusID { get; set; }
    public int DoctorID { get; set; }
    public int OptometristcID { get; set; }


    public int VisionID { get; set; }
    public string Visittype { get; set; }

    public string PaymentFor { get; set; }
    public int PaymentAgainstID { get; set; }
    public string PaymentMode { get; set; }
    public string InstrumentNumber { get; set; }
    public DateTime? Instrumentdate { get; set; }
    public string BankName { get; set; }
    public string BankBranch { get; set; }
    public DateTime? Expirydate { get; set; }
    public decimal Amount { get; set; }
    public decimal AdvAmount { get; set; }
    public int PaymentReferenceID { get; set; }
}
public class PatientVSInsurance
{
    public int InsurancevsMiddlemenID { get; set; }
    public string PolicyName { get; set; }
    public string PolicyNo { get; set; }
    public DateTime? PolicyTakenOn { get; set; }
    public DateTime? PeriodFrom { get; set; }
    public DateTime? PeriodTo { get; set; }
    public bool IsJointPolicy { get; set; }
    public decimal SumAssured { get; set; }
    public decimal AmountAvailed { get; set; }
    public string Remarks { get; set; }
}

public class MyPatientlists
{
    public string UIN { get; set; }
    public string Name { get; set; }
    public string MName { get; set; }
    public string LName { get; set; }
    public DateTime DOR { get; set; }
    public string Age { get; set; }
    public string ages { get; set; }
    public DateTime now { get; set; }
    public DateTime? datesss { get; set; }
    public string Sex { get; set; }
    public string Remarks { get; set; }
    public string Status { get; set; }
    public string AdminName { get; set; }
    public int RegistrationTranID { get; set; }
    public DateTime? AssignedDate { get; set; }


    public string AssDoctorID { get; set; }
    public DateTime? ReviewDate { get; set; }
    public string Doctorname { get; set; }

}


public class Patientlists
{
    public string UIN { get; set; }
    public string OLDUIN { get; set; }
    public string Name { get; set; }
    public string MName { get; set; }
    public string LName { get; set; }
    public DateTime DOR { get; set; }

    public DateTime DOB { get; set; }
    public string Age { get; set; }
    public string ages { get; set; }
    public DateTime now { get; set; }
    public DateTime? datesss { get; set; }
    public string Sex { get; set; }
    public string Remarks { get; set; }
    public string Status { get; set; }
    public int StatusID { get; set; }
    public string AdminName { get; set; }
    public int RegistrationTranID { get; set; }
    public DateTime? AssignedDate { get; set; }


    public string AssDoctorID { get; set; }
    public decimal? ConsultationFee { get; set; }
    
    public DateTime? ReviewDate { get; set; }
    public string Doctorname { get; set; }
    public string Relationship { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string ContactNumber { get; set; }


    public string Regdate { get; set; }
    public string Assdate { get; set; }

    public string EmailID { get; set; }
    public string Statusvisits { get; set; }
    public int? OptometristBucket { get; set; }
    public bool PrimaryKinId { get; set; }

}

public class RevPatientlist
{
    public string RUIN { get; set; }
    public string RName { get; set; }
    public string RMName { get; set; }
    public string RLName { get; set; }
    public DateTime? DOV { get; set; }
    public string RAge { get; set; }
    public string RSex { get; set; }
    public DateTime? FDate { get; set; }

    public string RAdminName { get; set; }
    public string RStatus { get; set; }
    public int RegistrationTranID { get; set; }
    public string DoctorID { get; set; }
    public DateTime? AssignedDate { get; set; }
    public int? OptometristBucket { get; set; }
  
}