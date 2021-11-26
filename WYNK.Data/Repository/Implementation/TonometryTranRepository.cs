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
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class TonometryTranRepository : RepositoryBase<TonometryTranViewmodel>, ITonometryTranRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public TonometryTranRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertTonoTrans(TonometryTranViewmodel Addtono)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    if (Addtono.TonometryTran.Count() > 0)
                    {
                        foreach (var item in Addtono.TonometryTran.ToList())
                        {
                            var TT = new TonometryTran();

                            TT.CmpID = item.CmpID;
                            TT.UIN = item.UIN;
                            var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                            TT.RegistrationtranID = rid;
                            TT.VisitDatetime = item.VisitDatetime;
                            TT.TonometryID = item.TonometryID;
                            TT.BOD = item.BOD;
                            TT.BOS = item.BOS;
                            TT.AOD = item.AOD;
                            TT.AOS = item.AOS;
                            TT.Dilation = item.Dilation;
                            TT.Time = item.Time;
                            TT.StaffID = item.StaffID;
                            TT.IsDeleted = item.IsDeleted;
                            TT.CreatedUTC = DateTime.UtcNow;
                            TT.CreatedBy = item.CreatedBy;
                            WYNKContext.TonometryTran.AddRange(TT);
                            string cmpname = CMPSContext.Company.Where(x => x.CmpID == item.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                            string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == item.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                            string userid = Convert.ToString(item.CreatedBy);
                            ErrorLog oErrorLogs = new ErrorLog();
                            oErrorLogs.WriteErrorLogTitle(cmpname, "Tonometry", "User name :", username, "User ID :", userid, "Mode : Sumbit");
                            object names = TT;
                            oErrorLogs.WriteErrorLogArray("TonometryTran", names);
                            WYNKContext.SaveChanges();
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();


                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }


                    return new
                    {
                        Success = true,
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                }
                return new
                {
                    Success = false,
                };
            }
        }
        public dynamic Gettonometry(string UIN, int cmpid)
        {
            var Details = new TonometryTranViewmodel();
            var tono = WYNKContext.Tonometry.ToList();
            var DC = CMPSContext.DoctorMaster.ToList();

            Details.tonometrydetails = new List<tonometrydetails>();

            var r = WYNKContext.TonometryTran.Where(x => x.UIN == UIN).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationtranID).FirstOrDefault();

            Details.tonometrydetails = (from REF in WYNKContext.TonometryTran.Where(u => u.RegistrationtranID == r && u.IsDeleted == false && u.CmpID == cmpid)
                                                 select new tonometrydetails
                                                 {
                                                     ID = REF.ID,
                                                     RegistrationtranID = REF.RegistrationtranID,
                                                     UIN = REF.UIN,
                                                     CmpID = REF.CmpID,
                                                     VisitDatetime = REF.VisitDatetime,
                                                     TonometryID = REF.TonometryID,
                                                     Tonometryname = tono.Where(x => x.ID == REF.TonometryID).Select(x => x.Description).FirstOrDefault(),
                                                     BOD = REF.BOD,
                                                     BOS = REF.BOS,
                                                     AOD = REF.AOD,
                                                     AOS = REF.AOS,
                                                     Time = REF.Time,
                                                     Dilation = REF.Dilation.ToString(),
                                                     Staffname = DC.Where(x => x.DoctorID == REF.StaffID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                                     StaffID = REF.StaffID,
                                                     RemovedReasons = REF.RemovedReasons,
                                                     RemovedBy = REF.RemovedBy,
                                                     Dropsname = REF.Dropsname,
                                                     Removedname = DC.Where(x => x.DoctorID == REF.RemovedBy).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                                     RemovedDatetime = REF.RemovedDatetime,
                                                 }).ToList();



            return Details;
        }
        public dynamic updateTonoTrans(TonometryTranViewmodel uptono, string uin)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var regtid = WYNKContext.RegistrationTran.Where(x => x.UIN == uin).Select(x => x.RegistrationTranID).LastOrDefault();
                    var reg = Convert.ToInt32(WYNKContext.TonometryTran.Where(x => x.RegistrationtranID == regtid).Select(x => x.RegistrationtranID).LastOrDefault());
                    if (reg == 0)
                    {
                        if (uptono.TonometryTran.Count() > 0)
                        {
                            foreach (var item in uptono.TonometryTran.ToList())
                            {
                                var TT = new TonometryTran();

                                TT.CmpID = item.CmpID;
                                TT.UIN = item.UIN;
                                var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                TT.RegistrationtranID = rid;
                                TT.VisitDatetime = item.VisitDatetime;
                                TT.TonometryID = item.TonometryID;
                                TT.BOD = item.BOD;
                                TT.BOS = item.BOS;
                                TT.AOD = item.AOD;
                                TT.AOS = item.AOS;
                                TT.Dilation = item.Dilation;
                                TT.Time = item.Time;
                                TT.StaffID = item.StaffID;
                                TT.IsDeleted = item.IsDeleted;
                                TT.RemovedReasons = item.RemovedReasons;
                                TT.RemovedBy = item.RemovedBy;
                                TT.RemovedDatetime = item.RemovedDatetime;
                                TT.CreatedUTC = DateTime.UtcNow;
                                TT.CreatedBy = item.CreatedBy;
                                WYNKContext.TonometryTran.AddRange(TT);
                                string cmpname = CMPSContext.Company.Where(x => x.CmpID == item.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == item.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                                string userid = Convert.ToString(item.CreatedBy);
                                ErrorLog oErrorLogs = new ErrorLog();
                                oErrorLogs.WriteErrorLogTitle(cmpname, "Tonometry", "User name :", username, "User ID :", userid, "Mode : Sumbit");
                                object names = TT;
                                oErrorLogs.WriteErrorLogArray("TonometryTran", names);
                                WYNKContext.SaveChanges();
                            }
                        }
                    }

                    else
                    {

                        if (uptono.TonometryTran.Count() > 0)
                        {
                            foreach (var item in uptono.TonometryTran.ToList())
                            {
                                var TT = new TonometryTran();
                                var ID = item.ID;

                                if (ID != 0)
                                {
                                    TT = WYNKContext.TonometryTran.Where(x => x.ID == item.ID).FirstOrDefault();
                                    TT.CmpID = item.CmpID;
                                    TT.UIN = item.UIN;
                                    var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                    TT.RegistrationtranID = rid;
                                    TT.VisitDatetime = item.VisitDatetime;
                                    TT.TonometryID = item.TonometryID;
                                    TT.BOD = item.BOD;
                                    TT.BOS = item.BOS;
                                    TT.AOD = item.AOD;
                                    TT.AOS = item.AOS;
                                    TT.Dilation = item.Dilation;
                                    TT.Time = item.Time;
                                    TT.StaffID = item.StaffID;
                                    TT.IsDeleted = item.IsDeleted;
                                    TT.RemovedReasons = item.RemovedReasons;
                                    TT.RemovedBy = item.RemovedBy;
                                    TT.RemovedDatetime = item.RemovedDatetime;
                                    TT.UpdatedUTC = DateTime.UtcNow;
                                    TT.UpdatedBy = item.CreatedBy;
                                    WYNKContext.TonometryTran.UpdateRange(TT);
                                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == item.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == item.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                                    string userid = Convert.ToString(item.CreatedBy);
                                    ErrorLog oErrorLogs = new ErrorLog();
                                    oErrorLogs.WriteErrorLogTitle(cmpname, "Tonometry", "User name :", username, "User ID :", userid, "Mode : Update");
                                    object names = TT;
                                    oErrorLogs.WriteErrorLogArray("TonometryTran", names);
                                    WYNKContext.SaveChanges();
                                }
                                else
                                {
                                    TT.CmpID = item.CmpID;
                                    TT.UIN = item.UIN;
                                    var rid = WYNKContext.RegistrationTran.Where(x => x.UIN == item.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                                    TT.RegistrationtranID = rid;
                                    TT.VisitDatetime = item.VisitDatetime;
                                    TT.TonometryID = item.TonometryID;
                                    TT.BOD = item.BOD;
                                    TT.BOS = item.BOS;
                                    TT.AOD = item.AOD;
                                    TT.AOS = item.AOS;
                                    TT.Dilation = item.Dilation;
                                    TT.Time = item.Time;
                                    TT.StaffID = item.StaffID;
                                    TT.IsDeleted = item.IsDeleted;
                                    TT.RemovedReasons = item.RemovedReasons;
                                    TT.RemovedBy = item.RemovedBy;
                                    TT.RemovedDatetime = item.RemovedDatetime;
                                    TT.CreatedUTC = DateTime.UtcNow;
                                    TT.CreatedBy = item.CreatedBy;
                                    WYNKContext.TonometryTran.AddRange(TT);
                                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == item.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == item.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                                    string userid = Convert.ToString(item.CreatedBy);
                                    ErrorLog oErrorLogs = new ErrorLog();
                                    oErrorLogs.WriteErrorLogTitle(cmpname, "Tonometry", "User name :", username, "User ID :", userid, "Mode : Sumbit");
                                    object names = TT;
                                    oErrorLogs.WriteErrorLogArray("TonometryTran", names);
                                    WYNKContext.SaveChanges();
                                }
                            }
                        }

                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    if (WYNKContext.SaveChanges() >= 0)
                    {
                        ErrorLog oErrorLog = new ErrorLog();
                        oErrorLog.WriteErrorLog("Information :", "Saved Successfully");
                    }
                    return new
                    {
                        Success = true,
                    };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    ErrorLog oErrorLog = new ErrorLog();
                    oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                }
                return new
                {
                    Success = false,
                };

            }

        }

        public dynamic GetHistoryiopDetails(string UIN, int cmpid)
        {
            var HDetails = new TonometryTranViewmodel();
            var tono = WYNKContext.Tonometry.ToList();
            var DC = CMPSContext.DoctorMaster.ToList();
            HDetails.tonometrydetailss = new List<tonometrydetailss>();

            var TID = WYNKContext.TonometryTran.Where(u => u.UIN == UIN).GroupBy(x => x.RegistrationtranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;


                HDetails.tonometrydetailss = (from REF in WYNKContext.TonometryTran.Where(u => u.RegistrationtranID == rid && u.IsDeleted == false && u.CmpID == cmpid)
                                            select new tonometrydetailss
                                            {
                                                ID = REF.ID,
                                                RegistrationtranID = REF.RegistrationtranID,
                                                UIN = REF.UIN,
                                                CmpID = REF.CmpID,
                                                VisitDatetime = REF.VisitDatetime,
                                                TonometryID = REF.TonometryID,
                                                Tonometryname = tono.Where(x => x.ID == REF.TonometryID).Select(x => x.Description).FirstOrDefault(),
                                                BOD = REF.BOD,
                                                BOS = REF.BOS,
                                                AOD = REF.AOD,
                                                AOS = REF.AOS,
                                                Time = REF.Time,
                                                Dilation = REF.Dilation.ToString(),
                                                Staffname = DC.Where(x => x.DoctorID == REF.StaffID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                                StaffID = REF.StaffID,
                                                RemovedReasons = REF.RemovedReasons,
                                                RemovedBy = REF.RemovedBy,
                                                Dropsname = REF.Dropsname,
                                                Removedname = DC.Where(x => x.DoctorID == REF.RemovedBy).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault(),
                                                RemovedDatetime = REF.RemovedDatetime,
                                            }).ToList();


            }

            return HDetails;
        }





    }
}
