
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class FinalBillingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public FinalBillingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("Getcmpdtls/{cmpid}")]
        public FinalBillingMaster Getcmpdtls(int cmpid)
        {
            return _repoWrapper.FinalBilling.Getcmpdtls(cmpid);
        }
        [HttpGet("Getunbilleddtls/{cmpid}/{tranid}")]
        public FinalBillingMaster Getunbilleddtls(int cmpid, int tranid)
        {
            return _repoWrapper.FinalBilling.Getunbilleddtls(cmpid, tranid);
        }
        [HttpGet("Getbillingdtls/{paid}/{cmpid}")]
        public FinalBillingMaster Getbillingdtls(int paid, int cmpid)
        {
            return _repoWrapper.FinalBilling.Getbillingdtls(paid,cmpid);
        }

        [HttpGet("Getrepbillingdtls/{paid}/{cmpid}/{gmt}")]
        public FinalBillingMaster Getrepbillingdtls(int paid, int cmpid, string gmt)
        {
            return _repoWrapper.FinalBilling.Getrepbillingdtls(paid, cmpid, gmt);
        }

        [HttpGet("Getbreakupdtls/{padid}/{cmpid}")]
        public FinalBillingMaster Getbreakupdtls(int padid, int cmpid)
        {
            return _repoWrapper.FinalBilling.Getbreakupdtls(padid, cmpid);
        }
        [HttpGet("Getprint/{paid}/{bill}/{cid}/{tid}/{gmt}")]
        public FinalBillingMaster Getprint(int paid, string bill, int cid, int tid, string gmt)
        {
            return _repoWrapper.FinalBilling.Getprint(paid, bill, cid, tid, gmt);
        }

        [HttpGet("GetReBillingDetails/{id}/{gmt}")]
        public FinalBillingMaster GetReBillingDetails(int id, string gmt)
        {
            return _repoWrapper.FinalBilling.GetReBillingDetails(id, gmt);
        }

        [HttpPost("InsertBilling/{cmpid}/{UIN}/{TransactionTypeid}/{paid}")]
        public dynamic InsertBilling([FromBody] FinalBillingMaster payment, int cmpid, string UIN, int TransactionTypeid, int paid)
        {

            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(TransactionTypeid, cmpid);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Mapped in Transaction Table"
                };
            }
            payment.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "GetRunningNo");

            if (payment.ReceiptRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Exist"
                };
            }


            string generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");
            if (generatenumber == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            payment.payment.InVoiceNumber = generatenumber;

            return _repoWrapper.FinalBilling.InsertBilling(payment, cmpid, UIN, TransactionTypeid, paid);
        }

        [HttpGet("Getunbilleddtlspackage/{cmpid}/{tranid}")]
        public FinalBillingMaster Getunbilleddtlspackage(int cmpid, string tranid)
        {
            return _repoWrapper.FinalBilling.Getunbilleddtlspackage(cmpid, tranid);
        }

        [HttpGet("Getbilleddtlspackage/{cmpid}/{tranid}")]
        public dynamic Getbilleddtlspackage(int cmpid, int tranid)
        {
            return _repoWrapper.FinalBilling.Getbilleddtlspackage(cmpid, tranid);
        }
        


        [HttpGet("Getbillingdtlspackage/{paid}/{cmpid}/{billtype}")]
        public FinalBillingMaster Getbillingdtlspackage(int paid, int cmpid, string billtype)
        {
            return _repoWrapper.FinalBilling.Getbillingdtlspackage(paid, cmpid, billtype);
        }

        [HttpGet("Getbreakupdtlspackage/{padid}/{cmpid}")]
        public FinalBillingMaster Getbreakupdtlspackage(int padid, int cmpid)
        {
            return _repoWrapper.FinalBilling.Getbreakupdtlspackage(padid, cmpid);
        }

        [HttpPost("InsertBillingpackage/{cmpid}/{UIN}/{TransactionTypeid}/{paid}/{netamount}/{discamount}")]
        public dynamic InsertBillingpackage([FromBody] FinalBillingMaster payment, int cmpid, string UIN, int TransactionTypeid, int paid, decimal netamount, decimal discamount)
        {

            if(payment.PaymentMaster.Count != 0)
            {

                int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(TransactionTypeid, cmpid);

                if (RecContraID == null)
                {
                    return new
                    {
                        Success = false,
                        Message = "Running Number Does'nt Mapped in Transaction Table"
                    };
                }
                payment.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), cmpid, "GetRunningNo");

                if (payment.ReceiptRunningNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Exist"
                    };
                }


                string generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");
                if (generatenumber == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Running Number Does'nt Exist"
                    };
                }
                payment.InVoiceNumber = generatenumber;
            }
            return _repoWrapper.FinalBilling.InsertBillingpackage(payment, cmpid, UIN, TransactionTypeid, paid, netamount, discamount);
        }

        [HttpGet("Getprintpackage/{paid}/{bill}/{cid}/{tid}/{gmt}")]
        public FinalBillingMaster Getprintpackage(int paid, string bill, int cid, int tid, string gmt)
        {
            return _repoWrapper.FinalBilling.Getprintpackage(paid, bill, cid, tid, gmt);
        }

        [HttpPost("UpdateBilling/{cmpid}/{padtid}")]
        public dynamic UpdateBilling([FromBody] FinalBillingMaster payment, int cmpid, int padtid)
        {            
            return _repoWrapper.FinalBilling.UpdateBilling(payment, cmpid, padtid);
        }

        [HttpPost("UpdatePackageBilling/{cmpid}/{padtid}")]
        public dynamic UpdatePackageBilling([FromBody] FinalBillingMaster payment, int cmpid, int padtid)
        {
            return _repoWrapper.FinalBilling.UpdatePackageBilling(payment, cmpid, padtid);
        }

        [HttpGet("Getbreakupbilling/{cmpid}/{padtid}")]
        public dynamic Getbreakupbilling(int cmpid, int padtid)
        {
            return _repoWrapper.FinalBilling.Getbreakupbilling(cmpid, padtid);
        }

        [HttpGet("GetPrintbillingdtlspackage/{paid}/{cmpid}/{billtype}")]
        public dynamic GetPrintbillingdtlspackage(int paid, int cmpid, string billtype)
        {
            return _repoWrapper.FinalBilling.GetPrintbillingdtlspackage(paid, cmpid, billtype);
        }

    }

}



