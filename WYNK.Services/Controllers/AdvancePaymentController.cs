
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class AdvancePaymentController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public AdvancePaymentController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("getADVRePrint/{UIN}/{cmpid}")]
        public dynamic getADVRePrint(string UIN,int cmpid)
        {
            return _repoWrapper.AdvancePayment.getADVRePrint(UIN, cmpid);
        }
        [HttpGet("getADVRePrintEdit/{ReceiptNumber}")]
        public dynamic getADVRePrintEdit(string ReceiptNumber)
        {
            return _repoWrapper.AdvancePayment.getADVRePrintEdit(ReceiptNumber);
        }
        [HttpGet("getADVRePrintF/{ReceiptNumber}/{cmpid}")]
        public dynamic getADVRePrintF(string ReceiptNumber,int cmpid)
        {
            return _repoWrapper.AdvancePayment.getADVRePrintF(ReceiptNumber, cmpid);
        }
        
        [HttpPost("AddAdvance/{CompanyID}/{userid}/{TransactionTypeid}")]
        public dynamic AddAdvance([FromBody] AdvancePaymentViewModel AddBill, int CompanyID, int userid, int TransactionTypeid)
        {
            string generatenumber = _repoWrapper.Common.GenerateRunningCtrlNoo(TransactionTypeid, CompanyID, "GetRunningNo");
            if (generatenumber == "Running Number Does'nt Exist")
            {
                return new
                {
                    Success = false,
                    Message = "Running Number Does'nt Exist"
                };
            }
            AddBill.payment.ReceiptNumber = generatenumber;
            return _repoWrapper.AdvancePayment.AddAdvance(AddBill, CompanyID, TransactionTypeid);
        }

      
    }
}
