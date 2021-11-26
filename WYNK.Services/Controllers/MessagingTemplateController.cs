
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class MessagingTemplateController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public MessagingTemplateController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("InsertMessagingTemplate")]
        public dynamic InsertMessagingTemplate([FromBody] MessagingTemplate MT)
        {
            return _repoWrapper.MessagingTemplate.InsertMessagingTemplate(MT);
        }


        [HttpPost("InsertBulsmstemplateTemplate")]
        public dynamic InsertBulsmstemplateTemplate([FromBody] MessagingTemplate MT)
        {
            return _repoWrapper.MessagingTemplate.InsertBulsmstemplateTemplate(MT);
        }


        [HttpGet("GetDBColumns/{Statuscolumn}")]
        public MessagingTemplate GetDBColumns(string Statuscolumn)
        {
            return _repoWrapper.MessagingTemplate.GetDBColumns(Statuscolumn);
        }


        [HttpGet("Gettemplates/{cmpid}")]
        public MessagingTemplate Gettemplates(int cmpid)
        {
            return _repoWrapper.MessagingTemplate.Gettemplates(cmpid);
        }

        [HttpGet("Getperioddetails/{cmpid}/{periodtype}/{visisttype}")]
        public MessagingTemplate Getperioddetails(int cmpid,string periodtype,string visisttype)
        {
            return _repoWrapper.MessagingTemplate.Getperioddetails(cmpid, periodtype, visisttype);
        }


        [HttpGet("Getspecificperioddetails/{cmpid}/{fromdate}/{todate}/{visisttype}")]
        public MessagingTemplate Getspecificperioddetails(int cmpid, string fromdate,string todate,string visisttype)
        {
            return _repoWrapper.MessagingTemplate.Getspecificperioddetails(cmpid, fromdate, todate, visisttype);
        }



        [HttpGet("Updatesmstemplate/{cmpid}/{Desc}/{id}")]
        public MessagingTemplate Updatesmstemplate(int cmpid, string Desc, string id)
        {
            return _repoWrapper.MessagingTemplate.Updatesmstemplate(cmpid, Desc, id);
        }



        [HttpGet("Updatemailtemplate/{cmpid}/{Desc}/{id}")]
        public MessagingTemplate Updatemailtemplate(int cmpid, string Desc, string id)
        {
            return _repoWrapper.MessagingTemplate.Updatemailtemplate(cmpid, Desc, id);
        }



        [HttpGet("Updatewhatsapptemplate/{cmpid}/{Desc}/{id}")]
        public MessagingTemplate Updatewhatsapptemplate(int cmpid, string Desc, string id)
        {
            return _repoWrapper.MessagingTemplate.Updatewhatsapptemplate(cmpid, Desc, id);
        }


        [HttpGet("Getformattemplates/{cmpid}/{format}")]
        public MessagingTemplate Getformattemplates(int cmpid, string format)
        {
            return _repoWrapper.MessagingTemplate.Getformattemplates(cmpid, format);
        }
        

        [HttpGet("GetAadhaarOTP/{Aadhaatrnumber}")]
        public dynamic GetAadhaarOTP(string Aadhaatrnumber)
        {
            return _repoWrapper.MessagingTemplate.GetAadhaarOTP(Aadhaatrnumber);
        }

        [HttpGet("SubmitAadhaarOTP/{OTP}/{ClientID}")]
        public dynamic SubmitAadhaarOTP(string OTP, string ClientID)
        {
            return _repoWrapper.MessagingTemplate.SubmitAadhaarOTP(OTP, ClientID);
        }



        [HttpGet("Panverification/{pan}/{amount}")]
        public dynamic Panverification(string pan, int amount)
        {
            return _repoWrapper.MessagingTemplate.Panverification(pan,amount);
        }


    }
}