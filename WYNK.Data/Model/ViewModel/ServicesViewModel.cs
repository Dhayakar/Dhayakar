using System.Collections.Generic;

namespace WYNK.Data.Model.ViewModel
{
    public class ServicesViewModel
    {
        public int cmpid { get; set; }
        public int ROLEID { get; set; }
        public int Insuranceid { get; set; }
        public int Parentid { get; set; }
        public string Stag { get; set; }
        public ICollection<ServicesGridData> ServicesGridData { get; set; }
        public ICollection<ServicesGridDataExcel> ServicesGridDataExcel { get; set; }
        public ICollection<serviceDropdown> serviceDropdown { get; set; }
        public ICollection<serviceDropdown> Subservice { get; set; }
        public ICollection<Servicesupload> Servicesupload { get; set; }
        public ICollection<SchDelete> SchDelete { get; set; }

        ////////////////////////////RoleVsService///////////////////////////
        public ICollection<ServicesMap> ServicesMap { get; set; }
        public ICollection<GetRoleVsServices> GetRoleVsServices { get; set; }
        public ICollection<GetRoleVsServiceschecked> GetRoleVsServiceschecked { get; set; }
    }
    public class GetRoleVsServiceschecked
    {
        public int ServiceID { get; set; }
        public int RoleID { get; set; }
        public bool checkeds { get; set; }
    }
    public class GetRoleVsServices
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public string ServicesName { get; set; }
        
        public int RoleID { get; set; }
        public bool checkeds { get; set; }

    }
    public class ServicesMap
    {
        public int ServiceID { get; set; }
        public int RoleID { get; set; }
        public bool checkeds { get; set; }

    }
    public class SchDelete
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }

    }
    public class serviceDropdown
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string ParentID { get; set; }
        public string STag { get; set; }
        
    }
    public class Servicesupload
    {
        public string Childdesc { get; set; }
        public string VisitType { get; set; }
        public string insurance { get; set; }
        public decimal serviceamt { get; set; }
        public decimal eligibleamt { get; set; }
        public decimal netamount { get; set; }
    }
    public class ServicesGridData
    {
        public string Parentdesc { get; set; }
        public string Childdesc { get; set; }
        public int VisitType { get; set; }
        public int ID { get; set; }
        public string insurance { get; set; }
        public string insuranceid { get; set; }
        public string Icdcode { get; set; }
        public string IcdSpeciality { get; set; }
        public int IcdSpecialityCode { get; set; }
        
        public string roomid { get; set; }
        public string docid { get; set; }
        public string childid { get; set; }
        public string parentid { get; set; }
        public string room { get; set; }
        public decimal serviceamt { get; set; }
        public string doctor { get; set; }
        public string percentage { get; set; }
        public decimal eligibleamt { get; set; }
        public decimal? netamount { get; set; }

}
    public class ServicesGridDataExcel
    {
        public string Parentdesc { get; set; }
        public string Childdesc { get; set; }
        public int VisitType { get; set; }
        public int ID { get; set; }
        public string insurance { get; set; }
        public string insuranceid { get; set; }
        public string Icdcode { get; set; }
        public string IcdSpeciality { get; set; }
        public int IcdSpecialityCode { get; set; }
        public string Status { get; set; }
        public string roomid { get; set; }
        public string docid { get; set; }
        public string childid { get; set; }
        public string parentid { get; set; }
        public string room { get; set; }
        public decimal serviceamt { get; set; }
        public string doctor { get; set; }
        public string percentage { get; set; }
        public decimal eligibleamt { get; set; }
        public decimal? netamount { get; set; }

    }
}
