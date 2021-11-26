
using System.Collections.Generic;
using System.Threading.Tasks;
using WYNK.Data.Common;


namespace WYNK.Data.Repository
{
    public interface ICommonRepository : IRepositoryBase<Dropdown>
    {
        Task<IEnumerable<Dropdown>> GetDropdown(string tableName, string valueColumn, string textColumn, string whereColumn = null, string whereValue = null);
      
        dynamic getConcerntextfile(int CompanyID);
        dynamic GetAccessdetails(int cmpid, string user, string path);//GetAccessdetailsstring
        dynamic GetAccessdetailsstring(int cmpid, string user, string path, string suffix);
        dynamic GetAccessdetailsolm(int cmpid, string user, string path, string MasterNameplus);//ErrorList
        dynamic GetAccessdetailsSquintM(int cmpid, string user, string path, string MasterNameplus);//ErrorList
        dynamic ErrorList(string ErrMsg, string Fname, int cmpid, int Uid);
        IEnumerable<Dropdown> GetBrandContactLens(int cmpid);
        IEnumerable<Dropdown> GetPositionofGlobe();
        IEnumerable<Dropdown> GetGradeofThyroidEyeDisease();
        IEnumerable<Dropdown> GetPalpableMassLocation();
        IEnumerable<Dropdown> GetPalpableMassShape();
        IEnumerable<Dropdown> GetPalpableMassTexture();
        IEnumerable<Dropdown> GetPalpableMassSize();
        IEnumerable<Dropdown> GetModules();
        IEnumerable<Dropdown> Getuniversity();
        IEnumerable<Dropdown> GetChartype();
        IEnumerable<Dropdown> GetRoomType();
        IEnumerable<Dropdown> GetFrameStyle();
        IEnumerable<Dropdown> GetFrameWidth();
        IEnumerable<Dropdown> GetDescriptionsvalue(int id);
        IEnumerable<Dropdown> GetFrameType();
        IEnumerable<Dropdown> GetFrameShape();
        IEnumerable<Dropdown> GetBrandFrame(int cmpid);
        IEnumerable<Dropdown> Getindex();
        IEnumerable<Dropdown> GetOrgtype();
        IEnumerable<Dropdown> GetOrgtypealldata();
        IEnumerable<Dropdown> GetOrgnizations();
        IEnumerable<Dropdown> GetCampNames();
        dynamic Getalldropdownvalues(int cmpid, int RoleID);
        dynamic Getalldinsurance(int cmpid);
        dynamic Getallchilddropdownvalues(int id);
        dynamic saveparentdesc(string id, int cmpid, string ServiceTagID);
        dynamic savechilddesc(string id, string desc, int cmpid,string stag);//GetBillType
        dynamic GetBillType(string uin, int cmpid);
        IEnumerable<Dropdown> getallroles();
        dynamic Gettimeinterval(int cmpid);
        dynamic Getstorecatgtypes(int cmpid);
        dynamic Getstorecatgalltypes(int cmpid);
        dynamic GetCMID(string URL);
        dynamic GetDoctorFees(int CMPID, int DOCID);

        IEnumerable<Dropdown> GetDescriptionsvalues();

        IEnumerable<Dropdown> Gettonometrymas();

        IEnumerable<Dropdown> getdatetime(int cmpid, string uin, string Time);

        IEnumerable<Dropdown> Getdocnae(int Cmpid);

        IEnumerable<Dropdown> Getcountryvalues();

        IEnumerable<Dropdown> getanathesthist();
        IEnumerable<Dropdown> Getqualification();
        IEnumerable<Dropdown> GetLocvalues();

        IEnumerable<Dropdown> Getinstitute(int ID);
        IEnumerable<Dropdown> GetDescvalues(int id);
        IEnumerable<Dropdown> GetEyedoctornamevalues(int CompanyID);        
        dynamic Getvisiondoctornamevalues(int CompanyID);
        IEnumerable<Dropdown> GetAllModels();
        IEnumerable<Dropdown> GetBrands();
        IEnumerable<Dropdown> GetInsuranceData();
        IEnumerable<Dropdown> GetMiddleManData(int IID);


