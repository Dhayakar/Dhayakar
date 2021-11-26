using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class IntraOcularPressure
    {
        [Key]
        public int ID { get; set; }
        public int RegistrationTranID { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string Ocular { get; set; }
        public string Iolnct { get; set; }
        public string Iolat { get; set; }
        public string Time { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string RefTag { get; set; }

    }
}













