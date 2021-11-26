using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IPatientRepository : IRepositoryBase<PatientViewModel>
    {
        dynamic GetDetails(DateTime FromDate, DateTime ToDate, int CMPID);
    }
}
