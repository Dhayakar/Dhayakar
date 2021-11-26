using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Employee_master
    {

        public Employee_Master employeeMaster { get; set; }
        public EmployeeStatutory EmployeeStatutory { get; set; }
        public EmployeeBank EmployeeBank { get; set; }
        public User_Role usersvsrole { get; set; }
        public string Employeerole { get; set; }
        public EmployeeCommunication EmployeeCommunication { get; set; }
        public ICollection<EmployeeExperience> EmployeeExperience { get; set; }
        public QualificationExt QualificationExt { get; set; }
        public ICollection<qualificationexten> qualificationexten { get; set; }
        public ICollection<qualificationexten1> qualificationexten1 { get; set; }
        public ICollection<empqua> empqua { get; set; }
        public EmployeeQualification EmployeeQualification { get; set; }
        public ICollection<empqua1> empqua1 { get; set; }
        public string ParentDescriptionstatee { get; set; }
        public string ParentDescriptioncountryy { get; set; }

    }

    public class qualificationexten
    {
        public string ID { get; set; }
        public string QualExtDescription { get; set; }
        public int QID { get; set; }
    }

    public class qualificationexten1
    {
        public string QualExtAnsDescription { get; set; }
        public int QID { get; set; }
    }

    public class empqua
    {
        public string qualification { get; set; }
        public string Degree { get; set; }
        public string specialization { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string university { get; set; }
        public string instition { get; set; }
        public string yearofpassing { get; set; }
        public decimal percentageofmarks { get; set; }
        public int qid { get; set; }
        public int uid { get; set; }
    }

    public class empqua1
    {
        public string qualification { get; set; }
        public string Degree { get; set; }
        public string specialization { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string university { get; set; }
        public string instition { get; set; }
        public string yearofpassing { get; set; }
        public decimal percentageofmarks { get; set; }
        public int qid { get; set; }
        public int uid { get; set; }
        public int createby { get; set; }
    }

}

