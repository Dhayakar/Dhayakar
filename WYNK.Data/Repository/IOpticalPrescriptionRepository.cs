using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOpticalPrescriptionRepository : IRepositoryBase<OpticalPrescriptionview>
    {

        dynamic GetfinalopDetails(string UIN, int id);
        dynamic GetopticalDetails(int RegID);
        dynamic Getfinopprint(int rid, int CMPID, string Time);
        dynamic GetUINDetails(int cid);
        dynamic GetHistoryDetails(string UIN, int rid);
    }
}
