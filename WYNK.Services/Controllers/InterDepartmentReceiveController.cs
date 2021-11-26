
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System.Collections.Generic;
using WYNK.Data.Common;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InterDepartmentReceiveController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InterDepartmentReceiveController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetStoreDetails/{ID}/{IssueCode}/{cmpid}")]
        public dynamic GetStoreDetails(int ID,int IssueCode, int cmpid)
        {
            return _repoWrapper.InterDeparmentReceiver.GetStoreDetails(ID, IssueCode, cmpid);
        }



        [HttpGet("GetStockTransferDetails/{StockTransferNo}/{cmpid}")]
        public dynamic GetStockTransferDetails(string StockTransferNo, int cmpid)
        {
            return _repoWrapper.InterDeparmentReceiver.GetStockTransferDetails(StockTransferNo, cmpid);
        }


        [HttpGet("GetstoreDropdownvalues/{cmpid}")]
        public IEnumerable<Dropdown> GetstoreDropdownvalues(int cmpid)
        {
            return _repoWrapper.InterDeparmentReceiver.GetstoreDropdownvalues(cmpid);
        }

        [HttpGet("GetInterDepartmentTransferNo/{InterDepRecNo}")]
        public dynamic GetInterDepartmentTransferNo(int InterDepRecNo)
        {
            return _repoWrapper.InterDeparmentReceiver.GetInterDepartmentTransferNo(InterDepRecNo);
        }


        [HttpPost("AddReceivedStockDetails")]
        public dynamic AddReceivedStockDetails([FromBody] InterDepartmentStockDetails AddStock)
        {
            AddStock.RunningNoStock = _repoWrapper.Common.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "GetRunningNo");

            if (AddStock.RunningNoStock == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            return _repoWrapper.InterDeparmentReceiver.AddReceivedStockDetails(AddStock);
        }

        [HttpGet("GetRecDetails/{ID}/{RecCode}/{cmpid}")]
        public dynamic GetRecDetails(int ID, int RecCode, int cmpid)
        {
            return _repoWrapper.InterDeparmentReceiver.GetRecDetails(ID, RecCode, cmpid);
        }

        [HttpGet("GetRecStockTransferDetails/{StockTransferNo}/{cmpid}/{GMT}")]
        public dynamic GetRecStockTransferDetails(string StockTransferNo, int cmpid,string GMT)
        {
            return _repoWrapper.InterDeparmentReceiver.GetRecStockTransferDetails(StockTransferNo, cmpid, GMT);
        }

        [HttpGet("GetInterDepartmentTransferNo1/{InterDepRecNo}")]
        public dynamic GetInterDepartmentTransferNo1(int InterDepRecNo)
        {
            return _repoWrapper.InterDeparmentReceiver.GetInterDepartmentTransferNo1(InterDepRecNo);
        }

        [HttpGet("GetStoreDetails1/{IssueCode}/{cmpid}")]
        public dynamic GetStoreDetails1(int IssueCode, int cmpid)
        {
            return _repoWrapper.InterDeparmentReceiver.GetStoreDetails1(IssueCode, cmpid);
        }

        [HttpPost("GetStockTransferDetails1/{cmpid}")]
        public dynamic GetStockTransferDetails1([FromBody] InterDepartmentStockDetails1 InterOpStkDetails, int cmpid)
        {
            return _repoWrapper.InterDeparmentReceiver.GetStockTransferDetails1(InterOpStkDetails, cmpid);
        }


        [HttpPost("AddReceivedStockDetails1")]
        public dynamic AddReceivedStockDetails1([FromBody] SubmitReceiptDetails AddStock)
        {
            AddStock.TcRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(AddStock.TransactionID, AddStock.cmpid, "GetRunningNo");
            AddStock.DamageRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(AddStock.DamageTcID, AddStock.cmpid, "GetRunningNo");
            AddStock.LossInTransitRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(AddStock.LossInTransitTcID, AddStock.cmpid, "GetRunningNo");
            AddStock.OthersRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(AddStock.OthersTcID, AddStock.cmpid, "GetRunningNo");

            if (AddStock.TcRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            if (AddStock.DamageRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Damage Qty Running Number Does'nt Exist"
                };
            }

            if (AddStock.LossInTransitRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Loss in Transit Qty Running Number Does'nt Exist"
                };
            }

            if (AddStock.OthersRunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Other Qty Running Number Does'nt Exist"
                };
            }

            return _repoWrapper.InterDeparmentReceiver.AddReceivedStockDetails1(AddStock);
        }
    }
}
