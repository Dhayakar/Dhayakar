
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InsuranceVsMiddlemanController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InsuranceVsMiddlemanController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpGet("GetMiddlemanDetails/{MiddlemanName}")]
        public InsuranceVsMiddlemanViewModel GetMiddlemanDetails(int MiddlemanName)
        {
            return _repoWrapper.InsuranceVsMiddleman.GetMiddlemanDetails(MiddlemanName);
        }
        [HttpGet("GetInsuranceDetails")]
        public InsuranceVsMiddlemanViewModel GetInsuranceDetails()

        {
            return _repoWrapper.InsuranceVsMiddleman.GetInsuranceDetails();
        }
        [HttpPost("InsertInsuranceVsMiddleman/{MMID}/{userroleID}")]
        public dynamic InsertInsuranceVsMiddleman([FromBody]InsuranceVsMiddlemanViewModel AddInsuranceVsMiddleman,int MMID,int userroleID)
        {
            return _repoWrapper.InsuranceVsMiddleman.InsertInsuranceVsMiddleman(AddInsuranceVsMiddleman, MMID, userroleID);
        }
        [HttpPost("UpdateInsuranceVsMiddleman/{MMID}/{userroleID}")]
        public dynamic UpdateInsuranceVsMiddleman([FromBody] InsuranceVsMiddlemanViewModel InsuranceVsMiddlemanUpdate, int MMID,int userroleID)
        {
            return _repoWrapper.InsuranceVsMiddleman.UpdateInsuranceVsMiddleman(InsuranceVsMiddlemanUpdate,  MMID, userroleID);
        }
      
        
    }
}