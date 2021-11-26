
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class CampMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public CampMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("AddOrganization/{ID}")]
        public dynamic AddOrganization(string ID)
        {
            return _repoWrapper.CampMaster.AddOrganization(ID);
        }


        [HttpGet("UpdateOrganization/{ID}/{status}/{Desc}")]
        public dynamic UpdateOrganization(string ID,string status,string Desc)
        {
            return _repoWrapper.CampMaster.UpdateOrganization(ID, status, Desc);
        }
        [HttpGet("DeleteOrganization/{ID}/{valueolmid}")]
        public dynamic DeleteOrganization(string ID, int valueolmid)
        {
            return _repoWrapper.CampMaster.DeleteOrganization(ID, valueolmid);
        }
        [HttpGet("Getallorganization")]
        public dynamic Getallorganization()
        {
            return _repoWrapper.CampMaster.Getallorganization();
        }
        [HttpGet("Getallcampmasterdata")]
        public dynamic Getallcampmasterdata()
        {
            return _repoWrapper.CampMaster.Getallcampmasterdata();
        }
        


        [HttpPost("SubmitOrganizationData")]
        public dynamic SubmitOrganizationData([FromBody]campmasterViewModel Fundus)
        {
            return _repoWrapper.CampMaster.SubmitOrganizationData(Fundus);
        }
        [HttpPost("SubmitCampmasterData")]
        public dynamic SubmitCampmasterData([FromBody]campmasterViewModel Fundus)
        {
            return _repoWrapper.CampMaster.SubmitCampmasterData(Fundus);
        }
      
        [HttpPost("UpdateOrganizationData")]
        public dynamic UpdateOrganizationData([FromBody]campmasterViewModel Fundus)
        {
            return _repoWrapper.CampMaster.UpdateOrganizationData(Fundus);
        }

        [HttpPost("Updatecampmasterdata")]
        public dynamic Updatecampmasterdata([FromBody]campmasterViewModel Fundus)
        {
            return _repoWrapper.CampMaster.Updatecampmasterdata(Fundus);
        }
        

    }
}