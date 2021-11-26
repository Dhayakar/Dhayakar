using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class SetupmasterClass
    {
        [Key]
        public int SetupID { get; set; }
        public int CMPID { get; set; }
        public string Pediatric { get; set; }
        public string Country { get; set; }
        public string RoomCutOffTime { get; set; }
        public string Symbol { get; set; }
        public string LogoPath { get; set; }
        public string UTCTime { get; set; }
        public string Language { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? GapInterval { get; set; }
        public int? UpdatedBy { get; set; }

        public TimeSpan FSFromTime { get; set; }
        public TimeSpan FSToTime { get; set; }
        public TimeSpan SSFromTime { get; set; }
        public TimeSpan SSToTime { get; set; }
        public bool? Registrationfeeapplicable { get; set; }
        public bool? Insuranceapplicable { get; set; }
        public string BillingType { get; set; }
        public bool? IsNotification { get; set; }
        public bool? RegistrationDateManage { get; set; }
        public string Procurement { get; set; }

        public string Timezone { get; set; }
        public string BufferTime { get; set; }
        public string CheckoutTime { get; set; }
    }
}
