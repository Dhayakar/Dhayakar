using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class GlaucomaEvaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public int RID { get; set; }
        public DateTime Date { get; set; }
        public string REGAT { get; set; }
        public string LEGAT { get; set; }
        public string Time { get; set; }
        public string REoptic { get; set; }
        public string LEoptic { get; set; }
        public string GlaucomaDrugs { get; set; }
        public string HVF { get; set; }
        public string OCT { get; set; }
        public string Intervention { get; set; }

        public string StableProgression { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public Boolean IsActive { get; set; }

    }
}