        IEnumerable<Dropdown> GetComplainrdetailsvalues();

        IEnumerable<Dropdown> Desc();
        IEnumerable<Dropdown> Descc();
        IEnumerable<Dropdown> Getdoctornamedetails(int CompanyID);
        IEnumerable<Dropdown> Getdoctornamevalues(int CompanyID);
        IEnumerable<Dropdown> GetRegistrationsourceofrefvalues();
        IEnumerable<Dropdown> GetRegistrationTypeofvisistvalues();
        IEnumerable<Dropdown> Getpaymentvalues();
        IEnumerable<Dropdown> GetRelation();
        IEnumerable<Dropdown> GetMaritalStatus();
        IEnumerable<Dropdown> GetStatesvalues();
        //-----------------------surgery-------------------
        IEnumerable<Dropdown> GetSurgeryDescription();
        IEnumerable<Dropdown> GetSurgeonName(int Cmpid);
        IEnumerable<Dropdown> GetOperationTheatre(int Cmpid);        
        dynamic GetOperationTheatreCost(int Cmpid,int ID);        

        IEnumerable<Dropdown> GetregDropdownvalues();
        IEnumerable<Dropdown> GetMiddleman();
        IEnumerable<Dropdown> GetRoleDescription();
        IEnumerable<Dropdown> GetInsurance();

        //-------------Doctormastervalues--------------------
        IEnumerable<Dropdown> GetlocDropdownvalues();
        IEnumerable<Dropdown> GetspecDropdownvalues();
        IEnumerable<Dropdown> GetICDSpecialityCode(string ICDCODE);
        IEnumerable<Dropdown> GetICDSpCode();
        IEnumerable<Dropdown> Geticdcode();
        

        IEnumerable<Dropdown> GetEngageDropdownvalues();


        //------------refractiovalues----------------------

        IEnumerable<Dropdown> GetCATvalues();
        IEnumerable<Dropdown> GetINvalues();
        IEnumerable<Dropdown> GetDVvalues();
        IEnumerable<Dropdown> GetNVvalues();//GetGoniovalues
        IEnumerable<Dropdown> GetGoniovalues();
        IEnumerable<Dropdown> Getlensvalues();
        IEnumerable<Dropdown> GetDVvaluesdis();
        IEnumerable<Dropdown> GetNVvaluesnear();
        //--------------medicalprecriptionvalues------------
        IEnumerable<Dropdown> GetDossgevalues();
        IEnumerable<Dropdown> GetFYvalues();
        IEnumerable<Dropdown> GetFDvalues();
        IEnumerable<Dropdown> GetBrand();
        IEnumerable<Dropdown> GetUOM();
        IEnumerable<Dropdown> Getsurgerydescvalues();
        IEnumerable<Dropdown> GetopiniondoctorDropdownvalues();        
        IEnumerable<Dropdown> GetmedicineDropdownvalues();        
        IEnumerable<Dropdown> GetICDDropdownvalues();
        IEnumerable<Dropdown> Getdoctorvalues();
        IEnumerable<Dropdown> GetCompdoctorvalues(int Cmpid);
        IEnumerable<Dropdown> GetICDvalues();
        IEnumerable<Dropdown> GetViewvalues();
        IEnumerable<Dropdown> getrolevalues();
        IEnumerable<Dropdown> getrolevaluesexceptadmin();

        IEnumerable<Dropdown> GetTranTypes();

        string GenerateRunningCtrlNoo(int TransactionTypeid, int CompanyID, string updateRno);        
        dynamic GettingRunningNo(int Cmpid, int TC);

        int? GettingReceiptTcID(int TC, int CompanyID);

