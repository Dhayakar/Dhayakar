using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class Storemasters
    {
        [Key]
        public int StoreID { get; set; }
        public int CmpID { get; set; }
        public string Storename { get; set; }
        public string StoreLocation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreKeeper { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsDelete { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int? CatgType { get; set; }
        
    }
}



