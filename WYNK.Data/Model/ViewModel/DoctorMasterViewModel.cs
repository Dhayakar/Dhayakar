using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class DoctorMaster
    {


        public Doctor_Master doctorMaster { get; set; }
        public Speciality_master speciality { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public ICollection<Speciality_trans> Specialitytrans { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
        public ICollection<sptrans> Spetrans { get; set; }

        public string Doctorroleid { get; set; }
        public string Doctorname { get; set; }
        public string Docday { get; set; }
        public string FFtime { get; set; }
        public string FTTime { get; set; }
        public string SSFtime { get; set; }
        public string SSSTime { get; set; }
        public string Maxfpatient { get; set; }
        public string Maxtpatient { get; set; }
        public string CPMID { get; set; }
        public string USERID { get; set; }
        public string EveningFFtime { get; set; }
        public string EveningFTTime { get; set; }
        public string EveningSSFtime { get; set; }
        public string EveningSSSTime { get; set; }

        public ICollection<DoctorGridconsulting> DoctorGridconsulting { get; set; }

        public ICollection<Doctorbreakupconsulting> Doctorbreakupconsulting { get; set; }

        public ICollection<DeleteDoctorGridconsulting> DeleteDoctorGridconsulting { get; set; }
        public ICollection<DoctorGridconsultingnewcolumns> DoctorGridconsultingnewcolumns { get; set; }

        public Engages Engages { get; set; }
    }


    public class DoctorGridconsultingnewcolumns
    {
        public string CMPID { get; set; }

        public string USERID { get; set; }
        public string Days { get; set; }
        public string firstname { get; set; }
        public string Lastname { get; set; }

        public int DoctorID { get; set; }
        public string firstsessionstart { get; set; }
        public string firstsessionend { get; set; }
        public string Secondsessionstart { get; set; }
        public string fsecondsessionend { get; set; }
        public int? Maxpatientsmorning { get; set; }
        public int? Maxpatientecening { get; set; }
    }
    public class Doctorbreakupconsulting
    {
        public int ExtensionID { get; set; }
        public string Days { get; set; }
        public string firstname { get; set; }
        public string Lastname { get; set; }

        public int DoctorID { get; set; }
        public string firstsessionstart { get; set; }
        public string firstsessionend { get; set; }
        public string Secondsessionstart { get; set; }
        public string fsecondsessionend { get; set; }
        public int? Maxpatientsmorning { get; set; }
        public int? Maxpatientecening { get; set; }
    }

    public class DeleteDoctorGridconsulting
    {
        public string Doctorname { get; set; }
        public string DoctorID { get; set; }
        public string Docday { get; set; }
        public string EveningSSSTime { get; set; }
        public string EveningSSFtime { get; set; }
        public string EveningFTTime { get; set; }
        public string EveningFFtime { get; set; }
        public string Maxtpatient { get; set; }
        public string Maxfpatient { get; set; }
        public string SSSTime { get; set; }
        public string SSFtime { get; set; }
        public string FTTime { get; set; }
        public string FFtime { get; set; }
    }


    public class DoctorGridconsulting
    {
        public string Doctorname { get; set; }
        public string DoctorID { get; set; }
        public string Docday { get; set; }
        public string EveningSSSTime { get; set; }
        public string EveningSSFtime { get; set; }
        public string EveningFTTime { get; set; }
        public string EveningFFtime { get; set; }
        public string Maxtpatient { get; set; }
        public string Maxfpatient { get; set; }
        public string SSSTime { get; set; }
        public string SSFtime { get; set; }
        public string FTTime { get; set; }
        public string FFtime { get; set; }
    }

    public class sptrans
    {

        public int olmid { get; set; }
    }


    public class Engages 
    {

        public string EngagementType { get; set; }
        public int USERID { get; set; }

    }


}

