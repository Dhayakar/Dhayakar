using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;



namespace WYNK.Data.Repository
{
    public interface IPurchaseorderDetRepository : IRepositoryBase<PurchaseOrdDetView>
    {

        dynamic getPODet(DateTime FromDate, DateTime Todate);

        dynamic getP_D_Det(string P_Po_No, DateTime P_Po_Date);
    }
}
