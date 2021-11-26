using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class RoomMasterViewM
    {
        public Room RoomMaster { get; set; }
        public RoomDetails RoomDetails { get; set; }
        public ICollection<getRoomDet> getRoomDet { get; set; }
        public ICollection<RoomDetails1> RoomDetails1 { get; set; }
        public ICollection<RoomDetails2> RoomDetails2 { get; set; }
        public ICollection<RoomTariff> RoomTariff { get; set; }

        public ICollection<ToiletType> ToiletType { get; set; }
        
    }
    public class RoomTariff
    {
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public decimal Roomcost { get; set; }
        
        public string PAddress { get; set; }
        public string PAddress2 { get; set; }
        public string PAddress3 { get; set; }
        public string Pphone { get; set; }
        public string Pweb { get; set; }
        public string PCompnayname { get; set; }
    }
    public class getRoomDet
    {
        public int No { get; set; }
        public object ToiletType { get; set; }
        public string BedNo { get; set; }

    }
    public class ToiletType
    {
        public int RoomNo { get; set; }
        public string RestRoomType { get; set; }

    }
    public class RoomDetails1
    {

        public int No { get; set; }
        public string ToiletType { get; set; }
        public string BedNo { get; set; }
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public decimal Roomcost { get; set; }
        public string IsActive { get; set; }
        public int RoomDetID { get; set; }

    }
    public class RoomDetails2
    {

        public int No { get; set; }
        public string ToiletType { get; set; }
        public string BedNo { get; set; }
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public decimal Roomcost { get; set; }
        public string IsActive { get; set; }
        public int RoomDetID { get; set; }

    }

}
