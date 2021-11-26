using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class UserLogRepository :RepositoryBase<UserLogViewM>, IUserLogRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public UserLogRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public UserLogViewM getAllUserLogDet(DateTime TodayDate, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TodayDate).ToString("yyyy-MM-dd");
            getAllData.getUserLogDetails = (from LogUser in M_logUser.Where(x=> Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) || Convert.ToDateTime(x.LogoutDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID)
                                            join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                            join User in M_User 
                                            on LogUser.UserID equals User.Userid
                                           select new getUserLogDetails
                                           {
                                             UserLogID = LogUser.LoguserID,
                                             CompanyName = Company.CompanyName,
                                             UserName = LogUser.LoginUsername,
                                             LoggedInDateTime = LogUser.LoginDatetime,
                                             LoggedOutDateTime = LogUser.LogoutDatetime,
                                             Status = LogUser.IsLogged

                                           }).ToList();

            return getAllData;
        }
         public UserLogViewM getspecUserLogDet(DateTime FromDate, DateTime ToDate, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
            getAllData.getspecUserLogDetails = (from LogUser in M_logUser.Where(x=> (Convert.ToDateTime(x.LoginDatetime).Date >= Convert.ToDateTime(Fdate)  &&  Convert.ToDateTime(x.LoginDatetime).Date <= Convert.ToDateTime(Tdate)) && x.CmpID == ID)
                                            join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                            join User in M_User 
                                            on LogUser.UserID equals User.Userid
                                           select new getspecUserLogDetails
                                           {
                                             UserLogID = LogUser.LoguserID,
                                             CompanyName = Company.CompanyName,
                                             UserName = LogUser.LoginUsername,
                                             LoggedInDateTime = LogUser.LoginDatetime,
                                             LoggedOutDateTime = LogUser.LogoutDatetime,
                                             Status = LogUser.IsLogged

                                           }).ToList();

            return getAllData;
        }
         public UserLogViewM specCompanyNames(DateTime FromDate, DateTime ToDate, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
           var speccompany = (from LogUser in M_logUser.Where(x=> (Convert.ToDateTime(x.LoginDatetime).Date >= Convert.ToDateTime(Fdate)  &&  Convert.ToDateTime(x.LoginDatetime).Date <= Convert.ToDateTime(Tdate)) && x.CmpID == ID)
                                            join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                            join User in M_User 
                                            on LogUser.UserID equals User.Userid
                                           select new getspecCompanyNames
                                           {
                                             S_UserLogID = LogUser.CmpID,
                                             S_CompanyName = Company.CompanyName,

                                           }).ToList();


            getAllData.getspecCompanyNames = (from result in speccompany.GroupBy(x => x.S_CompanyName)
                                             select new getspecCompanyNames
                                             {

                                                 S_CompanyName = result.Key,
                                                 S_UserLogID = result.Select(x => x.S_UserLogID).FirstOrDefault()

                                             }).ToList();


            return getAllData;
        }
          public UserLogViewM specUserNames(DateTime FromDate, DateTime ToDate, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
           var specUSer = (from LogUser in M_logUser.Where(x=> (Convert.ToDateTime(x.LoginDatetime).Date >= Convert.ToDateTime(Fdate)  &&  Convert.ToDateTime(x.LoginDatetime).Date <= Convert.ToDateTime(Tdate)) && x.CmpID == ID)
                                            join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                            join User in M_User 
                                            on LogUser.UserID equals User.Userid
                                           select new getspecUserNames
                                           {
                                             S_UserLogID = LogUser.UserID,
                                             S_UserName = LogUser.LoginUsername,

                                           }).ToList();


            getAllData.getspecUserNames = (from result in specUSer.GroupBy(x => x.S_UserName)
                                             select new getspecUserNames
                                             {

                                                 S_UserName = result.Key,
                                                 S_UserLogID = result.Select(x => x.S_UserLogID).FirstOrDefault()

                                             }).ToList();


            return getAllData;
        }

        public UserLogViewM TdyCompanyNames(DateTime TDyDate, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TDyDate).ToString("yyyy-MM-dd");
            var getgroupedcompanies = (from LogUser in M_logUser.Where(x => Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) || Convert.ToDateTime(x.LogoutDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID)
                                            join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                            join User in M_User
                                            on LogUser.UserID equals User.Userid
                                            select new getTdyCompanyNames
                                            {
                                                T_UserLogID = LogUser.CmpID,
                                                T_CompanyName = Company.CompanyName
                                            }).ToList();

            getAllData.getTdyCompanyNames = (from result in getgroupedcompanies.GroupBy(x => x.T_CompanyName)
                                             select new getTdyCompanyNames
                                             {

                                                 T_CompanyName = result.Key,
                                                 T_UserLogID = result.Select(x=> x.T_UserLogID).FirstOrDefault()

                                             }).ToList();






            return getAllData;
        }

             public UserLogViewM TdyUserNames(DateTime TDyDate, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TDyDate).ToString("yyyy-MM-dd");
            var getgroupedUsers = (from LogUser in M_logUser.Where(x => Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) || Convert.ToDateTime(x.LogoutDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID)
                                            join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                            join User in M_User
                                            on LogUser.UserID equals User.Userid
                                            select new getTdyUserNames
                                            {
                                                T_UserLogID = LogUser.UserID,
                                                T_UserNames = LogUser.LoginUsername
                                            }).ToList();

            getAllData.getTdyUserNames = (from result in getgroupedUsers.GroupBy(x => x.T_UserNames)
                                             select new getTdyUserNames
                                             {

                                                 T_UserNames = result.Key,
                                                 T_UserLogID = result.Select(x=> x.T_UserLogID).FirstOrDefault()

                                             }).ToList();


            return getAllData;
        }


        public UserLogViewM TdyCompnayDetls(DateTime TDyDate,int TdyCompanyID, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TDyDate).ToString("yyyy-MM-dd");
           
            getAllData.TdyCompDet = (from LogUser in M_logUser.Where(x => Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID && x.CmpID == TdyCompanyID)
                                                join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                                join User in M_User
                                                on LogUser.UserID equals User.Userid
                                                select new TdyCompDet
                                                {
                                                    UserLogID = LogUser.LoguserID,
                                                    CompanyName = Company.CompanyName,
                                                    UserName = LogUser.LoginUsername,
                                                    LoggedInDateTime = LogUser.LoginDatetime,
                                                    LoggedOutDateTime = LogUser.LogoutDatetime,
                                                    Status = LogUser.IsLogged

                                                }).ToList();

            return getAllData;
        }

        public UserLogViewM TdyUsernameDetls(DateTime TDyDate, int TdyUnernameID, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TDyDate).ToString("yyyy-MM-dd");

            getAllData.TdyUsernameDet = (from LogUser in M_logUser.Where(x => Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID && x.UserID == TdyUnernameID)
                                     join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                     join User in M_User
                                     on LogUser.UserID equals User.Userid
                                     select new TdyUsernameDet
                                     {
                                         UserLogID = LogUser.LoguserID,
                                         CompanyName = Company.CompanyName,
                                         UserName = LogUser.LoginUsername,
                                         LoggedInDateTime = LogUser.LoginDatetime,
                                         LoggedOutDateTime = LogUser.LogoutDatetime,
                                         Status = LogUser.IsLogged

                                     }).ToList();

            return getAllData;
        }

        public UserLogViewM TdyLoggedinDetls(DateTime TDyDate, Boolean status, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TDyDate).ToString("yyyy-MM-dd");

            getAllData.TdyLoggedinDet = (from LogUser in M_logUser.Where(x => Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID && x.IsLogged == status)
                                         join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                         join User in M_User
                                         on LogUser.UserID equals User.Userid
                                         select new TdyLoggedinDet
                                         {
                                             UserLogID = LogUser.LoguserID,
                                             CompanyName = Company.CompanyName,
                                             UserName = LogUser.LoginUsername,
                                             LoggedInDateTime = LogUser.LoginDatetime,
                                             LoggedOutDateTime = LogUser.LogoutDatetime,
                                             Status = LogUser.IsLogged

                                         }).ToList();

            return getAllData;
        }

        public UserLogViewM TdyLoggedOutDetls(DateTime TDyDate, Boolean status, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Tdate = Convert.ToDateTime(TDyDate).ToString("yyyy-MM-dd");

            getAllData.TdyLoggedOutDet = (from LogUser in M_logUser.Where(x => Convert.ToDateTime(x.LoginDatetime).Date == Convert.ToDateTime(Tdate) && x.CmpID == ID && x.IsLogged == status)
                                         join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                         join User in M_User
                                         on LogUser.UserID equals User.Userid
                                         select new TdyLoggedOutDet
                                         {
                                             UserLogID = LogUser.LoguserID,
                                             CompanyName = Company.CompanyName,
                                             UserName = LogUser.LoginUsername,
                                             LoggedInDateTime = LogUser.LoginDatetime,
                                             LoggedOutDateTime = LogUser.LogoutDatetime,
                                             Status = LogUser.IsLogged

                                         }).ToList();

            return getAllData;
        }

        public UserLogViewM specCompnayDetls(DateTime FromDate, DateTime ToDate,int specCompanyID, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
            getAllData.specCompanyDetails = (from LogUser in M_logUser.Where(x => (Convert.ToDateTime(x.LoginDatetime).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.LoginDatetime).Date <= Convert.ToDateTime(Tdate)) && x.CmpID == ID && x.CmpID == specCompanyID)
                                                join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                                join User in M_User
                                                on LogUser.UserID equals User.Userid
                                                select new specCompanyDetails
                                                {
                                                    UserLogID = LogUser.LoguserID,
                                                    CompanyName = Company.CompanyName,
                                                    UserName = LogUser.LoginUsername,
                                                    LoggedInDateTime = LogUser.LoginDatetime,
                                                    LoggedOutDateTime = LogUser.LogoutDatetime,
                                                    Status = LogUser.IsLogged

                                                }).ToList();

            return getAllData;
        }

        public UserLogViewM specUserDetls(DateTime FromDate, DateTime ToDate,int SpecUserID, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
            getAllData.specUserDetails = (from LogUser in M_logUser.Where(x => (Convert.ToDateTime(x.LoginDatetime).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.LoginDatetime).Date <= Convert.ToDateTime(Tdate)) && x.CmpID == ID && x.UserID == SpecUserID)
                                                join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                                join User in M_User
                                                on LogUser.UserID equals User.Userid
                                                select new specUserDetails
                                                {
                                                    UserLogID = LogUser.LoguserID,
                                                    CompanyName = Company.CompanyName,
                                                    UserName = LogUser.LoginUsername,
                                                    LoggedInDateTime = LogUser.LoginDatetime,
                                                    LoggedOutDateTime = LogUser.LogoutDatetime,
                                                    Status = LogUser.IsLogged

                                                }).ToList();

            return getAllData;
        }
         public UserLogViewM specLoggeddetails(DateTime FromDate, DateTime ToDate,Boolean status, int ID)
        {
            var getAllData = new UserLogViewM();

            var M_logUser = CMPSContext.LogUser.ToList();
            var M_Company = CMPSContext.Company.ToList();
            var M_User = CMPSContext.Users.ToList();
            var Fdate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            var Tdate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
            getAllData.specLogDetails = (from LogUser in M_logUser.Where(x => (Convert.ToDateTime(x.LoginDatetime).Date >= Convert.ToDateTime(Fdate) && Convert.ToDateTime(x.LoginDatetime).Date <= Convert.ToDateTime(Tdate)) && x.CmpID == ID && x.IsLogged == status)
                                                join Company in M_Company on LogUser.CmpID equals Company.CmpID
                                                join User in M_User
                                                on LogUser.UserID equals User.Userid
                                                select new specLogDetails
                                                {
                                                    UserLogID = LogUser.LoguserID,
                                                    CompanyName = Company.CompanyName,
                                                    UserName = LogUser.LoginUsername,
                                                    LoggedInDateTime = LogUser.LoginDatetime,
                                                    LoggedOutDateTime = LogUser.LogoutDatetime,
                                                    Status = LogUser.IsLogged

                                                }).ToList();

            return getAllData;
        }





    }
}
