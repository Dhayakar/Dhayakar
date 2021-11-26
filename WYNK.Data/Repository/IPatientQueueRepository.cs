
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IPatientQueueRepository : IRepositoryBase<PatientQueueViewModel>
    {
        PatientQueueViewModel GetQueueDate(int CompanyID);
    }
}


