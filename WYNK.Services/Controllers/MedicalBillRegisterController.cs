
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class MedicalBillRegisterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public MedicalBillRegisterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getMedBillDet/{FromDate}/{Todate}/{CompanyID}")]
        public dynamic getMedBillDet(string FromDate, string Todate, int CompanyID)
        {
            return _repoWrapper.medBillReg.getMedBillDet(FromDate, Todate, CompanyID);
        }




    }
}