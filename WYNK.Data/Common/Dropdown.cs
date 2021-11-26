using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Common
{
    public class Dropdown
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public int Values { get; set; }
        public int? SpecialityCode { get; set; }
        public decimal? Amt { get; set; }
        public decimal? Taxx { get; set; }
        public string itemvalue { get; set; }
        public bool status { get; set; }
        public bool? Registrationfeeapplicable { get; set; }
        public bool? Insuranceapplicable { get; set; }
        public string Drugname { get; set; }
        public DateTime DateTime { get; set; }
    }
}

