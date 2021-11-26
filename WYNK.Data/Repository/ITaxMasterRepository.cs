using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
namespace WYNK.Data.Repository
{
    public interface ITaxMasterRepository : IRepositoryBase<TaxMasterViewM>
    {
        dynamic insertTaxMaster(TaxMasterViewM taxMaster, int CMPID);
        dynamic updateTaxMaster(TaxMasterViewM taxMaster, int ID, int CMPID);

    }
}
