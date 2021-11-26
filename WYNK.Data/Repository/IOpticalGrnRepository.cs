using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpticalGrnRepository : IRepositoryBase<OpticalGrnDataView>
    {

        dynamic GetGrn(int val, string Time);
        dynamic GetGrntrns(string ID);
        dynamic Insertopticalgrn(OpticalGrnDataView Addopticalgrn, int cmpid, int TransactionTypeid);
        dynamic GetGrndetails(int val, string Time);
        dynamic GetGrntrnsdetails(string RandomUniqueID);
        dynamic GetGrnloc(string val);
        dynamic GetGrnlocation(string RandomUniqueID);
        dynamic Opticalgrnprint(string RandomUniqueID, int CMPID, string Time);

    }
}
