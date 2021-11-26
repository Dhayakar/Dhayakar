using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class RoomMasterRepository : RepositoryBase<RoomMasterViewM>, IRoomMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public RoomMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic getConcerntextfile(int CompanyID)
        {
            var Registrationmaster = new ConcentUploadingViewModel();

            string[] lines;
            var list = new List<string>();
            var osfi = CompanyID;
            var osfn = "RoomMaster.text";
            var currentDir = Directory.GetCurrentDirectory();
            string path = currentDir + "/ConcernPages/" + osfi + '/' + osfn;

            if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();
                Registrationmaster.TOtalLines = lines;
            }
            else
            {
                Registrationmaster.ErrorMessaGE = "No Concern";
            }

            return Registrationmaster;
        }
        public dynamic insertdata(RoomMasterViewM roomMaster)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var M_RoomMaster = new Room();

                    M_RoomMaster.CMPID = roomMaster.RoomMaster.CMPID;
                    M_RoomMaster.RoomType = roomMaster.RoomMaster.RoomType;
                    M_RoomMaster.RoomDescription = roomMaster.RoomMaster.RoomDescription;
                    M_RoomMaster.RoomCost = roomMaster.RoomMaster.RoomCost;
                    M_RoomMaster.NoofRooms = roomMaster.RoomMaster.NoofRooms;
                    M_RoomMaster.NoofBed = roomMaster.RoomMaster.NoofBed;
                    M_RoomMaster.CreatedUTC = DateTime.UtcNow;
                    M_RoomMaster.CreatedBy = roomMaster.RoomMaster.CreatedBy;
                    M_RoomMaster.IsActive = true;

                    WYNKContext.Room.Add(M_RoomMaster);
                    WYNKContext.SaveChanges();


                    foreach (var arr in roomMaster.getRoomDet.ToList())
                    {
                        var RoomDetails = new RoomDetails();
                        RoomDetails.RoomID = M_RoomMaster.ID;
                        RoomDetails.RoomNumber = arr.No;
                        RoomDetails.BedNo = arr.BedNo;
                        RoomDetails.IsActive = true;
                        RoomDetails.CreatedUTC = DateTime.UtcNow;
                        RoomDetails.CreatedBy = RoomDetails.CreatedBy;
                        WYNKContext.RoomDetails.Add(RoomDetails);
                        WYNKContext.SaveChanges();
                    }
                    foreach (var arrr in roomMaster.ToiletType.ToList())
                    {
                        var RoomDetailsExtension = new RoomDetailsExtension();
                        RoomDetailsExtension.RoomDetailsID = WYNKContext.RoomDetails.Where(x => x.RoomID == M_RoomMaster.ID && x.RoomNumber == arrr.RoomNo).Select(c => c.ID).LastOrDefault();
                        RoomDetailsExtension.RestRoomType = arrr.RestRoomType == "IndianToilet" ? 1 : 2;
                        RoomDetailsExtension.CreatedUTC = DateTime.UtcNow;
                        RoomDetailsExtension.CreatedBy = roomMaster.RoomMaster.CreatedBy;
                        WYNKContext.RoomDetailsExtension.Add(RoomDetailsExtension);
                        WYNKContext.SaveChanges();
                    }
                   
                    WYNKContext.SaveChanges();
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
                        dbContextTransaction.Rollback();
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



        public RoomMasterViewM getRoomDet(int CompanyID, int roomID)
        {
            var M_Room = WYNKContext.Room.Where(x=>x.CMPID== CompanyID).AsNoTracking().ToList();
            var M_RoomDetail = WYNKContext.RoomDetails.ToList();
            var oneline = CMPSContext.OneLineMaster.AsNoTracking().Where(x => x.ParentTag == "RoomType").ToList();
            var getRoomDet = new RoomMasterViewM();

            getRoomDet.RoomDetails1 = (from Room in M_Room.Where(x => x.CMPID == CompanyID)
                                       join RoomDet in M_RoomDetail.Where(x => x.RoomID == roomID)
                                       on Room.ID equals RoomDet.RoomID

                                       select new RoomDetails1
                                       {
                                           RoomType = oneline.Where(x => x.OLMID == Convert.ToInt32(Room.RoomType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                           RoomDescription = Room.RoomDescription,
                                           Roomcost = Room.RoomCost,
                                           No = RoomDet.RoomNumber,
                                           BedNo = RoomDet.BedNo,                                           
                                           IsActive = Enum.GetName(typeof(ISactive), RoomDet.IsActive),
                                           RoomDetID = RoomDet.ID
                                       }).ToList();


            return getRoomDet;
        }




        public dynamic Updatedata(RoomMasterViewM roomMaster)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
          
                    foreach (var arr in roomMaster.RoomDetails2.ToList())
                    {
                        var RoomDetails = new RoomDetails();

                        RoomDetails = WYNKContext.RoomDetails.Where(x => x.ID == arr.RoomDetID).FirstOrDefault();
                        if (arr.IsActive == "Yes")
                        {
                            RoomDetails.IsActive = true;
                        }
                        else 
                        {
                            RoomDetails.IsActive = false;
                        }
                        RoomDetails.UpdatedUTC = DateTime.UtcNow;
                        RoomDetails.UpdatedBy = RoomDetails.UpdatedBy;
                        WYNKContext.RoomDetails.UpdateRange(RoomDetails);
                        WYNKContext.SaveChanges();
                    }
                    WYNKContext.SaveChanges();
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

                    Console.Write(ex);
                }

            }
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };
        }



        public RoomMasterViewM getRoomTariff(int CmpID)
        {
            var Room = WYNKContext.Room.AsNoTracking().ToList();
            var RoomDetail = WYNKContext.RoomDetails.AsNoTracking().ToList();
            var oneline = CMPSContext.OneLineMaster.AsNoTracking().Where(x => x.ParentTag == "RoomType").ToList();
            var getRoomDet = new RoomMasterViewM();

            getRoomDet.RoomTariff = (from R in Room.Where(x => x.CMPID == CmpID)
                                   select new RoomTariff
                                     {

                                         RoomType = oneline.Where(x => x.OLMID == Convert.ToInt32(R.RoomType)).Select(x => x.ParentDescription).FirstOrDefault(),
                                         RoomDescription = R.RoomDescription,
                                         Roomcost= R.RoomCost,
                                       PAddress = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.Address1).FirstOrDefault(),
                                       PAddress2 = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.Address2).FirstOrDefault(),
                                       PAddress3 = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.Address3).FirstOrDefault(),
                                       Pphone = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.Phone1).FirstOrDefault(),
                                       Pweb = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.Website).FirstOrDefault(),
                                       PCompnayname = CMPSContext.Company.Where(x => x.CmpID == CmpID).Select(x => x.CompanyName).FirstOrDefault(),

                                   }).ToList();


            return getRoomDet;
        }


    }
}
