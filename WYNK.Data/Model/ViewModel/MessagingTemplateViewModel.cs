using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class MessagingTemplate
    {
        public string MsgTemplateName { get; set; }
        public string MsgTemplateSMSDESC { get; set; }
        public string MsgTemplateMAILDESCR { get; set; }
        public string MsgTemplateSUBJECT { get; set; }
        public string MsgTemplateWHATSAPPDESCRIP { get; set; }
        public string MsgTemplateCreatedby { get; set; }

        public string MsgTemplateCMPID { get; set; }

        public string TranslatedCode { get; set; }

        public string GETDET { get; set; }
        public string FULLNAME { get; set; }
        public string DOB { get; set; }
        public string GENDER { get; set; }
        public string   Address { get; set; }
        public string PRofioleimage { get; set; }

        public string DNO { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string dist { get; set; }
        public string landmark { get; set; }
        public string LOCATION { get; set; }
        public string COUNYTRY { get; set; }
        public string status { get; set; }

        public ICollection<GetColumns> GetColumns { get;set;}

        public ICollection<Getpdetails> Getpdetailss { get; set; }


        public ICollection<PatientChecklistDetails> PatientChecklistDetailssssssss { get; set; }
        public ICollection<PatientChecklistDetailsmal> PatientChecklistDetailssssssssmail { get; set; }
        public ICollection<PatientChecklistDetailswhatsap> PatientChecklistDetailsssssssswhatsapp { get; set; }
        public ICollection<PatientchecklistChecklistDetails> ParientChecklistdetailsss { get; set; }
        public ICollection<languageGetColumns> languageGetColumns { get; set; }

    }


    public class languageGetColumns
    {
        public string err { get; set; }
        public string result { get; set; }
        public string cacheUse { get; set; }
        public string source { get; set; }
        public string from { get; set; }
        public string sourceTransliteration { get; set; }
        public string targetTransliteration { get; set; }
    }


    public class GetColumns
    {
        public string ColumnDescription { get; set; }
        public int ID { get; set; }
        public string mailColumnDescription { get; set; }
        public string whatsappColumnDescription { get; set; }
        public Boolean selected { get; set; }
    }


    public class Getpdetails
    {
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public string phonenumber { get; set; }
        public string emails { get; set; }
        public string UIN { get; set; }
        public Boolean selected { get; set; }

        


    }



    public class PatientchecklistChecklistDetails
    {
        public string phonenumber { get; set; }
        public string UIN { get; set; }

        public string email { get; set; }
        public Boolean Select { get; set; }

    }

    public class PatientChecklistDetailsmal
    {
        public string ItemDescription { get; set; }

    }
    public class PatientChecklistDetails
    {
        public string ItemDescription { get; set; }

    }
    public class PatientChecklistDetailswhatsap
    {
        public string ItemDescription { get; set; }

    }

}
