using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace WYNK.Data.Model
{
  public class OTIndenttranModel
    {
        public int ID { get; set; }
        public int OTIndentID { get; set; }
        public int DrugID { get; set; }
        public int UOMID { get; set; }
        public int IndentQty { get; set; }
        public int? ReceivedQty { get; set; }
        
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
