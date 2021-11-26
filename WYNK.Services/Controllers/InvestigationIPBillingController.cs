
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InvestigationIPBillingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InvestigationIPBillingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetRegINVIPBILLDetails/{Cmpid}")]
        public InvestigationIPBilling GetRegINVIPBILLDetails(int Cmpid)
        {
            return _repoWrapper.InvestigationIPBilling.GetRegINVIPBILLDetails(Cmpid);
        }


        [HttpGet("GetRegINVBILLDetails/{Cmpid}")]
        public InvestigationIPBilling GetRegINVBILLDetails(int Cmpid)
        {
            return _repoWrapper.InvestigationIPBilling.GetRegINVBILLDetails(Cmpid);
        }

        [HttpGet("GetInvIPDetails/{uin}/{cmpid}")]
        public InvestigationIPBilling GetInvIPDetails(string uin,int cmpid)
        {
            return _repoWrapper.InvestigationIPBilling.GetInvIPDetails(uin,cmpid);
        }

        [HttpGet("GetIPTaxBillingDetails/{ipid}/{TaxID}/{inpID}")]
        public InvestigationIPBilling GetIPTaxBillingDetails(string ipid, int TaxID,int inpID)
        {
            return _repoWrapper.InvestigationIPBilling.GetIPTaxBillingDetails(ipid, TaxID, inpID);
        }
        [HttpGet("GetIPBillingDetails/{ipid}/{TaxID}")]
        public InvestigationIPBilling GetIPBillingDetails(string ipid, int TaxID)
        {
            return _repoWrapper.InvestigationIPBilling.GetIPBillingDetails(ipid, TaxID);
        }

        [HttpPost("insertInvIPBilling/{cmpPid}/{userid}/{TransactionTypeid}/{UIN}")]
        public dynamic insertInvIPBilling([FromBody] InvestigationIPBilling InvIPBilling, int cmpPid,int userid , int TransactionTypeid,string UIN)
        {

            String billid = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");
            if (TransactionTypeid == 0)
            {
                var Errordata = _repoWrapper.Common.ErrorList("TransactionTypeid=null", "insertInvIPBilling", cmpPid, userid);

                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"
                };
            }
            if (billid == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            InvIPBilling.InvestigationBillMaster.BillNo = billid;

            return _repoWrapper.InvestigationIPBilling.insertInvIPBilling(InvIPBilling, cmpPid, TransactionTypeid, UIN);
       
        
        }

        [HttpGet("GetTaxDetails")]
        public InvestigationIPBilling GetTaxDetails()
        {
            return _repoWrapper.InvestigationIPBilling.GetTaxDetails();
        }

    }
}
