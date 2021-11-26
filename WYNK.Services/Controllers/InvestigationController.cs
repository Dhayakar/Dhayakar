
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System.Collections.Generic;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class InvestigationController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public InvestigationController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpPost("uploadImag/{id}/{desc}/{uin}")]
        public bool uploadImag(string id, string desc, string uin)
        {
            var file1 = Request.Form.Files[0];
            return _repoWrapper.Investigation.uploadImag(file1, uin, desc, id);
        }


        [HttpGet("Getimage/{UIN}")]
        public dynamic Getimage(string UIN)
        {
            return _repoWrapper.Investigation.Getimage(UIN);
        }

        [HttpGet("Getpahistory/{cmpid}")]
        public dynamic Getpahistory(int cmpid)
        {
            return _repoWrapper.Investigation.Getpahistory(cmpid);
        }


        [HttpGet("GetInvpresDetails/{ID}")]
        public InvestigationImage GetInvpresDetails(string ID)
        {
            return _repoWrapper.Investigation.GetInvpresDetails(ID);
        }

        [HttpGet("GetInvpastDetails/{cmpid}/{UIN}")]
        public InvestigationImage GetInvpastDetails(int cmpid, string UIN)
        {
            return _repoWrapper.Investigation.GetInvpastDetails(cmpid, UIN);
        }

        [HttpGet("GetInvpresTranDetails/{ID}/{NO}")]
        public InvestigationImage GetInvpresTranDetails(string ID, int NO)
        {
            return _repoWrapper.Investigation.GetInvpresTranDetails(ID, NO);
        }

        [HttpGet("GetPatDetails/{UIN}/{cmpid}/{GMT}")]
        public InvestigationImage GetPatDetails(string UIN, int cmpid, string GMT)
        {
            return _repoWrapper.Investigation.GetPatDetails(UIN, cmpid, GMT);
        }

        [HttpGet("GetUINDetails/{cid}")]
        public InvestigationImage GetUINDetails(int cid)
        {
            return _repoWrapper.Investigation.GetUINDetails(cid);
        }


        [HttpPost("UpdateInvestigation/{UIN}/{ipid}")]
        public dynamic UpdateInvestigation([FromBody] InvestigationImage Investigation, string UIN, string ipid)
        {
            return _repoWrapper.Investigation.UpdateInvestigation(Investigation, UIN, ipid);
        }


        [HttpPost("UpdateInv/{UIN}/{IID}/{cmpid}")]
        public dynamic UpdateInv([FromBody] InvestigationImage Investigation, string UIN, int IID, int cmpid)
        {
            return _repoWrapper.Investigation.UpdateInv(Investigation, UIN, IID, cmpid);
        }


        [HttpGet("Getnotificationalerts/{Docid}/{cmpid}")]
        public dynamic Getnotificationalerts(int Docid, int cmpid)
        {
            return _repoWrapper.Investigation.Getnotificationalerts(Docid, cmpid);
        }

        [HttpGet("GettestDetails/{cmpid}/{tag}")]
        public IEnumerable<Test> GettestDetails(int cmpid, string tag)
        {
            return _repoWrapper.Investigation.GettestDetails(cmpid, tag);
        }
        [HttpGet("GetamtDetails/{cmpid}/{id}")]
        public dynamic GetamtDetails(int cmpid, string id)
        {
            return _repoWrapper.Investigation.GetamtDetails(cmpid, id);
        }

    }
}
