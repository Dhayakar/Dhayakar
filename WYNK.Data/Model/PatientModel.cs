using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WYNK.Data.Model
{
    class PatientModel
    {
        [Key]
        public string UIN { get; set; }
        public DateTime FromDate { get; set; }
        public string Name { get; set; }
        public DateTime ToDate { get; set; }
    }
}
