using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class YearEndProcessController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public YearEndProcessController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
     
        [HttpGet("GetFinancialYearDetail/{cmpid}")]
        public YearEndProcessViewModel GetFinancialYearDetail(int cmpid)
        {
            return _repoWrapper.YearEndProcess.GetFinancialYearDetail(cmpid);
        }

        [HttpGet("GetFinancialID/{ID}/{cmpid}")]
        public dynamic GetFinancialID(int ID, int cmpid)
        {
            return _repoWrapper.YearEndProcess.GetFinancialID(ID, cmpid);
        }

        [HttpGet("GetFinancialYearItemBalance/{ID}/{cmpid}/{FFYID}/{TFYID}")]
        public dynamic GetFinancialYearItemBalance(string ID, int cmpid,int FFYID,int TFYID)
        {
            return _repoWrapper.YearEndProcess.GetFinancialYearItemBalance(ID, cmpid, FFYID, TFYID);
        }



        [HttpGet("Submit/{cmpid}/{FFYID}/{TFYID}/{Store}/{Createdby}")]
        public dynamic Submit(int cmpid, int FFYID, int TFYID, string Store,int Createdby)
        {
            return _repoWrapper.YearEndProcess.Submit(cmpid, FFYID, TFYID, Store, Createdby);
        }


        [HttpGet("GetOpticaLFinancialYearItemBalance/{ID}/{cmpid}/{FFYID}/{TFYID}")]
        public dynamic GetOpticaLFinancialYearItemBalance(string ID, int cmpid, int FFYID, int TFYID)
        {
            return _repoWrapper.YearEndProcess.GetOpticaLFinancialYearItemBalance(ID, cmpid, FFYID, TFYID);
        }

        [HttpGet("SubmitOptical/{cmpid}/{FFYID}/{TFYID}/{Store}/{Createdby}")]
        public dynamic SubmitOptical(int cmpid, int FFYID, int TFYID, string Store, int Createdby)
        {
            return _repoWrapper.YearEndProcess.SubmitOptical(cmpid, FFYID, TFYID, Store, Createdby);
        }


    }
}


