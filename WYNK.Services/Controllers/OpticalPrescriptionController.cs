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
    public class OpticalPrescriptionController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public OpticalPrescriptionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("GetfinalopDetails/{UIN}/{id}")]
        public dynamic GetfinalopDetails(string UIN, int id)
        {
            return _repoWrapper.OpticalPrescription.GetfinalopDetails(UIN, id);
        }


        [HttpGet("GetopticalDetails/{RegID}")]
        public dynamic GetopticalDetails(int RegID)
        {
            return _repoWrapper.OpticalPrescription.GetopticalDetails(RegID);
        }

        [HttpGet("Getfinopprint/{rid}/{CMPID}/{Time}")]
        public dynamic Getfinopprint(int rid, int CMPID, string Time)
        {
            return _repoWrapper.OpticalPrescription.Getfinopprint(rid, CMPID, Time);
        }


        [HttpGet("GetUINDetails/{cid}")]
        public dynamic GetUINDetails(int cid)
        {
            return _repoWrapper.OpticalPrescription.GetUINDetails(cid);
        }

        [HttpGet("GetHistoryDetails/{UIN}/{rid}")]
        public dynamic GetHistoryDetails(string UIN, int rid)
        {
            return _repoWrapper.OpticalPrescription.GetHistoryDetails(UIN, rid);
        }


    }
}
