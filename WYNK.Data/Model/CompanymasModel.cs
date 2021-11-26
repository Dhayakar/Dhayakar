using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class Companymas
    {
        [Key]


        public int CmpID { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public string GSTNo { get; set; }
        public string ContactPerson { get; set; }
        public int ParentID { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailID { get; set; }
        public string Website { get; set; }
        public bool IsDeleted { get; set; }
        public string URLPATH { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

    }
}