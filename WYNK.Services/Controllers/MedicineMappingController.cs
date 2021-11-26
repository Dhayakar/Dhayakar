
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class MedicineMappingController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public MedicineMappingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("Getvalues")]
        public MedicineMapping Getvalues()
        {
            return _repoWrapper.MedicineMapping.Getvalues();
        }

        [HttpGet("Getchilddrug/{drugid}/{cmpid}")]
        public MedicineMapping Getchilddrug(int drugid,int cmpid)
        {
            return _repoWrapper.MedicineMapping.Getchilddrug(drugid,cmpid);
        }

        [HttpGet("Getrecdtls/{drugid}/{cmpid}")]
        public MedicineMapping Getrecdtls(int drugid, int cmpid)
        {
            return _repoWrapper.MedicineMapping.Getrecdtls(drugid, cmpid);
        }

        [HttpPost("InsertMedicineMapping/{userid}/{drugid}")]
        public dynamic InsertMedicineMapping([FromBody] MedicineMapping MedicineMapping, int userid, int drugid)
        {
            
            return _repoWrapper.MedicineMapping.InsertMedicineMapping(MedicineMapping, userid, drugid);
        }


    }
}