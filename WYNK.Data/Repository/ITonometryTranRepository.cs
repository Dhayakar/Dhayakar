using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ITonometryTranRepository : IRepositoryBase<TonometryTranViewmodel>
    {
        dynamic InsertTonoTrans(TonometryTranViewmodel Addtono);
        dynamic Gettonometry(string UIN, int cmpid);
        dynamic updateTonoTrans(TonometryTranViewmodel uptono, string uin);
        dynamic GetHistoryiopDetails(string UIN, int cmpid);
    }
}
