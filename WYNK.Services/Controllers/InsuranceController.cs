
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InsuranceController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InsuranceController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        
        [HttpGet("GetPinCodeDetails/{id}")]
        public InsuranceViewModel GetPinCodeDetails(int id)

        {
            return _repoWrapper.Insurance.GetPinCodeDetails(id);
        }
        [HttpGet("GetlocationDetails/{id}")]
        public InsuranceViewModel GetlocationDetails(int id)

        {
            return _repoWrapper.Insurance.GetlocationDetails(id);
        }
        [HttpPost("InsertInsurance")]
        public dynamic InsertInsurance([FromBody]InsuranceViewModel AddInsurance)
        {
            return _repoWrapper.Insurance.InsertInsurance(AddInsurance);
        }
        [HttpPost("UpdateInsurance/{ID}")]
        public dynamic UpdateInsurance([FromBody] InsuranceViewModel InsuranceUpdate, int ID)
        {
            return _repoWrapper.Insurance.UpdateInsurance(InsuranceUpdate,ID);
        }

        ////////////////////////////////////PreAuthorization////////////////////////////////////////
        [HttpGet("GetPreAuthorizationdtls/{cmpid}")]
        public InsuranceViewModel GetPreAuthorizationdtls(int cmpid)
        {
            return _repoWrapper.Insurance.GetPreAuthorizationdtls(cmpid);
        }

        [HttpPost("InsertPreAuthorization/{ID}")]
        public dynamic InsertPreAuthorization([FromBody] InsuranceViewModel AddPreAuthorization, int ID)
        {
            return _repoWrapper.Insurance.InsertPreAuthorization(AddPreAuthorization, ID);
        }
        [HttpGet("GetPreAUZupdate/{cmpid}")]
        public InsuranceViewModel GetPreAUZupdate(int cmpid)
        {
            return _repoWrapper.Insurance.GetPreAUZupdate(cmpid);
        }

        [HttpPost("DeletePreA/{ID}")]
        public dynamic DeletePreA([FromBody] InsuranceViewModel Insurancedelete, int ID)
        {
            return _repoWrapper.Insurance.DeletePreA(Insurancedelete, ID);
        }
        ////////////////////////////////////EstimateTracking////////////////////////////////////////
        [HttpGet("GetEstimateTrackingdtls/{cmpid}")]
        public InsuranceViewModel GetEstimateTrackingdtls(int cmpid)
        {
            return _repoWrapper.Insurance.GetEstimateTrackingdtls(cmpid);
        }
       
        [HttpPost("InsertEstimateTracking")]
        public dynamic InsertEstimateTracking([FromBody] InsuranceViewModel AddEstimateTracking)
        {
            return _repoWrapper.Insurance.InsertEstimateTracking(AddEstimateTracking);
        }

        [HttpGet("GetETDetails/{cmpid}")]
        public InsuranceViewModel GetETDetails(int cmpid)
        {
            return _repoWrapper.Insurance.GetETDetails(cmpid);
        }

    }
}