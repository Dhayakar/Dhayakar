using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;


namespace WYNK.Data.Repository.Implementation
{
    class UserListRepository : RepositoryBase<UsersListView>, IUserListRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       
        public UserListRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public UsersListView GetUsersList(int CompanyID)
        {

            var UserDtls = new UsersListView();

            var user = CMPSContext.Users.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();

            var acuser = (from US in user.Where(x => x.CMPID == CompanyID && x.Isactive == true)
                                 join UR in userrole
                                 on US.Userid equals UR.UserID


                                 select new
                                 {
                                     usid = US.Userid,
                                     username = US.Username,
                                     role = UR.RoleDescription,                                     
                                     status = "Active",
                                     

                                 }).ToList();

            var inacuser = (from US in user.Where(x => x.CMPID == CompanyID && x.Isactive == false)
                          join UR in userrole
                          on US.Userid equals UR.UserID


                          select new
                          {
                              usid= US.Userid,
                              username = US.Username,
                              role = UR.RoleDescription,
                              status = "In Active",


                          }).ToList();

            var res = acuser.Concat(inacuser);

            UserDtls.UDetails = (from r in res
                                 
                                 select new UDetails
                                 {
                                     usid = r.usid,
                                     username = r.username,
                                     role = r.role,
                                     status = r.status,


                                 }).ToList();

            return UserDtls;


        }


        public dynamic UpdateStatus(UsersListView UserList, int UID)
        {



            var users = CMPSContext.Users.Where(x => x.Userid == UID).ToList();
            if (users != null)
            {

                users.All(x => { x.Isactive = UserList.Users.Isactive;
                    x.Inactivedate = DateTime.UtcNow;
                    x.Updatedby = UserList.Users.Updatedby;
                    x.Updatedutc = DateTime.UtcNow;
                    return true; });
                CMPSContext.Users.UpdateRange(users);
            }

            var usersrole = CMPSContext.UserVsRole.Where(x => x.UserID == UID).ToList();

            if (usersrole != null && UserList.Users.Isactive == false)
            {

                usersrole.All(x => {
                    x.IsDeleted = true;
                    x.UpdatedBy = UserList.Users.Updatedby;
                    x.UpdatedUTC = DateTime.UtcNow;
                    return true;
                });
                CMPSContext.UserVsRole.UpdateRange(usersrole);
            }

            else if (usersrole != null && UserList.Users.Isactive == true)
            {

                usersrole.All(x => {
                    x.IsDeleted = false;
                    x.UpdatedBy = UserList.Users.Updatedby;
                    x.UpdatedUTC = DateTime.UtcNow;
                    return true;
                });
                CMPSContext.UserVsRole.UpdateRange(usersrole);
            }
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {

                        Success = true,
                        Message = CommonMessage.saved,
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };



        }



    }
}
