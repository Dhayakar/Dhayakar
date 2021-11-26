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
    public class StockController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public StockController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;

        }

        [HttpGet("Search/{Date}/{CompanyID}")]
        public StockViewmodel Search(DateTime Date, int CompanyID)
        {
            return _repoWrapper.Stock.Search(Date, CompanyID);
        }
        [HttpGet("GetStockDetails2/{BrandName}/{Date}")]
        public StockViewmodel GetStockDetails2(string BrandName, DateTime Date)
        {
            return _repoWrapper.Stock.GetStockDetails2(BrandName, Date);
        }
        [HttpGet("GetStockDetails3/{BrandName}/{Date}")]
        public StockViewmodel GetStockDetails3(string BrandName, DateTime Date)
        {
            return _repoWrapper.Stock.GetStockDetails3(BrandName, Date);
        }
        [HttpGet("GetStockDetails4/{BrandName}/{Date}")]
        public StockViewmodel GetStockDetails4(string BrandName, DateTime Date)
        {
            return _repoWrapper.Stock.GetStockDetails4(BrandName, Date);
        }


    }
}





