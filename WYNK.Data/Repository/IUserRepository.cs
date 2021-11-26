using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {


        dynamic Postinternaluserdetails(User AddEmp);
        dynamic PostExternaluserdetails(User AddEmp);
        dynamic updateUserDet(User User, int ID, Boolean IsActive, int DoctorID);
        dynamic GetRoledetails(string roletext,int CompanyID);
        dynamic GetModuletransactiondetails(string ModuleDescription, int CompanyID);
        dynamic GetModuletransactiondetailsstring(string ModuleDescription, string suffix, int CompanyID);
        dynamic Getconsulttranid(string ModuleDescription, int CompanyID);
        dynamic GetWorkflodata(string res1);

        
    }
}
