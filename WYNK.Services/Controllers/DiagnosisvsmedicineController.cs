
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class DiagnosisVSMedicineController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public DiagnosisVSMedicineController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

       
        [HttpGet("Getdruggvalues/{cmpid}")]
        public diagnosisvsmedicine Getdruggvalues(int cmpid)
        {
            return _repoWrapper.DiagnosisVSMedicine.Getdruggvalues(cmpid);
        }
        [HttpGet("GetSelectedmeddetials/{ID}/{cmpid}")]
        public diagnosisvsmedicine GetSelectedmeddetials(int ID, int cmpid)
        {
            return _repoWrapper.DiagnosisVSMedicine.GetSelectedmeddetials(ID, cmpid);
        }

        [HttpGet("GetsubSelectedmeddetials/{ID}/{cmpid}")]
        public diagnosisvsmedicine GetsubSelectedmeddetials(string ID, int cmpid)
        {
            return _repoWrapper.DiagnosisVSMedicine.GetsubSelectedmeddetials(ID, cmpid);
        }

        [HttpPost("Insertdiagmeddata")]
        public dynamic Insertdiagmeddata([FromBody] diagnosisvsmedicine DiagnosisVSMedicine)
        {
            return _repoWrapper.DiagnosisVSMedicine.Insertdiagmeddata(DiagnosisVSMedicine);
        }

    }
}