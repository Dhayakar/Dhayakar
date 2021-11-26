using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
   public interface ITransactionTypeRepository : IRepositoryBase<TransactionTypeViewM>
    {
        TransactionTypeViewM getContraTranDet();
        dynamic insertdata(TransactionTypeViewM TransactionType);
        dynamic updatedata(TransactionTypeViewM TransactionType, int ID);

    }
}
