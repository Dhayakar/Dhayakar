
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Repository;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class NumberControlController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public NumberControlController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        

       [HttpPost("InsertNum")]
        public dynamic InsertNum([FromBody] NumberControlViewModel NumberControl)
        {
            return _repoWrapper.NumberControl.InsertNum (NumberControl);
        }
        [HttpPost("UpdateNum/{VCID}")]
        public dynamic UpdateNum([FromBody] NumberControlViewModel NumberControl,int VCID)
        {
            return _repoWrapper.NumberControl.UpdateNum(NumberControl, VCID);
        }
        [HttpPost("DeleteNum/{VCID}")]
        public dynamic DeleteNum([FromBody] NumberControlViewModel NumberControl, int VCID)
        {
            return _repoWrapper.NumberControl.DeleteNum(NumberControl, VCID);
        }


        [HttpGet("getNumberControl/{CmpID}/{Description}")]
        public NumberControlViewModel getNumberControl(int CmpID,int Description)
        {
            return _repoWrapper.NumberControl.getNumberControl(CmpID, Description);
        }
        [HttpGet("getgridNC/{CmpID}")]
        public NumberControlViewModel getgridNC(int CmpID)
        {
            return _repoWrapper.NumberControl.getgridNC(CmpID);
        }
    }

}



