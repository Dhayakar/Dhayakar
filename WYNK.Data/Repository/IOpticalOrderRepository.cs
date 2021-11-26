using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpticalOrderRepository : IRepositoryBase<OpticalOrderView>
    {


        dynamic GetvenderDetails(int VendorName,int CMPID);
        OpticalOrderView GetlocationDetails(int CityID);
        dynamic GetOpticalDetails(int lensTranID,int CmpID);
        dynamic InsertOpticalOrder(OpticalOrderView AddOptical, int Cmpid, int TransactionTypeid);
        dynamic OpticalUpdateDetails(int Cmpid);
        dynamic OpticalbindingDetails(int Cmpid,string OOID);
        dynamic UpdateOpticalOrder(OpticalOrderView OpticalUpdate, int Cmpid, int TransactionTypeid, int OpticalOrderID);
        dynamic DeleteOpticalOrder(OpticalOrderView OpticalUpdate, int Cmpid, int TransactionTypeid, int OpticaltrnID);
        OpticalOrderView getpayment(string OrderNumber, int Cmpid);
       
    }
}
