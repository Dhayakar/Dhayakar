
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class DonorController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public DonorController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("SubmitDonor")]
        public dynamic SubmitDonor([FromBody] DonorViewModel DonorDetails)
        {
            return _repoWrapper.Donor.SubmitDonor(DonorDetails);
        }

        [HttpPost("UpdateDonor/{ID}")]
        public dynamic UpdateDonor([FromBody] DonorViewModel DonorDetails, int ID)
        {
            return _repoWrapper.Donor.UpdateDonor(DonorDetails, ID);
        }

        [HttpGet("DeleteDonor/{ID}/{CMPID}/{USERID}/{Companyname}")]
        public dynamic DeleteDonor(int ID, int CMPID, int USERID,string Companyname)
        {
            return _repoWrapper.Donor.DeleteDonor(ID, CMPID, USERID, Companyname);
        }


    }
}



