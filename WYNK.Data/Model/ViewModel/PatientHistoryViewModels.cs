using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class PatientHistoryViewModels
    {
        public ICollection<PatientHistory1> listofHistory { get; set; }
        public int USERID { get; set; }
    }


    public class PatientHistory1
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public string TotalMonths { get; set; }
        public string Actions { get; set; }
        public string UIN { get; set; }
    }





}
