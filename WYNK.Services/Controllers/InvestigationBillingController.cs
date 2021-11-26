
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InvestigationBillingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InvestigationBillingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetPatDetails/{uin}")]
        public InvestigationBilling GetPatDetails(string uin)
        {
            return _repoWrapper.InvestigationBilling.GetPatDetails(uin);
        }

        [HttpGet("GetBillingDetails/{ipid}/{TaxID}")]
        public InvestigationBilling GetBillingDetails(string ipid,int TaxID)
        {
            return _repoWrapper.InvestigationBilling.GetBillingDetails(ipid, TaxID);
        }

      
        [HttpPost("UpdateInvBilling/{cmpPid}/{TransactionTypeid}")]
        public dynamic UpdateGRN([FromBody] InvestigationBilling InvestigationBilling, int cmpPid, int TransactionTypeid)
        {
            String ib = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

            if (ib == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            InvestigationBilling.InvestigationBillMaster.BillNo = ib;
            return _repoWrapper.InvestigationBilling.UpdateInvBilling(InvestigationBilling, cmpPid, TransactionTypeid);
        }

        [HttpGet("GetInvesprint/{id}/{rid}/{cid}/{tid}")]
        public InvestigationBilling GetInvesprint(int id, int rid, int cid, int tid)
        {
            return _repoWrapper.InvestigationBilling.GetInvesprint(id, rid, cid, tid);
        }

        [HttpGet("Getprint/{billno}/{cid}/{tid}")]
        public InvestigationBilling Getprint(string billno, int cid, int tid)
        {
            return _repoWrapper.InvestigationBilling.Getprint(billno, cid, tid);
        }

        [HttpGet("GetReBillingDetails/{id}")]
        public InvestigationBilling GetReBillingDetails(int id)
        {
            return _repoWrapper.InvestigationBilling.GetReBillingDetails(id);
        }

        [HttpGet("Getreprint/{oid}/{rid}/{cid}/{tid}")]
        public InvestigationBilling Getreprint(int oid, int rid, int cid, int tid)
        {
            return _repoWrapper.InvestigationBilling.Getreprint(oid, rid, cid, tid);
        }

        [HttpGet("GetTaxDetails")]
        public InvestigationBilling GetTaxDetails()
        {
            return _repoWrapper.InvestigationBilling.GetTaxDetails();
        }

        [HttpGet("GetBillingTaxDetails/{ipid}/{TaxID}/{iptrid}")]
        public InvestigationBilling GetBillingTaxDetails(string ipid, int TaxID, int iptrid)
        {
            return _repoWrapper.InvestigationBilling.GetBillingTaxDetails(ipid, TaxID, iptrid);
        }
    }
}
