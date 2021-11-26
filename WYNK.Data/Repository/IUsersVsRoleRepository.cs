using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IUsersVsRoleRepository : IRepositoryBase<UserVsRoleViewModel>
    {
        UserVsRoleViewModel GetUsersDetails(int CmpID,int RoleID);

        dynamic InsertUserVsRole(UserVsRoleViewModel UserVsRoleInsert, int RoleID, int userroleID,int CmpID,string Dtag);


    }
}

