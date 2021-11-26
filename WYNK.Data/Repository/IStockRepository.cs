
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IStockRepository : IRepositoryBase<StockViewmodel>
    {
        dynamic Search(DateTime Date, int CompanyID);

        dynamic GetStockDetails2(string DrugName, DateTime Date);

        dynamic GetStockDetails3(string DrugName, DateTime Date);
        dynamic GetStockDetails4(string DrugName, DateTime Date);

    }
}


