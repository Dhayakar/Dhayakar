using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WYNK.Data.Model
{
    public class RoomOccupiedstatus
    {

        [Key]
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int RoomDetailsID { get; set; }
        public string UIN { get; set; }
        public string BedNo { get; set; }
        public int? Status { get; set; }
        public DateTime? RoomOccupationFromDate { get; set; }
        public DateTime? RoomOccupationToDate { get; set; }
        public string RoomOccupationFromTime { get; set; }
        public string RoomOccupationToTime { get; set; }
        public DateTime? VacatedOn { get; set; }
        public int? NoOfDays { get; set; }
        public string VacantType { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public Boolean IsOccupied { get; set; }
        public string AdmID { get; set; }
        public int CmpID { get; set; }
    }
}
