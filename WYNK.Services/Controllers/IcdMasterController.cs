
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class IcdMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public IcdMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("AddIcd")]
        public dynamic AddIcd([FromBody] ICDMaster AddIcd)
        {
            return _repoWrapper.icdMaster.AddIcd(AddIcd);
        }


        [HttpPost("SurgeryCostDetailSubmit")]
        public dynamic SurgeryCostDetailSubmit([FromBody] ICDMaster AddIcd)
        {
            return _repoWrapper.icdMaster.SurgeryCostDetailSubmit(AddIcd);
        }



        [HttpPost("AddSpecial")]
        public dynamic AddSpecial([FromBody] ICDMaster AddSpecial)
        {
            return _repoWrapper.icdMaster.AddSpecial(AddSpecial);
        }


        [HttpPost("AddIcdgroup")]
        public dynamic AddIcdgroup([FromBody] ICDMaster Addicds)
        {
            return _repoWrapper.icdMaster.AddIcdgroup(Addicds);
        }



        [HttpPost("DeleteICD/{Code}")]
        public dynamic DeleteICD(string Code)
        {
            return _repoWrapper.icdMaster.DeleteICD(Code);
        }


        [HttpPost("UpdateIcd/{Code}/{id}")]
        public dynamic UpdateIcd([FromBody] ICDMaster icdmaster, string Code, int id)
        {
            return _repoWrapper.icdMaster.UpdateIcd(icdmaster, Code, id);
        }



        [HttpGet("getSurgeryCostDetail/{ICDCODE}/{CMPID}/{roomtype}")]
        public dynamic getSurgeryCostDetail(string ICDCODE,int CMPID,string roomtype)
        {
            return _repoWrapper.icdMaster.getSurgeryCostDetail(ICDCODE,CMPID, roomtype);
        }

        [HttpGet("GetDoctorSpecialitydetails/{DID}/{CMPID}")]
        public ICDMaster GetDoctorSpecialitydetails(int DID, int CMPID)
        {
            return _repoWrapper.icdMaster.GetDoctorSpecialitydetails(DID, CMPID);
        }

        [HttpGet("GetAllICDDescription/{docid}")]
        public dynamic GetAllICDDescription(string docid)
        {
            return _repoWrapper.icdMaster.GetAllICDDescription(docid);
        }

    }
}
