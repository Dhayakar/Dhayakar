
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Common;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]

    public class CommonController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public CommonController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("getallroles")]
        public IEnumerable<Dropdown> getallroles()
        {
            return _repoWrapper.Common.getallroles();
        }

        [HttpGet("GetModules")]
        public IEnumerable<Dropdown> GetModules()
        {
            return _repoWrapper.Common.GetModules();
        }

        [HttpGet("Getalldropdownvalues/{cmpid}/{RoleID}")]
        public dynamic Getalldropdownvalues(int cmpid,int RoleID)
        {
            return _repoWrapper.Common.Getalldropdownvalues(cmpid, RoleID);
        }

        [HttpGet("Getalldinsurance/{cmpid}")]
        public dynamic Getalldinsurance(int cmpid)
        {
            return _repoWrapper.Common.Getalldinsurance(cmpid);
        }


        [HttpGet("GetBrandContactLens/{cmpid}")]
        public IEnumerable<Dropdown> GetBrandContactLens(int cmpid)
        {
            return _repoWrapper.Common.GetBrandContactLens(cmpid);
        }

        [HttpGet("Getallchilddropdownvalues/{id}")]
        public dynamic Getallchilddropdownvalues(int id)
        {
            return _repoWrapper.Common.Getallchilddropdownvalues(id);
        }

        [HttpGet("saveparentdesc/{id}/{cmpid}/{ServiceTagID}")]
        public dynamic saveparentdesc(string id, int cmpid,string ServiceTagID)
        {
            return _repoWrapper.Common.saveparentdesc(id, cmpid, ServiceTagID);
        }


        [HttpGet("savechilddesc/{id}/{desc}/{cmpid}/{stag}")]
        public dynamic savechilddesc(string id, string desc, int cmpid,string stag)
        {
            return _repoWrapper.Common.savechilddesc(id, desc, cmpid, stag);
        }

        [HttpGet("GetBillType/{uin}/{cmpid}")]
        public dynamic GetBillType(string uin, int cmpid)
        {
            return _repoWrapper.Common.GetBillType(uin, cmpid);
        }


        [HttpGet("GetPositionofGlobe")]
        public IEnumerable<Dropdown> GetPositionofGlobe()
        {
            return _repoWrapper.Common.GetPositionofGlobe();
        }
        [HttpGet("GetGradeofThyroidEyeDisease")]
        public IEnumerable<Dropdown> GetGradeofThyroidEyeDisease()
        {
            return _repoWrapper.Common.GetGradeofThyroidEyeDisease();
        }
        [HttpGet("GetPalpableMassLocation")]
        public IEnumerable<Dropdown> GetPalpableMassLocation()
        {
            return _repoWrapper.Common.GetPalpableMassLocation();
        }
        [HttpGet("GetPalpableMassShape")]
        public IEnumerable<Dropdown> GetPalpableMassShape()
        {
            return _repoWrapper.Common.GetPalpableMassShape();
        }
        [HttpGet("GetPalpableMassTexture")]
        public IEnumerable<Dropdown> GetPalpableMassTexture()
        {
            return _repoWrapper.Common.GetPalpableMassTexture();
        }
        [HttpGet("GetPalpableMassSize")]
        public IEnumerable<Dropdown> GetPalpableMassSize()
        {
            return _repoWrapper.Common.GetPalpableMassSize();
        }


        [HttpGet("Gettimeinterval/{cmpid}")]
        public dynamic Gettimeinterval(int cmpid)
        {
            return _repoWrapper.Common.Gettimeinterval(cmpid);
        }

        [HttpGet("Getstorecatgtypes/{cmpid}")]
        public dynamic Getstorecatgtypes(int cmpid)
        {
            return _repoWrapper.Common.Getstorecatgtypes(cmpid);
        }

        [HttpGet("Getstorecatgalltypes/{cmpid}")]
        public dynamic Getstorecatgalltypes(int cmpid)
        {
            return _repoWrapper.Common.Getstorecatgalltypes(cmpid);
        }


        [HttpGet("GetDoctorFees/{CMPID}/{DOCID}")]
        public dynamic GetDoctorFees(int CMPID, int DOCID)
        {
            return _repoWrapper.Common.GetDoctorFees(CMPID, DOCID);
        }

        [HttpGet("GetCMID/{URL}")]
        public dynamic GetCMID(string URL)
        {
            return _repoWrapper.Common.GetCMID(URL);
        }

        [HttpGet("GetOrgtypealldata")]
        public IEnumerable<Dropdown> GetOrgtypealldata()
        {
            return _repoWrapper.Common.GetOrgtypealldata();
        }
        [HttpGet("GetOrgnizations")]
        public IEnumerable<Dropdown> GetOrgnizations()
        {
            return _repoWrapper.Common.GetOrgnizations();
        }
        [HttpGet("GetCampNames")]
        public IEnumerable<Dropdown> GetCampNames()
        {
            return _repoWrapper.Common.GetCampNames();
        }

        [HttpGet("GetChartype")]
        public IEnumerable<Dropdown> GetChartype()
        {
            return _repoWrapper.Common.GetChartype();
        }
        [HttpGet("GetRoomType")]
        public IEnumerable<Dropdown> GetRoomType()
        {
            return _repoWrapper.Common.GetRoomType();
        }
        [HttpGet("GetOrgtype")]
        public IEnumerable<Dropdown> GetOrgtype()
        {
            return _repoWrapper.Common.GetOrgtype();
        }
        [HttpGet("Getindex")]
        public IEnumerable<Dropdown> Getindex()
        {
            return _repoWrapper.Common.Getindex();
        }

        [HttpGet("GetFrameShape")]
        public IEnumerable<Dropdown> GetFrameShape()
        {
            return _repoWrapper.Common.GetFrameShape();
        }

        [HttpGet("Getinstitute/{ID}")]
        public IEnumerable<Dropdown> Getinstitute(int ID)
        {
            return _repoWrapper.Common.Getinstitute(ID);
        }

        [HttpGet("GetFrameType")]
        public IEnumerable<Dropdown> GetFrameType()
        {
            return _repoWrapper.Common.GetFrameType();
        }

        [HttpGet("GetFrameWidth")]
        public IEnumerable<Dropdown> GetFrameWidth()
        {
            return _repoWrapper.Common.GetFrameWidth();
        }

        [HttpGet("GetDescriptionsvalue/{id}")]
        public IEnumerable<Dropdown> GetDescriptionsvalue(int id)
        {
            return _repoWrapper.Common.GetDescriptionsvalue(id);
        }

        [HttpGet("GetDescriptionsvalues")]
        public IEnumerable<Dropdown> GetDescriptionsvalues()
        {
            return _repoWrapper.Common.GetDescriptionsvalues();
        }


        [HttpGet("GetFrameStyle")]
        public IEnumerable<Dropdown> GetFrameStyle()
        {
            return _repoWrapper.Common.GetFrameStyle();
        }


        [HttpGet("GetCountryDetail/{Cmpid}")]
        public dynamic GetCountryDetail(int cmpid)
        {
            return _repoWrapper.Common.GetCountryDetail(cmpid);
        }


        [HttpGet("Getlogdetails/{Cmpid}/{user}/{path}")]
        public dynamic Getlogdetails(int cmpid, string user, string path)
        {
            return _repoWrapper.Common.Getlogdetails(cmpid, user, path);
        }

        [HttpGet("Getlogdetailsstring/{Cmpid}/{user}/{path}/{suffix}")]
        public dynamic Getlogdetailsstring(int cmpid, string user, string path, string suffix)
        {
            return _repoWrapper.Common.Getlogdetailsstring(cmpid, user, path, suffix);
        }


        [HttpGet("GetAccessdetails/{Cmpid}/{user}/{path}")]
        public dynamic GetAccessdetails(int cmpid, string user, string path)
        {
            return _repoWrapper.Common.GetAccessdetails(cmpid, user, path);
        }

        [HttpGet("GetAccessdetailsstring/{Cmpid}/{user}/{path}/{suffix}")]
        public dynamic GetAccessdetailsstring(int cmpid, string user, string path, string suffix)
        {
            return _repoWrapper.Common.GetAccessdetailsstring(cmpid, user, path, suffix);
        }


        [HttpGet("GetAccessdetailsolm/{Cmpid}/{user}/{path}/{MasterNameplus}")]
        public dynamic GetAccessdetailsolm(int cmpid, string user, string path, string MasterNameplus)
        {
            return _repoWrapper.Common.GetAccessdetailsolm(cmpid, user, path, MasterNameplus);
        }


        [HttpGet("GetAccessdetailsSquintM/{Cmpid}/{user}/{path}/{MasterNameplus}")]
        public dynamic GetAccessdetailsSquintM(int cmpid, string user, string path, string MasterNameplus)
        {
            return _repoWrapper.Common.GetAccessdetailsSquintM(cmpid, user, path, MasterNameplus);
        }


        [HttpGet("ErrorList/{ErrMsg}/{Fname}/{cmpid}/{Uid}")]
        public dynamic ErrorList(string ErrMsg, string Fname, int cmpid, int Uid)
        {
            return _repoWrapper.Common.ErrorList(ErrMsg, Fname, cmpid, Uid);
        }


        [HttpGet("getDetails")]
        public IEnumerable<Dropdown> getDetails()
        {
            return _repoWrapper.Common.getDetails();
        }




        [HttpGet("Gettonometrymas")]
        public IEnumerable<Dropdown> Gettonometrymas()
        {
            return _repoWrapper.Common.Gettonometrymas();
        }

        [HttpGet("getdatetime/{Cmpid}/{uin}/{Time}")]
        public IEnumerable<Dropdown> getdatetime(int Cmpid, string uin, string Time)
        {
            return _repoWrapper.Common.getdatetime(Cmpid, uin, Time);
        }

        [HttpGet("Getdocnae/{Cmpid}")]
        public IEnumerable<Dropdown> Getdocnae(int Cmpid)
        {
            return _repoWrapper.Common.Getdocnae(Cmpid);
        }



        [HttpGet("getDetailsstate/{id}")]
        public IEnumerable<Dropdown> getDetailsstate(int id)
        {
            return _repoWrapper.Common.getDetailsstate(id);
        }
        [HttpGet("getDetailsdistrict/{id}")]
        public IEnumerable<Dropdown> getDetailsdistrict(int id)
        {
            return _repoWrapper.Common.getDetailsdistrict(id);
        }
        [HttpGet("getDetailslocation/{id}")]
        public IEnumerable<Dropdown> getDetailslocation(int id)
        {
            return _repoWrapper.Common.getDetailslocation(id);
        }

        [HttpGet("GetInsuranceData")]
        public IEnumerable<Dropdown> GetInsuranceData()
        {
            return _repoWrapper.Common.GetInsuranceData();
        }
        [HttpGet("GetMiddleManData/{IID}")]
        public IEnumerable<Dropdown> GetMiddleManData(int IID)
        {
            return _repoWrapper.Common.GetMiddleManData(IID);
        }
        [HttpGet("GetBrands")]
        public IEnumerable<Dropdown> GetBrands()
        {
            return _repoWrapper.Common.GetBrands();
        }

        [HttpGet("GetComplainrdetailsvalues")]
        public IEnumerable<Dropdown> GetComplainrdetailsvalues()
        {
            return _repoWrapper.Common.GetComplainrdetailsvalues();
        }





        [HttpGet("GetAllModels")]
        public IEnumerable<Dropdown> GetAllModels()
        {
            return _repoWrapper.Common.GetAllModels();
        }

        [HttpGet("GetMiddleman")]
        public IEnumerable<Dropdown> GetMiddleman()
        {
            return _repoWrapper.Common.GetMiddleman();
        }
        [HttpGet("GetRoleDescription")]
        public IEnumerable<Dropdown> GetRoleDescription()
        {
            return _repoWrapper.Common.GetRoleDescription();
        }
        [HttpGet("GetInsurance")]
        public IEnumerable<Dropdown> GetInsurance()
        {
            return _repoWrapper.Common.GetInsurance();
        }

        [HttpGet("Desc")]
        public IEnumerable<Dropdown> Desc()
        {
            return _repoWrapper.Common.Desc();
        }
        [HttpGet("Descc")]
        public IEnumerable<Dropdown> Descc()
        {
            return _repoWrapper.Common.Descc();
        }

        [HttpGet("GetTaxPercentage")]
        public IEnumerable<Dropdown> GetTaxPercentage()
        {
            return _repoWrapper.Common.GetTaxPercentage();
        }
        [HttpGet("Getqualification")]
        public IEnumerable<Dropdown> Getqualification()
        {
            return _repoWrapper.Common.Getqualification();
        }
        [HttpGet("Getuniversity")]
        public IEnumerable<Dropdown> Getuniversity()
        {
            return _repoWrapper.Common.Getuniversity();
        }

        [HttpGet("GetBRModuleDescription/{CompanyID}")]
        public IEnumerable<Dropdown> GetBRModuleDescription(int CompanyID)
        {
            return _repoWrapper.Common.GetBRModuleDescription(CompanyID);
        }
        [HttpGet("GetICDSpecialityDescription")]
        public IEnumerable<Dropdown> GetICDSpecialityDescription()
        {
            return _repoWrapper.Common.GetICDSpecialityDescription();
        }


        [HttpGet("Getvisiondoctornamevalues/{CompanyID}")]
        public dynamic Getvisiondoctornamevalues(int CompanyID)
        {
            return _repoWrapper.Common.Getvisiondoctornamevalues(CompanyID);
        }





        [HttpGet("GetLocvalues")]
        public IEnumerable<Dropdown> GetLocvalues()
        {
            return _repoWrapper.Common.GetLocvalues();
        }

        [HttpGet("GetDepartments")]
        public IEnumerable<Dropdown> GetDepartments()
        {
            return _repoWrapper.Common.GetDepartments();
        }

        [HttpGet("GetNumberControlDes")]
        public IEnumerable<Dropdown> GetNumberControlDes()
        {
            return _repoWrapper.Common.GetNumberControlDes();
        }

        [HttpGet("GetstoreDropdownvalues")]
        public IEnumerable<Dropdown> GetstoreDropdownvalues()
        {
            return _repoWrapper.Common.GetstoreDropdownvalues();
        }

        [HttpGet("GetBranchAll/{CompanyID}")]
        public IEnumerable<Dropdown> GetBranchAll(int CompanyID)
        {
            return _repoWrapper.Common.GetBranchAll(CompanyID);
        }

        //-----------------------surgery-------------------
        [HttpGet("GetSurgeryDescription")]
        public IEnumerable<Dropdown> GetSurgeryDescription()
        {
            return _repoWrapper.Common.GetSurgeryDescription();
        }
        [HttpGet("GetSurgeonName/{Cmpid}")]
        public IEnumerable<Dropdown> GetSurgeonName(int Cmpid)
        {
            return _repoWrapper.Common.GetSurgeonName(Cmpid);
        }


        [HttpGet("GetOperationTheatre/{Cmpid}")]
        public IEnumerable<Dropdown> GetOperationTheatre(int Cmpid)
        {
            return _repoWrapper.Common.GetOperationTheatre(Cmpid);
        }

        [HttpGet("GetOperationTheatreCost/{Cmpid}/{ID}")]
        public dynamic GetOperationTheatreCost(int Cmpid,int ID)
        {
            return _repoWrapper.Common.GetOperationTheatreCost(Cmpid, ID);
        }

        [HttpGet("getDropdown/{tableName}/{valueColumn}/{textColumn}/{whereColumn?}/{whereValue?}")]
        public async Task<IEnumerable<Dropdown>> GetDropdown(string tableName, string valueColumn, string textColumn,
            string whereColumn = null, string whereValue = null)
        {
            return await _repoWrapper.Common.GetDropdown(tableName, valueColumn, textColumn, whereColumn, whereValue);
        }


        [HttpGet("GetDrugGroup/{CMPID}")]
        public IEnumerable<Dropdown> GetDrugGroup(int cmpid)
        {
            return _repoWrapper.Common.GetDrugGroup(cmpid);
        }

        [HttpGet("GetDrugForm")]
        public IEnumerable<Dropdown> GetDrugForm()
        {
            return _repoWrapper.Common.GetDrugForm();
        }

        [HttpGet("GetStatesvalues")]
        public IEnumerable<Dropdown> GetStatesvalues()
        {
            return _repoWrapper.Common.GetStatesvalues();
        }


        [HttpGet("GetBloodGroups")]
        public IEnumerable<Dropdown> GetBloodGroups()
        {
            return _repoWrapper.Common.GetBloodGroups();
        }

        [HttpGet("GetTitles")]
        public IEnumerable<Dropdown> GetTitles()
        {
            return _repoWrapper.Common.GetTitles();
        }



        [HttpGet("Getlensvalues")]
        public IEnumerable<Dropdown> Getlensvalues()
        {
            return _repoWrapper.Common.Getlensvalues();
        }


        [HttpGet("GetCATvalues")]
        public IEnumerable<Dropdown> GetCATvalues()
        {
            return _repoWrapper.Common.GetCATvalues();
        }

        [HttpGet("GetDescvalues/{id}")]
        public IEnumerable<Dropdown> GetDescvalues(int id)
        {
            return _repoWrapper.Common.GetDescvalues(id);
        }

        [HttpGet("GetINvalues")]
        public IEnumerable<Dropdown> GetINvalues()
        {
            return _repoWrapper.Common.GetINvalues();
        }
        [HttpGet("GetDVvalues")]
        public IEnumerable<Dropdown> GetDVvalues()
        {
            return _repoWrapper.Common.GetDVvalues();
        }

        [HttpGet("GetNVvalues")]
        public IEnumerable<Dropdown> GetNVvalues()
        {
            return _repoWrapper.Common.GetNVvalues();
        }

        [HttpGet("GetGoniovalues")]
        public IEnumerable<Dropdown> GetGoniovalues()
        {
            return _repoWrapper.Common.GetGoniovalues();
        }


        [HttpGet("getanathesthist")]
        public IEnumerable<Dropdown> getanathesthist()
        {
            return _repoWrapper.Common.getanathesthist();
        }

        [HttpGet("GetDVvaluesdis")]
        public IEnumerable<Dropdown> GetDVvaluesdis()
        {
            return _repoWrapper.Common.GetDVvaluesdis();
        }

        [HttpGet("GetNVvaluesnear")]
        public IEnumerable<Dropdown> GetNVvaluesnear()
        {
            return _repoWrapper.Common.GetNVvaluesnear();
        }

        [HttpGet("GetTranTypes")]
        public IEnumerable<Dropdown> GetTranTypes()
        {
            return _repoWrapper.Common.GetTranTypes();
        }


        [HttpGet("getrolevalues")]
        public IEnumerable<Dropdown> getrolevalues()
        {
            return _repoWrapper.Common.getrolevalues();
        }
        [HttpGet("getrolevaluesexceptadmin")]
        public IEnumerable<Dropdown> getrolevaluesexceptadmin()
        {
            return _repoWrapper.Common.getrolevaluesexceptadmin();
        }



        [HttpGet("GetViewvalues")]
        public IEnumerable<Dropdown> GetViewvalues()
        {
            return _repoWrapper.Common.GetViewvalues();
        }

        [HttpGet("GetEyedoctornamevalues/{CompanyID}")]
        public IEnumerable<Dropdown> GetEyedoctornamevalues(int CompanyID)
        {
            return _repoWrapper.Common.GetEyedoctornamevalues(CompanyID);
        }



        [HttpGet("Getdoctornamedetails/{CompanyID}")]
        public IEnumerable<Dropdown> Getdoctornamedetails(int CompanyID)
        {
            return _repoWrapper.Common.Getdoctornamedetails(CompanyID);
        }


        [HttpGet("Getdoctornamevalues/{CompanyID}")]
        public IEnumerable<Dropdown> Getdoctornamevalues(int CompanyID)
        {
            return _repoWrapper.Common.Getdoctornamevalues(CompanyID);
        }


        [HttpGet("GetICDvalues")]
        public IEnumerable<Dropdown> GetICDvalues()
        {
            return _repoWrapper.Common.GetICDvalues();
        }


        [HttpGet("Getdoctorvalues")]
        public IEnumerable<Dropdown> Getdoctorvalues()
        {
            return _repoWrapper.Common.Getdoctorvalues();
        }



        [HttpGet("GetCompdoctorvalues/{Cmpid}")]
        public IEnumerable<Dropdown> GetCompdoctorvalues(int Cmpid)
        {
            return _repoWrapper.Common.GetCompdoctorvalues(Cmpid);
        }


        [HttpGet("Getpaymentvalues")]
        public IEnumerable<Dropdown> Getpaymentvalues()
        {
            return _repoWrapper.Common.Getpaymentvalues();
        }

        [HttpGet("GetRelation")]
        public IEnumerable<Dropdown> GetRelation()
        {
            return _repoWrapper.Common.GetRelation();
        }
        [HttpGet("GetMaritalStatus")]
        public IEnumerable<Dropdown> GetMaritalStatus()
        {
            return _repoWrapper.Common.GetMaritalStatus();
        }
        [HttpGet("Getsurgerydescvalues")]
        public IEnumerable<Dropdown> Getsurgerydescvalues()
        {
            return _repoWrapper.Common.Getsurgerydescvalues();
        }



        [HttpGet("GetICDDropdownvalues")]
        public IEnumerable<Dropdown> GetICDDropdownvalues()
        {
            return _repoWrapper.Common.GetICDDropdownvalues();
        }



        [HttpGet("GetmedicineDropdownvalues")]
        public IEnumerable<Dropdown> GetmedicineDropdownvalues()
        {
            return _repoWrapper.Common.GetmedicineDropdownvalues();
        }


        [HttpGet("GetopiniondoctorDropdownvalues")]
        public IEnumerable<Dropdown> GetopiniondoctorDropdownvalues()
        {
            return _repoWrapper.Common.GetopiniondoctorDropdownvalues();
        }

        [HttpGet("GetregDropdownvalues")]
        public IEnumerable<Dropdown> GetregDropdownvalues()
        {
            return _repoWrapper.Common.GetregDropdownvalues();
        }


        [HttpGet("GetRegistrationsourceofrefvalues")]
        public IEnumerable<Dropdown> GetRegistrationsourceofrefvalues()
        {
            return _repoWrapper.Common.GetRegistrationsourceofrefvalues();
        }
        [HttpGet("GetRegistrationTypeofvisistvalues")]
        public IEnumerable<Dropdown> GetRegistrationTypeofvisistvalues()
        {
            return _repoWrapper.Common.GetRegistrationTypeofvisistvalues();
        }

        [HttpGet("GetlocDropdownvalues")]
        public IEnumerable<Dropdown> GetlocDropdownvalues()
        {
            return _repoWrapper.Common.GetlocDropdownvalues();
        }

        [HttpGet("GetICDSpecialityCode/{ICDCODE}")]
        public IEnumerable<Dropdown> GetICDSpecialityCode(string ICDCODE)
        {
            return _repoWrapper.Common.GetICDSpecialityCode(ICDCODE);
        }

        [HttpGet("GetICDSpCode")]
        public IEnumerable<Dropdown> GetICDSpCode()
        {
            return _repoWrapper.Common.GetICDSpCode();
        }
        [HttpGet("Geticdcode")]
        public IEnumerable<Dropdown> Geticdcode()
        {
            return _repoWrapper.Common.Geticdcode();
        }
        [HttpGet("GetspecDropdownvalues")]
        public IEnumerable<Dropdown> GetspecDropdownvalues()
        {
            return _repoWrapper.Common.GetspecDropdownvalues();
        }

        [HttpGet("GetEngageDropdownvalues")]
        public IEnumerable<Dropdown> GetEngageDropdownvalues()
        {
            return _repoWrapper.Common.GetEngageDropdownvalues();
        }



        ///////-------medicalprecriptionvalues------------


        [HttpGet("GetFDvalues")]
        public IEnumerable<Dropdown> GetFDvalues()
        {
            return _repoWrapper.Common.GetFDvalues();
        }

        [HttpGet("GetFYvalues")]
        public IEnumerable<Dropdown> GetFYvalues()
        {
            return _repoWrapper.Common.GetFYvalues();
        }

        [HttpGet("GetDossgevalues")]
        public IEnumerable<Dropdown> GetDossgevalues()
        {
            return _repoWrapper.Common.GetDossgevalues();
        }
        [HttpGet("GetBrand")]
        public IEnumerable<Dropdown> GetBrand()
        {
            return _repoWrapper.Common.GetBrand();
        }


        [HttpGet("GetUOM")]
        public IEnumerable<Dropdown> GetUOM()
        {
            return _repoWrapper.Common.GetUOM();
        }

        [HttpGet("GetVendornames/{CMPID}")]
        public IEnumerable<Dropdown> GetVendornames(int CMPID)
        {
            return _repoWrapper.Common.GetVendornames(CMPID);
        }
        [HttpGet("GetBrandValues/{CMPID}")]
        public IEnumerable<Dropdown> GetBrandValues(int CMPID)
        {
            return _repoWrapper.Common.GetBrandValues(CMPID);
        }
        [HttpGet("Getsuppliervalues/{id}")]
        public IEnumerable<Dropdown> Getsuppliervalues(int id)
        {
            return _repoWrapper.Common.Getsuppliervalues(id);
        }


        [HttpGet("GetVendornamevalues/{Cmpid}")]
        public IEnumerable<Dropdown> GetVendornamevalues(int Cmpid)
        {
            return _repoWrapper.Common.GetVendornamevalues(Cmpid);
        }

        [HttpGet("GetstoreDropdownvalues/{CompanyID}/{id}/{name}")]
        public IEnumerable<Dropdown> GetstoreDropdownvalues(int CompanyID, int id, string name)
        {
            return _repoWrapper.Common.GetstoreDropdownvalues(CompanyID, id, name);
        }

        [HttpGet("GetstoreDropdownvaluesdesc/{CompanyID}/{name}")]
        public IEnumerable<Dropdown> GetstoreDropdownvaluesdesc(int CompanyID, string name)
        {
            return _repoWrapper.Common.GetstoreDropdownvaluesdesc(CompanyID, name);
        }


        [HttpGet("GetFullstoreDropdownvalues/{CompanyID}")]
        public IEnumerable<Dropdown> GetFullstoreDropdownvalues(int CompanyID)
        {
            return _repoWrapper.Common.GetFullstoreDropdownvalues(CompanyID);
        }

        [HttpGet("Getdrugndgroup/{CompanyID}")]
        public IEnumerable<Dropdown> Getdrugndgroup(int CompanyID)
        {
            return _repoWrapper.Common.Getdrugndgroup(CompanyID);
        }


        [HttpGet("GetbranchstoreDropdownvalues/{CompanyID}/{name}")]
        public IEnumerable<Dropdown> GetbranchstoreDropdownvalues(int CompanyID, string name)
        {
            return _repoWrapper.Common.GetbranchstoreDropdownvalues(CompanyID, name);
        }

        [HttpGet("GetDrugvalues/{id}/{cmpid}")]
        public IEnumerable<Dropdown> GetDrugvalues(int id, int cmpid)
        {
            return _repoWrapper.Common.GetDrugvalues(id, cmpid);
        }

        [HttpGet("Getlensvalues1/{VID}")]
        public IEnumerable<Dropdown> Getlensvalues1(int VID)
        {
            return _repoWrapper.Common.Getlensvalues1(VID);
        }

        [HttpGet("Getlocationcityvalues")]
        public IEnumerable<Dropdown> Getlocationcityvalues()
        {
            return _repoWrapper.Common.Getlocationcityvalues();
        }

        [HttpGet("Getlocationvalues/{LID}")]
        public IEnumerable<Dropdown> Getlocationvalues(int LID)
        {
            return _repoWrapper.Common.Getlocationvalues(LID);
        }

        //-----------------------InvestigationPrescription-------------------

        [HttpGet("GetInvDep/{CmpID}")]
        public IEnumerable<Dropdown> GetInvDep(int CmpID)
        {
            return _repoWrapper.Common.GetInvDep(CmpID);
        }

        [HttpGet("Getinvestvalues/{CmpID}")]
        public IEnumerable<Dropdown> Getinvestvalues(int CmpID)
        {
            return _repoWrapper.Common.Getinvestvalues(CmpID);
        }

        [HttpGet("Gettestdiagnosisvalues/{ID}/{CmpID}")]
        public IEnumerable<Dropdown> Gettestdiagnosisvalues(int ID, int CmpID)
        {
            return _repoWrapper.Common.Gettestdiagnosisvalues(ID, CmpID);
        }
        [HttpGet("GetTab/{CmpID}")]
        public IEnumerable<Dropdown> GetTab(int CmpID)
        {
            return _repoWrapper.Common.GetTab(CmpID);
        }

        [HttpGet("Gettaxvalues")]
        public IEnumerable<Dropdown> Gettaxvalues()
        {
            return _repoWrapper.Common.Gettaxvalues();
        }

        [HttpGet("GetDrugvalues1/{CompanyID}")]
        public IEnumerable<Dropdown> GetDrugvalues1(int CompanyID)
        {
            return _repoWrapper.Common.GetDrugvalues1(CompanyID);
        }

        [HttpGet("GetRevnueValues")]
        public IEnumerable<Dropdown> GetRevnueValues()
        {
            return _repoWrapper.Common.GetRevnueValues();
        }
        [HttpGet("GetDrug/{Grn}/{StoreID}/{CMPID}")]
        public IEnumerable<Dropdown> GetDrug(string Grn, int StoreID, int CMPID)
        {
            return _repoWrapper.Common.GetDrug(Grn, StoreID, CMPID);
        }

        [HttpGet("Geticdspecvalues")]
        public IEnumerable<Dropdown> Geticdspecvalues()
        {
            return _repoWrapper.Common.Geticdspecvalues();
        }

        [HttpGet("Getpackagevalues/{CMPID}")]
        public IEnumerable<Dropdown> Getpackagevalues(int CMPID)
        {
            return _repoWrapper.Common.Getpackagevalues(CMPID);
        }

        [HttpGet("getGen/{CMPID}")]
        public IEnumerable<Dropdown> getGen(int CMPID)
        {
            return _repoWrapper.Common.getGen(CMPID);
        }

        [HttpGet("Geteyeerrvalues")]
        public IEnumerable<Dropdown> Geteyeerrvalues()
        {
            return _repoWrapper.Common.Geteyeerrvalues();
        }
        
        [HttpGet("GetServiceTag")]
        public IEnumerable<Dropdown> GetServiceTag()
        {
            return _repoWrapper.Common.GetServiceTag();
        }
        [HttpGet("Geticddescvalues/{ID}")]
        public IEnumerable<Dropdown> Geticddescvalues(int ID)
        {
            return _repoWrapper.Common.Geticddescvalues(ID);
        }

        [HttpGet("Geticicddescvalues/{id}/{Cmpid}")]
        public IEnumerable<Dropdown> Geticicddescvalues(int id, int Cmpid)
        {
            return _repoWrapper.Common.Geticicddescvalues(id, Cmpid);
        }

        [HttpGet("Getoptmodelvalues/{id}")]
        public IEnumerable<Dropdown> Getoptmodelvalues(int id)
        {
            return _repoWrapper.Common.Getoptmodelvalues(id);
        }

        [HttpGet("Getsqutreat/{id}")]
        public IEnumerable<Dropdown> Getsqutreat(int id)
        {
            return _repoWrapper.Common.Getsqutreat(id);
        }


        ////////////////////////////////////////////////////////////////Yours Lens
        [HttpGet("GstSearch")]
        public IEnumerable<Dropdown> GstSearch()
        {
            return _repoWrapper.Common.GstSearch();
        }
        [HttpGet("CessSearch/{ID}")]
        public IEnumerable<Dropdown> CessSearch(int ID)
        {
            return _repoWrapper.Common.CessSearch(ID);
        }
        [HttpGet("AddCessSearch/{ID}")]
        public IEnumerable<Dropdown> AddCessSearch(int ID)
        {
            return _repoWrapper.Common.AddCessSearch(ID);
        }


        [HttpGet("GSTperSearch/{ID}")]
        public IEnumerable<Dropdown> GSTperSearch(int ID)
        {
            return _repoWrapper.Common.GSTperSearch(ID);
        }


        [HttpGet("GetBrandLens/{cmpid}")]
        public IEnumerable<Dropdown> GetBrandLens(int cmpid)
        {
            return _repoWrapper.Common.GetBrandLens(cmpid);
        }

        [HttpGet("GetBrandFrame/{cmpid}")]
        public IEnumerable<Dropdown> GetBrandFrame(int cmpid)
        {
            return _repoWrapper.Common.GetBrandFrame(cmpid);
        }

        [HttpGet("UOMSearch")]
        public IEnumerable<Dropdown> UOMSearch()
        {
            return _repoWrapper.Common.UOMSearch();
        }

        ///////////////////////////////////////////////////////////////////////////// Yours TreatmentMASTER 

        [HttpGet("GetSplName")]
        public IEnumerable<Dropdown> GetSplName()
        {
            return _repoWrapper.Common.GetSplName();
        }

        [HttpGet("GetSquintvalue")]
        public IEnumerable<Dropdown> GetSquintvalue()
        {
            return _repoWrapper.Common.GetSquintvalue();
        }

        [HttpGet("print/{PAID}")]
        public dynamic print(string PAID)
        {
            return _repoWrapper.Common.print(PAID);
        }

        [HttpGet("GetSurgeryLens/{CMPID}")]
        public IEnumerable<Dropdown> GetSurgeryLens(int CMPID)
        {
            return _repoWrapper.Common.GetSurgeryLens(CMPID);
        }


        [HttpGet("GetRooms/{cmpid}")]
        public IEnumerable<Dropdown> GetRooms(int cmpid)
        {
            return _repoWrapper.Common.GetRooms(cmpid);
        }

        [HttpGet("GetRole")]
        public IEnumerable<Dropdown> GetRole()
        {
            return _repoWrapper.Common.GetRole();
        }
        [HttpGet("Getservice/{cmpid}")]
        public IEnumerable<Dropdown> Getservice(int cmpid)
        {
            return _repoWrapper.Common.Getservice(cmpid);
        }
        [HttpGet("GetRoomstatus")]
        public IEnumerable<Dropdown> GetRoomstatus()
        {
            return _repoWrapper.Common.GetRoomstatus();
        }


        [HttpGet("GetIndentSurgeonDetails")]
        public IEnumerable<Dropdown> GetIndentSurgeonDetails()
        {
            return _repoWrapper.Common.GetIndentSurgeonDetails();
        }

        [HttpGet("GetIndentOTDetails")]
        public IEnumerable<Dropdown> GetIndentOTDetails()
        {
            return _repoWrapper.Common.GetIndentOTDetails();
        }

        [HttpGet("GetIndentDrugDetails")]
        public IEnumerable<Dropdown> GetIndentDrugDetails()
        {
            return _repoWrapper.Common.GetIndentDrugDetails();
        }

        [HttpGet("GetIndentUOMDetails")]
        public IEnumerable<Dropdown> GetIndentUOMDetails()
        {
            return _repoWrapper.Common.GetIndentUOMDetails();
        }


        [HttpGet("SSCTypeDetails")]
        public IEnumerable<Dropdown> SSCTypeDetails()
        {
            return _repoWrapper.Common.SSCTypeDetails();
        }

        [HttpGet("GetBrandAll/{CompanyID}")]
        public IEnumerable<Dropdown> GetBrandAll(int CompanyID)
        {
            return _repoWrapper.Common.GetBrandAll(CompanyID);
        }

        [HttpGet("GetBrandAllDrugs")]
        public IEnumerable<Dropdown> GetBrandAllDrugs()
        {
            return _repoWrapper.Common.GetBrandAllDrugs();
        }

        [HttpGet("GetBrands1")]
        public IEnumerable<Dropdown> GetBrands1()
        {
            return _repoWrapper.Common.GetBrands1();
        }


        [HttpGet("GetRoomtypes")]
        public IEnumerable<Dropdown> GetRoomtypes()
        {
            return _repoWrapper.Common.GetRoomtypes();
        }


        [HttpGet("getConcerntextfile/{CompanyID}")]
        public dynamic getConcerntextfile(int CompanyID)
        {
            return _repoWrapper.Common.getConcerntextfile(CompanyID);
        }


        [HttpGet("Getcountryvalues")]
        public dynamic Getcountryvalues()
        {
            return _repoWrapper.Common.Getcountryvalues();
        }

        ///////////////////////////////currency////////////////////////////////////////

        [HttpGet("GetCurrencyvalues/{CMPID}")]
        public IEnumerable<Dropdown> GetCurrencyvalues(int CMPID)
        {
            return _repoWrapper.Common.GetCurrencyvalues(CMPID);
        }
        [HttpGet("Getsetupregvalues/{CMPID}")]
        public IEnumerable<Dropdown> Getsetupregvalues(int CMPID)
        {
            return _repoWrapper.Common.Getsetupregvalues(CMPID);
        }
        [HttpGet("GetEyedoctornamevalueswithappointmentonly/{CMPID}")]
        public IEnumerable<Dropdown> GetEyedoctornamevalueswithappointmentonly(int CMPID)
        {
            return _repoWrapper.Common.GetEyedoctornamevalueswithappointmentonly(CMPID);
        }


        [HttpGet("GetFDDTDescriptionsvalues/{CMPID}")]
        public IEnumerable<Dropdown> GetFDDTDescriptionsvalues(int CMPID)
        {
            return _repoWrapper.Common.GetFDDTDescriptionsvalues(CMPID);
        }

        [HttpGet("GetSyringingDescriptions/{CMPID}")]
        public IEnumerable<Dropdown> GetSyringingDescriptions(int CMPID)
        {
            return _repoWrapper.Common.GetSyringingDescriptions(CMPID);
        }



        [HttpGet("GetRegurgitationDescriptions/{CMPID}")]
        public IEnumerable<Dropdown> GetRegurgitationDescriptions(int CMPID)
        {
            return _repoWrapper.Common.GetRegurgitationDescriptions(CMPID);
        }


        [HttpGet("GetFluidDescriptions/{CMPID}")]
        public IEnumerable<Dropdown> GetFluidDescriptions(int CMPID)
        {
            return _repoWrapper.Common.GetFluidDescriptions(CMPID);
        }


        [HttpGet("GetStopDescriptions/{CMPID}")]
        public IEnumerable<Dropdown> GetStopDescriptions(int CMPID)
        {
            return _repoWrapper.Common.GetStopDescriptions(CMPID);
        }

        [HttpGet("Loadallavailablelanguages")]
        public dynamic Loadallavailablelanguages()
        {
            return _repoWrapper.Common.Loadallavailablelanguages();
        }


        [HttpGet("GetPatientDob/{UIN}/{CMPID}")]
        public dynamic GetPatientDob(string UIN, int CMPID)
        {
            return _repoWrapper.Common.GetPatientDob(UIN, CMPID);
        }

        [HttpGet("Getocularvalues")]
        public dynamic Getocularvalues()
        {
            return _repoWrapper.Common.Getocularvalues();
        }

        [HttpGet("Getsystemicvalues")]
        public dynamic Getsystemicvalues()
        {
            return _repoWrapper.Common.Getsystemicvalues();
        }

        [HttpGet("Getocumvalues/{ID}")]
        public IEnumerable<Dropdown> Getocumvalues(int ID)
        {
            return _repoWrapper.Common.Getocumvalues(ID);
        }

        [HttpGet("Getvfvalues/{ID}")]
        public IEnumerable<Dropdown> Getvfvalues(int ID)
        {
            return _repoWrapper.Common.Getvfvalues(ID);
        }
        [HttpGet("Getanglevalues/{ID}")]
        public IEnumerable<Dropdown> Getanglevalues(int ID)
        {
            return _repoWrapper.Common.Getanglevalues(ID);
        }
        [HttpGet("Getposvalues/{ID}")]
        public IEnumerable<Dropdown> Getposvalues(int ID)
        {
            return _repoWrapper.Common.Getposvalues(ID);
        }
        [HttpGet("Getacamvalues/{ID}")]
        public IEnumerable<Dropdown> Getacamvalues(int ID)
        {
            return _repoWrapper.Common.Getacamvalues(ID);
        }
        [HttpGet("Getacavvalues/{ID}")]
        public IEnumerable<Dropdown> Getacavvalues(int ID)
        {
            return _repoWrapper.Common.Getacavvalues(ID);
        }

        [HttpGet("Getwfdtvalues/{ID}")]
        public IEnumerable<Dropdown> Getwfdtvalues(int ID)
        {
            return _repoWrapper.Common.Getwfdtvalues(ID);
        }
        [HttpGet("Getspmvalues/{ID}")]
        public IEnumerable<Dropdown> Getspmvalues(int ID)
        {
            return _repoWrapper.Common.Getspmvalues(ID);
        }
        [HttpGet("Getspvvalues/{ID}")]
        public IEnumerable<Dropdown> Getspvvalues(int ID)
        {
            return _repoWrapper.Common.Getspvvalues(ID);
        }
        [HttpGet("Getarcvalues/{ID}")]
        public IEnumerable<Dropdown> Getarcvalues(int ID)
        {
            return _repoWrapper.Common.Getarcvalues(ID);
        }
        [HttpGet("Getpbcvalues/{ID}")]
        public IEnumerable<Dropdown> Getpbcvalues(int ID)
        {
            return _repoWrapper.Common.Getpbcvalues(ID);
        }
        [HttpGet("Getampvalues/{ID}")]
        public IEnumerable<Dropdown> Getampvalues(int ID)
        {
            return _repoWrapper.Common.Getampvalues(ID);
        }
        [HttpGet("Getfrqyvalues/{ID}")]
        public IEnumerable<Dropdown> Getfrqyvalues(int ID)
        {
            return _repoWrapper.Common.Getfrqyvalues(ID);
        }
        [HttpGet("Gettypvalues/{ID}")]
        public IEnumerable<Dropdown> Gettypvalues(int ID)
        {
            return _repoWrapper.Common.Gettypvalues(ID);
        }
        [HttpGet("Getpurvalues/{ID}")]
        public IEnumerable<Dropdown> Getpurvalues(int ID)
        {
            return _repoWrapper.Common.Getpurvalues(ID);
        }
        [HttpGet("Getsacvalues/{ID}")]
        public IEnumerable<Dropdown> Getsacvalues(int ID)
        {
            return _repoWrapper.Common.Getsacvalues(ID);
        }
        [HttpGet("Getabhvalues/{ID}")]
        public IEnumerable<Dropdown> Getabhvalues(int ID)
        {
            return _repoWrapper.Common.Getabhvalues(ID);
        }
        [HttpGet("Getconvvalues/{ID}")]
        public IEnumerable<Dropdown> Getconvvalues(int ID)
        {
            return _repoWrapper.Common.Getconvvalues(ID);
        }
        [HttpGet("Getooevalues/{ID}")]
        public IEnumerable<Dropdown> Getooevalues(int ID)
        {
            return _repoWrapper.Common.Getooevalues(ID);
        }
        [HttpGet("Getoscvalues/{ID}")]
        public IEnumerable<Dropdown> Getoscvalues(int ID)
        {
            return _repoWrapper.Common.Getoscvalues(ID);
        }

        [HttpGet("GetGenericvalue/{ID}")]
        public IEnumerable<Dropdown> GetGenericvalue(int ID)
        {
            return _repoWrapper.Common.GetGenericvalue(ID);
        }


        [HttpGet("GettingRunningNo/{CMPID}/{TC}")]
        public dynamic GettingRunningNo(int CMPID, int TC)
        {
            return _repoWrapper.Common.GettingRunningNo(CMPID, TC);
        }

        [HttpGet("Gettmhvalues")]
        public IEnumerable<Dropdown> Gettmhvalues()
        {
            return _repoWrapper.Common.Gettmhvalues();
        }
        [HttpGet("Gettbutvalues")]
        public IEnumerable<Dropdown> Gettbutvalues()
        {
            return _repoWrapper.Common.Gettbutvalues();
        }

        [HttpGet("Gettimeintervalesbetweendates/{cmpid}")]
        public dynamic Gettimeintervalesbetweendates(int cmpid)
        {
            return _repoWrapper.Common.Gettimeintervalesbetweendates(cmpid);
        }

        [HttpGet("Checktimefordoctor/{cmpid}/{date}/{time}/{doc}/{gap}")]
        public dynamic Checktimefordoctor(int cmpid, string date, string time, int doc, int gap)
        {
            return _repoWrapper.Common.Checktimefordoctor(cmpid, date, time, doc, gap);
        }
        [HttpGet("Getdiaspecvalues/{CMPID}")]
        public IEnumerable<Dropdown> Getdiaspecvalues(int CMPID)
        {
            return _repoWrapper.Common.Getdiaspecvalues(CMPID);
        }


    }
}
