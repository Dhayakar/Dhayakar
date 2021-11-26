
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class IpOpticalBillingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public IpOpticalBillingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("GetPatientDetails/{id}")]
        public OpticalBilling GetPatientDetails(int id)
        {
            return _repoWrapper.IpOpticalBilling.GetPatientDetails(id);
        }

        [HttpGet("GetOpticalDetails/{id}/{rid}")]
        public OpticalBilling GetOpticalDetails(int id, int rid)
        {
            return _repoWrapper.IpOpticalBilling.GetOpticalDetails(id, rid);
        }

        [HttpGet("GetLensDetails")]
        public OpticalBilling GetLensDetails()
        {
            return _repoWrapper.IpOpticalBilling.GetLensDetails();
        }

        [HttpPost("UpdateOPBilling/{UIN}")]
        public dynamic UpdateOPBilling([FromBody] OpticalBilling OpticalBilling, string UIN)
        {
            return _repoWrapper.IpOpticalBilling.UpdateOPBilling(OpticalBilling, UIN);
        }


    }
}
