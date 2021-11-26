using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class operationTheatreBooking_Controller : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public operationTheatreBooking_Controller(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("GetDetails/{uin}")]
        public dynamic GetDetails(string uin)
        {
            return _repoWrapper.OperationTheatreBooking.GetDetails(uin);
        }
        [HttpGet("GetDetailsuin/{companyid}")]
        public dynamic GetDetailsuin(int companyid)
        {
            return _repoWrapper.OperationTheatreBooking.GetDetailsuin(companyid);
        }
        [HttpPost("BookingDetails/{uin}/{companyid}/{operationid}/{userid}")]
        public dynamic BookingDetails([FromBody] OperationTheatreBooking_Viewmodel opb,string uin,int companyid,int operationid,int userid)
        {
            return _repoWrapper.OperationTheatreBooking.BookingDetails(opb, uin, companyid, operationid, userid);
        }
        [HttpGet("GetOperationname/{opid}")]
        public dynamic GetOperationname(int opid)
        {
            return _repoWrapper.OperationTheatreBooking.GetOperationname(opid);
        }
        [HttpGet("GetDetails")]
        public dynamic GetDetails()
        {
            return _repoWrapper.OperationTheatreBooking.GetDetails();
        }
    }
}
