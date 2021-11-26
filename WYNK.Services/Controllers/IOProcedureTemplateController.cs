
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class IOProcedureTemplateController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public IOProcedureTemplateController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        
        [HttpPost("Submit/{userID}/{icdspecialitycode}")]
        public dynamic Submit([FromBody] IOProcedureTemplateViewModel submit,int userID, int icdspecialitycode)
        {
          return _repoWrapper.IOProcedureTemplate.Submit(submit, userID, icdspecialitycode);
        }


        [HttpPost("Update/{userID}/{icdspecialitycode}")]
        public dynamic Update([FromBody] IOProcedureTemplateViewModel submit, int userID, int icdspecialitycode)
        {
            return _repoWrapper.IOProcedureTemplate.Update(submit, userID, icdspecialitycode);
        }

        [HttpGet("DeleteIOTemplateTRan/{userID}/{ID}")]
        public dynamic DeleteIOTemplateTRan(int userID, int ID)
        {
            return _repoWrapper.IOProcedureTemplate.DeleteIOTemplateTRan(userID, ID);
        }

        [HttpGet("GetSurgeryDescriptions/{IcdSpecCode}")]
        public dynamic GetSurgeryDescriptions(int IcdSpecCode)
        {
            return _repoWrapper.IOProcedureTemplate.GetSurgeryDescriptions(IcdSpecCode);
        }

    }
}
