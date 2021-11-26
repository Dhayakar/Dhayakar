using System;

using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface IErrorlogRepository : IRepositoryBase<ErrorLogviewmodel>
    {
        ErrorLogviewmodel Geterrorlogfile(DateTime FromDate, DateTime Todate, string Time);
        dynamic gettotallines();
        dynamic gettotalreleases();
        dynamic gettotalreleasesbasedondate(string textname);
    }
}
