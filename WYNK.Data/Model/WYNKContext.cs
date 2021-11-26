using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WYNK.Data.Model
{
    public class WYNKContext : DbContext
    {

        public WYNKContext(DbContextOptions<WYNKContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestigationPrescription>()
             .HasKey(o => new { o.CMPID, o.INVPRESNO, o.Fyear });

            modelBuilder.Entity<StockMaster>()
               .HasKey(o => new { o.CMPID, o.DocumentNumber, o.Fyear });

            modelBuilder.Entity<MedicalPrescription>()
                .HasKey(o => new { o.RandomUniqueID, o.CmpID, o.MedicalPrescriptionNo, o.Fyear });

            modelBuilder.Entity<PurchaseOrder>()
                .HasKey(o => new { o.PONumber, o.Fyear, o.CMPID });

            modelBuilder.Entity<OpticalStockMaster>()
              .HasKey(o => new { o.DocumentNumber, o.Fyear, o.CMPID });

            modelBuilder.Entity<OpticalInvoiceMaster>()
                .HasKey(o => new { o.InvoiceNumber, o.Fyear, o.CMPID });


           
        }
        public DbSet<UploadMedicalPrescription> UploadMedicalPrescription { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }

        public DbSet<Squint_ExtnMaster> SquintExtnMaster { get; set; }
        public DbSet<OpticalPrescriptionmaster> OpticalPrescriptionmaster { get; set; }
        public DbSet<OrbitalDisorders> OrbitalDisorders { get; set; }
        public DbSet<preAuthorizationInsurance> preAuthorizationInsurance { get; set; }
        public DbSet<EstimateTracking> EstimateTracking { get; set; }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<Icd_Master> ICDMaster { get; set; }
        public DbSet<InvestigationTran> InvestigationTran { get; set; }
        public DbSet<Lensmaster> Lensmaster { get; set; }
        public DbSet<Tonometry> Tonometry { get; set; }
        public DbSet<LensTranModel> Lenstrans { get; set; }
        public DbSet<OpticalBalance> OpticalBalance { get; set; }
        public DbSet<LensExtension> LensExtension { get; set; }
        public DbSet<MedicalPrescription> MedicalPrescription { get; set; }
        public DbSet<MedicalPrescriptionTran> MedicalPrescriptionTran { get; set; }
        public DbSet<OcularComplaints> OcularComplaints { get; set; }
        public DbSet<OcularComplaintsHistory> OcularComplaintsHistory { get; set; }
        public DbSet<OpinionMaster> Opinion { get; set; }
        public DbSet<Patient_Assign> PatientAssign { get; set; }
        public DbSet<PatientHistory> PatientHistory { get; set; }
        public DbSet<Payment_Master> PaymentMaster { get; set; }
        public DbSet<RefractionMas> Refraction { get; set; }
        public DbSet<Registration_Master> Registration { get; set; }
        public DbSet<AttendersPass> AttendersPass { get; set; }
        public DbSet<VehiclePassmaster> VehiclePassmaster { get; set; }
        public DbSet<VehiclePasstran> VehiclePasstran { get; set; }
        public DbSet<RegistrationTran_Master> RegistrationTran { get; set; }
        public DbSet<RegistrationExtension> RegistrationExtension { get; set; }
        public DbSet<SlitLamp> SlitLamp { get; set; }
        public DbSet<Surgery_Master> SurgeryMaster { get; set; }
        public DbSet<Surgery_CarriedOutDetails> SurgeryCarriedOutDetails { get; set; }
        public DbSet<SurgeryCostDetails> SurgeryCostDetail { get; set; }
        public DbSet<ICDExtenstion> ICDExtenstion { get; set; }

        
        public DbSet<OpticalPrescriptionn> OpticalPrescription { get; set; }
        public DbSet<allergy> allergy { get; set; }
        public DbSet<AllergyTran> AllergyTran { get; set; }
        public DbSet<Speciality_trans> SpecialityTrans { get; set; }
        public DbSet<Fundus> Fundus { get; set; }
        public DbSet<PatientGeneralDatamodel> PatientGeneral { get; set; }
        public DbSet<MedicalBill_Master> MedicalBillMaster { get; set; }
        public DbSet<MedicalBill_Tran> MedicalBillTran { get; set; }
        public DbSet<InvestigationImages> InvestigationImages { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchaseOrderTrans> PurchaseOrderTrans { get; set; }
        public DbSet<OperationTheatre> OperationTheatre { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<RoomDetails> RoomDetails { get; set; }
        public DbSet<RoomDetailsExtension> RoomDetailsExtension { get; set; }
        public DbSet<OperationExtension> OperationTheatreExtension { get; set; }
        public DbSet<SurgeryMaster> Surgery { get; set; }
        public DbSet<Surgery_Tran> SurgeryTran { get; set; }
        public DbSet<SurgeryDoctor> SurgeryDoctor { get; set; }
        public DbSet<Admission> Admission { get; set; }
        public DbSet<IntraOperative> IntraOperative { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<InsuranceVsMiddlemen> InsuranceVsMiddlemen { get; set; }
        public DbSet<PreOperative> PreOperative { get; set; }
        public DbSet<Surgery_Discharge> SurgeryDischarge { get; set; }
        public DbSet<StockMaster> StockMaster { get; set; }
        public DbSet<StockTran> StockTran { get; set; }
        public DbSet<ItemBatch> ItemBatch { get; set; }
        public DbSet<ItemVendorMapping> ItemVendorMapping { get; set; }
        public DbSet<Postoperative> PostOperative { get; set; }
        public DbSet<PatientAccount> PatientAccount { get; set; }
        public DbSet<PatientAccountDetail> PatientAccountDetail { get; set; }
        public DbSet<PatientAccountDetailTax> PatientAccountDetailTax { get; set; }
        public DbSet<ItemBatchTrans> ItemBatchTrans { get; set; }
        public DbSet<InvestigationPrescription> InvestigationPrescription { get; set; }
        public DbSet<InvestigationPrescriptionTran> InvestigationPrescriptionTran { get; set; }
        public DbSet<ManagementMaster> PatientFootfall { get; set; }
        public DbSet<OpticalMaster> OpticalMaster { get; set; }
        public DbSet<OpticalTran> OpticalTran { get; set; }
        public DbSet<DrugTemplate> DrugTemplate { get; set; }
        public DbSet<SlitlampExtn> SlitlampExtn { get; set; }
        public DbSet<Glaucoma> Glaucoma { get; set; }
        public DbSet<GlaucomaImage> GlaucomaImage { get; set; }
        public DbSet<PatientKINDetails> PatientKINDetails { get; set; }
        public DbSet<ForeignNational> ForeignNational { get; set; }
        public DbSet<BusinessRule> BussinessRule { get; set; }
        public DbSet<BusinessRuleTran> BussinessRuleTran { get; set; }
        public DbSet<CounsellingChecklist> CounsellingChecklist { get; set; }
        public DbSet<CounsellingExtensionModel> CounsellingExtension { get; set; }
        public DbSet<CounsellingModel> Counselling { get; set; }
        public DbSet<Configure> Configuration { get; set; }
        public DbSet<ConfigureTrans> ConfigurationTrans { get; set; }
        public DbSet<FindingsExt> FindingsExt { get; set; }
        public DbSet<ICDSpecialityCode> ICDSpecialityCode { get; set; }
        public DbSet<Financial> FinancialYear { get; set; }
        public DbSet<OverTime> OverTime { get; set; }
        public DbSet<TreatmentMasterModel> TreatmentMaster { get; set; }
        public DbSet<TonometryTran> TonometryTran { get; set; }
        public DbSet<SquintMaster> SquintMaster { get; set; }
        public DbSet<SquintTran> SquintTran { get; set; }
        public DbSet<SquintImage> SquintImage { get; set; }
        public DbSet<SlitLampImage> SlitLampImage { get; set; }
        public DbSet<FundusImage> FundusImage { get; set; }
        public DbSet<RoomOccupiedstatus> RoomOccupiedStatus { get; set; }
        public DbSet<CustomerMaster> CustomerMaster { get; set; }
        public DbSet<Donor> Donor { get; set; }
        public DbSet<OperationTheatreBooking> OperationTheatreBooking { get; set; }
        public DbSet<OperationTheatre_Booking_Status> OperationTheatreBookingStatus { get; set; }
        public DbSet<SurgeryAssigned> SurgeryAssigned { get; set; }
        public DbSet<SurgeryAssignedTran> SurgeryAssignedTran { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<FinancialStatus> FinancialYearStatus { get; set; }
        public DbSet<OTIndentModel> OTIndent { get; set; }
        public DbSet<OTIndenttranModel> OTIndentTran { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<OfferTran> OfferTran { get; set; }
        public DbSet<OfferTranExtension> OfferTranExtension { get; set; }
        public DbSet<VISUALACUITY> VISUALACUITY { get; set; }
        public DbSet<IntraOcularPressure> IntraOcularPressure { get; set; }
        public DbSet<ItemSerial> ItemSerial { get; set; }
        public DbSet<ItemBalance> ItemBalance { get; set; }
        public DbSet<PGP> PGP { get; set; }
        public DbSet<REFRACTIONEXT> REFRACTIONEXT { get; set; }
        public DbSet<ACCEPTANCE> ACCEPTANCE { get; set; }
        public DbSet<FINALPRESCRIPTION> FINALPRESCRIPTION { get; set; }
        public DbSet<Amsler> Amsler { get; set; }
        public DbSet<ColorVision> ColorVision { get; set; }
        public DbSet<OTConsumption> OTConsumption { get; set; }
        public DbSet<SurgerySafetyCheckList> SurgerySafetyCheckList { get; set; }
        public DbSet<InvestigationBillMaster> InvestigationBillMaster { get; set; }
        public DbSet<InvestigationBillTran> InvestigationBillTran { get; set; }
        public DbSet<SscResponse> SscResponse { get; set; }

        public DbSet<SscResponseTran> SscResponseTran { get; set; }
        public DbSet<Monitoring> Monitoring { get; set; }

        public DbSet<MainStockMaster> MainStockMaster { get; set; }
        public DbSet<MainStockTran> MainStockTran { get; set; }
        public DbSet<MainItemBatch> MainItemBatch { get; set; }
        public DbSet<MainItemBatchTrans> MainItemBatchTrans { get; set; }
        public DbSet<MainItemSerial> MainItemSerial { get; set; }
        public DbSet<MainItemBalance> MainItemBalance { get; set; }
        public DbSet<InsuranceImageTran> InsuranceImageTran { get; set; }
        public DbSet<PatientVsInsurance> PatientVsInsurance { get; set; }
        public DbSet<PatientInsuranceTran> PatientInsuranceTran { get; set; }
        public DbSet<PatientInsuranceBilgTran> PatientInsuranceBilgTran { get; set; }
        public DbSet<JointFamilyDetails> JointFamilyDetails { get; set; }
        public DbSet<PACHistoryModel> PACHistory { get; set; }
        public DbSet<PACHistoryTranModel> PACHistoryTran { get; set; }
        public DbSet<PACExamination> PACExamination { get; set; }
        public DbSet<PACInvestigation> PACInvestigation { get; set; }

        public DbSet<PACPreOperativeInstruction> PACPreOperativeInstruction { get; set; }
        public DbSet<BMI> BMI { get; set; }
        public DbSet<PatientCurrentMedication> PatientCurrentMedication { get; set; }
        public DbSet<IOProcedureTemplate> IOProcedureTemplate { get; set; }
        public DbSet<IOTemplateTran> IOTemplateTran { get; set; }
        public DbSet<IOMaster> IOMaster { get; set; }
        public DbSet<IOTran> IOTran { get; set; }
        public DbSet<OtImagepath> OtImagepath { get; set; }
        public DbSet<VisualField> VisualField { get; set; }
        public DbSet<GlaucomaEvaluation> GlaucomaEvaluation { get; set; }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Appointmenttran> AppointmentTrans { get; set; }
        public DbSet<AppointmentExtension> AppointmentExtension { get; set; }

        public DbSet<ConsultantionSummary> ConsultantionSummary { get; set; }
        public DbSet<SpecialityVSTest> SpecialityVSTest { get; set; }
        public DbSet<DrugTappering> DrugTappering { get; set; }
        public DbSet<itembatchdiff> itembatchdiff { get; set; }
        public DbSet<SlitLampMaster> SlitLampMaster { get; set; }
        public DbSet<FundusMaster> FundusMaster { get; set; }
        public DbSet<OpticalOrder> OpticalOrder { get; set; }
        public DbSet<OpticalOrderTran> OpticalOrderTran { get; set; }

        public DbSet<CustomerOrder> CustomerOrder { get; set; }
        public DbSet<CustomerOrderTran> CustomerOrderTran { get; set; }
        public DbSet<SurgeryHistory> SurgeryHistory { get; set; }

        public DbSet<OpticalStockMaster> OpticalStockMaster { get; set; }
        public DbSet<OpticalStockTran> OpticalStockTran { get; set; }
        public DbSet<OpticalInvoiceMaster> OpticalInvoiceMaster { get; set; }
        public DbSet<OpticalInvoiceTran> OpticalInvoiceTran { get; set; }
        
        public DbSet<SYRINGEFDDT> SYRINGEFDDT { get; set; }
        public DbSet<Drug_Master> DrugMaster { get; set; }
        public DbSet<Drug_Group> DrugGroup { get; set; }
        public DbSet<DiagnosisVSMedicine> DiagnosisVSMedicine { get; set; }
        public DbSet<SquintExt> SquintExt { get; set; }
        public DbSet<SchirmerTest> SchirmerTest { get; set; }
        public DbSet<BirthHistory> BirthHistory { get; set; }
        public DbSet<RevenueSummary> RevenueSummary { get; set; }
        public DbSet<Tax_Summary> TaxSummary { get; set; }
        public DbSet<ProTax> ProfessionalTax { get; set; }
        public DbSet<ProTaxTran> ProfessionalTaxTran { get; set; }
        public DbSet<EmployeeExperience> Experience { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<QualificationExt> QualificationExt { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<UniversityExt> UniversityExt { get; set; }
        public DbSet<EmployeeQualification> EmployeeQualification { get; set; }
        public DbSet<DryEyes> DryEyes { get; set; }
        public DbSet<Camporganization_Model> CAMPORGANIZATION { get; set; }
        public DbSet<CampMaster_Model> CAMP { get; set; }


        public DbSet<CampRegistration> CampRegistration { get; set; }
        public DbSet<CampRegistrationTran> CampRegistrationTran { get; set; }
        public DbSet<CampRegistrationExtension> CampRegistrationExtension { get; set; }
        public DbSet<CampPatientKINDetails> CampPatientKINDetails { get; set; }
       public DbSet<UploadInvestigationPrescription> UploadInvestigationPrescription { get; set; }
        public DbSet<ServiceMaster> ServiceMaster { get; set; }
        public DbSet<CampPatientFootfall> CampPatientFootfall { get; set; }
        public DbSet<CampPatientFootfallTran> CampPatientFootfallTran { get; set; }
        public DbSet<CampPatientFootDoctor> CampPatientFootDoctor { get; set; }
        public DbSet<AlternativeDrug> AlternativeDrug { get; set; }
        public DbSet<EyelidDisorder> EyelidDisorder { get; set; }
        public DbSet<EyelidImage> EyelidImage { get; set; }
        public DbSet<SpecialityMaster> SpecialityMaster { get; set; }
        public DbSet<OpticalSummary> OpticalSummary { get; set; }
        public DbSet<InvestigationExtn> InvestigationExtn { get; set; }
        public DbSet<Workflowtran> Workflowtran { get; set; }
        public DbSet<Workflowmaster> Workflowmaster { get; set; }
        public DbSet<PackageMapping> PackageMapping { get; set; }
        public DbSet<Service_Master> Services { get; set; }

        public DbSet<RoleVsService> RoleVsService { get; set; }
        public DbSet<TestMaster> TestMaster { get; set; }
        public DbSet<TestTran> TestTran { get; set; }
        public DbSet<DiagnosisVSTest> DiagnosisVSTest { get; set; }
    }
    public class CMPSContext : DbContext
    {

        public CMPSContext(DbContextOptions<CMPSContext> options) : base(options) { }
        public DbSet<SetupCountry> Country { get; set; }
        public DbSet<Consentextension> ConsentExtension { get; set; }
        public DbSet<SetupmasterClass> Setup { get; set; }
        public DbSet<DoctorExtension> DoctorExtension { get; set; }
        public DbSet<LocationMaster> LocationMaster { get; set; }
        public DbSet<Companymas> Company { get; set; }
        public DbSet<Doctor_Master> DoctorMaster { get; set; }
        public DbSet<Speciality_master> DoctorSpeciality { get; set; }
        public DbSet<Number_Control> NumberControl { get; set; }
        public DbSet<OneLine_Masters> OneLineMaster { get; set; }
        public DbSet<Employee_Master> Employee { get; set; }
        public DbSet<Role_Master> Role { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ModuleMaster> ModuleMaster { get; set; }
        public DbSet<Number_Control_Template> NumberControlTemplate { get; set; }

        public DbSet<ModuleMasterTemplate> ModuleMasterTemplate { get; set; }
        public DbSet<RoleVsModuleaccess> RoleVsModule { get; set; }
        public DbSet<User_Role> UserVsRole { get; set; }
        public DbSet<UOM_Master> uommaster { get; set; }
        public DbSet<Vendor_Masters> VendorMaster { get; set; }
        public DbSet<Storemasters> Storemasters { get; set; }
        public DbSet<TaxMaster> TaxMaster { get; set; }
        public DbSet<EmployeeStatutory> EmployeeStatutory { get; set; }
        public DbSet<EmployeeBank> EmployeeBank { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<EmployeeCommunication> EmployeeCommunication { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<LogUser> LogUser { get; set; }
        public DbSet<MessageTemplate> MessagingTemplate { get; set; }
        public DbSet<DrugExtension> DrugExtension { get; set; }
      


    }


    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }
        public DbSet<MainUsers> Users { get; set; }
        public DbSet<Connectionstring> Connectionstring { get; set; }
    }
}
