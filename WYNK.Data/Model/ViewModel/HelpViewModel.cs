using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Help
    {
        public ICollection<Copat> Copat { get; set; }//ConsDet
        public ICollection<FullDet> FullDet { get; set; }
        public ICollection<ConfigureDetails> ConfigureDetail { get; set; }
        public ICollection<getTaxDetS> getTaxDet { get; set; }
        public ICollection<PatientAssignStatus> PatientAssignStatus { get; set; }
        public ICollection<OperationExtensionDetails> OperationExtensionDetails { get; set; }
        public ICollection<docdetail> docdetail { get; set; }
        public ICollection<Regdetail> Regdetail { get; set; }
        public ICollection<Appdetail> Appdetail { get; set; }
        public ICollection<CampSurgerypatients> CampSurgerypatients { get; set; }
        public ICollection<CampSurgeryunderwentpatients> CampSurgeryunderwentpatients { get; set; }
        public string CAddress { get; set; }
        public string Cphone { get; set; }
        public string Cweb { get; set; }
        public string Cname { get; set; }
        public ICollection<CampwiseSummaryCount> CampwiseSummaryCount { get; set; }
        public ICollection<CampwiseSurgeryCount> CampwiseSurgeryCount { get; set; }
        public ICollection<CampwiseSurgeryunderwentCount> CampwiseSurgeryunderwentCount { get; set; }
        public ICollection<FBdetail> FBdetail { get; set; }
        public ICollection<helpmaterialreturn> helpmaterialreturn { get; set; }
        public ICollection<Surgerydetails> Surgerydetail { get; set; }
        public ICollection<ComplaintsDetails> ComplaintsDetails { get; set; }
        public ICollection<PatientHistoryDetails> PatientHistorys { get; set; }
        public ICollection<IcdDetail> IcdDetail { get; set; }
        public ICollection<UserDoc> UserDoc { get; set; }
        public ICollection<UserEmp> UserEmp { get; set; }
        public ICollection<docdetail1> docdetail1 { get; set; }
        public ICollection<EmployeeDetail> EmployeeDetail { get; set; }
        public ICollection<DrugDetail> DrugDetail { get; set; }
        public ICollection<DrugGroupDetail> DrugGroupDetail { get; set; }
        public ICollection<VendorMas> Vendordetail { get; set; }
        public ICollection<Genraldetails> Familyhistorydetails { get; set; }
        public ICollection<StoreMaster> Storedetail { get; set; }
        public ICollection<OperationTheatreDetails> OperationTheatreDetails { get; set; }
        public IntraOperative Intraoperative { get; set; }
        public PreOperative PreOperative { get; set; }
        public ICollection<Surgerydetail1> SurgeryDT1 { get; set; }
        public ICollection<Surgerydetail2> SurgeryDT2 { get; set; }
        public ICollection<Surgerydetail3> SurgeryDT3 { get; set; }
        public ICollection<Surgerydetail4> SurgeryDT4 { get; set; }
        public ICollection<Surgerydetail5> SurgeryDT5 { get; set; }
        public ICollection<Surgerydetail> SurgeryDT { get; set; }
        public ICollection<SurgeryDischargedetail> SurgeryDischarge { get; set; }
        public ICollection<SurgeryPatientDischargedDetails> SurgeryPatientDischargedDetails { get; set; }
        public ICollection<PreviousSurgeryDetails> PreviousSurgeryDetails { get; set; }
        public ICollection<getModuleMasDetails> getModuleMasDetails { get; set; }
        public string cmed { get; set; }
        public string all { get; set; }
        public string fhis { get; set; }
        public Postoper Postoper { get; set; }
        public ICollection<Postoper1> Postoper1 { get; set; }
        public ICollection<Postoper2> Postoper2 { get; set; }
        public ICollection<Postoper3> Postoper3 { get; set; }
        public ICollection<Employeestatutory> Employeestatutory { get; set; }
        public SurgeryTdetail SurgeryDTT { get; set; }
        public ICollection<SurgeryTdetail1> SurgeryDTT1 { get; set; }
        public ICollection<SurgeryTdetail2> SurgeryDTT2 { get; set; }
        public ICollection<SurgeryTdetail3> SurgeryDTT3 { get; set; }
        public latestadminandsur latestadminandsur { get; set; }
        public preoperativelat preoperativelat { get; set; }
        public Intraoperativelat Intraoperativelat { get; set; }
        public Postoperativelast Postoperativelast { get; set; }
        public ICollection<InvPre> InvPre1 { get; set; }
        public ICollection<InvPre1> InvPre2 { get; set; }
        public ICollection<NumCol> NumColdetail { get; set; }
        public ICollection<Employeebank> Employeebank { get; set; }
        public ICollection<Employeebank1> Employeebank1 { get; set; }
        public ICollection<Employeeexperience> Employeeexperience { get; set; }
        public ICollection<Employeequa> Employeequa { get; set; }
        public string DRNAME { get; set; }
        public string DRID { get; set; }
        public string P1Address { get; set; }
        public string P1Address2 { get; set; }
        public string P1phone { get; set; }
        public string P1web { get; set; }
        public string P1Compnayname { get; set; }
        public ICollection<FundusDet> FundusDet { get; set; }
        public ICollection<getSlitLampDet> getSlitLampDet { get; set; }
        public ICollection<getTrantypeDetails> getTrantypeDetails { get; set; }
        public ICollection<getBusinessRule2> getBusinessRule2 { get; set; }
        public ICollection<getBusinessRule1> getBusinessRule1 { get; set; }
        public ICollection<getInsurance> getInsurance { get; set; }
        public ICollection<getCompDetails1> getCompDetails1 { get; set; }
        public ICollection<getCompName1> getCompName1 { get; set; }
        public ICollection<getDrugGroupDetails> getDrugGroupDetails { get; set; }
        public ICollection<GetDataArraysss> GetDataArraysss { get; set; }
        public ICollection<GetDataArrayss> GetDataArrayss { get; set; }
        public ICollection<helpRoom> helpRoom { get; set; }
        public ICollection<storedetailsDetails> Storeroom { get; set; }
        public ICollection<CustomerDetails> CustomerDetails { get; set; }
        public ICollection<DonorDetails> DonorDetails { get; set; }
        public ICollection<OfferDetail> OfferDetails { get; set; }
        public ICollection<GetOpticalGrn> GetOpticalGrn { get; set; }
        public ICollection<Getindentsearchdetails> Getindentsearchdetails { get; set; }
        public ICollection<UserDetails> UserDetails { get; set; }
        public ICollection<Getindentsearchtotaldetails> Getindentsearchtotaldetails { get; set; }
        public string Operationtheatre { get; set; }
        public string Indentraisedby { get; set; }
        public DateTime Indentdate { get; set; }
        public string storename { get; set; }
        public string Address1 { get; set; }

        public string Indewntnumber { get; set; }
        public string Address2 { get; set; }

        public string Storekeeprr { get; set; }

        public string Storenumber { get; set; }

        public bool status { get; set; }

        public string Yesstatus { get; set; }
        public ICollection<Advpaydetail> Advpaydetail { get; set; }
        public ICollection<BMIDetails> BMIDetails { get; set; }
        public ICollection<getPACDetail> getPACDetail { get; set; }
        public ICollection<SurgeryTypeDet> SurgeryTypeDet { get; set; }
        public ICollection<GetRoomTransfer> GetRoomTransfer { get; set; }
        public ICollection<getPatientInsurancedetail> getPatientInsurancedetail { get; set; }
        public ICollection<getMedicalRegistersdetail> getMedicalRegistersdetail { get; set; }

        public ICollection<getInvRegdetail> getInvRegdetail { get; set; }

        //public ICollection<CampScreenedPat> CampScreenedPat { get; set; }

        public ICollection<getInvBillRegtaxsummary> getInvBillRegTaxsummary { get; set; }
        public decimal? Amountsum { get; set; }
        public decimal? DiscountAmtsum { get; set; }
        public decimal? TRate { get; set; }
        public decimal? TTAXAMOUNT { get; set; }
        
            
        public decimal? GrossAmountsum    { get; set; }
        public decimal? TotalAmountsum { get; set; }


        public decimal? TAXTotalProductValue{ get; set; }
        public decimal? TAXCESSAmount       { get; set; }
        public decimal? TAXAddCESSAmount    { get; set; }
        public decimal? TAXTaxPayable       { get; set; }
        public decimal? TAXGSTTaxValue      { get; set; }
        public decimal? TAXTaxableTurnover { get; set; }

        public ICollection<getDrugdetail> getDrugdetail { get; set; }
        public ICollection<getDrugdetail1> getDrugdetail1 { get; set; }
        
        public ICollection<getAPpointmentdetails> getAPpointmentdetailsss { get; set; }
    }
    public class FullDet
    {
        public DateTime Dttime { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string dname { get; set; }
        public Decimal? Charges { get; set; }

    }
    public class Copat
    {
        public DateTime Date { get; set; }
        public int NoOfPat { get; set; }
        public Decimal? Charges { get; set; }

    }
    public class CampwiseSummaryCount
    {
       
        public string CampName { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Transgender { get; set; }
    }
    public class CampwiseSurgeryCount
    {

        public string CampName { get; set; }
        public string ICDSpeciality { get; set; }
        public int Surgerycount { get; set; }
 
    }
    public class CampwiseSurgeryunderwentCount
    {

        public string CampName { get; set; }
        public string ICDSpeciality { get; set; }
        public int Surgerycount { get; set; }

    }
    
    public class getInvBillRegtaxsummary
    {

        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public int? TaxID { get; set; }
        public decimal? TotalProductValue { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AddCESSAmount { get; set; }
        public decimal? TaxPayable { get; set; }
        public decimal? TaxableTurnover { get; set; }

    }
    public class getAPpointmentdetails
    {
        public string UIN { get; set; }        
        public string Name { get; set; }       
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Doctor { get; set; }
    }
    public class getDrugdetail1
    {
        public int DrugID { get; set; }
        public decimal? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemValue { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AdditionalCESSValue { get; set; }
    }
        public class getDrugdetail
    {
        public int DrugID { get; set; }
        public decimal? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemValue { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public decimal? CESSValue { get; set; }
        public decimal? AdditionalCESSValue { get; set; }  
    }
        public class getMedicalRegistersdetail
    {      
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public decimal? GrossProductValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalTaxValue { get; set; }
        public decimal? TotalBillValue { get; set; }                              
        }




    public class getInvRegdetail
    {
      public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string   PatName { get; set; }
        public string InvDescription { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? GST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AdditionalCESS { get; set; }
        public string TaxDescription { get; set; }
        public string CESSDescription { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public decimal? GSTAmount { get; set; }
        public decimal? CESSAmount { get; set; }
        public decimal? AddlCESSAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public string CompanyName { get; set; }
        public string CAddress1 { get; set; }
        public string CAddress2 { get; set; }
        public string CAddress3 { get; set; }
        public string CPhone1 { get; set; }
        public string CWebsite { get; set; }
    } 




    public class getPatientInsurancedetail
        {
        public string UIN { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Name { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }    
        public string Age { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Phone { get; set; }
        }
    public class GetRoomTransfer
    {
        public string Time { get; set; }
        public DateTime? FromRoomDate { get; set; }
        public string FromRoomTime { get; set; }
        public string UIN { get; set; }
        public string PatientName { get; set; }
        public DateTime DateofBirth { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }

    }
    public class SurgeryTypeDet
    {
        public string surgerytype { get; set; }
    }
    public class BMIDetails
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public decimal? FromRange { get; set; }
        public decimal? ToRange { get; set; }
    }
    public class getPACDetail
    {
        public int AdmID { get; set; }
        public string UIN { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string FamilyHo { get; set; }
        public string KnownAllergy { get; set; }
        public string SurgeryType { get; set; }
        public int Surgerycode { get; set; }

        public int SAID { get; set; }

    }
    public class Advpaydetail
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
    }
    public class FundusDet
    {
        public int FundusID { get; set; }
        public string FundusDescription { get; set; }
        public Boolean FundusIsActive { get; set; }
    }
    public class getSlitLampDet
    {
        public int SlitID { get; set; }
        public string SlitDescription { get; set; }
        public Boolean SlitIsActive { get; set; }
    }

    public class UserDetails
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string Role { get; set; }

        public Boolean Status { get; set; }

        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CompanyName { get; set; }

    }
    public class ConfigureDetails
    {

        public int id { get; set; }
        public string RRDescription { get; set; }
        public Int16 RRAdvdays { get; set; }
        public Int16 FrequencyperDay { get; set; }
        public string HostEmailID { get; set; }
        public string HostPassword { get; set; }
        public string Phonenumber { get; set; }
        public Boolean SendSMS { get; set; }
        public Boolean SendEmail { get; set; }
        public string Frequencytime { get; set; }

    }
    public class getTaxDetS
    {
        public int ID { get; set; }
        public string TaxDescription { get; set; }
        public Int16? GSTPercentage { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public Int16? IGSTPercentage { get; set; }

        public string CESSDescription { get; set; }
        public decimal? CESSPercentage { get; set; }
        public string AdditionalCESSDescription { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }

        public string TaxGroupId { get; set; }
        public bool IsActive { get; set; }
        public bool istransact { get; set; }
        
    }
    public class Getindentsearchtotaldetails
    {
        
        public string Brand { get; set; }
        public string Manufacture { get; set; }
        public string Generic { get; set; }
        public string druggroup { get; set; }
        public string uom { get; set; }
        public int Iqty { get; set; }
        public int? Rqty { get; set; }
    }


    public class Getindentsearchdetails
    {
        public int OperationtheatreIDD { get; set; }
        public string Operationtheatre { get; set; }
        public string Indentraisedby { get; set; }
        public DateTime Indentdate { get; set; }
        public string storename { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    
        public bool status { get; set; }
        public string Brand { get; set; }
        public int Iqty { get; set; }
    }







    public class getInsurance
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string InsuranceCategory { get; set; }
        public bool IsActive { get; set; }
    }



    //public class GetOpticalGrn
    //{
    //    public int ID { get; set; }
    //    public string OrderNumber { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public string ReferenceNumber { get; set; }
    //    public DateTime? ReferenceDate { get; set; }
    //    public int Validity { get; set; }
    //    public string VendorName { get; set; }
    //    public string Location { get; set; }
    //    public string Address1 { get; set; }
    //    public string Address2 { get; set; }
    //    public string PhoneNumber { get; set; }
    //    public string GST { get; set; }
    //    public string DeliveryName { get; set; }
    //    public string DAddress1 { get; set; }
    //    public string DAddress2 { get; set; }
    //    public string DAddress3 { get; set; }
    //    public string DLocation { get; set; }

    //    public int VendorID { get; set; }



    //}

    public class helpRoom
    {
        public int ID { get; set; }
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public decimal RoomCost { get; set; }
        public int NoofRooms { get; set; }
        public int RoomNumber { get; set; }
        public int RestRoomType { get; set; }
        public int NoofBed { get; set; }
        public bool IsActive { get; set; }
    }


    public class storedetailsDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public string PhoneNo { get; set; }

    }



    public class CustomerDetails
    {
        public int ID { get; set; }
        public int CmpID { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public string GSTNo { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string ContactPerson { get; set; }

        public string City { get; set; }
        //  public string LocationID { get; set; }
    }
    public class DonorDetails
    {
        public int ID { get; set; }
        public int CmpID { get; set; }
        public string Name { get; set; }
        public string DonorType { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public string GSTNo { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string ContactPerson { get; set; }
        public string City { get; set; }
    }
    ///////Yours Treatmentmster
    public class GetDataArrayss
    {
        public int ID { get; set; }
        public string SpecialityID1 { get; set; }
        public string Description1 { get; set; }
        public Boolean IsActive1 { get; set; }
    }

    public class OfferDetail
    {
        public int LTID { get; set; }
        public int LMID { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string LensOptions { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public string Model { get; set; }
        public decimal? GST { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AddCess { get; set; }

        public string GSTDesc { get; set; }
        public string CESSDesc { get; set; }
        public string AddCessDesc { get; set; }
        public int? TaxIDD { get; set; }
        public string UOM { get; set; }
        public int? uomid { get; set; }

        
        public string HSNNo { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
    }

    public class GetDataArraysss
    {
        public int TID { get; set; }
        public int ID { get; set; }
        public string Brand { get; set; }
        public string LensType { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public string Model { get; set; }
        public decimal? GST { get; set; }
        public decimal? CESS { get; set; }
        public decimal? AddCess { get; set; }
        public int? TaxIDD { get; set; }
        public string UOM { get; set; }
        public string HSNNo { get; set; }

    }

    public class getBusinessRule2
    {
        public int BRID { get; set; }
        public int From { get; set; }
        public int TO { get; set; }
        public decimal Amount { get; set; }
        public int NODays { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public string icdspy { get; set; }
        
        public DateTime? EffectiveDate { get; set; }
        public string ModuleDescription { get; set; }
        public DateTime? DrugID { get; set; }
        public int ModuleID { get; set; }
        public int ID { get; set; }
    }
    public class getBusinessRule1
    {
        public int BRID { get; set; }
        public int From { get; set; }
        public int TO { get; set; }
        public decimal Amount { get; set; }
        public int NODays { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime CreatedUTC { get; set; }
        
        public string ModuleDescription { get; set; }
        public DateTime? DrugID { get; set; }
        public int ModuleID { get; set; }
        public int ID { get; set; }
    }
    public class getDrugGroupDetails
    {

        public int? ID { get; set; }
        public string Description { get; set; }
        public int? DrugFormID { get; set; }
        public Boolean IsStepDown { get; set; }

        public string DrugFormName { get; set; }
        public string SideEffects { get; set; }
        public string Precautions { get; set; }
        public string Overdose { get; set; }
        public Int16 RetestInterval { get; set; }
        public Int16 RetestCriticalInterval { get; set; }
    }
    public class getCompName1
    {
        public string companyName { get; set; }
        public int CmpID { get; set; }
        public int ParentID { get; set; }
    }
    public class getCompDetails1
    {
        public int CmpID { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public string City { get; set; }
        public string GSTNo { get; set; }
        public string ContactPerson { get; set; }
        public int ParentID { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public string Website { get; set; }
        public string CompanyGroup { get; set; }
        public string CompanyCategory { get; set; }
        
    }
    public class getTrantypeDetails
    {
        public int TransactionID { get; set; }
        public string Description { get; set; }
        public int? ContraTransactionid { get; set; }
        public string ContraTransaction { get; set; }


        public int? RecPayTransactionid { get; set; }
        public string RecPayTransaction { get; set; }
        public string Rec_Issue_type { get; set; }
    }
    public class OperationExtensionDetails
    {
        public int ID { get; set; }
        public string ICDCODE { get; set; }
        public int ICDSPECIALITY { get; set; }
        public int OTID { get; set; }
    }



    public class Employeestatutory
    {


        public bool IsPFEligible { get; set; }
        public string PFNumber { get; set; }
        public DateTime? PFCommencementDate { get; set; }
        public decimal VPFPercentage { get; set; }
        public bool IsESIEligible { get; set; }
        public string ESINumber { get; set; }
        public DateTime? ESICommencementDate { get; set; }
        public bool IsProfessionalTaxEligible { get; set; }
        public string GratuityNo { get; set; }
        public string UANNo { get; set; }


    }
    public class Employeebank
    {
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountHoldersName { get; set; }
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }



    }

    public class Employeebank1
    {
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountHoldersName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string AccountType1 { get; set; }
        public Boolean delete { get; set; }
    }

    public class Employeeexperience
    {
        public string Organisation { get; set; }
        public string Designation { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }

    public class Employeequa
    {
        public string qualification { get; set; }
        public string Degree { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string specialization { get; set; }
        public decimal percentageofmarks { get; set; }
        public string yearofpassing { get; set; }
        public string university { get; set; }
        public string instition { get; set; }
        public int qid { get; set; }
        public int uid { get; set; }


    }
    public class helpmaterialreturn
    {
        public string ReciptNumber { get; set; }
        public string StoreName { get; set; }
        public string VendorName { get; set; }
        public string Brand { get; set; }
        public int StoreID { get; set; }
        public long? PoID { get; set; }
        public int UOMID { get; set; }
    }

    public class getModuleMasDetails
    {

        public int ModuleID { get; set; }
        public int? TransactionID { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleType { get; set; }
        public decimal ParentModuleid1 { get; set; }
        public string ParentModule { get; set; }
        public string Parentmoduledescription { get; set; }
        public int CmpID { get; set; }
        public bool IsActive { get; set; }
        public bool? isvisible { get; set; }
        public bool Tag { get; set; }

    }

    public class InvPre
    {
        public string UIN { get; set; }
        public DateTime? Dateofinvestigation { get; set; }
        public string Remarks { get; set; }
        public string PrescribedBy { get; set; }
        public string DRNumber { get; set; }
        public string InvestigationID { get; set; }
        public decimal Amount { get; set; }
     
    }
    public class InvPre1
    {
        public string IPID { get; set; }
        public DateTime? Dateofinvestigation1 { get; set; }
        public string Remarks1 { get; set; }
        public string PrescribedBy1 { get; set; }
        public string DRNumber { get; set; }


    }


    public class latestadminandsur
    {
        public DateTime AdmDate { get; set; }
        public DateTime DateofSurgery { get; set; }
        public string Description { get; set; }
        public string Surgeonname { get; set; }
        public string OTID { get; set; }
        public string Anaestheticname { get; set; }
        public string Ocular { get; set; }
        public Boolean? IsSurgeryCompleted { get; set; }
        public string Remarks { get; set; }
        public DateTime? Review { get; set; }
        public string Anaesthesia { get; set; }
        public decimal? SurgeryCost { get; set; }

    }
    public class preoperativelat
    {
        public DateTime PreOpDate { get; set; }
        public string AntibioticGivenBy { get; set; }
        public string CaseSheetPreparedBy { get; set; }
        public string Note { get; set; }
        public string Instruction { get; set; }



    }
    public class Intraoperativelat
    {
        public string Description { get; set; }
        public string Surgeonname { get; set; }
        public string postoperativeins { get; set; }
    }
    public class Postoperativelast
    {
        public string ComplicationDetails { get; set; }
        public string TreatmentAdvive { get; set; }
        public DateTime PostOperativeDate { get; set; }
        public DateTime? ReviewDate { get; set; }
    }

    public class Postoper
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Allergy { get; set; }
        public string Ocular { get; set; }
        public DateTime DateofSurgery { get; set; }
        public string OTID { get; set; }
        public string Doctorname { get; set; }
        public string Anaestheticname { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public int SID { get; set; }

        public DateTime? ReviewDate { get; set; }

    }
    public class Postoper1
    {

        public int DiagnosisId { get; set; }
        public string ParentDescription { get; set; }
        public Boolean IsOD { get; set; }
        public Boolean IsOS { get; set; }
        public Boolean IsOU { get; set; }
    }
    public class Postoper2
    {
        public string Description { get; set; }
        public DateTime FromUTC { get; set; }
        public int Months { get; set; }

    }
    public class Postoper3
    {

        public DateTime DateofSurgery { get; set; }
        public string Description { get; set; }
        public string Doctorname { get; set; }
        public string Ocular { get; set; }
        public string ComplicationDetails { get; set; }
        public string TreatmentAdvive { get; set; }
        public DateTime PostOperativeDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int ID { get; set; }

    }


    public class SurgeryDischargedetail
    {
        public int? DoctorID { get; set; }
        public string SurID { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime DOB { get; set; }
        public DateTime SurgeryDate { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Gender { get; set; }
        public string ocular { get; set; }
        public int RegistrationTranId { get; set; }
        public string Allergy { get; set; }
        public int SurgeonID { get; set; }
        public string Surgery { get; set; }
        public string AnaestheticName { get; set; }
        public string AdmissionID { get; set; }
        public string SurgeryDescription { get; set; }
        public string SurgeonRegistrationNumber { get; set; }

        public ICollection<SurgeonDetail> SurgeonDetails { get; set; }
    }

    public class SurgeryPatientDischargedDetails
    {

        public string UIN { get; set; }
        public int RegistrationTranId { get; set; }
        public string SurgeryId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime SurgeryDate { get; set; }
        public DateTime DischargeDate { get; set; }
        public string TreatmentAdvice { get; set; }
        public string DischargeType { get; set; }
        public string AdmissionId { get; set; }
        public int? TransactionId { get; set; }
        public int? CMPID { get; set; }
        public string Allergy { get; set; }
        public string Surgery { get; set; }
        public string ocular { get; set; }
        public string SurgeryDescription { get; set; }
        public ICollection<SurgeonDetail> SurgeonDetails { get; set; }

    }


    public class PreviousSurgeryDetails
    {
        public string Name { get; set; }
        public string Surgeon { get; set; }
        public string SurgeryDescription { get; set; }
        public DateTime SurgeryDate { get; set; }
        public string ocular { get; set; }
    }

    public class NumCol
    {
        public int VCID { get; set; }
        public int TransactionID { get; set; }
        public int DepartmentID { get; set; }
        public int CmpID { get; set; }
        public int Location { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Description { get; set; }
        public string Autonumber { get; set; }
        public Int64 RunningNumber { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string IsActive { get; set; }
        public string IsDeleted { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


    }

    public class Surgerydetail
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Allergy { get; set; }
        public int Regtranid { get; set; }

        public string Ocular { get; set; }
        public DateTime DateofSurgery { get; set; }
        public string OTID { get; set; }
        public string Doctorname { get; set; }
        public string Anaestheticname { get; set; }
        public string Description { get; set; }
        public int SID { get; set; }

    }
    public class Surgerydetail1
    {

        public int DiagnosisId { get; set; }
        public string ParentDescription { get; set; }
        public Boolean IsOD { get; set; }
        public Boolean IsOS { get; set; }
        public Boolean IsOU { get; set; }
    }
    public class Surgerydetail2
    {
        public string Description { get; set; }
        public DateTime FromUTC { get; set; }
        public int Months { get; set; }
    }
    public class Surgerydetail3
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Allergy { get; set; }
        public int Regtranid { get; set; }
        public string Ocular { get; set; }
        public DateTime DateofSurgery { get; set; }
        public string OTID { get; set; }
        public string Doctorname { get; set; }
        public string Anaestheticname { get; set; }
        public string Description { get; set; }
        public DateTime AdmDate { get; set; }
        public string Remarks { get; set; }
        public int? DischargeID { get; set; }
        public int AdmId { get; set; }
        public int SurId { get; set; }
        public bool? IsSurgeryCompleted { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Anaesthesia { get; set; }
        public decimal? SurgeryCost { get; set; }
    }

    public class Surgerydetail4
    {
        public string UIN { get; set; }
        public DateTime IODate { get; set; }
        public string AntibioticGivenBy { get; set; }
        public string CaseSheetPreparedBy { get; set; }
        public string Note { get; set; }
        public string Instruction { get; set; }

    }
    public class Surgerydetail5
    {
        public string UIN { get; set; }
        public DateTime PreOpDate { get; set; }
        public string AntibioticGivenBy { get; set; }
        public string CaseSheetPreparedBy { get; set; }
        public string Note { get; set; }
        public string Instruction { get; set; }

    }


    public class OperationTheatreDetails
    {
        public int OTID { get; set; }
        public string OTDescription { get; set; }
        public string OTType { get; set; }
        public string OTAddress1 { get; set; }
        public string OTAddress2 { get; set; }
        public string OTAddress3 { get; set; }
        public string OTLocation { get; set; }
        public decimal? OTCharge { get; set; }
        public decimal? GST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public Boolean IsActive { get; set; }
        public string IsActiveName { get; set; }
    }



    public class StoreMaster
    {

        public int StoreID { get; set; }
        public int CmpID { get; set; }
        public string Storename { get; set; }
        public int? Catgtype { get; set; }
        public string StoreLocation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string StoreKeeper { get; set; }
        public string IsActive { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }


    public class DrugGroupDetail
    {
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string MedicineName { get; set; }

    }

    public class VendorMas
    {

            public int ID { get; set; }
            public int VendorCode { get; set; }
            public string VendorCategory { get; set; }
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Address3 { get; set; }
            public string LocationID { get; set; }
            public string City { get; set; }
            public int? Location { get; set; }
            public string DLNo { get; set; }
            public string DLNo1 { get; set; }
            public DateTime? DLNoDate { get; set; }
            public DateTime? DLNoDate1 { get; set; }
            public string Landmark { get; set; }
            public string FSSAINo { get; set; }
            public string GSTNo { get; set; }
            public string MobileNo { get; set; }
            public string PhoneNo { get; set; }
            public string ContactPerson { get; set; }
            public Int16? CreditDays { get; set; }
            public decimal? Creditlimit { get; set; }
            public Int16? Leadtime { get; set; }
            public Boolean IsDeleted { get; set; }
            public string IsActive { get; set; }
            public DateTime? CreatedUTC { get; set; }
            public DateTime? UpdatedUTC { get; set; }
            public int CreatedBy { get; set; }
            public int UpdatedBy { get; set; }
        }

    public class EmployeeDetail
    {
        public int EmployeeID { get; set; }
        public int CMPID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string Gender { get; set; }
        public Boolean PhysicallyChallenged { get; set; }
        public string MaritalStatus { get; set; }
        public string Category { get; set; }
        public string DeptCode { get; set; }
        public string AadhaarNo { get; set; }
        public string PANNo { get; set; }
        public string FatherHusbandName { get; set; }
        public string ReasonForResignation { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime? DOR { get; set; }
        public string Designation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public string BloodGroup { get; set; }
        public string PresentAddress1 { get; set; }
        public string PresentAddress2 { get; set; }
        public string PresentLocationID { get; set; }
        public string PresentCity { get; set; }
        public string PresentLandmark { get; set; }
        public string PermanentAddress1 { get; set; }
        public string PermanentAddress2 { get; set; }
        public string PermanentLocationID { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentLandmark { get; set; }
        public string Phone { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string EmailID { get; set; }
        public string AlternateEmailID { get; set; }
        public bool IsPFEligible { get; set; }
        public string PFNumber { get; set; }
        public DateTime? PFCommencementDate { get; set; }
        public decimal VPFPercentage { get; set; }
        public bool IsESIEligible { get; set; }
        public string ESINumber { get; set; }
        public DateTime? ESICommencementDate { get; set; }
        public bool IsProfessionalTaxEligible { get; set; }
        public string GratuityNo { get; set; }
        public string UANNo { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountHoldersName { get; set; }
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
        public Boolean Status { get; set; }
        public int EmproleID { get; set; }
        public string IFSCCode { get; set; }


    }


    public class docdetail1
    {
        public int DoctorID { get; set; }
        public int DoctorSpecialityID { get; set; }
        public string CMPID { get; set; }
        public string DoctorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationID { get; set; }
        public string Designation { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngagementType { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public string Speciality { get; set; }


    }


    public class UserDoc
    {
        public int? DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string RegistrationNumber { get; set; }

    }

    public class UserEmp
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string DOJ { get; set; }

    }





    public class DrugDetail
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string UOM { get; set; }
        public string DrugGroup { get; set; }
        public string MedicineName { get; set; }
        public string SideEffects { get; set; }
        public string Precautionss { get; set; }
        public string Overdose { get; set; }
        public decimal? Rate { get; set; }
        public string HSNNo { get; set; }
        public decimal? GST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public int? TaxID { get; set; }
        public decimal? CESSPercentage { get; set; }
        public decimal? AdditionalCESSPercentage { get; set; }
        public string Status { get; set; }

        public string DrugCategory { get; set; }
        public string TrackingType { get; set; }
        public string DrugSubDescription { get; set; }
        public string DrugComposition { get; set; }
        public string Power { get; set; }
        public string Aconstant { get; set; }
        public string ModelNo { get; set; }
        public string OpticDia { get; set; }
        public string Length { get; set; }



    }



    public class docdetail
    {
        public int? DoctorID { get; set; }
        public int DoctorSpecialityID { get; set; }
        public string CMPID { get; set; }
        public string DoctorName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationID { get; set; }
        public string Designation { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngagementType { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public string Speciality { get; set; }
        public string City { get; set; }

        public string DOCTORTAG { get; set; }
        public int roleid { get; set; }
        public Boolean Status { get; set; }
    }

    public class FBdetail
    {
        public string UIN { get; set; }
        public int CMPID { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }               
        public string Age { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Phone { get; set; }
        //////////////Print///////////////////
        public string PAddress { get; set; }
        public string PAddress2 { get; set; }
        public string PAddress3 { get; set; }
        public string Pphone { get; set; }
        public string Pweb { get; set; }
        public string PCompnayname { get; set; }
        /////////////////PatientVsInsurance///////////////////////
        public int PAINSID { get; set; }
        public decimal SumAssured { get; set; }
        
    }
    
        public class CampSurgerypatients
       {

        public string UIN { get; set; }
        public string RegType { get; set; }
        public int CMPID { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ICDSpeciality { get; set; }
        public string CampName { get; set; }

    }
    public class CampSurgeryunderwentpatients
    {

        public string UIN { get; set; }
        public string RegType { get; set; }
        public int CMPID { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ICDSpeciality { get; set; }
        public string CampName { get; set; }

    }

    
    public class Appdetail
    {
        public string RandomUniqueID { get; set; }
        public int CMPID { get; set; }
        public string Age { get; set; }
        public string PatientName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime Dateofbirth { get; set; }
        public string UIN { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public decimal ConsulationFees { get; set; }
        public DateTime CreatedUTC { get; set; }
    }
        public class Regdetail
    {
        public string UIN { get; set; }
        public string OLDUIN { get; set; }
        public string RegType { get; set; }
        public int CMPID { get; set; }
        public DateTime DateofRegistration { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public string FatherHusbandName { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public string AadharNumber { get; set; }

        public string PanCardNo { get; set; }
        public string DrivingLicenseNo { get; set; }
        public string PassportNo { get; set; }
        public string IsForeignNational { get; set; }

        public string EmailID { get; set; }
        public string SourceofReferralID { get; set; }
        public string ReferralPhone { get; set; }
        public DateTime? DateofVisit { get; set; }
        public string TypeofVisit { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDateTime { get; set; }
        public string Remarks { get; set; }
        public decimal? Amount { get; set; }
        

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string AlternateMailID { get; set; }
        public string TreatmentAdvice { get; set; }
        public string PreferredLanguage { get; set; }
        public string MaritalStatus { get; set; }
        public decimal? ConsulationFees { get; set; }
        public string City { get; set; }
        public string CmpAddress { get; set; }

        public string PAddress { get; set; }
        public string PAddress2 { get; set; }
        public string PAddress3 { get; set; }
        public string Pphone { get; set; }
        public string Pweb { get; set; }
        public string PCompnayname { get; set; }
        public string RegisteredBy { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? DateofReg { get; set; }

        public string CampName { get; set; }
        public string SurgeryName { get; set; }
        
    }

    public class Surgerydetails
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

    }

    public class ComplaintsDetails
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public DateTime? Fromdate { get; set; }
        public bool ISOD { get; set; }
        public bool ISOS { get; set; }
        public bool ISOU { get; set; }

        public bool Disabled { get; set; }
        public bool Contenteditable { get; set; }
    }


    public class PatientHistoryDetails
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public DateTime? FromUTC { get; set; }
        public string Duration { get; set; }
        public bool Disabled { get; set; }
    }

    public class IcdDetail
    {

        public string Code { get; set; }
        public string description { get; set; }
        public string speciality { get; set; }
        public string icdgroup { get; set; }
        public string Status { get; set; }
        public bool? IsIOLReqd { get; set; }
        
        //public int id { get; set; }
        //public string icdbrand { get; set; }
        public string roomtype { get; set; }
        //public decimal? surgerycost { get; set; }
        //public decimal? packagerate { get; set; }
        //public decimal? dressingcharge { get; set; }
        //public decimal? medicinecharge { get; set; }
        //public decimal? surgeoncharge { get; set; }

    }




    public class Genraldetails
    {

        public string Currentmedications { get; set; }
        public string allergy { get; set; }
        public string familyhistory { get; set; }


    }




    public class SurgeryTdetail
    {
        public string UIN { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Allergy { get; set; }
        public string Ocular { get; set; }
        public DateTime DateofSurgery { get; set; }
        public string OTID { get; set; }
        public string Doctorname { get; set; }
        public string Anaestheticname { get; set; }
        public string Description { get; set; }
        public int SID { get; set; }

    }
    public class SurgeryTdetail1
    {

        public string DiagnosisId { get; set; }
        public string ParentDescription { get; set; }
        public Boolean IsOD { get; set; }
        public Boolean IsOS { get; set; }
        public Boolean IsOU { get; set; }
    }
    public class SurgeryTdetail2
    {
        public string Description { get; set; }
        public DateTime FromUTC { get; set; }
        public int Months { get; set; }

    }

    public class SurgeryTdetail3
    {

        public DateTime DateofSurgery { get; set; }
        public string Description { get; set; }
        public string Doctorname { get; set; }
        public string Ocular { get; set; }
        public string ComplicationDetails { get; set; }
        public string TreatmentAdvive { get; set; }
        public DateTime PostOperativeDate { get; set; }
        public DateTime ReviewDate { get; set; }

    }




}
