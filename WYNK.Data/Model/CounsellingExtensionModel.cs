using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    public class CounsellingExtensionModel
    {
        [Key]

        public int ID { get; set; }
        public int CCID { get; set; }
        public int CID { get; set; }
        public DateTime CheckListDoneOn { get; set; }
        public int CheckedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        
    }

}
