
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class CustomerOrderController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public CustomerOrderController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetOfferDetail/{CMPID}/{LMID}/{LTID}")]
        public dynamic GetOfferDetail(int CMPID,int LMID,int LTID)
        {
            return _repoWrapper.Customerorder.GetOfferDetail(CMPID,LMID,LTID);
        }

        [HttpPost("SubmitCustomerOrder")]
        public dynamic SubmitCustomerOrder([FromBody] CustomerOrderViewModel CustomerOrderDetails)
        {

            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(CustomerOrderDetails.Tc, CustomerOrderDetails.Cmpid);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Mapped in Transaction Table"

                };
            }


            if (CustomerOrderDetails.paymenttran.Count > 0)
            {

                CustomerOrderDetails.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CustomerOrderDetails.Cmpid, "GetRunningNo");

                if (CustomerOrderDetails.ReceiptRunningNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Exist"
                    };
                }
            }

            CustomerOrderDetails.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(CustomerOrderDetails.Tc, CustomerOrderDetails.Cmpid, "GetRunningNo");
            if (CustomerOrderDetails.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            return _repoWrapper.Customerorder.SubmitCustomerOrder(CustomerOrderDetails);
        }


        [HttpGet("GetCustomerOrderedList/{CMPID}/{TC}")]
        public dynamic GetCustomerOrderedList(int CMPID,int TC)
        {
            return _repoWrapper.Customerorder.GetCustomerOrderedList(CMPID, TC);
        }


        [HttpGet("GetOrderNoDetails/{CMPID}/{OrderNo}")]
        public dynamic GetOrderNoDetails(int CMPID, string OrderNo)
        {
            return _repoWrapper.Customerorder.GetOrderNoDetails(CMPID, OrderNo);
        }


        [HttpPost("SubmitCustomerOrderCancel/{CancelDate}")]
        public dynamic SubmitCustomerOrderCancel([FromBody] CustomerOrderViewModel CustomerOrderCancelDetails,DateTime CancelDate)
        {

            int? RecContraID = _repoWrapper.Common.GettingReceiptTcID(CustomerOrderCancelDetails.Tc, CustomerOrderCancelDetails.Cmpid);

            if (RecContraID == null)
            {
                return new
                {
                    Success = false,
                    Message = "Receipt Running Number Does'nt Mapped in Transaction Table"
                };
            }


            if (CustomerOrderCancelDetails.paymenttran.Count > 0)
            {
                CustomerOrderCancelDetails.ReceiptRunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(Convert.ToInt32(RecContraID), CustomerOrderCancelDetails.Cmpid, "GetRunningNo");


                if (CustomerOrderCancelDetails.ReceiptRunningNo == "Running Number Does'nt Exist")
                {
                    return new
                    {
                        Success = false,
                        Message = "Receipt Running Number Does'nt Exist"
                    };
                }
            }
            CustomerOrderCancelDetails.RunningNo = _repoWrapper.Common.GenerateRunningCtrlNoo(CustomerOrderCancelDetails.Tc, CustomerOrderCancelDetails.Cmpid, "GetRunningNo");
            if (CustomerOrderCancelDetails.RunningNo == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            return _repoWrapper.Customerorder.SubmitCustomerOrderCancel(CustomerOrderCancelDetails, CancelDate);
        }


        [HttpGet("GetCustomerCancelOrderedList/{CMPID}/{TC}")]
        public dynamic GetCustomerCancelOrderedList(int CMPID, int TC)
        {
            return _repoWrapper.Customerorder.GetCustomerCancelOrderedList(CMPID, TC);
        }

        [HttpGet("GetCancelOrderNoDetails/{CMPID}/{CancelOrderNo}")]
        public dynamic GetCancelOrderNoDetails(int CMPID, string CancelOrderNo)
        {
            return _repoWrapper.Customerorder.GetCancelOrderNoDetails(CMPID, CancelOrderNo);
        }


        [HttpGet("GetfinalopDetails/{CMPID}")]
        public dynamic GetfinalopDetails(int CMPID)
        {
            return _repoWrapper.Customerorder.GetfinalopDetails(CMPID);
        }



        [HttpGet("IsCustomerFound/{CMPID}/{UIN}")]
        public dynamic IsCustomerFound(int CMPID,string UIN)
        {
            return _repoWrapper.Customerorder.IsCustomerFound(CMPID, UIN);
        }

        [HttpGet("GetOpticalPrescription/{CusMasID}/{CMPID}")]
        public dynamic GetOpticalPrescription(int CusMasID, int CMPID)
        {
            return _repoWrapper.Customerorder.GetOpticalPrescription(CusMasID, CMPID);
        }

        [HttpPost("CustomerDetailsSubmit/{CMPID}/{UserId}")]
        public dynamic CustomerDetailsSubmit([FromBody] CustomerSubmit CustomerSubmitDetails, int CMPID,int UserId)
        {
            return _repoWrapper.Customerorder.CustomerDetailsSubmit(CustomerSubmitDetails, CMPID, UserId);
        }


        [HttpPost("UploadImage/{CustomerOrderNo}")]
        public bool UploadImage(string CustomerOrderNo)
        {
            var file = Request.Form.Files[0];
            return _repoWrapper.Customerorder.UploadImage(file, CustomerOrderNo);
        }

        [HttpPost("InsertOpticalPrescription/{CustomerID}/{cmpID}/{userid}")]
        public dynamic InsertOpticalPrescription([FromBody] CustomerOrderViewModel AddOpticalPrescription, int CustomerID ,int cmpID ,int userid)
        {
            return _repoWrapper.Customerorder.InsertOpticalPrescription(AddOpticalPrescription, CustomerID, cmpID, userid);
        }
    }
}



