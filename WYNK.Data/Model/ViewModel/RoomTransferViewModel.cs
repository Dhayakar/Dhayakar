using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
   public class RoomTransferDataView
    {
        public ICollection<CurrentRoomInfo> CurrentRoomInfo { get; set; }
        public ICollection<AvailableRoomInfo> AvailableRoomInfo { get; set; }
        public ICollection<RoomOccupiedArray> RoomOccupiedArray { get; set; }
    }
    public class CurrentRoomInfo
    {
        public int RoomDetailsID { get; set; }
        public int RoomNo { get; set; }
        public string BedNo { get; set; }
        public string RestRoomType { get; set; }
        public string RoomType { get; set; }
        public string Status { get; set; }
        public DateTime? FromRoomDate { get; set; }
        public string FromRoomTime { get; set; }
        public int? NoOfDays { get; set; }
        public DateTime? ch { get; set; }
    }

    public class AvailableRoomInfo
    {
       
        public int RoomID { get; set; }
        public int RoomDetailsID { get; set; }
        public int RoomNo { get; set; }
        public string BedNo { get; set; }
        public string RestRoomType { get; set; }
        public string RoomType { get; set; }
        public string Status { get; set; }
        public Boolean room { get; set; }
    }

    public class RoomOccupiedArray
    {
        public int RoomID { get; set; }
        public int RoomDetailsID { get; set; }
        public string UIN { get; set; }
        public string BedNo { get; set; }
        public string Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public Boolean IsOccupied { get; set; }
        public DateTime CreatedUTC { get; set; }
        public Boolean UpdatedUTC { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
