using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class AppointmentView
    {

        public string cmpid { get; set; }
        public string Message { get; set; }
        public string DoctorMessage { get; set; }
        public string stattus { get; set; }
        public string NumberMessage { get; set; }
        public string DoctorConsultingMessage { get; set; }

        public string DoctorlimitConsultingMessage { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string uin { get; set; }
        public string lanme { get; set; }
        public DateTime dateofbirth { get; set; }
        public int Cretedby { get; set; }

        public DateTime Appointmentrequesteddate { get; set; }
        public string Appointmentrequestedtime { get; set; }
        public string bookedby { get; set; }

        public string Age { get; set; }
        public string gender { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string phonenumber { get; set; }
        public decimal? fees { get; set; }
        public string Apptreasons { get; set; }
        public string location { get; set; }
        public string doctorid { get; set; }
        public string appointmentresons { get; set; }

        public ICollection<Appointmenttran> appointmentrans { get; set; }
        public ICollection<Appointmenttran> appointmentranssubtract { get; set; }
        public string HH { get; set; }
        public string MM { get; set; }

        public ICollection<APpointmentpatientdetails> APpointmentpatientdetails { get; set; }
        public TimeSpan firstfromTime { get; set; }
        public TimeSpan firsttoTime { get; set; }
        public TimeSpan secondfromTime { get; set; }
        public TimeSpan sectoTime { get; set; }
        public ICollection<APpointmentListpatientdetails> APpointmentListpatientdetails { get; set; }

    }
    public class APpointmentListpatientdetails
    {
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string backgroundColor { get; set; }
        public string textColor { get; set; }
        public string borderColor { get; set; }
        public Boolean allDay { get; set; }
        public string RUID { get; set; }
        public DateTime? Apptdate { get; set; }
        public string Appttime { get; set; }
        

    }

    public class APpointmentpatientdetails
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }

}
