using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IUserLogRepository : IRepositoryBase<UserLogViewM>
    {
        UserLogViewM getAllUserLogDet(DateTime TodayDate, int ID);
        UserLogViewM getspecUserLogDet(DateTime FromDate, DateTime ToDate, int ID);
        UserLogViewM specCompnayDetls(DateTime FromDate, DateTime ToDate,int specCompanyID, int ID);
        UserLogViewM specUserDetls(DateTime FromDate, DateTime ToDate, int SpecUserID, int ID);
        UserLogViewM TdyCompanyNames(DateTime TDyDate, int ID);
        UserLogViewM specCompanyNames(DateTime FromDate, DateTime ToDate, int ID);
        UserLogViewM specUserNames(DateTime FromDate, DateTime ToDate, int ID);
        UserLogViewM TdyUserNames(DateTime TDyDate, int ID);
        UserLogViewM TdyCompnayDetls(DateTime TDyDate,int TdyCompanyID, int ID);
        UserLogViewM TdyUsernameDetls(DateTime TDyDate,int TdyUnernameID, int ID);
        UserLogViewM TdyLoggedinDetls(DateTime TDyDate, Boolean status, int ID);
        UserLogViewM TdyLoggedOutDetls(DateTime TDyDate, Boolean status, int ID);
        UserLogViewM specLoggeddetails(DateTime FromDate, DateTime ToDate, Boolean status, int ID);
    }
}
