using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class ConfigureViewModel
    {
        public Configure Cons { get; set; }
        public ICollection<ConfigureDetails> ConfigureDetail { get; set; }
        public ICollection<ConfigureDetails1> ConfigureDetails1 { get; set; }
        public ICollection<getConfigureationTrans> getConfigureationTrans { get; set; }
        public ICollection<ConfigureTransDetails> ConfigureTrans { get; set; }
        public ICollection<ConfigureTrans> ConfigureTranss { get; set; }

    }

    //public class ConfigureDetails
    //{

    //    public int id { get; set; }
    //    public string RRDescription { get; set; }
    //    public Int16 RRAdvdays { get; set; }
    //    public Int16 FrequencyperDay { get; set; }
    //    public string HostEmailID { get; set; }
    //    public string HostPassword { get; set; }
    //    public string Phonenumber { get; set; }
    //    public Boolean SendSMS { get; set; }
    //    public Boolean SendEmail { get; set; }
    //    public string Frequencytime { get; set; }

    //}

    public class ConfigureTransDetails
    {
        public int ID { get; set; }
        public string Frequencytime { get; set; }


    }

    public class ConfigureDetails1
    {
        public int id { get; set; }
        public string RRDescription { get; set; }
        public Int16 RRAdvdays { get; set; }
        public Int16 FrequencyperDay { get; set; }
        public string HostEmailID { get; set; }
        public string HostPassword { get; set; }
        public string Phonenumber { get; set; }
        public Boolean SendSMS { get; set; }
        public Boolean SendEmail { get; set; }
        public string Frequencytime { get; set; }

    }
    public class getConfigureationTrans
    {
        public int id { get; set; }
        public string RRDescription { get; set; }
        public Int16 RRAdvdays { get; set; }
        public Int16 FrequencyperDay { get; set; }
        public string Frequencytime { get; set; }
        public bool? NotifyDoctor_SMS { get; set; }
        public bool? NotifyDoctor_Mail { get; set; }
        public bool? NotifyPatient_SMS { get; set; }
        public bool? NotifyPatient_Mail { get; set; }

        public bool? NotifyPatient_Whatsapp { get; set; }
        public bool? NotifyDoctor_Whatsapp { get; set; }
    }

}



