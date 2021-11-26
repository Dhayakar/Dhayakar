
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;
using System;


namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class ErrorlogController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public ErrorlogController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpGet("Geterrorlogfile/{FromDate}/{Todate}/{Time}")]
        public ErrorLogviewmodel Geterrorlogfile(DateTime FromDate, DateTime Todate, string Time)
        {
            return _repoWrapper.Errorlog.Geterrorlogfile(FromDate, Todate, Time);
        }

        [HttpGet("gettotallines")]
        public dynamic gettotallines()
        {
            return _repoWrapper.Errorlog.gettotallines();
        }

        [HttpGet("gettotalreleases")]
        public dynamic gettotalreleases()
        {
            return _repoWrapper.Errorlog.gettotalreleases();
        }

        [HttpGet("gettotalreleasesbasedondate/{textname}")]
        public dynamic gettotalreleasesbasedondate(string textname)
        {
            return _repoWrapper.Errorlog.gettotalreleasesbasedondate(textname);
        }
        

    }
}