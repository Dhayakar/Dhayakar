using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IPurchaseordercancellationRepository : IRepositoryBase<Purchaseordercancellation>
    {
        dynamic purchaseordercancel(int id, int TransactionId, string Time);
        dynamic purchaseordercancellation(int id, int TransactionId, string Time);
        dynamic Getpurchasecal(string RandomUniqueID, string Time);
        dynamic Getpurchaseprintt(string RandomUniqueID, int id, string Time);
        dynamic Insertpurchaseordercancel(Purchaseordercancellation cancelpurchase, int cmpid, int TransactionTypeid, string Time);

    }
}

