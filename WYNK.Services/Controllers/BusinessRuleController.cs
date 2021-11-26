using System;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class BusinessRuleController : Controller
    {

        private IRepositoryWrapper _repoWrapper;

        public BusinessRuleController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("InsertBusinessRule/{CmpID}/{BRID}")]
        public dynamic InsertBusinessRule([FromBody] BusinessRuleViewModel BusinessRule,int CmpID,int BRID)
        {
            return _repoWrapper.BusinessRule.InsertBusinessRule(BusinessRule, CmpID, BRID);
        }


        [HttpGet("GetBRMDescription/{Cmpid}/{Descriptionid}/{EFDate}")]
        public BusinessRuleViewModel GetBRMDescription(int cmpid, int Descriptionid, DateTime EFDate)
        {
            return _repoWrapper.BusinessRule.GetBRMDescription(cmpid, Descriptionid, EFDate);
        }


    }
}