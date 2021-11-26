using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ITonometryRepository : IRepositoryBase<TonomentryViewmodel>
    {

        dynamic Inserttonometry(TonomentryViewmodel Addtonometry);
        dynamic Fulltonometrylist();
        dynamic updatetonometry(TonomentryViewmodel Uptonometry, int ID);

    }
}
