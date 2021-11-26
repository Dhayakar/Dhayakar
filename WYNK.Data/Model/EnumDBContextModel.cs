using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model
{
    public class EnumDBContext : DbContext
    {
        public EnumDBContext(DbContextOptions<EnumDBContext> options) : base(options) { }

        public IEnumerable<Gender> Genders { get; set; }


    }
}
