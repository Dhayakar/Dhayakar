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
    public class UserLogController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public UserLogController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getAllUserLogDet/{TodayDate}/{ID}")]
        public UserLogViewM getAllUserLogDet(DateTime TodayDate, int ID)
        {
            return _repoWrapper.UserLogViewM.getAllUserLogDet(TodayDate,ID);
        }

        [HttpGet("getspecUserLogDet/{FromDate}/{ToDate}/{ID}")]
        public UserLogViewM getspecUserLogDet(DateTime FromDate, DateTime ToDate, int ID)
        {
            return _repoWrapper.UserLogViewM.getspecUserLogDet(FromDate, ToDate, ID);
        }

        [HttpGet("TdyCompanyNames/{TDyDate}/{ID}")]
        public UserLogViewM TdyCompanyNames(DateTime TDyDate, int ID)
        {
            return _repoWrapper.UserLogViewM.TdyCompanyNames( TDyDate, ID);
        }

        [HttpGet("specCompanyNames/{FromDate}/{ToDate}/{ID}")]
        public UserLogViewM specCompanyNames(DateTime FromDate, DateTime ToDate, int ID)
        {
            return _repoWrapper.UserLogViewM.specCompanyNames(FromDate, ToDate, ID);
        }
          [HttpGet("specUserNames/{FromDate}/{ToDate}/{ID}")]
        public UserLogViewM specUserNames(DateTime FromDate, DateTime ToDate, int ID)
        {
            return _repoWrapper.UserLogViewM.specUserNames(FromDate, ToDate, ID);
        }




        [HttpGet("specCompnayDetls/{FromDate}/{ToDate}/{specCompanyID}/{ID}")]
        public UserLogViewM specCompnayDetls(DateTime FromDate, DateTime ToDate, int specCompanyID, int ID)
        {
            return _repoWrapper.UserLogViewM.specCompnayDetls(FromDate, ToDate, specCompanyID, ID);
        }
       
        [HttpGet("specUserDetls/{FromDate}/{ToDate}/{SpecUserID}/{ID}")]
        public UserLogViewM specUserDetls(DateTime FromDate, DateTime ToDate, int SpecUserID, int ID)
        {
            return _repoWrapper.UserLogViewM.specUserDetls(FromDate, ToDate, SpecUserID, ID);
        }
       

        [HttpGet("TdyUserNames/{TDyDate}/{ID}")]
        public UserLogViewM TdyUserNames( DateTime TDyDate, int ID)
        {
            return _repoWrapper.UserLogViewM.TdyUserNames( TDyDate, ID);
        }

        [HttpGet("TdyCompnayDetls/{TDyDate}/{TdyCompanyID}/{ID}")]
        public UserLogViewM TdyCompnayDetls( DateTime TDyDate,int TdyCompanyID, int ID)
        {
            return _repoWrapper.UserLogViewM.TdyCompnayDetls( TDyDate, TdyCompanyID, ID);
        }

         [HttpGet("TdyUsernameDetls/{TDyDate}/{TdyUnernameID}/{ID}")]
        public UserLogViewM TdyUsernameDetls( DateTime TDyDate,int TdyUnernameID, int ID)
        {
            return _repoWrapper.UserLogViewM.TdyUsernameDetls( TDyDate, TdyUnernameID, ID);
        }

        [HttpGet("TdyLoggedinDetls/{TDyDate}/{status}/{ID}")]
        public UserLogViewM TdyLoggedinDetls( DateTime TDyDate,Boolean status, int ID)
        {
            return _repoWrapper.UserLogViewM.TdyLoggedinDetls( TDyDate, status, ID);
        }
          [HttpGet("TdyLoggedOutDetls/{TDyDate}/{status}/{ID}")]
        public UserLogViewM TdyLoggedOutDetls( DateTime TDyDate,Boolean status, int ID)
        {
            return _repoWrapper.UserLogViewM.TdyLoggedOutDetls( TDyDate, status, ID);
        }

        [HttpGet("specLoggeddetails/{FromDate}/{ToDate}/{status}/{ID}")]
        public UserLogViewM specLoggeddetails( DateTime FromDate,DateTime ToDate, Boolean status, int ID)
        {
            return _repoWrapper.UserLogViewM.specLoggeddetails(FromDate, ToDate, status, ID);
        }


    }
}