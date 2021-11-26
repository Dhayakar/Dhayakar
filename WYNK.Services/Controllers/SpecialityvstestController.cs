using System;
using System.Collections.Generic;
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
    public class SpecialityVSTestController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public SpecialityVSTestController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("Getinvestigationvalues/{CmpID}")]
        public specialityvstest Getinvestigationvalues(int CmpID)
        {
            return _repoWrapper.SpecialityVSTest.Getinvestigationvalues(CmpID);
        }

        [HttpGet("GetSelectedspecdetials/{ID}/{CmpID}")]
        public specialityvstest GetSelectedspecdetials(int ID, int CmpID)
        {
            return _repoWrapper.SpecialityVSTest.GetSelectedspecdetials(ID, CmpID);
        }
        [HttpGet("GetSelecteddiadetials/{ID}/{CmpID}")]
        public specialityvstest GetSelecteddiadetials(int ID, int CmpID)
        {
            return _repoWrapper.SpecialityVSTest.GetSelecteddiadetials(ID, CmpID);
        }
        [HttpGet("GetSelectedservdetials/{ID}/{CmpID}")]
        public specialityvstest GetSelectedservdetials(int ID, int CmpID)
        {
            return _repoWrapper.SpecialityVSTest.GetSelectedservdetials(ID, CmpID);
        }

        [HttpPost("Insertspecialitydata")]
        public dynamic Insertspecialitydata([FromBody] specialityvstest SpecialityVSTest)
        {
            return _repoWrapper.SpecialityVSTest.Insertspecialitydata(SpecialityVSTest);
        }

        [HttpPost("Insertdiagnosisdata")]
        public dynamic Insertdiagnosisdata([FromBody] specialityvstest SpecialityVSTest)
        {
            return _repoWrapper.SpecialityVSTest.Insertdiagnosisdata(SpecialityVSTest);
        }

        [HttpPost("Insertpackagedata")]
        public dynamic Insertpackagedata([FromBody] specialityvstest SpecialityVSTest)
        {
            return _repoWrapper.SpecialityVSTest.Insertpackagedata(SpecialityVSTest);
        }

        [HttpPost("UpdateTest")]
        public dynamic UpdateTest([FromBody] specialityvstest SpecialityVSTest)
        {
            return _repoWrapper.SpecialityVSTest.UpdateTest(SpecialityVSTest);
        }

        [HttpGet("Getservicevalues/{CmpID}")]
        public specialityvstest Getservicevalues(int CmpID)
        {
            return _repoWrapper.SpecialityVSTest.Getservicevalues(CmpID);
        }
    }
}