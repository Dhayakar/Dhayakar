using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class UserLogViewM
    {
        public ICollection<getUserLogDetails> getUserLogDetails { get; set; }
        public ICollection<getspecUserLogDetails> getspecUserLogDetails { get; set; }
        public ICollection<getTdyCompanyNames> getTdyCompanyNames { get; set; }
        public ICollection<getspecCompanyNames> getspecCompanyNames { get; set; }
        public ICollection<getTdyUserNames> getTdyUserNames { get; set; }
        public ICollection<TdyCompDet> TdyCompDet { get; set; }
        public ICollection<TdyUsernameDet> TdyUsernameDet { get; set; }
        public ICollection<TdyLoggedinDet> TdyLoggedinDet { get; set; }
        public ICollection<TdyLoggedOutDet> TdyLoggedOutDet { get; set; }
        public ICollection<specCompanyDetails> specCompanyDetails { get; set; }
        public ICollection<getspecUserNames> getspecUserNames { get; set; }
        public ICollection<specUserDetails> specUserDetails { get; set; }
        public ICollection<specLogDetails> specLogDetails { get; set; }
    }



    public class getUserLogDetails
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }

    public class getspecUserLogDetails
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime? LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }

    public class getTdyCompanyNames
    {
        public int T_UserLogID { get; set; }
        public string T_CompanyName { get; set; }
    }
     public class getspecCompanyNames
    {
        public int S_UserLogID { get; set; }
        public string S_CompanyName { get; set; }
    }
      public class getspecUserNames
    {
        public int S_UserLogID { get; set; }
        public string S_UserName { get; set; }
    }

    public class getTdyUserNames
    {
        public int T_UserLogID { get; set; }
        public string T_UserNames{ get; set; }
    }
     public class TdyCompDet
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }
      public class TdyUsernameDet
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }



    public class TdyLoggedinDet
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }

     public class TdyLoggedOutDet
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }
     public class specCompanyDetails
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }
     public class specUserDetails
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }
      public class specLogDetails
    {

        public int UserLogID { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public DateTime LoggedInDateTime { get; set; }
        public DateTime?  LoggedOutDateTime { get; set; }
        public Boolean Status { get; set; }
    }




}
