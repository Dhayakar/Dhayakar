
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
         [Route("[controller]")]
        public class DepartController : Controller
        {
        private IRepositoryWrapper _repoWrapper;
        public DepartController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        [HttpPost("InsertPart")]
        public dynamic InsertPart([FromBody]DepartView De)
        {
            return _repoWrapper.Depart.InsertPart(De);
        }

        [HttpGet("GetDepartDetail")]
        public DepartView GetDepartDetail()
        {
            return _repoWrapper.Depart.GetDepartDetail();
        }
        [HttpPost("UpdatePart/{ID}")]
        public dynamic UpdatePart([FromBody]DepartView De1,int ID)
        {
            return _repoWrapper.Depart.UpdatePart(De1, ID);
        }
        [HttpPost("deletepart/{ID}")]
        public dynamic deletepart(int? ID)
        {
            return _repoWrapper.Depart.deletepart(ID);
        }
        

    }

}



