using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Vendor_Masters
    {
        [Key]
        public int ID { get; set; }
        public int CmpID { get; set; }
        public int VendorCode { get; set; }
        public int VendorCategory { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? Location { get; set; }
        public string DLNo { get; set; }
        public string DLNo1 { get; set; }
        public DateTime? DLNoDate { get; set; }
        public DateTime? DLNoDate1 { get; set; }
        public string Landmark { get; set; }
        public string FSSAINo { get; set; }
        public string GSTNo { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string ContactPerson { get; set; }
        public Int16? CreditDays { get; set; }
        public decimal? Creditlimit { get; set; }
        public Int16? Leadtime { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

    }
}
