
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InterDepartmentTransferController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InterDepartmentTransferController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetStoreDetails/{ID}")]
        public dynamic GetStoreDetails(int ID)
        {
            return _repoWrapper.InterDepartmentTransfer.GetStoreDetails(ID);
        }

        [HttpGet("GetdrugDetails/{ID}")]
        public dynamic GetdrugDetails(int ID)
        {
            return _repoWrapper.InterDepartmentTransfer.GetdrugDetails(ID);
        }

        [HttpGet("GetDrugvalues/{storeID}/{CmpId}")]
        public IEnumerable<Dropdown> GetDrugvalues(int storeID,int Cmpid)
        {
            return _repoWrapper.InterDepartmentTransfer.GetDrugvalues(storeID,Cmpid);
        }



        [HttpPost("AddstockDetails/{GMT}")]
        public dynamic AddstockDetails([FromBody] SubmitTransferdetails AddTransfer, string GMT)
        {
            AddTransfer.RunningNoStock = _repoWrapper.Common.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "GetRunningNo");
            if(AddTransfer.RunningNoStock == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
            };
            }
            return _repoWrapper.InterDepartmentTransfer.AddstockDetails(AddTransfer, GMT);
        }


        [HttpGet("InterDepartmentTransferDetails/{transactionCode}/{GMT}")]
        public dynamic InterDepartmentTransferDetails(int transactionCode, string GMT)
        {
            return _repoWrapper.InterDepartmentTransfer.InterDepartmentTransferDetails(transactionCode, GMT);
        }


        [HttpGet("StockTransferDetails/{StockTransferNo}/{CmpId}/{GMT}")]
        public dynamic StockTransferDetails(string StockTransferNo, int CmpId, string GMT)
        {
            return _repoWrapper.InterDepartmentTransfer.StockTransferDetails(StockTransferNo, CmpId, GMT);
        }

        [HttpPost("GetMixedCompanyDetails/{CmpId}")]
        public dynamic GetMixedCompanyDetails([FromBody] GetCompanyIds GetCompanyIds, int CmpId)
        {
            return _repoWrapper.InterDepartmentTransfer.GetMixedCompanyDetails(GetCompanyIds, CmpId);
        }

        [HttpPost("AddInterBranchTransfer")]
        public dynamic AddInterBranchTransfer([FromBody] SubmitInterBanchdetails AddTransfer)
        {
            AddTransfer.RunningNoStock = _repoWrapper.Common.GenerateRunningCtrlNoo(AddTransfer.TransactionId, AddTransfer.Cmpid, "GetRunningNo");
            if (AddTransfer.RunningNoStock == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            return _repoWrapper.InterDepartmentTransfer.AddInterBranchTransfer(AddTransfer);
        }

        [HttpGet("InterBranchIsuueDetails/{transactionCode}/{CmpId}/{GMT}")]
        public dynamic InterBranchIsuueDetails(int transactionCode, int Cmpid, string GMT)
        {
            return _repoWrapper.InterDepartmentTransfer.InterBranchIsuueDetails(transactionCode, Cmpid, GMT);
        }

        [HttpGet("InterBranchIssueDetailed/{StockTransferNo}/{CmpId}")]
        public dynamic InterBranchIssueDetailed(string StockTransferNo, int CmpId)
        {
            return _repoWrapper.InterDepartmentTransfer.InterBranchIssueDetailed(StockTransferNo, CmpId);
        }

        [HttpPost("GetItemListDetails/{FromDate}/{ToDate}/{CmpId}/{GMT}")]
        public dynamic GetItemListDetails([FromBody] GetItemListDetails GetItemListDetail, string FromDate, string Todate, int CmpId, string GMT)
        {
            return _repoWrapper.InterDepartmentTransfer.GetItemListDetails(GetItemListDetail, FromDate, Todate, CmpId, GMT);
        }

        [HttpPost("IssueStatusUpdate/{Cmpid}")]
        public dynamic IssueStatusUpdate([FromBody] IssueStatusDetails IssueStatusDetail, int Cmpid)
        {
            return _repoWrapper.InterDepartmentTransfer.IssueStatusUpdate(IssueStatusDetail, Cmpid);
        }

    }
}
