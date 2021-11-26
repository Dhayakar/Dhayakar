
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class GrnController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public GrnController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpGet("GetPoDetails/{id}")]
        public Grn GetPoDetails(int id)
        {
            return _repoWrapper.Grn.GetPoDetails(id);
        }

        [HttpGet("GetItemDetails/{ID}/{storeID}/{CmpID}")]
        public Grn GetItemDetails(string ID, int storeID, int CmpID)
        {
            return _repoWrapper.Grn.GetItemDetails(ID, storeID, CmpID);
        }

        [HttpPost("UpdateGRN/{SID}/{cmpPid}/{TransactionTypeid}")]
        public dynamic UpdateGRN([FromBody] Grn GRN, int SID, int cmpPid, int TransactionTypeid)
        {
            String gr = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpPid, "GetRunningNo");

            if (gr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            GRN.StockMaster.DocumentNumber = gr;
            return _repoWrapper.Grn.UpdateGRN(GRN, SID, cmpPid, TransactionTypeid);
        }

        [HttpGet("GetGrnDetails/{id}/{tid}/{Getloctime}")]
        public Grn GetGrnDetails(int id, int tid,string Getloctime)
        {

            return _repoWrapper.Grn.GetGrnDetails(id, tid, Getloctime);
        }


        [HttpGet("GetGrnItemDetails/{ID}/{cmpPid}")]
        public Grn GetGrnItemDetails(string ID, int cmpPid)
        {
            return _repoWrapper.Grn.GetGrnItemDetails(ID, cmpPid);
        }
        [HttpGet("GetGrnSerialDetails/{gnno}")]
        public Grn GetGrnSerialDetails(string gnno)
        {
            return _repoWrapper.Grn.GetGrnSerialDetails(gnno);
        }



        [HttpGet("CheckRNC/{cmpid}/{tranid}")]
        public dynamic CheckRNC(int cmpid,int tranid)
        {
            return _repoWrapper.Grn.CheckRNC(cmpid, tranid);
        }


        

    }
}
