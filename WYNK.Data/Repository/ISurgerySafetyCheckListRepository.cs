using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;


namespace WYNK.Data.Repository
{
    public interface ISurgerySafetyCheckListRepository : IRepositoryBase<SurgeryCheckListViewModel>
    {
        dynamic SubmitSurgerySafetyChecklist(SurgeryCheckListViewModel SubmitSurgerySafetyChecklist);
        dynamic GetSurgerySafetyChecklist(int CMPID);
        dynamic DeleteSSCdetail(int CMPID, int id);
        dynamic UpdateSScDetails(SurgeryCheckListViewModel SubmitSurgerySafetyChecklist, int id);
        dynamic PreviousSSCGroupdesc(int cMPID, int value);
    }
}
