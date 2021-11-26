using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace WYNK.Data.Model
{
    public class LogUser
    {
        [Key]
        public int LoguserID { get; set; }
        public int CmpID { get; set; }
        public int UserID { get; set; }
        public string LoginUsername { get; set; }
        public DateTime LoginDatetime { get; set; }
        public DateTime? LogoutDatetime { get; set; }
        public bool IsLogged { get; set; }
    }
}
