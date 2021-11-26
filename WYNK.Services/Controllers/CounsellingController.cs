
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class CounsellingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public CounsellingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("getConcerntextfile/{CompanyID}")]
        public Counselling_Master getConcerntextfile(int CompanyID)
        {
            return _repoWrapper.CounsellingRepository.getConcerntextfile(CompanyID);
        }





        [HttpGet("GetParticularLens/{Specdesc}")]
        public Counselling_Master GetParticularLens(string Specdesc)
        {
            return _repoWrapper.CounsellingRepository.GetParticularLens(Specdesc);
        }



        [HttpGet("getsearchdetails/{CompanyID}")]
        public Counselling_Master getsearchdetails(int CompanyID)
        {
            return _repoWrapper.CounsellingRepository.getsearchdetails(CompanyID);
        }

        [HttpGet("getuintotaldatahistory/{CompanyID}/{UIN}")]
        public dynamic getuintotaldatahistory(int CompanyID, string UIN)
        {
            return _repoWrapper.CounsellingRepository.getuintotaldatahistory(CompanyID, UIN);
        }

        [HttpPost("InsertCouns")]
        public dynamic InsertCouns([FromBody]Counselling_Master couns)
        {
            return _repoWrapper.CounsellingRepository.InsertCouns(couns);
        }
        [HttpGet("GetCnsDetail")]
        public Counselling_Master GetCnsDetail()
        {
            return _repoWrapper.CounsellingRepository.GetCnsDetail();
        }
        [HttpPost("deletecns/{ID}")]
        public dynamic deletecns(int? ID)
        {
            return _repoWrapper.CounsellingRepository.deletecns(ID);
        }
        [HttpPost("UpdateCouns/{ID}")]
        public dynamic UpdateCouns([FromBody]Counselling_Master cps, int ID)
        {
            return _repoWrapper.CounsellingRepository.UpdateCouns(cps, ID);
        }


        [HttpPost("InsertCounsellingData")]
        public dynamic InsertCounsellingData([FromBody] savingCounsellingdetails InsertCounselling)
        {
            return _repoWrapper.CounsellingRepository.InsertCounsellingData(InsertCounselling);
        }

        [HttpPost("getSurgeonDetails")]
        public dynamic getSurgeonDetails([FromBody] SURGDETAILS SUrgeondsesss)
        {
            return _repoWrapper.CounsellingRepository.getSurgeonDetails(SUrgeondsesss);
        }

        [HttpPost("getanesthetistDetails")]
        public dynamic getanesthetistDetails([FromBody] SURGDETAILS Anesthesesss)
        {
            return _repoWrapper.CounsellingRepository.getanesthetistDetails(Anesthesesss);
        }


    }
}