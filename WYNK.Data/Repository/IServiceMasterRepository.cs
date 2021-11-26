using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository
{
    public interface IServiceMasterRepository : IRepositoryBase<ServicesViewModel>
    {        
        dynamic Saveservicemasterdata(ServicesViewModel BMI);
        dynamic Deleteservicemasterdata(string Pid, string childid, string docid);
        dynamic InsertRoleVsService(ServicesViewModel AddRoleVsService, int Role, int userroleID,int RoleID);
        ServicesViewModel GetServiceDetails(int RoleName,int CMPID);
       
    }
}