        //-------------Employeemastervalues--------------------
        IEnumerable<Dropdown> GetBloodGroups();
        IEnumerable<Dropdown> GetTitles();
        IEnumerable<Dropdown> GetDrugForm();
        IEnumerable<Dropdown> GetDrugGroup(int cmpid);


        IEnumerable<Dropdown> Getinvestvalues(int CmpID);
        IEnumerable<Dropdown> Gettestdiagnosisvalues(int ID, int CmpID);
        IEnumerable<Dropdown> Gettaxvalues();

        //------------ItemvsVendor------------
        IEnumerable<Dropdown> GetVendornames(int CMPID);
        IEnumerable<Dropdown> GetBrandValues(int CMPID);
        IEnumerable<Dropdown> Getsuppliervalues(int id);
        IEnumerable<Dropdown> GetVendornamevalues(int Cmpid);
        IEnumerable<Dropdown> GetstoreDropdownvalues(int CompanyID, int id, string name);//Getdrugndgroup
        IEnumerable<Dropdown> GetstoreDropdownvaluesdesc(int CompanyID, string name);
        IEnumerable<Dropdown> GetFullstoreDropdownvalues(int CompanyID);
        IEnumerable<Dropdown> Getdrugndgroup(int CompanyID);
        IEnumerable<Dropdown> GetbranchstoreDropdownvalues(int CompanyID, string name);
        IEnumerable<Dropdown> GetDrugvalues(int id, int cmpid);
        IEnumerable<Dropdown> GetInvDep(int CmpID);
        IEnumerable<Dropdown> GetDrugvalues1(int CompanyID);
        IEnumerable<Dropdown> GetstoreDropdownvalues();
        IEnumerable<Dropdown> GetBranchAll(int CompanyID);
        IEnumerable<Dropdown> GetRevnueValues();
        
        IEnumerable<Dropdown> Getlensvalues1(int VID);
        IEnumerable<Dropdown> Getlocationvalues(int LID);
        IEnumerable<Dropdown> Getlocationcityvalues();
        IEnumerable<Dropdown> GetTab(int CmpID);
        IEnumerable<Dropdown> GetDepartments();
        IEnumerable<Dropdown> GetBRModuleDescription(int CompanyID);
        IEnumerable<Dropdown> GetICDSpecialityDescription();

        IEnumerable<Dropdown> GetNumberControlDes();        
        IEnumerable<Dropdown> GetDrug(string Grn, int StoreID, int CompanyID);
        IEnumerable<Dropdown> Getpackagevalues(int CMPID);
        dynamic Getlogdetails(int cmpid, string user, string path);
        dynamic Getlogdetailsstring(int cmpid, string user, string path, string suffix);
        IEnumerable<Dropdown> Geticdspecvalues();
        IEnumerable<Dropdown> getGen(int CMPID);
        IEnumerable<Dropdown> Geticddescvalues(int ID);//getGen
        IEnumerable<Dropdown> Geticicddescvalues(int id, int Cmpid);
        IEnumerable<Dropdown> getDetails();
        IEnumerable<Dropdown> getDetailsstate(int id);
        IEnumerable<Dropdown> getDetailsdistrict(int id);
        IEnumerable<Dropdown> getDetailslocation(int id);
        IEnumerable<Dropdown> Geteyeerrvalues();
        IEnumerable<Dropdown> GetServiceTag();
        
        IEnumerable<Dropdown> Getoptmodelvalues(int id);
        IEnumerable<Dropdown> Getsqutreat(int id);
        IEnumerable<Dropdown> GetTaxPercentage();

        ////////Yours LM
        IEnumerable<Dropdown> GstSearch();
        IEnumerable<Dropdown> CessSearch(int ID);
        IEnumerable<Dropdown> AddCessSearch(int ID);
        IEnumerable<Dropdown> GSTperSearch(int ID);
        IEnumerable<Dropdown> GetBrandLens(int cmpid);        

        IEnumerable<Dropdown> UOMSearch();



