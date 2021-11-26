using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model.ViewModel;
using System.Threading.Tasks;
using WYNK.Data.Repository;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class Patientcontroller : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public Patientcontroller(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetDetails/{FromDate}/{ToDate}/{CMPID}")]
        public PatientViewModel GetDetails(DateTime FromDate, DateTime ToDate, int CMPID)


        {
            return _repoWrapper.Registration.GetDetails(FromDate, ToDate, CMPID);
        }


    }
}
