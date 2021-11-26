using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OperationTheatreBooking
    {

        [Key]
        public int OTBID { get; set;}
        public int CMPID { get; set; }
        public string UIN { get; set; }
        public int OPID { get; set; }
        public DateTime OTRequiredDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int OTCompleted { get; set; }
        public string SurgeryType { get; set; }
        public int Status { get; set; }
        public string OTBookedBy { get; set; }
        public DateTime OTBookedOn { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
