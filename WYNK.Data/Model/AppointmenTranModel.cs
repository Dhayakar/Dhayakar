using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class Appointmenttran
    {
        [Key]
        public int AppointmentTranID { get; set; }
        public string AppointmentMasterID { get; set; }
        public int CMPID { get; set; }
        public int? Doctorid { get; set; }
        public string Contraid { get; set; }
        public DateTime? AppointmentDateandTime { get; set; }
        public DateTime? Appointmentconfirmationsenttodoctor { get; set; }
        public DateTime? iscancelleddateandtime { get; set; }
        public string Isstatus { get; set; }

        public string AppointmentTime { get; set; }

        
        public string iscancelledreason { get; set; }

        public string Cancelledby { get; set; }

        public string AppointmentBookedby { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
