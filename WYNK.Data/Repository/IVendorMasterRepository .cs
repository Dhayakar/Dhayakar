using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IVendorMasterRepository : IRepositoryBase<VendorMasterViewModel>

    {
        dynamic InsertVendorMas(VendorMasterViewModel VendorMaster);
        dynamic InsertVendorData(VendorMasterViewModel Insertvendordata);
        dynamic InsertItemData(VendorMasterViewModel InsertItemdata);
        VendorMasterViewModel GetlocationDetails(int id);

        dynamic UpdateVendorMas(VendorMasterViewModel VendorMaster, int ID);

        dynamic DeleteVendorMas(VendorMasterViewModel VendorMaster, int ID);

        dynamic Getvendordetials(int CompanyID);
        dynamic GetItemdetials(int CompanyID);
        dynamic GetSelectedvendordetials(int code, int CMPID);
        dynamic GetSelectedItemdetials(int code, int CMPID);


    }

}


