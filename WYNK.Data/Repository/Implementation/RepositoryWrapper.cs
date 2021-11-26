
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WYNK.Data.Model;

namespace WYNK.Data.Repository.Implementation
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private WYNKContext _Wynkcontext;
        private CMPSContext _Cmpscontext;
        
        private IOpticalOrderRepository _OpticalOrder;
        private IReferralMasterRepository _ReferralMaster;
        private ICommonRepository _common;
        private IPatientRepository _Registration;
        private IDoctorMasterRepository _doctorMaster;
        private IHelpRepository _Help;
        private IRegistrationMasterRepository _RegistrationMaster;
        private IMedicalPrescriptionRepository _MedicalPrescription;
        private IcdMasterRepository _icdMaster;
        private IOcularComplaintsRepository _OcularComplaints;
        private IFindingsRepository _Findings;
        private IUserRepository _User;
        private IPatientHistoryRepository _PatientHistory;
        private IrefractionRepository _refraction;
        private IOneLineMasterRepository _Oneline;
        private ISquintExtnMasterRepository _SquintExtnMaster;
        private IEmployeeMasterrepository _employee;
        private IDrugMasterRepository _drugMaster;
        private IVendorMasterRepository _VendorMas;
        private IRoleVsAccessRespository _RoleVsAccess;
        private IStoremasterRepository _Storemaster;
        private IBillingMasterRepository _Billing;
        private IInvestigationRepository _Investigation;
        private IOperationTheatreRepository _OperationTheatre;
        private IpurchaseorderRepository _purchaseorder;
        private ISurgeryRepository _Surgery;
        private ISurgeryDischargeRepository _surgeryDischarge;
        private IGrnRepository _Grn;
        private IPostoperativeRepository _Postoperative;
        private IMed _med;
        private IReportssrepository _reportss;
        public IMedicalBillRegisterRepository _MedicalBillRegister;
        public IPurchaseorderDetRepository _PurchaseorderDet;
        private IMedicineRepository _medicine;
        private IStockRepository _Stock;
        private IManagementDashRepository _managementDash;
        public ICancePORegRepository _CancePOReg;
        public IPurchaseOrderRegRepository _PurchaseOrderReg;
        private IPurchaseordercancellationRepository _Purchaseordercancellation;
        private IPurchaseOrderPrintingRepository _PurchaseOrderPrinting;
        private IPurchaseOrderCancellationPrintingRepository _PurchaseOrderCancellationPrinting;
        private IInvestigationPrescriptionRepository _InvestigationPrescription;
        private IOpticalPrescriptionRepository _OpticalPrescription;
        private IGrnWoPoRepository _GrnWoPo;
        private IFinalBillingRepository _FinalBilling;
        public ICompanyMasterRepository _CompanyMaster;
        public INumberControlRepository _NumberControl;
        private IOpticalBillingRepository _OpticalBilling;
        public ITaxMasterRepository _TaxMaster;
        public ITaxSummaryRepository _TaxSummary;
        public IInterDeparmentTransferRepository _InterDepartmentTransfer;
        public IInterDeparmentReceiverRepository _InterDeparmentReceiver;
        private IIpOpticalBillingRepository _IpOpticalBilling;
        private IExceluploadrepo _IExceluploadrepo;
        public IModuleMasterRepository _ModuleMaster;
        private IBusinessRuleRepository _BusinessRule;
        private IMeterialreturnRepository _meterial;
        public ICounsellingRepository _CounsellingMaster;
        private IDepartRepository _Depart;
        private IConfigureRepository _Configuration;
        private IFinancialRepository _financial;
        public IProfRepository _Proffesional;
        private IlocationRepository _Loc;
        public IOvertimeRepository _Overtime;
        public ITransactionTypeRepository _TransactionTypeViewM;
        private IOperationTheatreBookingRepository _OperationTheatreBooking;
        private IMedservRepository _medserv;
        private ICustomerMasterRepository _Customermaster;
        private IDonorRepository _Donor;
        private ICustomerOrderRepository _Customerorder;
        private IRoomMasterRepository _RoomMaster;
        private IAdvancePaymentRepository _AdvancePayment;
        public ILensMasterRepository _LensMaster;
        public ITreatmentMasterRepository _TreatmentMaster;
        public IQualificationRepository _Qualification;
        public IuniversityRepository _University;
        public IOpticalGrnRepository _OptiaclGrn;
        public IInsuranceRepository _Insurance;
        public IInsuranceVsMiddlemanRepository _InsuranceVsMiddleman;
        public IIndentRepository _Indentrepo;
        public IForeignNationalRepository _ForeignNational;
        public IOTConsumptionRepository _OTConsumption;
        public ICancellationReportRepository _CancellationReport;
        public IOfferRepository _Offer;
       public ISurgerySafetyCheckListRepository _SurgerySafetyCheckList;
        public IPatientVsInsuranceRepository _PatientVsInsurance;
        public IRoomTransferRepository _RoomTransfers;
        public ITonometryRepository _Tonometry;

        public IPatientQueueRepository _PatientQueue;
        public IUserLogRepository _UserLog;
        public IUsersVsRoleRepository _UsersVsRole;
        public IInvestigationBillingRepository _InvestigationBilling;
        public ISurgeryadmissionandassignRepository _Surgeryadmissionandassign;

        public IConcentUploadingRepository _Concent;
        public ISlitLampRepository _SlitLampViewM;
        public IFundusRepository _Fundus;
        public IOpticalStockSummaryRepository _OpticalStockSummary;
        public IDrugStockSummaryRepository _DrugStockSummary;
        public IOpticalStockLedgerRepository _OpticalStockLedger;
        public IMessagingTemplateRepository _MessagingTemplate;
        public IBMIRepository _BIM;
        public IPACRepository _PAC;
        public IIOProcedureTemplateRepository _IOProcedureTemplate;
        public IInvestigationIPBillingRepository _InvestigationIPBilling;
        public IBrandMasterRepository _BrandMaster;
        public ISetupMasterrepository _GetSetup;

        public IAppointmentRepository _AppointMent;

        public IYearEndProcessRepository _YearEndProcess;
        public IUomMasterRepository _UomMaster;
        public ISpecialityVSTestRepository _SpecialityVSTest;
        public ITonometryTranRepository _TonometryTran;
        public IOpticalBillRegisterRepository _OpticalBillRegister;
        public IUserListRepository _UserList;
        public IErrorlogRepository _Errorlog;
        public IAllergyrepository _Allergy;
        public IDiagnosisVSMedicineRepository _DiagnosisVSMedicine;
        public ICampMasterRepository _CampMaster;
        public ICampRegistrationRepository _CampRegistration;
        public ICampDashboardRepository _CampDashboard;
        public IForm3cRepository _form3c;
        public IMedicineMappingRepository _MedicineMapping;
        public IServiceMasterRepository _service;
        public IOpticalDashboardRepository _OpticalDashboard;
        public IServiceExtensionRepository _ServiceExtension;


        public RepositoryWrapper(WYNKContext context, CMPSContext Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
            
        }


        public IOpticalDashboardRepository OpticalDashboard
        {
            get
            {
                if (_OpticalDashboard == null)
                {
                    _OpticalDashboard = new OpticalDashboardRepository(_Wynkcontext, _Cmpscontext);

                }
                return _OpticalDashboard;
            }
        }
        public IServiceExtensionRepository ServiceExtension
        {
            get
            {
                if (_ServiceExtension == null)
                {
                    _ServiceExtension = new ServiceExtensionRepository(_Wynkcontext, _Cmpscontext);

                }
                return _ServiceExtension;
            }
        }
        public IServiceMasterRepository service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ServiceMasterRepository(_Wynkcontext, _Cmpscontext);

                }
                return _service;
            }
        }

        public IForm3cRepository form3c
        {
            get
            {
                if (_form3c == null)
                {
                    _form3c = new Form3cRepository(_Wynkcontext, _Cmpscontext);

                }
                return _form3c;
            }
        }

        public ICampDashboardRepository CampDashboard
        {
            get
            {
                if (_CampDashboard == null)
                {
                    _CampDashboard = new CampDashboardRepository(_Wynkcontext, _Cmpscontext);

                }
                return _CampDashboard;
            }
        }


        public ICampRegistrationRepository CampRegistration
        {
            get
            {
                if (_CampRegistration == null)
                {
                    _CampRegistration = new CampRegistrationRepository(_Wynkcontext, _Cmpscontext);

                }
                return _CampRegistration;
            }
        }
        public ICampMasterRepository CampMaster
        {
            get
            {
                if (_CampMaster == null)
                {
                    _CampMaster = new CampMasterRepository(_Wynkcontext, _Cmpscontext);

                }
                return _CampMaster;
            }
        }

        public IErrorlogRepository Errorlog
        {
            get
            {
                if (_Errorlog == null)
                {
                    _Errorlog = new ErrorlogRepository(_Wynkcontext, _Cmpscontext );

                }
                return _Errorlog;
            }
        }
        
              public ISquintExtnMasterRepository SquintExtnMaster
        {
            get
            {
                if (_SquintExtnMaster == null)
                {
                    _SquintExtnMaster = new SquintExtnMasterRepository(_Wynkcontext, _Cmpscontext );

                }
                return _SquintExtnMaster;
            }
        }
        public IUserListRepository UserList
        {
            get
            {
                if (_UserList == null)
                {
                    _UserList = new UserListRepository(_Wynkcontext, _Cmpscontext );

                }
                return _UserList;
            }
        }

        public IAllergyrepository Allergy
        {
            get
            {
                if (_Allergy == null)
                {
                    _Allergy = new Allergyrepository(_Wynkcontext, _Cmpscontext );

                }
                return _Allergy;
            }
        }

        public ISpecialityVSTestRepository SpecialityVSTest
        {
            get
            {
                if (_SpecialityVSTest == null)
                {
                    _SpecialityVSTest = new SpecialityVSTestRepository(_Wynkcontext, _Cmpscontext );

                }
                return _SpecialityVSTest;
            }
        }

        public IDiagnosisVSMedicineRepository DiagnosisVSMedicine
        {
            get
            {
                if (_DiagnosisVSMedicine == null)
                {
                    _DiagnosisVSMedicine = new DiagnosisVSMedicineRepository(_Wynkcontext, _Cmpscontext );

                }
                return _DiagnosisVSMedicine;
            }
        }

        public IMedicineMappingRepository MedicineMapping
        {
            get
            {
                if (_MedicineMapping == null)
                {
                    _MedicineMapping = new MedicineMappingRepository(_Wynkcontext, _Cmpscontext);

                }
                return _MedicineMapping;
            }
        }
        public IOpticalBillRegisterRepository OpticalBillRegister
        {
            get
            {
                if (_OpticalBillRegister == null)
                {
                    _OpticalBillRegister = new OpticalBillRegisterRepository(_Wynkcontext, _Cmpscontext );

                }
                return _OpticalBillRegister;
            }
        }
        public IUomMasterRepository UomMaster
        {
            get
            {
                if (_UomMaster == null)
                {
                    _UomMaster = new UomMasterRepository(_Wynkcontext, _Cmpscontext );

                }
                return _UomMaster;
            }
        }

        public IYearEndProcessRepository YearEndProcess
        {
            get
            {
                if (_YearEndProcess == null)
                {
                    _YearEndProcess = new YearEndProcessRepository(_Wynkcontext, _Cmpscontext );

                }

                return _YearEndProcess;

            }
        }

        public IAppointmentRepository AppointMent
        {
            get
            {
                if (_AppointMent == null)
                {
                    _AppointMent = new Appointmentrepository(_Wynkcontext, _Cmpscontext );

                }

                return _AppointMent;

            }
        }
        public IBrandMasterRepository BrandMaster
        {
            get
            {
                if (_BrandMaster == null)
                {
                    _BrandMaster = new BrandMasterRepository(_Wynkcontext, _Cmpscontext );

                }

                return _BrandMaster;

            }
        }


        public ITonometryRepository Tonometry
        {
            get
            {
                if (_Tonometry == null)
                {
                    _Tonometry = new TonometryRepository(_Wynkcontext, _Cmpscontext );

                }

                return _Tonometry;

            }
        }


        public ISetupMasterrepository GetSetup
        {
            get
            {
                if (_GetSetup == null)
                {
                    _GetSetup = new SetupRepository(_Wynkcontext, _Cmpscontext );

                }

                return _GetSetup;

            }
        }

        public IInvestigationIPBillingRepository InvestigationIPBilling
        {
            get
            {
                if (_InvestigationIPBilling == null)
                {
                    _InvestigationIPBilling = new InvestigationIPBillingRepository(_Wynkcontext, _Cmpscontext );

                }

                return _InvestigationIPBilling;

            }
        }
        public IIOProcedureTemplateRepository IOProcedureTemplate
        {
            get
            {
                if (_IOProcedureTemplate == null)
                {
                    _IOProcedureTemplate = new IOProcedureTemplateRepository(_Wynkcontext, _Cmpscontext );

                }

                return _IOProcedureTemplate;

            }
        }


        public ITonometryTranRepository TonometryTran
        {
            get
            {
                if (_TonometryTran == null)
                {
                    _TonometryTran = new TonometryTranRepository(_Wynkcontext, _Cmpscontext );

                }

                return _TonometryTran;

            }
        }
        public IPACRepository PACViewM
        {
            get
            {
                if (_PAC == null)
                {
                    _PAC = new PACRepository(_Wynkcontext, _Cmpscontext );

                }

                return _PAC;

            }
        }
        public IBMIRepository BIM
        {
            get
            {
                if (_BIM == null)
                {
                    _BIM = new BMIRepository(_Wynkcontext, _Cmpscontext );

                }

                return _BIM;

            }
        }

        public IRoomTransferRepository RoomTransfer
        {
            get
            {
                if (_RoomTransfers == null)
                {
                    _RoomTransfers = new RoomTransferRepository(_Wynkcontext, _Cmpscontext );

                }

                return _RoomTransfers;

            }
        }
        public IMessagingTemplateRepository MessagingTemplate
        {
            get
            {
                if (_MessagingTemplate == null)
                {
                    _MessagingTemplate = new MessageTemplateRepository(_Wynkcontext, _Cmpscontext );

                }

                return _MessagingTemplate;

            }
        }
        public IFundusRepository FundusViewM
        {
            get
            {
                if (_Fundus == null)
                {
                    _Fundus = new FundusRepository(_Wynkcontext, _Cmpscontext );

                }

                return _Fundus;

            }
        }


        public IOpticalStockSummaryRepository OpticalStockSummary
        {
            get
            {
                if (_OpticalStockSummary == null)
                {
                    _OpticalStockSummary = new OpticalStockSummaryRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OpticalStockSummary;
            }
        }

        public IDrugStockSummaryRepository DrugStockSummary
        {
            get
            {
                if (_DrugStockSummary == null)
                {
                    _DrugStockSummary = new DrugStockSummaryRepository(_Wynkcontext, _Cmpscontext );
                }

                return _DrugStockSummary;
            }
        }

        public IOpticalStockLedgerRepository OpticalStockLedger
        {
            get
            {
                if (_OpticalStockLedger == null)
                {
                    _OpticalStockLedger = new OpticalStockLedgerRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OpticalStockLedger;
            }
        }
        public ISlitLampRepository SlitLampViewM
        {
            get
            {
                if (_SlitLampViewM == null)
                {
                    _SlitLampViewM = new SlitLampRepository(_Wynkcontext, _Cmpscontext );

                }

                return _SlitLampViewM;

            }
        }
        public IInvestigationBillingRepository InvestigationBilling
        {
            get
            {
                if (_InvestigationBilling == null)
                {
                    _InvestigationBilling = new InvestigationBillingRepository(_Wynkcontext, _Cmpscontext );

                }

                return _InvestigationBilling;

            }
        }

        public IPatientVsInsuranceRepository PatientVsInsurance
        {
            get
            {
                if (_PatientVsInsurance == null)
                {
                    _PatientVsInsurance = new PatientVsInsuranceRepository(_Wynkcontext, _Cmpscontext );

                }

                return _PatientVsInsurance;

            }
        }

        public IPatientQueueRepository PatientQueue
        {
            get
            {
                if (_PatientQueue == null)
                {
                    _PatientQueue = new PatientQueueRepository(_Wynkcontext, _Cmpscontext );

                }

                return _PatientQueue;

            }
        }

        public IUserLogRepository UserLogViewM
        {
            get
            {
                if (_UserLog == null)
                {
                    _UserLog = new UserLogRepository(_Wynkcontext, _Cmpscontext );

                }

                return _UserLog;

            }
        }

        public ISurgeryadmissionandassignRepository Surgeryadmissionandassign
        {
            get
            {
                if (_Surgeryadmissionandassign == null)
                {
                    _Surgeryadmissionandassign = new SurgeryadmissionandassignRepository(_Wynkcontext, _Cmpscontext);
                }

                return _Surgeryadmissionandassign;
            }
        }



        public IConcentUploadingRepository ConcentUploading
        {
            get
            {
                if (_Concent == null)
                {
                    _Concent = new ConcentUploadingRepository(_Wynkcontext, _Cmpscontext );

                }

                return _Concent;

            }
        }
        public ISurgerySafetyCheckListRepository SurgerySafetyCheckList
        {
            get
            {
                if (_SurgerySafetyCheckList == null)
                {
                    _SurgerySafetyCheckList = new SurgerySafetyCheckListRepository(_Wynkcontext, _Cmpscontext );

                    

                }
                return _SurgerySafetyCheckList;
            }
        }
        public ICancellationReportRepository CancellationViewM
        {
            get
            {
                if (_CancellationReport == null)
                {
                    _CancellationReport = new CancellationReportRepository(_Wynkcontext, _Cmpscontext );

                }

                return _CancellationReport;

            }
        }
        public IOfferRepository OfferViewM
        {
            get
            {
                if (_Offer == null)
                {
                    _Offer = new OfferRepository(_Wynkcontext, _Cmpscontext );

                }
                return _Offer;
            }

        }
        public IOTConsumptionRepository OTConsumption
        {
            get
            {
                if (_OTConsumption == null)
                {
                    _OTConsumption = new OTConsumptionRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OTConsumption;
            }
        }

        public IUsersVsRoleRepository UsersVsRole
        {
            get
            {
                if (_UsersVsRole == null)
                {
                    _UsersVsRole = new UserVsRoleRepository(_Wynkcontext, _Cmpscontext );
                }

                return _UsersVsRole;
            }
        }
        public IIndentRepository Indentrepo
        {
            get
            {
                if (_Indentrepo == null)
                {
                    _Indentrepo = new IndentRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Indentrepo;
            }
        }
        public IForeignNationalRepository ForeignNational
        {
            get
            {
                if (_ForeignNational == null)
                {
                    _ForeignNational = new ForeignNationalRepository(_Wynkcontext, _Cmpscontext );
                }

                return _ForeignNational;
            }
        }


        public IOpticalGrnRepository OpticalGrn
        {
            get
            {
                if (_OptiaclGrn == null)
                {
                    _OptiaclGrn = new OpticalGrnRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OptiaclGrn;
            }
        }

        public IInsuranceRepository Insurance
        {
            get
            {
                if (_Insurance == null)
                {
                    _Insurance = new InsuranceRepository  (_Wynkcontext, _Cmpscontext );
                }

                return _Insurance;
            }
        }
        public IInsuranceVsMiddlemanRepository InsuranceVsMiddleman
        {
            get
            {
                if (_InsuranceVsMiddleman == null)
                {
                    _InsuranceVsMiddleman = new InsuranceVsMiddlemanRepository(_Wynkcontext, _Cmpscontext );
                }

                return _InsuranceVsMiddleman;
            }
        }
        public IRoomMasterRepository RoomMasterViewM
        {
            get
            {
                if (_RoomMaster == null)
                {
                    _RoomMaster = new RoomMasterRepository(_Wynkcontext, _Cmpscontext );

                }

                return _RoomMaster;

            }
        }

        public IOpticalOrderRepository OpticalOrder
        {
            get
            {
                if (_OpticalOrder == null)
                {
                    _OpticalOrder = new OpticalOrderRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OpticalOrder;
            }
        }

        public IQualificationRepository QualifyMaster
        {
            get
            {
                if (_Qualification == null)
                {
                    _Qualification = new QualificationRepository(_Wynkcontext, _Cmpscontext );

                }

                return _Qualification;

            }
        }

        public IuniversityRepository UniversityMaster
        {
            get
            {
                if (_University == null)
                {
                    _University = new UniversityRepository(_Wynkcontext, _Cmpscontext );

                }

                return _University;

            }
        }
        public IMedservRepository medserv
        {
            get
            {
                if (_medserv == null)
                {
                    _medserv = new Medservrepository(_Wynkcontext, _Cmpscontext );
                }

                return _medserv;
            }
        }

        public IlocationRepository Loc
        {
            get
            {
                if (_Loc == null)
                {
                    _Loc = new LocationRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Loc;
            }
        }
        public IAdvancePaymentRepository AdvancePayment
        {
            get
            {
                if (_AdvancePayment == null)
                {
                    _AdvancePayment = new AdvancePaymentRepository(_Wynkcontext, _Cmpscontext );
                }

                return _AdvancePayment;
            }
        }

        public ICustomerMasterRepository Customermaster
        {
            get
            {
                if (_Customermaster == null)
                {
                    _Customermaster = new CustomerMasterRepository(_Wynkcontext, _Cmpscontext );
                }
                return _Customermaster;
            }

        }
        public IDonorRepository Donor
        {
            get
            {
                if (_Donor == null)
                {
                    _Donor = new DonorRepository(_Wynkcontext, _Cmpscontext);
                }
                return _Donor;
            }

        }
        public ICustomerOrderRepository Customerorder
        {
            get
            {
                if (_Customerorder == null)
                {
                    _Customerorder = new CustomerOrderRepository(_Wynkcontext, _Cmpscontext );
                }
                return _Customerorder;
            }

        }


        public ILensMasterRepository LensMaster
        {
            get
            {
                if (_LensMaster == null)
                {

                    _LensMaster = new LensMasterRepository(_Wynkcontext, _Cmpscontext );
                }
                return _LensMaster;
            }
        }
        public IOperationTheatreBookingRepository OperationTheatreBooking
        {
            get
            {
                if (_OperationTheatreBooking == null)
                {
                    _OperationTheatreBooking = new OperationTheatreBookingRepository(_Wynkcontext, _Cmpscontext );

                }

                return _OperationTheatreBooking;

            }
        }

        public ITransactionTypeRepository TransactionTypeViewM
        {
            get
            {
                if (_TransactionTypeViewM == null)
                {
                    _TransactionTypeViewM = new TransactionTypeRepository(_Wynkcontext, _Cmpscontext );
                }

                return _TransactionTypeViewM;
            }
        }

        public ITreatmentMasterRepository TreatmentMaster
        {
            get
            {
                if (_TreatmentMaster == null)
                {
                    _TreatmentMaster = new TreatmentMasterRepository(_Wynkcontext, _Cmpscontext );
                }
                return _TreatmentMaster;
            }
        }

        public IOvertimeRepository OvertimeViewM
        {
            get
            {
                if (_Overtime == null)
                {
                    _Overtime = new OvertimeRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Overtime;
            }
        }

        public IProfRepository Prof
        {
            get
            {
                if (_Proffesional == null)
                {
                    _Proffesional = new ProfRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Proffesional;
            }
        }
        public IConfigureRepository Configure
        {
            get
            {
                if (_Configuration == null)
                {
                    _Configuration = new ConfigureRepository(_Wynkcontext, _Cmpscontext );

                }
                return _Configuration;
            }
        }


        public IDepartRepository Depart
        {
            get
            {
                if (_Depart == null)
                {
                    _Depart = new DepartRepository(_Wynkcontext, _Cmpscontext );
                }
                return _Depart;
            }
        }


        public IFinancialRepository Financial
        {
            get
            {
                if (_financial == null)
                {
                    _financial = new FinancialRepository(_Wynkcontext, _Cmpscontext );

                }
                return _financial;
            }

        }

        public IMeterialreturnRepository Meterial
        {
            get
            {
                if (_meterial == null)
                {
                    _meterial = new MeterialreturnRepository(_Wynkcontext, _Cmpscontext );
                }
                return _meterial;
            }
        }

        public IBusinessRuleRepository BusinessRule
        {
            get
            {
                if (_BusinessRule == null)
                {
                    _BusinessRule = new BusinessRuleRepository(_Wynkcontext, _Cmpscontext );
                }

                return _BusinessRule;
            }
        }

        public ICounsellingRepository CounsellingRepository
        {
            get
            {
                if (_CounsellingMaster == null)
                {
                    _CounsellingMaster = new CounsellingRepository(_Wynkcontext, _Cmpscontext );
                }

                return _CounsellingMaster;
            }
        }

        public IModuleMasterRepository ModuleMasterViewM
        {
            get
            {
                if (_ModuleMaster == null)
                {
                    _ModuleMaster = new ModuleMasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _ModuleMaster;
            }
        }


        public IExceluploadrepo IExceluploadrepo
        {
            get
            {
                if (_IExceluploadrepo == null)
                {
                    _IExceluploadrepo = new Exceluploadrepo(_Wynkcontext, _Cmpscontext );

                }

                return _IExceluploadrepo;

            }
        }


        public ITaxSummaryRepository TaxSummaryViewM
        {
            get
            {
                if (_TaxSummary == null)
                {
                    _TaxSummary = new TaxSummaryRepository(_Wynkcontext, _Cmpscontext );

                }

                return _TaxSummary;

            }
        }


        public ITaxMasterRepository TaxMasterViewM
        {
            get
            {
                if (_TaxMaster == null)
                {
                    _TaxMaster = new TaxMasterRepository(_Wynkcontext, _Cmpscontext );

                }

                return _TaxMaster;

            }
        }

        public IFinalBillingRepository FinalBilling
        {
            get
            {
                if (_FinalBilling == null)
                {
                    _FinalBilling = new FinalBillingRepository(_Wynkcontext, _Cmpscontext );
                }

                return _FinalBilling;
            }
        }




        public INumberControlRepository NumberControl
        {
            get
            {
                if (_NumberControl == null)
                {
                    _NumberControl = new NumberControlRepository(_Wynkcontext, _Cmpscontext );
                }

                return _NumberControl;
            }
        }

        public ICompanyMasterRepository CompanyMasterView
        {
            get
            {
                if (_CompanyMaster == null)
                {
                    _CompanyMaster = new CompanyMasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _CompanyMaster;
            }
        }

        public IGrnWoPoRepository GrnWoPo
        {
            get
            {
                if (_GrnWoPo == null)
                {
                    _GrnWoPo = new GrnWoPoRepository(_Wynkcontext, _Cmpscontext );
                }

                return _GrnWoPo;
            }
        }


        public IOpticalPrescriptionRepository OpticalPrescription
        {
            get
            {
                if (_OpticalPrescription == null)
                {
                    _OpticalPrescription = new OpticalPrescriptionRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OpticalPrescription;
            }
        }



        public IOpticalBillingRepository OpticalBilling
        {
            get
            {
                if (_OpticalBilling == null)
                {
                    _OpticalBilling = new OpticalBillingRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OpticalBilling;
            }
        }

        public IIpOpticalBillingRepository IpOpticalBilling
        {
            get
            {
                if (_IpOpticalBilling == null)
                {
                    _IpOpticalBilling = new IpOpticalBillingRepository(_Wynkcontext, _Cmpscontext );
                }

                return _IpOpticalBilling;
            }
        }


        public IInvestigationPrescriptionRepository InvestigationPrescription
        {
            get
            {
                if (_InvestigationPrescription == null)
                {
                    _InvestigationPrescription = new InvestigationPrescriptionRepository(_Wynkcontext, _Cmpscontext );
                }

                return _InvestigationPrescription;
            }
        }

        public IPurchaseOrderCancellationPrintingRepository PurchaseOrderCancellationPrinting
        {
            get
            {
                if (_PurchaseOrderCancellationPrinting == null)
                {
                    _PurchaseOrderCancellationPrinting = new PurchaseOrderCancellationPrintingRepository(_Wynkcontext, _Cmpscontext );
                }

                return _PurchaseOrderCancellationPrinting;
            }
        }

        public IPurchaseOrderPrintingRepository PurchaseOrderPrinting
        {
            get
            {
                if (_PurchaseOrderPrinting == null)
                {
                    _PurchaseOrderPrinting = new PurchaseOrderPrintingRepository(_Wynkcontext, _Cmpscontext );
                }

                return _PurchaseOrderPrinting;
            }
        }


        public IPurchaseordercancellationRepository Purchaseordercancellation
        {
            get
            {
                if (_Purchaseordercancellation == null)
                {
                    _Purchaseordercancellation = new PurchaseordercancellationRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Purchaseordercancellation;
            }
        }




        public ICancePORegRepository CancelPORegViewModel
        {
            get
            {
                if (_CancePOReg == null)
                {
                    _CancePOReg = new CancelPORegRepostiory(_Wynkcontext, _Cmpscontext );

                }
                return _CancePOReg;

            }
        }

        public IPurchaseOrderRegRepository PurchaseOrderRegView
        {
            get
            {
                if (_PurchaseOrderReg == null)
                {
                    _PurchaseOrderReg = new PurchaseOrderRegRepository(_Wynkcontext, _Cmpscontext );

                }
                return _PurchaseOrderReg;

            }
        }


        public IStockRepository Stock
        {
            get
            {
                if (_Stock == null)
                {
                    _Stock = new StockRepository(_Wynkcontext, _Cmpscontext );
                }
                return _Stock;
            }
        }

        public IManagementDashRepository managementDash
        {
            get
            {
                if (_managementDash == null)
                {
                    _managementDash = new ManagementDashboardRepository(_Wynkcontext, _Cmpscontext );
                }
                return _managementDash;
            }
        }


        public IMedicineRepository Medicine
        {
            get
            {
                if (_medicine == null)
                {
                    _medicine = new MedicineRepository(_Wynkcontext, _Cmpscontext );
                }
                return _medicine;
            }
        }


        public IPurchaseorderDetRepository PurchaseOrdDet
        {
            get
            {
                if (_PurchaseorderDet == null)
                {
                    _PurchaseorderDet = new PurchaseOrderDetRepository(_Wynkcontext, _Cmpscontext );

                }
                return _PurchaseorderDet;

            }
        }


        public IMedicalBillRegisterRepository medBillReg
        {
            get
            {
                if (_MedicalBillRegister == null)
                {
                    _MedicalBillRegister = new MedicalBillRegisterRepository(_Wynkcontext, _Cmpscontext );

                }
                return _MedicalBillRegister;

            }
        }

        public IInterDeparmentTransferRepository InterDepartmentTransfer
        {
            get
            {
                if (_InterDepartmentTransfer == null)
                {
                    _InterDepartmentTransfer = new InterDepartmentTransferRepository(_Wynkcontext, _Cmpscontext );

                }
                return _InterDepartmentTransfer;
            }
        }

        public IInterDeparmentReceiverRepository InterDeparmentReceiver
        {
            get
            {
                if (_InterDeparmentReceiver == null)
                {
                    _InterDeparmentReceiver = new InterDepartmentReceiverRepository(_Wynkcontext, _Cmpscontext );

                }
                return _InterDeparmentReceiver;
            }
        }


        public IMed medeg
        {
            get
            {
                if (_med == null)
                {
                    _med = new Medrepository(_Wynkcontext, _Cmpscontext );
                }

                return _med;
            }
        }
        public IReportssrepository Reportss
        {
            get
            {
                if (_reportss == null)
                {
                    _reportss = new Reportssrepository(_Wynkcontext, _Cmpscontext );
                }
                return _reportss;
            }
        }

        public IGrnRepository Grn
        {
            get
            {
                if (_Grn == null)
                {
                    _Grn = new GrnRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Grn;
            }
        }

        public IPatientRepository Registration
        {
            get
            {
                if (_Registration == null)
                {
                    _Registration = new PatientRepository(_Wynkcontext, _Cmpscontext );
                }
                return _Registration;
            }
        }


        public IPostoperativeRepository Postoperative

        {
            get
            {
                if (_Postoperative == null)
                {
                    _Postoperative = new PostoperativeRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Postoperative;
            }
        }


        public ISurgeryDischargeRepository surgeryDischarge
        {
            get
            {
                if (_surgeryDischarge == null)
                {
                    _surgeryDischarge = new SurgeryDischargeRepository(_Wynkcontext, _Cmpscontext );
                }

                return _surgeryDischarge;
            }
        }

        public ISurgeryRepository SurgeryMaster
        {
            get
            {
                if (_Surgery == null)
                {
                    _Surgery = new SurgeryRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Surgery;
            }
        }


        public IpurchaseorderRepository purchaseorder

        {
            get
            {
                if (_purchaseorder == null)
                {
                    _purchaseorder = new purchaseorderRepository(_Wynkcontext, _Cmpscontext );
                }

                return _purchaseorder;
            }
        }
        public IOperationTheatreRepository OperationTheatre

        {
            get
            {
                if (_OperationTheatre == null)
                {
                    _OperationTheatre = new OperationTheatreRepository(_Wynkcontext, _Cmpscontext );
                }

                return _OperationTheatre;
            }
        }


        public IInvestigationRepository Investigation
        {
            get
            {
                if (_Investigation == null)
                {
                    _Investigation = new InvestigationRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Investigation;
            }
        }
        public IBillingMasterRepository Billing

        {
            get
            {
                if (_Billing == null)
                {
                    _Billing = new BillingMasterrepository(_Wynkcontext, _Cmpscontext );
                }

                return _Billing;
            }
        }

        public IStoremasterRepository Storemaster

        {
            get
            {
                if (_Storemaster == null)
                {
                    _Storemaster = new StoremasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Storemaster;
            }
        }

        public IRoleVsAccessRespository RoleVsAccess

        {
            get
            {
                if (_RoleVsAccess == null)
                {
                    _RoleVsAccess = new RoleVsAccessRepository(_Wynkcontext, _Cmpscontext );
                }

                return _RoleVsAccess;
            }
        }



        public IVendorMasterRepository VendorMaster
        {
            get
            {
                if (_VendorMas == null)
                {
                    _VendorMas = new VendorMasterRepository(_Wynkcontext, _Cmpscontext );
                }
                return _VendorMas;
            }
        }

        public IDrugMasterRepository drugMaster

        {
            get
            {
                if (_drugMaster == null)
                {
                    _drugMaster = new DrugMasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _drugMaster;
            }
        }


        public IEmployeeMasterrepository employee

        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeMasterrepository(_Wynkcontext, _Cmpscontext );
                }

                return _employee;
            }
        }

        public IOneLineMasterRepository OneLineMaster

        {
            get
            {
                if (_Oneline == null)
                {
                    _Oneline = new OneLineMasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Oneline;
            }
        }
        public IrefractionRepository refraction

        {
            get
            {
                if (_refraction == null)
                {
                    _refraction = new RefractionRepository(_Wynkcontext, _Cmpscontext );
                }

                return _refraction;
            }
        }


        public IUserRepository User
        {
            get
            {
                if (_User == null)
                {
                    _User = new UserRepository(_Wynkcontext, _Cmpscontext );
                }

                return _User;
            }
        }

        public IFindingsRepository Findings
        {
            get
            {
                if (_Findings == null)
                {
                    _Findings = new FindingsRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Findings;
            }
        }

        public IPatientHistoryRepository PatientHistory
        {
            get
            {
                if (_PatientHistory == null)
                {
                    _PatientHistory = new PatientHistoryRepository(_Wynkcontext, _Cmpscontext );
                }
                return _PatientHistory;
            }
        }



        public IOcularComplaintsRepository OcularComplaints
        {
            get
            {
                if (_OcularComplaints == null)
                {
                    _OcularComplaints = new OcularComplaintsRepository(_Wynkcontext, _Cmpscontext );
                }
                return _OcularComplaints;
            }
        }


        public IHelpRepository Help
        {
            get
            {
                if (_Help == null)
                {
                    _Help = new HelpRepository(_Wynkcontext, _Cmpscontext );
                }

                return _Help;
            }
        }

        public IcdMasterRepository icdMaster
        {
            get
            {
                if (_icdMaster == null)
                {
                    _icdMaster = new IcdMasterrepository(_Wynkcontext, _Cmpscontext );
                }

                return _icdMaster;
            }
        }



        public ICommonRepository Common
        {
            get
            {
                if (_common == null)
                {
                    _common = new CommonRepository(_Wynkcontext, _Cmpscontext );
                }

                return _common;
            }
        }


       


        public IRegistrationMasterRepository RegistrationMaster
        {
            get
            {
                if (_RegistrationMaster == null)
                {
                    _RegistrationMaster = new RegistrationMasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _RegistrationMaster;
            }
        }



        public IMedicalPrescriptionRepository MedicalPrescription
        {
            get
            {
                if (_MedicalPrescription == null)
                {
                    _MedicalPrescription = new MedicalPrescriptionRepository(_Wynkcontext, _Cmpscontext );
                }

                return _MedicalPrescription;
            }
        }

        public IDoctorMasterRepository doctorMaster
        {
            get
            {
                if (_doctorMaster == null)
                {
                    _doctorMaster = new DoctorMasterrepository(_Wynkcontext, _Cmpscontext );
                }

                return _doctorMaster;
            }
        }



        public IReferralMasterRepository ReferralMaster
        {
            get
            {
                if (_ReferralMaster == null)
                {
                    _ReferralMaster = new ReferralMasterRepository(_Wynkcontext, _Cmpscontext );
                }

                return _ReferralMaster;
            }
        }


    }

}