using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class OpticalBalance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int LTID { get; set; }
        public int OpeningBalance { get; set; }
        public int REC01 { get; set; }
        public int REC02 { get; set; }
        public int REC03 { get; set; }
        public int REC04 { get; set; }
        public int REC05 { get; set; }
        public int REC06 { get; set; }
        public int REC07 { get; set; }
        public int REC08 { get; set; }
        public int REC09 { get; set; }
        public int REC10 { get; set; }
        public int REC11 { get; set; }
        public int REC12 { get; set; }
        public int ISS01 { get; set; }
        public int ISS02 { get; set; }
        public int ISS03 { get; set; }
        public int ISS04 { get; set; }
        public int ISS05 { get; set; }
        public int ISS06 { get; set; }
        public int ISS07 { get; set; }
        public int ISS08 { get; set; }
        public int ISS09 { get; set; }
        public int ISS10 { get; set; }
        public int ISS11 { get; set; }
        public int ISS12 { get; set; }
        public int ClosingBalance { get; set; }
        public int FYID { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int CmpID { get; set; }
        public int StoreID { get; set; }
        public int UOMID { get; set; }
    }
}
