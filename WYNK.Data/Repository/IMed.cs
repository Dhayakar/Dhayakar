using System;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{

    public interface IMed : IRepositoryBase<Medical_Prescription>
    {
        dynamic getDetails(DateTime FromDate, DateTime Todate, int companyid,string GMT);
        Medical_Prescription getDetailsbill(DateTime FromDate, DateTime Todate);
        Medical_Prescription getDetailspres(DateTime FromDate, DateTime Todate);






    }
}




