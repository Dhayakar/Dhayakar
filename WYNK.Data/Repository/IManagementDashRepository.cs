using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IManagementDashRepository : IRepositoryBase<RegistrationMasterViewModel>
    {

        RegistrationMasterViewModel GetPatientpopulationdetails(string Yearstring, string Monthstring, string Daystring, int ComID);
        ManagementDashboardViewModel GetRevenuedetails(string Yearstring, string Monthstring, string Daystring, int ComID);

        ManagementDashboardViewModel GetSurgerydetails(string Yearstring, string Monthstring, string Daystring, int ComID);
        ManagementDashboardViewModel GetbranchSurgerydetails(string Yearstring, string Monthstring, string Daystring, int CompanyID, string branch);
        ManagementDashboardViewModel GetSurgeryMonthFulldetails( string Monthstring, int ComID);
        ManagementDashboardViewModel GetSurgeryDayFulldetails(string Daystring, int ComID);

        ManagementDashboardViewModel GetCountSurgeryDayFulldetails(string Daystring,string Surgdescription, int ComID, string branch);
        ManagementDashboardViewModel GetCountSurgeryMonthFulldetails(string Monthstring, string Surgdescription, int ComID, string branch);
        dynamic GetCountRevenueMonthFulldetails(string Monthstring, string Surgdescription, int CompanyID, string format);


        RegistrationMasterViewModel GetSpecificperiodPatientpopulationdetails(string FromDate, string ToDate, int CompID, string Phase);
        ManagementDashboardViewModel GetSpecificperiodRevenuedetails(string FromDate, string ToDate, int CompID);
        ManagementDashboardViewModel GetSpecificperiodsurgerydetailsdetails(DateTime FromDate, DateTime ToDate, int compoid);
        ManagementDashboardViewModel GetSpecificperiodRevenueBriefdetails(string FromDate, string ToDate, int CompanyID);

        ManagementDashboardViewModel GetSpecificperiodRevenueBriefBreakupdetails(string FromDate, string ToDate, int CompanyID, string Description, string OLMID);        



        ManagementDashboardViewModel GetMonthREVENUEdetails(string FromDate, string ToDate, int CompanyID);
        ManagementDashboardViewModel GetMonthsurgerydetails(DateTime FromDate, DateTime ToDate, int CompanyID);
        ManagementDashboardViewModel GetSRevenueMonthBriefdetails(DateTime FromDate, DateTime ToDate, int CompanyID);
        ManagementDashboardViewModel GetSRevenueQuarterlyBriefdetails(string FromDate, string fyear, string ToDate, string toyear, int CompanyID);


        ManagementDashboardViewModel GetSRevenueQuartelyBriefservicedetails(string desc, string olmid, string FromDate, string fyear, string ToDate, string toyear, int CompanyID);
        ManagementDashboardViewModel GetSRevenueMonthBriefservicedetails(string Desc, string OLMID, DateTime FromDate, DateTime ToDate, int CompanyID);

        ManagementDashboardViewModel GetQuarterlycomparisionRevenuedetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel GetHalfyaerlycomparisionRevenuedetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel Gethalfyearlycomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel GetAnnualcomparisionRevenuedetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel GetQuarterlycomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel Getannualcomparisionsurgerydetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        RegistrationMasterViewModel GetMonthPatientpopulationdetails(string FromDate, string ToDate, int CompanyID, string phase);
        RegistrationMasterViewModel GetUsernamepasswordWITHPARENTIDZERO(string username, string password, string CompanyID, string Roldedescrip);
        ManagementDashboardViewModel GetQuarterlycomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel GetHalfyearlycomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel GetAnnualcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID);
        ManagementDashboardViewModel GetQuarterlyPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID, string id);
        ManagementDashboardViewModel GetHalfPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID, string id);
        ManagementDashboardViewModel GetAnnualPiechartcomparisionPatientpopulationdetails(string selecetdmonth, string selectedyear, string selectedtomonth, string selectedtoyear, int CompanyID, string id);

    }
}
