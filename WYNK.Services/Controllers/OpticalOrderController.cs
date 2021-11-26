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
using WYNK.Data.Model;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class OpticalOrderController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalOrderController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetvenderDetails/{VendorName}/{CMPID}")]
        public dynamic GetvenderDetails(int VendorName, int CMPID)

        {
            return _repoWrapper.OpticalOrder.GetvenderDetails(VendorName, CMPID);
        }
        [HttpGet("GetlocationDetails/{CityID}")]
        public OpticalOrderView GetlocationDetails(int CityID)

        {
            return _repoWrapper.OpticalOrder.GetlocationDetails(CityID);
        }

        [HttpGet("GetOpticalDetails/{lensTranID}/{CmpID}")]
        public dynamic GetOpticalDetails(int lensTranID, int CmpID)

        {
            return _repoWrapper.OpticalOrder.GetOpticalDetails(lensTranID, CmpID);
        }


        [HttpPost("InsertOpticalOrder/{Cmpid}/{TransactionTypeid}")]
        public dynamic InsertOpticalOrder([FromBody] OpticalOrderView AddOptical, int Cmpid, int TransactionTypeid)
        {


            if (TransactionTypeid == 0)
            {
               var Errordata = _repoWrapper.Common.ErrorList("TransactionTypeid=null", "OpticalOrder", Cmpid, Cmpid);
               
                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"

                };
           
            }


            int ? RecContraID = _repoWrapper.Common.GettingReceiptTcID(TransactionTypeid, Cmpid);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Number Does'nt Exist"
                };
            }
          
            if (AddOptical.paymenttran.Count > 0)
            {

                AddOptical.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), Cmpid, "GetRunningNo");

                if (AddOptical.ReceiptRunningNo == "Receipt Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Number Does'nt Exist"
                    };
                }
            }

            String gnr = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, Cmpid, "GetRunningNo");

            if (gnr == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }

            AddOptical.OpticalOrder.OrderNumber = gnr;

            return _repoWrapper.OpticalOrder.InsertOpticalOrder(AddOptical, Cmpid, TransactionTypeid);
        }


        [HttpGet("OpticalUpdateDetails/{Cmpid}")]
        public dynamic OpticalUpdateDetails(int Cmpid)

        {
            return _repoWrapper.OpticalOrder.OpticalUpdateDetails(Cmpid);
        }

        [HttpGet("OpticalbindingDetails/{Cmpid}/{OOID}")]
        public dynamic OpticalbindingDetails(int Cmpid, string OOID)

        {
            return _repoWrapper.OpticalOrder.OpticalbindingDetails(Cmpid, OOID);
        }

        [HttpPost("UpdateOpticalOrder/{Cmpid}/{TransactionTypeid}/{OpticalOrderID}")]
        public dynamic UpdateOpticalOrder([FromBody] OpticalOrderView OpticalUpdate, int Cmpid, int TransactionTypeid, int OpticalOrderID)
        {

            if (TransactionTypeid == 0) 
            {
                _repoWrapper.Common.ErrorList("TransactionTypeid=null", "OpticalOrder", Cmpid, Cmpid);
                return new
                {
                    Success = false,
                    Message = "TransactionTypeid Does'nt Exist"
                };
            }
         

           
            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(TransactionTypeid, Cmpid);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Exist"
                };
            }

            if (OpticalUpdate.paymenttran.Count > 0)
            {

                OpticalUpdate.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), Cmpid, "GetRunningNo");

                if (OpticalUpdate.ReceiptRunningNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Exist"
                    };
                }
            }
            return _repoWrapper.OpticalOrder.UpdateOpticalOrder(OpticalUpdate, Cmpid, TransactionTypeid, OpticalOrderID);
        }

        [HttpPost("DeleteOpticalOrder/{Cmpid}/{TransactionTypeid}/{OpticaltrnID}")]
        public dynamic DeleteOpticalOrder([FromBody] OpticalOrderView OpticalDelete, int Cmpid, int TransactionTypeid, int OpticaltrnID)
        {
            return _repoWrapper.OpticalOrder.DeleteOpticalOrder(OpticalDelete, Cmpid, TransactionTypeid, OpticaltrnID);
        }

        [HttpGet("getpayment/{OrderNumber}/{Cmpid}")]
        public OpticalOrderView getpayment(string OrderNumber, int Cmpid)
        {
            return _repoWrapper.OpticalOrder.getpayment(OrderNumber, Cmpid);
        }




    }

}



