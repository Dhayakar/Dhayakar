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
    public class VendorMasterController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public VendorMasterController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpPost("Insertvendordata")]
        public dynamic InsertVendorData([FromBody] VendorMasterViewModel Insertvendordata)
        {
            return _repoWrapper.VendorMaster.InsertVendorData(Insertvendordata);

        }
        [HttpGet("GetlocationDetails/{id}")]
        public VendorMasterViewModel GetlocationDetails(int id)

        {
            return _repoWrapper.VendorMaster.GetlocationDetails(id);
        }

        [HttpPost("InsertItemdata")]
        public dynamic InsertItemData([FromBody] VendorMasterViewModel InsertItemdata)
        {
            return _repoWrapper.VendorMaster.InsertItemData(InsertItemdata);

        }


        [HttpPost("InsertVendorMas")]
        public dynamic InsertVendorMas([FromBody]VendorMasterViewModel VendorMaster)
        {
            return _repoWrapper.VendorMaster.InsertVendorMas(VendorMaster);
        }
        [HttpPost("UpdateVendorMas/{ID}")]
        public dynamic UpdateVendorMas([FromBody] VendorMasterViewModel vendorMaster, int ID)
        {
            return _repoWrapper.VendorMaster.UpdateVendorMas(vendorMaster, ID);
        }
        [HttpPost("DeleteVendorMas/{ID}")]
        public dynamic DeleteVendorMas([FromBody] VendorMasterViewModel vendorMaster, int ID)
        {
            return _repoWrapper.VendorMaster.DeleteVendorMas(vendorMaster, ID);
        }


        [HttpGet("Getvendordetials/{CompanyID}")]
        public dynamic Getvendordetials(int CompanyID)
        {
            return _repoWrapper.VendorMaster.Getvendordetials(CompanyID);
        }

        [HttpGet("GetItemdetials/{CompanyID}")]
        public dynamic GetItemdetials(int CompanyID)
        {
            return _repoWrapper.VendorMaster.GetItemdetials(CompanyID);
        }
        [HttpGet("GetSelectedvendordetials/{code}/{CMPID}")]
        public dynamic GetSelectedvendordetials(int code, int CMPID)
        {
            return _repoWrapper.VendorMaster.GetSelectedvendordetials(code, CMPID);
        }
        
  [HttpGet("GetSelectedItemdetials/{code}/{CMPID}")]
        public dynamic GetSelectedItemdetials(int code, int CMPID)
        {
            return _repoWrapper.VendorMaster.GetSelectedItemdetials(code,CMPID);
        }

    }

}



