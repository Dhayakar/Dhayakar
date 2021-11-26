
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WebApiCore.Controllers
{
    [Route("[controller]")]
    public class HelpController : Controller
    {
        private IRepositoryWrapper _repoWrapper;




        public HelpController(IRepositoryWrapper repoWrapper)
        {
               _repoWrapper = repoWrapper;
        }


        [HttpGet("BMISearch/{ID}")]
        public Help BMISearch(int ID)
        {
            return _repoWrapper.Help.BMISearch(ID);
        }

        [HttpGet("PACSearch/{ID}")]
        public Help PACSearch(int ID)
        {
            return _repoWrapper.Help.PACSearch(ID);
        }


        [HttpGet("SlitLamp/{ID}")]
        public Help SlitLamp(int ID)
        {
            return _repoWrapper.Help.SlitLamp(ID);
        }
        [HttpGet("Fundus/{ID}")]
        public Help Fundus(int ID)
        {
            return _repoWrapper.Help.Fundus(ID);
        }

        [HttpGet("getTaxData/{id}")]
        public Help getTaxData(int id)
        {
            return _repoWrapper.Help.getTaxData(id);
        }

        [HttpGet("getBusinessRule/{Cmpid}/{ID}")]
        public Help getBusinessRule(int Cmpid,int ID)
        {
            return _repoWrapper.Help.getBusinessRule(Cmpid, ID);
        }

        [HttpGet("GetConsultationSummary/{Fromdate}/{Todate}/{Cmpid}")]
        public Help GetConsultationSummary(DateTime Fromdate, DateTime Todate,int Cmpid)
        {
            return _repoWrapper.Help.GetConsultationSummary(Fromdate, Todate, Cmpid);
        }
        [HttpGet("GetPatientDetails/{vdate}/{CompanyID}")]
        public Help GetPatientDetails(DateTime vdate, int CompanyID)
        {
            return _repoWrapper.Help.GetPatientDetails(vdate, CompanyID);
        }
        [HttpGet("getInsurance")]
        public Help getInsurance()
        {
            return _repoWrapper.Help.getInsurance();
        }
        [HttpGet("GetConfigureDetail")]
        public Help GetConfigureDetail()
        {
            return _repoWrapper.Help.GetConfigureDetail();
        }

        [HttpGet("StoreSearch")]
        public Help StoreSearch()
        {
            return _repoWrapper.Help.StoreSearch();
        }


        [HttpGet("searchStoreSearch")]
        public Help searchStoreSearch()
        {
            return _repoWrapper.Help.searchStoreSearch();
        }

        

  [HttpGet("getSearchdetailsofindent/{id}")]
        public Help getSearchdetailsofindent(int id)
        {
            return _repoWrapper.Help.getSearchdetailsofindent(id);
        }

        [HttpGet("CustomerMasterDetails/{Cmpid}")]
        public Help CustomerMasterDetails(int Cmpid)
        {
            return _repoWrapper.Help.CustomerMasterDetails(Cmpid);
        }

        [HttpGet("DonorDetails/{Cmpid}")]
        public Help DonorDetails(int Cmpid)
        {
            return _repoWrapper.Help.DonorDetails(Cmpid);
        }

        [HttpGet("RoomSearch/{CompanyID}")]
        public Help RoomSearch(int CompanyID)
        {
            return _repoWrapper.Help.RoomSearch(CompanyID);
        }

        [HttpGet("getDrugGroupDetails/{Cmpid}")]
        public Help getDrugGroupDetails(int Cmpid)
        {
            return _repoWrapper.Help.getDrugGroupDetails(Cmpid);
        }

        [HttpGet("getSurgeryDischargeDetails/{id}/{GMTTIME}")]
        public Help getSurgeryDischargeDetails(int id,string GMTTIME)
        {
            return _repoWrapper.Help.getSurgeryDischargeDetails(id, GMTTIME);
        }

        [HttpGet("getTranTypeDetails/{id}")]
        public Help getTranTypeDetails(int id)
        {
            return _repoWrapper.Help.getTranTypeDetails(id);
        }

        [HttpGet("getCompanyName/{CompanyID}")]
        public Help getCompanyName(string CompanyID)
        {
            return _repoWrapper.Help.getCompanyName(CompanyID);
        }


        [HttpGet("getOperationTheatreextension/{id}")]
        public Help getOperationTheatreextension(int id)
        {
            return _repoWrapper.Help.getOperationTheatreextension(id);
        }

        [HttpGet("getDischargedPatientDetails/{id}/{GMTTIME}")]
        public Help DischargedPatientDetails(int id,string GMTTIME)
        {
            return _repoWrapper.Help.DischargedPatientDetails(id, GMTTIME);
        }

        [HttpGet("getPreviousSurgeryDetails/{UIN}/{id}")]
        public Help PreviousSurgeryDetails(string UIN,int id)
        {
            return _repoWrapper.Help.PreviousSurgeryDetails(UIN,id);
        }


        [HttpGet("getSurgery")]
        public Help getSurgery()
        {
            return _repoWrapper.Help.getSurgery();
        }
        [HttpGet("getSurgery1/{name}/{id}")]
        public Help getSurgery1(string name,int id)
        {
            return _repoWrapper.Help.getSurgery1(name,id);
        }
        [HttpGet("GetDetailsNumCol/{CMPID}")]
        public Help GetDetailsNumCol(int CmpID)
        {
            return _repoWrapper.Help.GetDetailsNumCol(CmpID);
        }

        [HttpGet("getVendor/{name}")]
        public Help getVendor(int name)
        {
            return _repoWrapper.Help.getVendor(name);
        }
        [HttpGet("getVendor1/{name}/{id}")]
        public Help getVendor1(string name , int id)
        {
            return _repoWrapper.Help.getVendor1(name,id);
        }

        [HttpGet("getEmployeeAll/{name}")]
        public Help getEmployeeAll(int name)
        {
            return _repoWrapper.Help.getEmployeeAll(name);
        }


        [HttpGet("getEmployee/{empid}")]
        public Help getEmployee(int empid)
        {
            return _repoWrapper.Help.getEmployee(empid);
        }

        [HttpGet("getRegCode1")]
        public Help getRegCode1()
        {
            return _repoWrapper.Help.getRegCode1();
        }

        [HttpGet("getCmpRegCode/{name}/{ID}")]
        public Help getCmpRegCode(string name, int ID)
        {
            return _repoWrapper.Help.getCmpRegCode(name, ID);
        }
        [HttpGet("getAppRegCode/{name}/{ID}")]
        public Help getAppRegCode(string name, int ID)
        {
            return _repoWrapper.Help.getAppRegCode(name, ID);
        }
        [HttpGet("getFinalbilling/{name}/{ID}")]
        public Help getFinalbilling(string name, int id)
        {
            return _repoWrapper.Help.getFinalbilling(name, id);
        }
        [HttpGet("getFinalbilling1/{name}/{ID}")]
        public Help getFinalbilling1(string name, int id)
        {
            return _repoWrapper.Help.getFinalbilling1(name, id);
        }
        [HttpGet("getDashhelpCode1")]
        public Help getDashhelpCode1()
        {
            return _repoWrapper.Help.getDashhelpCode1();
        }

        

        [HttpGet("getRegCode/{name}/{ID}")]
        public Help getRegCode(string name,int ID)
        {
            return _repoWrapper.Help.getRegCode(name,ID);
        }
        [HttpGet("getAdvpay/{name}/{ID}")]
        public Help getAdvpay(string name, int ID)
        {
            return _repoWrapper.Help.getAdvpay(name, ID);
        }
        [HttpGet("GetComplaints/{RegistrationTranID}")]
        public Help GetComplaints(int RegistrationTranID)
        {
            return _repoWrapper.Help.GetComplaints(RegistrationTranID);
        }
        [HttpGet("GetComplaintsHistory/{UIN}")]
        public Help GetComplaintsHistory(string UIN)
        {
            return _repoWrapper.Help.GetComplaintsHistory(UIN);
        }


        [HttpGet("GetComplaintsHistoryWithstring/{UIN}/{d}")]
        public Help GetComplaintsHistoryWithstring(string UIN, string d)
        {
            return _repoWrapper.Help.GetComplaintsHistoryWithstring(UIN,  d);
        }


        [HttpGet("getDoc/{Name}")]
        public Help getDoc(int Name)
        {
            return _repoWrapper.Help.getDoc(Name);
        }

        [HttpGet("getDoc1/{name}")]
        public Help getDoc1(string name)
        {
            return _repoWrapper.Help.getDoc1(name);
        }

        [HttpGet("GetPatientHistory/{UIN}")]
        public Help GetPatientHistory(string UIN)
        {
            return _repoWrapper.Help.GetPatientHistory(UIN);
        }
        [HttpGet("GetPatientHistoryWithstring/{UIN}/{d}")]
        public Help GetPatientHistoryWithstring(string UIN, string d)
        {
            return _repoWrapper.Help.GetPatientHistoryWithstring(UIN,d);
        }

        
        [HttpGet("getRevReg/{UIN}")]
        public Help getRevReg(string UIN)
        {
            return _repoWrapper.Help.getRevReg(UIN);
        }


        [HttpGet("getSurgeryCode1")]
        public Help getSurgeryCode1()
        {
            return _repoWrapper.Help.getSurgeryCode1();
        }

        [HttpGet("getSurgeryCode/{name}")]
        public Help getSurgeryCode(string name)
        {
            return _repoWrapper.Help.getSurgeryCode(name);
        }
      
        [HttpGet("CustomerOrder/{CMPID}")]
        public Help CustomerOrder(int CMPID, string Value)
        {
            return _repoWrapper.Help.CustomerOrder(CMPID);
        }

        [HttpGet("getCode1/{ICDGROUPCODE}")]
        public Help getCode1(int ICDGROUPCODE)
        {
            return _repoWrapper.Help.getCode1(ICDGROUPCODE);
        }


        [HttpGet("getDrug/{Cmpid}")]
        public Help getDrug(int Cmpid)
        {
            return _repoWrapper.Help.getDrug(Cmpid);
        }

        [HttpGet("getUserDoc")]
        public Help getUserDoc()
        {
            return _repoWrapper.Help.getUserDoc();
        }

        [HttpGet("getUserEmp")]
        public Help getUserEmp()
        {
            return _repoWrapper.Help.getUserEmp();
        }

        [HttpGet("deleteDoc")]
        public Help deleteDoc()
        {
            return _repoWrapper.Help.deleteDoc();
        }



        [HttpGet("getDrugGroupDesc/{Id}")]
        public Help getDrugGroupDesc(int Id)
        {
            return _repoWrapper.Help.getDrugGroupDesc(Id);
        }
        [HttpGet("getStoremaster/{Name}")]
        public Help getStoremaster(int Name)
        {
            return _repoWrapper.Help.getStoremaster(Name);
        }


        [HttpGet("getStoremaster1/{Name}/{id}")]
        public Help getStoremaster1(string Name, int id)
        {
            return _repoWrapper.Help.getStoremaster1(Name, id);
        }

        [HttpGet("getOperationTheatre/{id}")]
        public Help getOperationTheatre(int id)
        {
            return _repoWrapper.Help.getOperationTheatre(id);
        }


        [HttpGet("getOperationTheatre1/{Name}/{id}")]
        public Help getOperationTheatre1(string Name, int id)
        {
            return _repoWrapper.Help.getOperationTheatre1(Name,id);
        }


        [HttpGet("getpostoperative/{name}")]
        public Help getpostoperative(string name)
        {
            return _repoWrapper.Help.getpostoperative(name);
        }

        [HttpGet("getpostoperative1/{name}/{phone}")]
        public Help getpostoperative1(string name, string phone)
        {
            return _repoWrapper.Help.getpostoperative1(name, phone);
        }

        [HttpGet("getpatientlatest/{name}")]
        public Help getpatientlatest(string name)
        {
            return _repoWrapper.Help.getpatientlatest(name);
        }

        [HttpGet("getpatientlatest1/{name}")]
        public Help getpatientlatest1(string name)
        {
            return _repoWrapper.Help.getpatientlatest1(name);
        }

        [HttpGet("getpatientlatest2/{name}")]
        public Help getpatientlatest2(string name)
        {
            return _repoWrapper.Help.getpatientlatest2(name);
        }


        [HttpGet("getpatientlatest3/{name}")]
        public Help getpatientlatest3(string name)
        {
            return _repoWrapper.Help.getpatientlatest3(name);
        }



        [HttpGet("getModuleMasterDetails/{id}")]
        public Help getModuleMasterDetails(int id)
        {
            return _repoWrapper.Help.getModuleMasterDetails(id);
        }


        [HttpGet("getInvPre/{uin}/{IPID}/{Cmpid}")]
        public Help getInvPre(string uin, string IPID, int Cmpid)
        {
            return _repoWrapper.Help.getInvPre(uin, IPID, Cmpid);
        }

        [HttpGet("PopupSearch/{CompanyID}")]
        public Help PopupSearch(int CompanyID)
        {
            return _repoWrapper.Help.PopupSearch(CompanyID);
        }
        [HttpGet("PopupSearch2/{OptionType}")]
        public Help PopupSearch2(string OptionType)
        {
            return _repoWrapper.Help.PopupSearch2(OptionType);
        }

      
        [HttpGet("PopupSearch1/{val}")]
        public Help PopupSearch1(int val)
        {
            return _repoWrapper.Help.PopupSearch1(val);
        }

        [HttpGet("{GetRoomTransferDetails}")]
        public Help GetRoomTransferDetails()
        {
            return _repoWrapper.Help.GetRoomTransferDetails();
        }


        [HttpGet("getIOProcedureTempDetails/{ICDSpecialityCode}")]
        public dynamic getIOProcedureTempDetails(int ICDSpecialityCode)
        {
            return _repoWrapper.Help.getIOProcedureTempDetails(ICDSpecialityCode);
        }

        [HttpGet("getPatientInsurance/{FromDate}/{ToDate}/{Insurancetype}/{CMPID}")]
        public dynamic getPatientInsurance(DateTime FromDate, DateTime ToDate,string Insurancetype,int CMPID)
        {
            return _repoWrapper.Help.getPatientInsurance(FromDate, ToDate, Insurancetype, CMPID);
        }

        [HttpGet("getInvReg/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic getInvReg(string FromDate, string ToDate, int CMPID)
        {
            return _repoWrapper.Help.getInvReg(FromDate, ToDate, CMPID);
        }
       
        [HttpGet("getCampScreenedpatients/{CampID}/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic getCampScreenedpatients(int CampID,DateTime FromDate, DateTime ToDate, int CMPID)
        {
            return _repoWrapper.Help.getCampScreenedpatients(CampID,FromDate, ToDate, CMPID);
        }
        [HttpGet("getCampSurgerypatients/{CampID}/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic getCampSurgerypatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {
            return _repoWrapper.Help.getCampSurgerypatients(CampID, FromDate, ToDate, CMPID);
        }

        [HttpGet("getCampSurgeryunderwentpatients/{CampID}/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic getCampSurgeryunderwentpatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {
            return _repoWrapper.Help.getCampSurgeryunderwentpatients(CampID, FromDate, ToDate, CMPID);
        }
        [HttpGet("getCamptoHospitalvisitedpatients/{CampID}/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic getCamptoHospitalvisitedpatients(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {
            return _repoWrapper.Help.getCamptoHospitalvisitedpatients(CampID, FromDate, ToDate, CMPID);
        }
        [HttpGet("GetPendingCampSurgeryadvisedPatient/{CampID}/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic GetPendingCampSurgeryadvisedPatient(int CampID, DateTime FromDate, DateTime ToDate, int CMPID)
        {
            return _repoWrapper.Help.GetPendingCampSurgeryadvisedPatient(CampID, FromDate, ToDate, CMPID);
        }

        [HttpGet("GetAppointmentDetails/{CMPID}/{Searchvalue}")]
        public dynamic GetAppointmentDetails( int CMPID,string Searchvalue)
        {
            return _repoWrapper.Help.GetAppointmentDetails(CMPID, Searchvalue);
        }



        [HttpGet("getopticalMaterialdetails")]
        public dynamic getopticalMaterialdetails()
        {
            return _repoWrapper.Help.getopticalMaterialdetails();
        }


        [HttpGet("getMedicalRegisters/{FromDate}/{ToDate}/{CMPID}")]
        public dynamic getMedicalRegisters(DateTime FromDate, DateTime ToDate,  int CMPID)
        {
            return _repoWrapper.Help.getMedicalRegisters(FromDate, ToDate, CMPID);
        }

        [HttpGet("InvestPresc/{cmpid}")]
        public dynamic InvestPresc(int cmpid)
        {
            return _repoWrapper.Help.InvestPresc(cmpid);
        }

    }
}

