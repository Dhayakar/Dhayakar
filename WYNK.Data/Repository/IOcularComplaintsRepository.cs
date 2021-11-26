using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IOcularComplaintsRepository : IRepositoryBase<OcularComplaintsViewModel>
    {
        dynamic InsertOcularComplaints(OcularComplaintsViewModel InsertOcularComplaints);
        dynamic updateOcularComplaints(OcularComplaintsViewModel InsertOcularComplaints, int ID);
        dynamic InsertnewOcularComplaints(OcularComplaintsViewModel InsertnewOcularComplaints, string UIN);
        dynamic Deleteocularcomplaints(int ID, string Reasons);
        dynamic DeletenewOcularComplaints(string UIN, string Description, string Reasons, int CMPID);
        dynamic Deletenewsystemicconditions(string UIN, string Description, string Reasons, int CMPID);
        dynamic GetDeletednewOcularComplaints(string UIN, int CMPID);
        dynamic GetDeletednewsystemic(string UIN, int CMPID);
        dynamic InsertOcularMaster(OcularComplaintsViewModel AddOculardata);
        dynamic Insertsystemconditions(OcularComplaintsViewModel Addsystemicdata);
        dynamic UpdateOcularMaster(OcularComplaintsViewModel updateOculardata,int ID);
        dynamic UpdatesystemicMaster(OcularComplaintsViewModel updatesystemicdata, int ID);
        dynamic DeleteOcular(int ID);
        dynamic Deletesystemic(int ID);
        
    }
}
