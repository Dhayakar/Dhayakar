using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IRoleVsAccessRespository : IRepositoryBase<Rolevsaccesscontrol>
    {

        dynamic GetAccessPrivilegesDetails(int RefID);
        dynamic GetUserDetails(string Roleuser,string CompanyID);
        dynamic GetAccessModule(int CMPID);
        dynamic GetMenuaccess(int userDoctorID, int userroleID, string CompanyID,string Description);
        dynamic GetAccessModulebasedonuser(string Role, int CMPID, string MViewaccess);
        dynamic InsertAccessPrivileges(Rolevsaccesscontrol Addaccessname);

    }
}

