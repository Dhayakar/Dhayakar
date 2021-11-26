using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{

    class RoomTransferRepository : RepositoryBase<RoomTransferDataView>, IRoomTransferRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public RoomTransferRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public RoomTransferDataView RoomDetailsSearch(string UIN)
        {

            var Room = WYNKContext.Room.ToList();
            var RoomDet = WYNKContext.RoomDetails.ToList();
            var RoomOcc = WYNKContext.RoomOccupiedStatus.ToList();
            var Onle = CMPSContext.OneLineMaster.ToList();


            var RoomDetailsSearch = new RoomTransferDataView();
            RoomDetailsSearch.CurrentRoomInfo = new List<CurrentRoomInfo>();


            RoomDetailsSearch.CurrentRoomInfo = (from ROC in RoomOcc.Where(x => x.UIN == UIN && x.VacantType != "Discharged")
                                                 join RD in RoomDet.Where(/*x => x.IsRoomVacant == true &&*/x=> x.IsActive == true) on ROC.RoomDetailsID equals RD.ID
                                                 join ROO in Room on RD.RoomID equals ROO.ID
                                                 join ON in Onle on ROC.Status equals ON.OLMID
                                                 select new CurrentRoomInfo
                                                 {
                                                     RoomDetailsID = ROC.RoomDetailsID,
                                                     RoomNo = RD.RoomNumber,
                                                     BedNo = RD.BedNo,
                                                     RoomType = ROO.RoomType,
                                                     Status = ON.ParentDescription,
                                                     FromRoomDate = ROC.RoomOccupationFromDate,
                                                     FromRoomTime = ROC.RoomOccupationFromTime,
                                                     NoOfDays = NoOfDaysCalc(ROC.NoOfDays , ROC.RoomOccupationFromDate),

                                                 }).OrderByDescending(x=>x.FromRoomDate).ToList();

            return RoomDetailsSearch;
        }


        public int NoOfDaysCalc(int ? days , DateTime? FromDate)
        {
            if (days == null) {

                DateTime d1 = Convert.ToDateTime(FromDate);
                DateTime d2 = DateTime.Now;
                TimeSpan t = d1 - d2;
                double NrOfDays = t.TotalDays;
                int res = Convert.ToInt32(NrOfDays);
                int NoOfDaysIn = Math.Abs(res);
                return NoOfDaysIn;
            }
            else
            {
                return Convert.ToInt32(days);
            }
            
        }


        public RoomTransferDataView AvailableRoomSearch(string Res)
        {
            var Room = WYNKContext.Room.ToList();
            var RoomDet = WYNKContext.RoomDetails.ToList();
            var RoomOCC = WYNKContext.RoomOccupiedStatus.ToList();
            var One = CMPSContext.OneLineMaster.ToList();
            var STINT1 = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Vacant").Select(x => x.OLMID).FirstOrDefault();

            var VT= Enum.GetName(typeof(VacantType), VacantType.Transfer);

            var AvailableRoomSearch = new RoomTransferDataView();
            AvailableRoomSearch.AvailableRoomInfo = new List<AvailableRoomInfo>();

            AvailableRoomSearch.AvailableRoomInfo = (from RO in Room.Where(x => x.RoomType == Res)
                                                     join RD in RoomDet.Where(x => x.IsRoomVacant == false && x.IsActive == true) on RO.ID equals RD.RoomID
                                                     join ROCS in RoomOCC.Where(x=>x.Status== STINT1 || x.VacantType == VT||x.VacantType == null) on RD.ID equals ROCS.RoomDetailsID
                                                     join On in One on ROCS.Status equals On.OLMID

                                                     select new AvailableRoomInfo
                                                     {
                                                         RoomDetailsID = ROCS.RoomDetailsID,
                                                         RoomID = ROCS.RoomID,
                                                         RoomNo = RD.RoomNumber,
                                                         BedNo = RD.BedNo,
                                                         RoomType = RO.RoomType,
                                                         Status = On.ParentDescription,
                                                         

                                                     }).ToList();

            return AvailableRoomSearch;
        }

        public dynamic InsertRoomDetails(RoomTransferDataView Con)
        {

            var UINS = Con.RoomOccupiedArray.Select(x => x.UIN).FirstOrDefault();
            var RoomDET = WYNKContext.RoomOccupiedStatus.Where(x => x.UIN == UINS && x.RoomOccupationFromDate != null && x.RoomOccupationFromTime != null && x.VacantType == null && x.NoOfDays==null).Select(x => x.RoomDetailsID).FirstOrDefault();
            var RoomDID = Con.RoomOccupiedArray.Select(x => x.RoomDetailsID).FirstOrDefault();
            var STTS = Con.RoomOccupiedArray.Select(x => x.Status).FirstOrDefault();
            var STINT = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == STTS).Select(x => x.OLMID).FirstOrDefault();
            var STINT1 = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Booked").Select(x => x.OLMID).FirstOrDefault();
            var STINT2 = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Provisionally Booked").Select(x => x.OLMID).FirstOrDefault();
            var BedNo = Con.RoomOccupiedArray.Select(x => x.BedNo).FirstOrDefault();
            var Dates = WYNKContext.RoomOccupiedStatus.Where(x => x.UIN == UINS && x.RoomDetailsID == RoomDET).Select(x => x.RoomOccupationFromDate).FirstOrDefault();
            var frm = Con.RoomOccupiedArray.Select(x => x.FromDate).FirstOrDefault();
            
            DateTime d1 = Convert.ToDateTime(Dates);
            DateTime d2 = frm;
            TimeSpan t = d1 - d2;
            double NrOfDays = t.TotalDays;
            int res = Convert.ToInt32(NrOfDays);
            int NoOfDaysIn = Math.Abs(res);

            var date = DateTime.Now;
            var Hrs=date.Hour;
            var MIN=date.Minute;
            var STRH = Convert.ToString(Hrs);
            var COL = ":";
            var STRM = Convert.ToString(MIN);
            string str1 = STRH;
            string str2 = STRM;
            string strRes = String.Concat(str1, COL);
            string StrTotal = String.Concat(strRes, str2);


            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var RoomOccupied = new RoomOccupiedstatus();
                    RoomOccupied = WYNKContext.RoomOccupiedStatus.Where(x => x.UIN == UINS && x.RoomDetailsID == RoomDET).FirstOrDefault();
                    RoomOccupied.VacantType = Enum.GetName(typeof(VacantType), VacantType.Transfer);
                    RoomOccupied.NoOfDays = NoOfDaysIn;
                    RoomOccupied.RoomOccupationToDate = Con.RoomOccupiedArray.Select(x => x.FromDate).FirstOrDefault();
                    RoomOccupied.RoomOccupationToTime = StrTotal;
                    RoomOccupied.Status = STINT;
                    RoomOccupied.VacatedOn = Con.RoomOccupiedArray.Select(x => x.FromDate).FirstOrDefault();
                    RoomOccupied.UpdatedBy = Con.RoomOccupiedArray.Select(x => x.UpdatedBy).FirstOrDefault();
                    RoomOccupied.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Entry(RoomOccupied).State = EntityState.Modified;



                    var RoomDetails = new RoomDetails();
                    RoomDetails = WYNKContext.RoomDetails.Where(x => x.ID == RoomDET).FirstOrDefault();
                    RoomDetails.IsRoomVacant = false;
                    RoomDetails.UpdatedBy = Con.RoomOccupiedArray.Select(x => x.UpdatedBy).FirstOrDefault();
                    RoomDetails.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Entry(RoomDetails).State = EntityState.Modified;


                    var RoomOCC = new RoomOccupiedstatus();
                    RoomOCC.RoomID = Con.RoomOccupiedArray.Select(x => x.RoomID).FirstOrDefault();
                    RoomOCC.RoomDetailsID = Con.RoomOccupiedArray.Select(x => x.RoomDetailsID).FirstOrDefault();
                    RoomOCC.UIN = Con.RoomOccupiedArray.Select(x => x.UIN).FirstOrDefault();
                    RoomOCC.BedNo = Con.RoomOccupiedArray.Select(x => x.BedNo).FirstOrDefault();
                    RoomOCC.Status = STINT1;
                    RoomOCC.RoomOccupationFromDate = Con.RoomOccupiedArray.Select(x => x.FromDate).FirstOrDefault();
                    RoomOCC.RoomOccupationFromTime = Con.RoomOccupiedArray.Select(x => x.FromTime).FirstOrDefault();
                    RoomOCC.CreatedUTC = DateTime.UtcNow;
                    RoomOCC.CreatedBy = Con.RoomOccupiedArray.Select(x => x.CreatedBy).FirstOrDefault();
                    WYNKContext.RoomOccupiedStatus.AddRange(RoomOCC);
                    WYNKContext.SaveChanges();


                    var RoomDETILS = new RoomDetails();
                    RoomDETILS = WYNKContext.RoomDetails.Where(x => x.ID == RoomDID).FirstOrDefault();
                    RoomDETILS.IsRoomVacant = true;
                    RoomDETILS.UpdatedBy = Con.RoomOccupiedArray.Select(x => x.UpdatedBy).FirstOrDefault();
                    RoomDETILS.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Entry(RoomDETILS).State = EntityState.Modified;
                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)

                            return new
                            {

                                Success = true,
                                Message = CommonMessage.saved,

                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }

            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };
        }
    }
}
