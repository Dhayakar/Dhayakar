using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
   public class OTIndentModel
    {
        [Key]
        public int ID { get; set; }
        public int OTID { get; set; }
        public Boolean IsCancelled { get; set; }
        public DateTime OTIndentDate { get; set; }
        public string IndentRaisedBy { get; set; }
        public int TCID { get; set; }

        public int StoreID { get; set; }

        public string IndentRunningNumber { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
