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
    public class ReportssController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public ReportssController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }



        [HttpGet("Todaysearch/{FromDate}/{ToDate}/{CompanyID}")]
        public Vreportss Todaysearch(string FromDate, string ToDate, int CompanyID)
        {
            return _repoWrapper.Reportss.Todaysearch(FromDate, ToDate, CompanyID);
        }


        [HttpGet("Getnewvisits/{date}/{Newcount}/{CompanyID}")]
        public Vreportss Getnewvisits(string date, string Newcount, int CompanyID)
        {
            return _repoWrapper.Reportss.Getnewvisits(date, Newcount, CompanyID);
        }

        [HttpGet("GetBranchwisedetails/{date}/{CompanyID}")]
        public Vreportss GetBranchwisedetails(string date, int CompanyID)
        {
            return _repoWrapper.Reportss.GetBranchwisedetails(date,  CompanyID);
        }

        

        [HttpGet("Getreviewvisits/{date}/{Newcount}/{CompanyID}")]
        public Vreportss Getreviewvisits(string date, string Newcount, int CompanyID)
        {
            return _repoWrapper.Reportss.Getreviewvisits(date, Newcount, CompanyID);
        }


        [HttpGet("Getsurgreviewvisits/{date}/{Newcount}/{CompanyID}")]
        public Vreportss Getsurgreviewvisits(string date, string Newcount, int CompanyID)
        {
            return _repoWrapper.Reportss.Getsurgreviewvisits(date, Newcount, CompanyID);
        }


        [HttpGet("Getnewgeneralocular/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss Getnewgeneralocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.Getnewgeneralocular(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("GetnewgeneralNormal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetnewgeneralNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetnewgeneralNormal(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("GetnewpediatricNormal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetnewpediatricNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetnewpediatricNormal(date, Generalocular, cmpid, CMPbranch);
        }



        [HttpGet("Getnewpediatricocular/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss Getnewpediatricocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.Getnewpediatricocular(date, Generalocular, cmpid, CMPbranch);
        }


        [HttpGet("Getnewtotal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss Getnewtotal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.Getnewtotal(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("GetReviewgeneralNormal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetReviewgeneralNormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetReviewgeneralNormal(date, Generalocular, cmpid, CMPbranch);
        }


        [HttpGet("GetReviewgeneralocular/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetReviewgeneralocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetReviewgeneralocular(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("GetReviewpediatricnormal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetReviewpediatricnormal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetReviewpediatricnormal(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("GetReviewpediatricocular/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetReviewpediatricocular(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetReviewpediatricocular(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("Getviewtotal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss Getviewtotal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.Getviewtotal(date, Generalocular, cmpid, CMPbranch);
        }

        [HttpGet("GetnewCumulativetotal/{date}/{Generalocular}/{cmpid}/{CMPbranch}")]
        public Vreportss GetnewCumulativetotal(DateTime date, string Generalocular, string cmpid, string CMPbranch)
        {
            return _repoWrapper.Reportss.GetnewCumulativetotal(date, Generalocular, cmpid, CMPbranch);
        }

        


    }
}
