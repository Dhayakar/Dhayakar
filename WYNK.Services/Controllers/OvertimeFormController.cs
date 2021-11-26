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
    public class OvertimeFormController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public OvertimeFormController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getEmployeeName")]
        public OvertimeViewM getEmployeeName()
        {
            return _repoWrapper.OvertimeViewM.getEmployeeName();
        }

        [HttpGet("getAddHistoryDet/{FromDate}/{Todate}/{EmpId}")]
        public OvertimeViewM getAddHistoryDet(DateTime FromDate, DateTime Todate, int EmpId)
        {
            return _repoWrapper.OvertimeViewM.getAddHistoryDet(FromDate, Todate, EmpId);
        }

        [HttpGet("getHistoryDet/{FromDate}/{Todate}/{EmpId}")]
        public OvertimeViewM getHistoryDet(DateTime FromDate, DateTime Todate, int EmpId)
        {
            return _repoWrapper.OvertimeViewM.getHistoryDet(FromDate, Todate, EmpId);
        }

        [HttpPost("insertOTDet")]
        public dynamic insertOTDet([FromBody]OvertimeViewM OTview)
        {
            return _repoWrapper.OvertimeViewM.insertOTDet(OTview);
        }


        [HttpPost("UpdateOvertimeDet/{ID}/{O_Id}")]
        public dynamic UpdateOvertimeDet([FromBody] OvertimeViewM overtimeMaster, int ID, int O_Id)
        {
            return _repoWrapper.OvertimeViewM.UpdateOvertimeDet(overtimeMaster, ID , O_Id);
        }


        [HttpPost("DeleteOTDet/{ID}")]
        public dynamic DeleteOTDet(int ID)
        {
            return _repoWrapper.OvertimeViewM.DeleteOTDet(ID);
        }


    }
}