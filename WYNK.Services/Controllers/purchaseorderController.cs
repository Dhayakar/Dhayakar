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
    public class purchaseorderController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public purchaseorderController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetvenderDetails/{Name}/{cmpid}")]
        public dynamic GetvenderDetails(int Name, int cmpid)

        {
            return _repoWrapper.purchaseorder.GetvenderDetails(Name, cmpid);
        }

        [HttpGet("Getcompantdetails/{cmpid}")]
        public dynamic Getcompantdetails(int cmpid)

        {
            return _repoWrapper.purchaseorder.Getcompantdetails(cmpid);
        }

        [HttpGet("GetdrugDetails/{ID}/{cmpid}")]
        public dynamic GetdrugDetails(int ID, int cmpid)

        {
            return _repoWrapper.purchaseorder.GetdrugDetails(ID, cmpid);
        }


        [HttpPost("Insertpurchaseorder/{cmpid}/{TransactionId}/{Time}")]
        public dynamic InsertStoreMas([FromBody]purchaseorderview Addpurchase, int cmpid, int TransactionId, string Time)
        {

            if (Addpurchase.PurchaseOrder.PONumber == "")
            {
                String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionId, cmpid, "GetRunningNo");

                if (gnr == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Running Number Does'nt Exist"
                    };
                }

                Addpurchase.PurchaseOrder.PONumber = gnr;

            }

            return _repoWrapper.purchaseorder.Insertpurchaseorder(Addpurchase, cmpid, TransactionId, Time);
        }
        [HttpGet("Getpurchaseprint/{RandomUniqueID}/{cmpid}/{Time}/{name}/{PONumber}")]
        public dynamic Getpurchaseprint(string RandomUniqueID, int cmpid, string Time, string name, string PONumber)
        {
            return _repoWrapper.purchaseorder.Getpurchaseprint(RandomUniqueID, cmpid, Time, name, PONumber);
        }


        [HttpGet("purchasehis/{cmpid}/{TransactionId}/{Time}")]
        public dynamic purchasehis(int cmpid, int TransactionId, string Time)
        {
            return _repoWrapper.purchaseorder.purchasehis(cmpid, TransactionId, Time);
        }

        [HttpGet("GetlocationDetails/{id}")]
        public dynamic GetlocationDetails(int id)

        {
            return _repoWrapper.purchaseorder.GetlocationDetails(id);
        }


    }

}




