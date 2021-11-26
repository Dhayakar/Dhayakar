using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WYNK.Data.Model
{
    public class Room
    {
        [Key]
        public int ID { get; set; }
        public int CMPID { get; set; }
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public decimal RoomCost { get; set; }
        public int NoofBed { get; set; }
        public int NoofRooms { get; set; }
        public DateTime CreatedUTC { get; set; }
        public DateTime? UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }

    }
}
