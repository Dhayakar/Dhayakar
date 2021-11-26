
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class CompanyMasterController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public CompanyMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("insertdata/{time}/{currency}/{cid}")]
        public dynamic insertdata([FromBody]CompanyMasterView companyMaster, string time, string currency, int cid)
        {
            return _repoWrapper.CompanyMasterView.insertdata(companyMaster, time, currency,cid);
        }


        [HttpGet("getCompanyName/{cmpid}")]
        public CompanyMasterView getCompanyName(int cmpid)
        {
            return _repoWrapper.CompanyMasterView.getCompanyName(cmpid);
        }


        [HttpPost("UpdateCompanyDet/{ID}/{time}/{currency}/{cid}")]
        public dynamic UpdateCompanyDet([FromBody] CompanyMasterView companyMaster, int ID, string time, string currency, int cid)
        {
            return _repoWrapper.CompanyMasterView.UpdateCompanyDet(companyMaster, ID, time, currency, cid);
        }

        [HttpPost("DeleteCompanyDet/{ID}")]
        public dynamic DeleteCompanyDet(int ID)
        {
            return _repoWrapper.CompanyMasterView.DeleteCompanyDet(ID);
        }

        [HttpGet("SelectCompany")]
        public dynamic SelectCompany()
        {
            return _repoWrapper.CompanyMasterView.SelectCompany();
        }


        [HttpGet("SelectModules")]
        public dynamic SelectModules()
        {
            return _repoWrapper.CompanyMasterView.SelectModules();
        }


        [HttpGet("SelecNumberControldata")]
        public dynamic SelecNumberControldata()
        {
            return _repoWrapper.CompanyMasterView.SelecNumberControldata();
        }

    }
}