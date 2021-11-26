using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class User
    {
        public User_Master userMaster { get; set; }
        public iNTERNALUserDETAILS Inetrnaluserdetails { get; set; }
        public ExternalUserDetails ExternalUserDetails { get; set; }
        public ICollection<Role_tran> RoleTran { get; set; }
        public string Check { get; set; }
    }

    public class iNTERNALUserDETAILS
    {
        public string Baserole { get; set; }
        public string Userrole { get; set; }
        public string password { get; set; }
        public string Confirmpassword { get; set; }
        public string companyid { get; set; }
        public string AccessRole { get; set; }
        public string Userid { get; set; }
        

    }



    public class ExternalUserDetails
    {
        public string Emaildid { get; set; }
        public string Userrole { get; set; }
        public string password { get; set; }
        public string companyid { get; set; }
    }

}

