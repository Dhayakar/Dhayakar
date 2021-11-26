using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class ManagementdashboardController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public ManagementdashboardController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetPatientpopulationdetails/{Yearstring}/{Monthstring}/{Daystring}/{CompanyID}")]
        public RegistrationMasterViewModel GetPatientpopulationdetails(string Yearstring, string Monthstring, string Daystring, int CompanyID)
        {
            return _repoWrapper.managementDash.GetPatientpopulationdetails(Yearstring, Monthstring, Daystring, CompanyID);
        }

        [HttpGet("GetSpecificperiodPatientpopulationdetails/{FromDate}/{ToDate}/{CompanyID}/{Phase}")]
        public RegistrationMasterViewModel GetSpecificperiodPatientpopulationdetails(string FromDate, string ToDate, int CompanyID, string Phase)
        {
            return _repoWrapper.managementDash.GetSpecificperiodPatientpopulationdetails(FromDate, ToDate, CompanyID, Phase);
        }


        ///Revenue Specific period Total details ////

        [HttpGet("GetSpecificperiodRevenueBriefdetails/{FromDate}/{ToDate}/{CompanyID}")]
        public ManagementDashboardViewModel GetSpecificperiodRevenueBriefdetails(string FromDate, string ToDate, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSpecificperiodRevenueBriefdetails(FromDate, ToDate, CompanyID);
        }



        [HttpGet("GetSpecificperiodRevenueBriefBreakupdetails/{FromDate}/{ToDate}/{CompanyID}/{Description}/{OLMID}")]
        public ManagementDashboardViewModel GetSpecificperiodRevenueBriefBreakupdetails(string FromDate, string ToDate, int CompanyID, string Description, string OLMID)
        {
            return _repoWrapper.managementDash.GetSpecificperiodRevenueBriefBreakupdetails(FromDate, ToDate, CompanyID, Description, OLMID);
        }

        
        [HttpGet("GetMonthPatientpopulationdetails/{FromDate}/{ToDate}/{CompanyID}/{phase}")]
        public RegistrationMasterViewModel GetMonthPatientpopulationdetails(string FromDate, string ToDate, int CompanyID, string phase)
        {
            return _repoWrapper.managementDash.GetMonthPatientpopulationdetails(FromDate, ToDate, CompanyID, phase);
        }

        [HttpGet("GetQuarterlycomparisionPatientpopulationdetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetQuarterlycomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetQuarterlycomparisionPatientpopulationdetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }


        [HttpGet("GetHalfyearlycomparisionPatientpopulationdetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetHalfyearlycomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetHalfyearlycomparisionPatientpopulationdetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }


        [HttpGet("GetAnnualcomparisionPatientpopulationdetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetAnnualcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetAnnualcomparisionPatientpopulationdetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }
        [HttpGet("GetQuarterlyPiechartcomparisionPatientpopulationdetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}/{id}")]
        public ManagementDashboardViewModel GetQuarterlyPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID, string id)
        {
            return _repoWrapper.managementDash.GetQuarterlyPiechartcomparisionPatientpopulationdetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID, id);
        }

        [HttpGet("GetHalfPiechartcomparisionPatientpopulationdetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}/{id}")]
        public ManagementDashboardViewModel GetHalfPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID, string id)
        {
            return _repoWrapper.managementDash.GetHalfPiechartcomparisionPatientpopulationdetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID, id);
        }

        [HttpGet("GetAnnualPiechartcomparisionPatientpopulationdetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}/{id}")]
        public ManagementDashboardViewModel GetAnnualPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID, string id)
        {
            return _repoWrapper.managementDash.GetAnnualPiechartcomparisionPatientpopulationdetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID, id);
        }

        [HttpGet("GetUsernamepasswordWITHPARENTIDZERO/{username}/{password}/{CompanyID}/{Roldedescrip}")]
        public RegistrationMasterViewModel GetUsernamepasswordWITHPARENTIDZERO(string username, string password, string CompanyID,  string  Roldedescrip)
        {
            return _repoWrapper.managementDash.GetUsernamepasswordWITHPARENTIDZERO(username, password, CompanyID, Roldedescrip);
        }

        [HttpGet("GetSpecificperiodRevenuedetails/{FromDate}/{ToDate}/{CompanyID}")]
        public ManagementDashboardViewModel GetSpecificperiodRevenuedetails(string FromDate, string ToDate, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSpecificperiodRevenuedetails(FromDate, ToDate, CompanyID);
        }

        [HttpGet("GetSpecificperiodsurgerydetailsdetails/{FromDate}/{ToDate}/{compoid}")]
        public ManagementDashboardViewModel GetSpecificperiodsurgerydetailsdetails(DateTime FromDate, DateTime ToDate, int compoid)
        {
            return _repoWrapper.managementDash.GetSpecificperiodsurgerydetailsdetails(FromDate, ToDate, compoid);
        }

        //GetMonthREVENUEdetails


            


        [HttpGet("GetAnnualcomparisionRevenuedetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetAnnualcomparisionRevenuedetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetAnnualcomparisionRevenuedetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }


        [HttpGet("GetHalfyaerlycomparisionRevenuedetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetHalfyaerlycomparisionRevenuedetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetHalfyaerlycomparisionRevenuedetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }



        [HttpGet("Gethalfyearlycomparisionsurgerydetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel Gethalfyearlycomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.Gethalfyearlycomparisionsurgerydetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }
        [HttpGet("Getannualcomparisionsurgerydetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel Getannualcomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.Getannualcomparisionsurgerydetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }

        

        [HttpGet("GetQuarterlycomparisionRevenuedetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetQuarterlycomparisionRevenuedetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetQuarterlycomparisionRevenuedetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }

        [HttpGet("GetQuarterlycomparisionsurgerydetails/{selecetdmonth}/{selectedyear}/{selectedtomonth}/{selectedtoyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetQuarterlycomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetQuarterlycomparisionsurgerydetails(selecetdmonth, selectedyear, selectedtomonth, selectedtoyear, CompanyID);
        }

        



        [HttpGet("GetMonthsurgerydetails/{FromDate}/{ToDate}/{CompanyID}")]
        public ManagementDashboardViewModel GetMonthsurgerydetails(DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            return _repoWrapper.managementDash.GetMonthsurgerydetails(FromDate, ToDate, CompanyID);
        }

        [HttpGet("GetMonthREVENUEdetails/{FromDate}/{ToDate}/{CompanyID}")]
        public ManagementDashboardViewModel GetMonthREVENUEdetails(string FromDate, string ToDate, int CompanyID)
        {
            return _repoWrapper.managementDash.GetMonthREVENUEdetails(FromDate, ToDate, CompanyID);
        }

        [HttpGet("GetSRevenueMonthBriefdetails/{FromDate}/{ToDate}/{CompanyID}")]
        public ManagementDashboardViewModel GetSRevenueMonthBriefdetails(DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSRevenueMonthBriefdetails(FromDate, ToDate, CompanyID);
        }

        [HttpGet("GetSRevenueQuarterlyBriefdetails/{FromDate}/{fyear}/{ToDate}/{toyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetSRevenueQuarterlyBriefdetails(string FromDate,string fyear, string ToDate,string toyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSRevenueQuarterlyBriefdetails(FromDate, fyear, ToDate, toyear, CompanyID);
        }



        [HttpGet("GetSRevenueQuartelyBriefservicedetails/{desc}/{olmid}/{FromDate}/{fyear}/{ToDate}/{toyear}/{CompanyID}")]
        public ManagementDashboardViewModel GetSRevenueQuartelyBriefservicedetails(string desc,string olmid,string FromDate, string fyear, string ToDate, string toyear, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSRevenueQuartelyBriefservicedetails(desc, olmid,FromDate, fyear, ToDate, toyear, CompanyID);
        }
        



        [HttpGet("GetSRevenueMonthBriefservicedetails/{Desc}/{OLMID}/{FromDate}/{ToDate}/{CompanyID}")]
        public ManagementDashboardViewModel GetSRevenueMonthBriefservicedetails(string Desc,string OLMID , DateTime FromDate, DateTime ToDate, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSRevenueMonthBriefservicedetails(Desc, OLMID,FromDate, ToDate, CompanyID);
        }
        


        [HttpGet("GetRevenuedetails/{Yearstring}/{Monthstring}/{Daystring}/{CompanyID}")]
        public ManagementDashboardViewModel GetRevenuedetails(string Yearstring, string Monthstring, string Daystring, int CompanyID)
        {
            return _repoWrapper.managementDash.GetRevenuedetails(Yearstring, Monthstring, Daystring, CompanyID);
        }


        [HttpGet("GetSurgerydetails/{Yearstring}/{Monthstring}/{Daystring}/{CompanyID}")]
        public ManagementDashboardViewModel GetSurgerydetails(string Yearstring, string Monthstring, string Daystring, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSurgerydetails(Yearstring, Monthstring, Daystring, CompanyID);
        }


        [HttpGet("GetbranchSurgerydetails/{Yearstring}/{Monthstring}/{Daystring}/{CompanyID}/{branch}")]
        public ManagementDashboardViewModel GetbranchSurgerydetails(string Yearstring, string Monthstring, string Daystring, int CompanyID, string branch)
        {
            return _repoWrapper.managementDash.GetbranchSurgerydetails(Yearstring, Monthstring, Daystring, CompanyID, branch);
        }


        [HttpGet("GetSurgeryMonthFulldetails/{Monthstring}/{CompanyID}")]
        public ManagementDashboardViewModel GetSurgeryMonthFulldetails(string Monthstring, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSurgeryMonthFulldetails(Monthstring, CompanyID);
        }
        


        [HttpGet("GetSurgeryDayFulldetails/{Daystring}/{CompanyID}")]
        public ManagementDashboardViewModel GetSurgeryDayFulldetails(string Daystring, int CompanyID)
        {
            return _repoWrapper.managementDash.GetSurgeryDayFulldetails(Daystring, CompanyID);
        }

        [HttpGet("GetCountSurgeryDayFulldetails/{Daystring}/{Surgdescription}/{CompanyID}/{branch}")]
        public ManagementDashboardViewModel GetCountSurgeryDayFulldetails(string Daystring,string Surgdescription, int CompanyID, string branch)
        {
            return _repoWrapper.managementDash.GetCountSurgeryDayFulldetails(Daystring, Surgdescription, CompanyID, branch);
        }


        [HttpGet("GetCountSurgeryMonthFulldetails/{Monthstring}/{Surgdescription}/{CompanyID}/{branch}")]
        public ManagementDashboardViewModel GetCountSurgeryMonthFulldetails(string Monthstring, string Surgdescription, int CompanyID, string branch)
        {
            return _repoWrapper.managementDash.GetCountSurgeryMonthFulldetails(Monthstring, Surgdescription, CompanyID, branch);
        }

        [HttpGet("GetCountRevenueMonthFulldetails/{Monthstring}/{Surgdescription}/{CompanyID}/{format}")]
        public dynamic GetCountRevenueMonthFulldetails(string Monthstring, string Surgdescription, int CompanyID, string format)
        {
            return _repoWrapper.managementDash.GetCountRevenueMonthFulldetails(Monthstring, Surgdescription, CompanyID, format);
        }

    }
}









