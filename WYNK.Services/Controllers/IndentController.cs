
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class IndentController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        public IndentController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("Insertindent")]
        public dynamic Insertindent([FromBody] IndentViewModel Indentdata)
        {
            return _repoWrapper.Indentrepo.Insertindent(Indentdata);
        }

        [HttpPost("updateindent")]
        public dynamic updateindent([FromBody] IndentViewModel Indentdata)
        {
            return _repoWrapper.Indentrepo.updateindent(Indentdata);
        }

        

        [HttpGet("GetdrugDetails/{id}")]
        public dynamic GetdrugDetails(int id)
        {
            return _repoWrapper.Indentrepo.GetdrugDetails(id);
        }


        

    }
}