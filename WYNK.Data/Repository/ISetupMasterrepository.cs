using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ISetupMasterrepository : IRepositoryBase<SetupMasterViewModel>
    {
        dynamic InsertSetupdata(SetupMasterViewModel Con);
        dynamic UpdateSetupdata(SetupMasterViewModel Con);

        dynamic Getcountrycurrency(string countryvalue);
        dynamic GetcountryTimesoffset(string countryvalue);
        dynamic GetcountryTimesoffsetwitjstring(string countryvalue);
        dynamic getsetupdata(string Cmpid);
        dynamic uploadImag(IFormFile file, string CompanyID);



    }

}


