using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Repository.Implementation;
using WYNK.Data.Repository;

namespace WYNK.Data.Repository
{
    public interface IRepositoryWrapper
    {
        IFundusRepository FundusViewM { get; }
        IErrorlogRepository Errorlog { get; }
        ITonometryRepository Tonometry { get; }
        ISlitLampRepository SlitLampViewM { get; }
        ITransactionTypeRepository TransactionTypeViewM { get; }
        ICommonRepository Common { get; }
        IReferralMasterRepository ReferralMaster { get; }
        IPatientRepository Registration { get; }
        IDoctorMasterRepository doctorMaster { get; }
        IMeterialreturnRepository Meterial { get; }
        ITonometryTranRepository TonometryTran { get; }
        IHelpRepository Help { get; }
        IRegistrationMasterRepository RegistrationMaster { get; }
        ICampRegistrationRepository CampRegistration { get; }
        IInsuranceRepository Insurance { get; }
        IInsuranceVsMiddlemanRepository InsuranceVsMiddleman { get; }
        IUsersVsRoleRepository UsersVsRole { get; }
        IMedicalPrescriptionRepository MedicalPrescription { get; }
        IOcularComplaintsRepository OcularComplaints { get; }
        IcdMasterRepository icdMaster { get; }
        IFindingsRepository Findings { get; }
        IServiceExtensionRepository ServiceExtension { get; }
        IPatientHistoryRepository PatientHistory { get; }
        IUserRepository User { get; }
        IrefractionRepository refraction { get; }
        IEmployeeMasterrepository employee { get; }
        IOneLineMasterRepository OneLineMaster { get; }
        ISquintExtnMasterRepository SquintExtnMaster { get; }
        IDrugMasterRepository drugMaster { get; }
        IVendorMasterRepository VendorMaster { get; }
        IRoleVsAccessRespository RoleVsAccess { get; }
        IStoremasterRepository Storemaster { get; }
        IBillingMasterRepository Billing { get; }
        IInvestigationRepository Investigation { get; }
        IOperationTheatreRepository OperationTheatre { get; }
        IpurchaseorderRepository purchaseorder { get; }
        IOpticalOrderRepository OpticalOrder { get; }
        ISurgeryRepository SurgeryMaster { get; }
        ISurgeryDischargeRepository surgeryDischarge { get; }
        IGrnRepository Grn { get; }
        IPostoperativeRepository Postoperative { get; }
        IMed medeg { get; }
        IReportssrepository Reportss { get; }
        IMedicalBillRegisterRepository medBillReg { get; }
        IMedicineRepository Medicine { get; }
        IPurchaseorderDetRepository PurchaseOrdDet { get; }
        IStockRepository Stock { get; }
        IManagementDashRepository managementDash { get; }
        ICancePORegRepository CancelPORegViewModel { get; }
        IPurchaseOrderRegRepository PurchaseOrderRegView { get; }
        IPurchaseordercancellationRepository Purchaseordercancellation { get; }
        IPurchaseOrderPrintingRepository PurchaseOrderPrinting { get; }
        IPurchaseOrderCancellationPrintingRepository PurchaseOrderCancellationPrinting { get; }
        IOpticalPrescriptionRepository OpticalPrescription { get; }
        IInvestigationPrescriptionRepository InvestigationPrescription { get; }
        IGrnWoPoRepository GrnWoPo { get; }
        IFinalBillingRepository FinalBilling { get; }
        ICompanyMasterRepository CompanyMasterView { get; }

     
        IUomMasterRepository UomMaster { get; }
        INumberControlRepository NumberControl { get; }
        IOpticalBillingRepository OpticalBilling { get; }
        ITaxMasterRepository TaxMasterViewM { get; }
        ITaxSummaryRepository TaxSummaryViewM { get; }
        ICounsellingRepository CounsellingRepository { get; }
        IInterDeparmentTransferRepository InterDepartmentTransfer { get; }
        IInterDeparmentReceiverRepository InterDeparmentReceiver { get; }
        IIpOpticalBillingRepository IpOpticalBilling { get; }
        IExceluploadrepo IExceluploadrepo { get; }
        IModuleMasterRepository ModuleMasterViewM { get; }
        IBusinessRuleRepository BusinessRule { get; }
        IDepartRepository Depart { get; }
        IConfigureRepository Configure { get; }
        IFinancialRepository Financial { get; }
        IProfRepository Prof { get; }
        IOperationTheatreBookingRepository OperationTheatreBooking { get; }
        IOvertimeRepository OvertimeViewM { get; }
        IlocationRepository Loc { get; }
        IMedservRepository medserv { get; }
        ICustomerMasterRepository Customermaster { get; }
        IDonorRepository Donor { get; }
        ICustomerOrderRepository Customerorder { get; }
        IOTConsumptionRepository OTConsumption { get; }
        ISurgerySafetyCheckListRepository SurgerySafetyCheckList { get; }
        IRoomMasterRepository RoomMasterViewM { get; }
        IAdvancePaymentRepository AdvancePayment { get; }
        ILensMasterRepository LensMaster { get; }
        ITreatmentMasterRepository TreatmentMaster { get; }
        IQualificationRepository QualifyMaster { get; }
        IuniversityRepository UniversityMaster { get; }
        IOpticalGrnRepository OpticalGrn { get; }
        IIndentRepository Indentrepo { get; }
        IForeignNationalRepository ForeignNational { get; }
        IOfferRepository OfferViewM { get; }
        ICancellationReportRepository CancellationViewM { get; }
        IUserLogRepository UserLogViewM { get; }
        IConcentUploadingRepository ConcentUploading { get; }

        IInvestigationBillingRepository InvestigationBilling { get; }

        IInvestigationIPBillingRepository InvestigationIPBilling { get; }

        IPatientVsInsuranceRepository PatientVsInsurance { get; }

        IPatientQueueRepository PatientQueue { get; }

        ISurgeryadmissionandassignRepository Surgeryadmissionandassign { get; }

        IOpticalStockSummaryRepository OpticalStockSummary { get; }//DrugStockSummary
        IDrugStockSummaryRepository DrugStockSummary { get; }
        IOpticalStockLedgerRepository OpticalStockLedger { get; }
        IMessagingTemplateRepository MessagingTemplate { get; }

        IRoomTransferRepository RoomTransfer { get; }

        IBMIRepository BIM { get; }

        IPACRepository PACViewM { get; }
        IIOProcedureTemplateRepository IOProcedureTemplate { get; }

        IBrandMasterRepository BrandMaster { get; }
        ICampMasterRepository CampMaster { get; }
        ISetupMasterrepository GetSetup { get; }
        IAppointmentRepository AppointMent { get; }
        IYearEndProcessRepository YearEndProcess { get; }
        ISpecialityVSTestRepository SpecialityVSTest { get; }
        IOpticalBillRegisterRepository OpticalBillRegister { get; }
        IUserListRepository UserList { get; }
        IAllergyrepository Allergy { get; }
        IDiagnosisVSMedicineRepository DiagnosisVSMedicine { get; }
        ICampDashboardRepository CampDashboard { get; }
        IForm3cRepository form3c { get; }
        IMedicineMappingRepository MedicineMapping { get; }
        IServiceMasterRepository service { get; }
        IOpticalDashboardRepository OpticalDashboard { get; }
    }

}
