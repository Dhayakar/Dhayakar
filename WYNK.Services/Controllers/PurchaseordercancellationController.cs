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
    public class PurchaseordercancellationController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PurchaseordercancellationController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("purchaseordercancel/{id}/{TransactionId}/{Time}")]
        public dynamic purchaseordercancel(int id, int TransactionId, string Time)
        {
            return _repoWrapper.Purchaseordercancellation.purchaseordercancel(id, TransactionId, Time);
        }

        [HttpGet("purchaseordercancellation/{id}/{TransactionId}/{Time}")]
        public dynamic purchaseordercancellation(int id, int TransactionId, string Time)
        {
            return _repoWrapper.Purchaseordercancellation.purchaseordercancellation(id, TransactionId, Time);
        }


        [HttpGet("Getpurchasecal/{RandomUniqueID}/{Time}")]
        public dynamic Getpurchasecal(string RandomUniqueID, string Time)
        {
            return _repoWrapper.Purchaseordercancellation.Getpurchasecal(RandomUniqueID, Time);
        }


        [HttpGet("Getpurchaseprintt/{RandomUniqueID}/{cmpid}/{Time}")]
        public dynamic Getpurchaseprintt(string RandomUniqueID, int cmpid, string Time)
        {
            return _repoWrapper.Purchaseordercancellation.Getpurchaseprintt(RandomUniqueID, cmpid, Time);
        }


        [HttpPost("Insertpurchaseordercancel/{cmpid}/{TransactionTypeid}/{Time}")]
        public dynamic Insertpurchaseordercancel([FromBody]Purchaseordercancellation cancelpurchase, int cmpid, int TransactionTypeid, string Time)
        {
            String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");
            if (gnr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            cancelpurchase.PurchaseOrdercancel.PONumber = gnr;

            return _repoWrapper.Purchaseordercancellation.Insertpurchaseordercancel(cancelpurchase, cmpid, TransactionTypeid, Time);
        }

    }

}




