
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WYNK.Data.Repository;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]

    public class HomeController : Controller
    {
 private IRepositoryWrapper _repoWrapper;

        public HomeController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet("dummy")]
        public string Dummy()
        {
            return "hi";
        }

        [HttpGet("test")]
        public async Task<object> Test()
        {
            Thread.Sleep(5000);
            return await _repoWrapper.Common.GetDropdown("Site_Master", "Site_Code", "Site_Name");
        }
      
        [HttpPost("uploadFile")]
        public bool uploadFile()
        {
            var file = Request.Form.Files[0];
            return false;
        }
    }
}
