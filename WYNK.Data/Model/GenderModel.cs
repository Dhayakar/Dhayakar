using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model
{
   public enum Gender
    {

        Male = 1,
        Female = 2,
        Transgender = 3

    }

    public partial class Genders
    {
        public int Id { get; set; }
        public int State { get; set; }
    }
}

