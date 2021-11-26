using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WYNK.Data.Repository
{
    public interface IUomMasterRepository : IRepositoryBase<UomMasterViewModel>
    {

        dynamic InsertUom(string UOMdep, int userID);
        dynamic UpdateUom(int UOMID,string UOMdep, int userID);

        dynamic DeleteUom(int UOMID);
        UomMasterViewModel GetUomDetails();
    }
}
