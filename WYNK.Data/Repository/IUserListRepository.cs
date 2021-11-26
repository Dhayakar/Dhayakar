using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IUserListRepository : IRepositoryBase<UsersListView>
    {
        UsersListView GetUsersList(int CompanyID);
        dynamic UpdateStatus(UsersListView UserList, int UID);

    }
}
