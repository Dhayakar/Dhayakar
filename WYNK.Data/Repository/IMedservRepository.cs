using System;

using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IMedservRepository : IRepositoryBase<Medservviewmodel>
    {
        Medservviewmodel getDetails(DateTime FromDate, DateTime Todate,int companyid);
        Medservviewmodel getDetailsIn(DateTime FromDate, DateTime Todate,string service);

    }
}
