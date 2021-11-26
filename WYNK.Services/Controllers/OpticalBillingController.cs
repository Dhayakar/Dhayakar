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
    public class OpticalBillingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalBillingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpGet("Getorders/{val}/{Time}")]
        public dynamic Getorders(int val, string Time)

        {
            return _repoWrapper.OpticalBilling.Getorders(val, Time);
        }

        [HttpGet("GetGrntrnsdetails/{RandomUniqueID}/{SID}/{CmpID}")]
        public dynamic GetGrntrnsdetails(string RandomUniqueID, int SID, int CmpID)
        {
            return _repoWrapper.OpticalBilling.GetGrntrnsdetails(RandomUniqueID, SID, CmpID);
        }

        [HttpGet("GetGrntrnsdetailsstore/{SID}/{CmpID}")]
        public dynamic GetGrntrnsdetailsstore(int SID, int CmpID)
        {
            return _repoWrapper.OpticalBilling.GetGrntrnsdetailsstore(SID, CmpID);
        }


        [HttpGet("stockavailable/{ID}/{SID}/{CmpID}")]
        public dynamic stockavailable(int ID, int SID, int CmpID)
        {
            return _repoWrapper.OpticalBilling.stockavailable(ID, SID, CmpID);
        }

        [HttpPost("Beforesubmitcheckstock/{SID}/{CmpID}")]
        public dynamic Beforesubmitcheckstock([FromBody] OpticalBilling GetOpticaldetailsfullcheck, int SID, int CmpID)
        {
            return _repoWrapper.OpticalBilling.Beforesubmitcheckstock(GetOpticaldetailsfullcheck, SID, CmpID);
        }

        [HttpPost("InsertOptBilling/{CmpID}/{TID}/{SID}")]
        public dynamic InsertOptBilling([FromBody] OpticalBilling OpticalBilling, int CmpID, int TID, int SID)
        {

            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(TID, CmpID);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Mapped in Transaction Table"
                };
            }
            OpticalBilling.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CmpID, "GetRunningNo");

            if (OpticalBilling.ReceiptRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Exist"
                };
            }

            String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(TID, CmpID, "GetRunningNo");

            if (gnr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            OpticalBilling.OpticalInvoiceMaster.InvoiceNumber = gnr;
            return _repoWrapper.OpticalBilling.InsertOptBilling(OpticalBilling, CmpID, TID, SID);
        }

        [HttpGet("Opticalbillingprint/{OpticalID}/{CMPID}/{TID}/{Time}")]
        public dynamic Opticalbillingprint(string OpticalID, int CMPID, int TID, string Time)
        {
            return _repoWrapper.OpticalBilling.Opticalbillingprint(OpticalID, CMPID, TID, Time);
        }

    }
}