        /////Yours TM
        IEnumerable<Dropdown> GetSplName();
        IEnumerable<Dropdown> GetSquintvalue();
        IEnumerable<Dropdown> GetRooms(int cmpid);
        IEnumerable<Dropdown> GetRole();
        IEnumerable<Dropdown> Getservice(int cmpid);
        
        IEnumerable<Dropdown> GetSurgeryLens(int CMPID);
        IEnumerable<Dropdown> GetRoomstatus();


        IEnumerable<Dropdown> GetIndentUOMDetails();
        IEnumerable<Dropdown> GetIndentSurgeonDetails();
        IEnumerable<Dropdown> GetIndentOTDetails();
        IEnumerable<Dropdown> GetIndentDrugDetails();

        IEnumerable<Dropdown> SSCTypeDetails();
        IEnumerable<Dropdown> GetBrandAll(int CompanyID);//GetBrandAllDrugs
        IEnumerable<Dropdown> GetBrandAllDrugs();
        IEnumerable<Dropdown> GetBrands1();
        dynamic print(string PAID);

        IEnumerable<Dropdown> GetRoomtypes();
        //////////////////////currency//////////////////////////

        IEnumerable<Dropdown> GetCurrencyvalues(int CMPID);
        IEnumerable<Dropdown> Getsetupregvalues(int CMPID);

        IEnumerable<Dropdown> GetEyedoctornamevalueswithappointmentonly(int CMPID);
        dynamic GetCountryDetail(int cmpid);
        
        IEnumerable<Dropdown> GetFDDTDescriptionsvalues(int CMPID);
        IEnumerable<Dropdown> GetSyringingDescriptions(int CMPID);
        IEnumerable<Dropdown> GetRegurgitationDescriptions(int CMPID);
        IEnumerable<Dropdown> GetFluidDescriptions(int CMPID);
        IEnumerable<Dropdown> GetStopDescriptions(int CMPID);
        dynamic Loadallavailablelanguages();

        dynamic GetPatientDob(string UIN, int CMPID);

        dynamic Getocularvalues();
        dynamic Getsystemicvalues();//Getwfdtvalues
        IEnumerable<Dropdown> Getocumvalues(int ID);
        IEnumerable<Dropdown> Getvfvalues(int ID);
        IEnumerable<Dropdown> Getanglevalues(int ID);
        IEnumerable<Dropdown> Getposvalues(int ID);
        IEnumerable<Dropdown> Getacamvalues(int ID);
        IEnumerable<Dropdown> Getacavvalues(int ID);
        IEnumerable<Dropdown> Getwfdtvalues(int ID);//Getpurvalues
        IEnumerable<Dropdown> Getspmvalues(int ID);
        IEnumerable<Dropdown> Getspvvalues(int ID);
        IEnumerable<Dropdown> Getarcvalues(int ID);
        IEnumerable<Dropdown> Getpbcvalues(int ID);
        IEnumerable<Dropdown> Getampvalues(int ID);
        IEnumerable<Dropdown> Getfrqyvalues(int ID);
        IEnumerable<Dropdown> Gettypvalues(int ID);
        IEnumerable<Dropdown> Getpurvalues(int ID);//Gettbutvalues
        IEnumerable<Dropdown> Getsacvalues(int ID);
        IEnumerable<Dropdown> Getabhvalues(int ID);
        IEnumerable<Dropdown> Getconvvalues(int ID);
        IEnumerable<Dropdown> Getooevalues(int ID);
        IEnumerable<Dropdown> Getoscvalues(int ID);
        IEnumerable<Dropdown> GetGenericvalue(int ID);
        IEnumerable<Dropdown> Gettmhvalues();
        IEnumerable<Dropdown> Gettbutvalues();
        IEnumerable<Dropdown> Getdiaspecvalues(int CMPID);
        dynamic Gettimeintervalesbetweendates(int cmpid);
        dynamic Checktimefordoctor(int cmpid, string date, string time, int doc, int gap);
    }
}
