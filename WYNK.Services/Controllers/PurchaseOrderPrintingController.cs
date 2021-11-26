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
    public class PurchaseOrderPrintingController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public PurchaseOrderPrintingController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


    }

}



