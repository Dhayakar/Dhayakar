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
    public class YearEndProcessRepository : RepositoryBase<YearEndProcessViewModel>, IYearEndProcessRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public YearEndProcessRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public YearEndProcessViewModel GetFinancialYearDetail(int cmpid)
        {
            var ddetail = new YearEndProcessViewModel();

            var FinancialYear = WYNKContext.FinancialYear.ToList();

            ddetail.FinancialDetails = (from get in FinancialYear.Where(x => x.CMPID == cmpid && x.IsActive == true)
                                        .OrderByDescending(x => x.FYAccYear)
                                        select new FinancialDetails
                                        {
                                            ID = get.ID,
                                            FYDescription = get.FYDescription,
                                            FYFrom = get.FYFrom,
                                            FYTo = get.FYTo,
                                            FYAccYear = get.FYAccYear,
                                            FYStatus = GetStatus(get.ID),
                                        }).ToList();
            return ddetail;
        }

        public string GetStatus(int ID)
        {
            var StatusID = WYNKContext.FinancialYearStatus.Where(x => x.FYID == ID).Select(x => x.FYStatus).LastOrDefault();
            var Status = Enum.GetName(typeof(Status), StatusID);
            return Status;
        }


        public dynamic GetFinancialID(int ID, int cmpid)
        {
            var StatusID = WYNKContext.FinancialYearStatus.Where(x => x.FYID == ID).Select(x => x.FYStatus).LastOrDefault();

            var Status = Enum.GetName(typeof(Status), StatusID);


            if (Status == "Open")
            {
                var FYAccYear = WYNKContext.FinancialYear.Where(x => x.ID == ID && x.CMPID == cmpid).Select(x => x.FYAccYear).LastOrDefault();

                var data = WYNKContext.FinancialYear.Where(x => x.FYAccYear == (FYAccYear + 1) && x.CMPID == cmpid && x.IsActive == true).LastOrDefault();


                if (data != null)
                {

                    return new
                    {
                        Success = true,
                        Message = "Open Status",
                        data = data
                    };

                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "Destination Financial Year Not Found",
                    };

                }


            }
            else
            {


                return new
                {
                    Success = false,
                    Message = "Closed status"

                };

            }




        }

        public dynamic GetFinancialYearItemBalance(string ID, int cmpid, int FFYID, int TFYID)
        {
            try
            {
                var StoreMaster = CMPSContext.Storemasters.ToList();
                var DrugMaster = WYNKContext.DrugMaster.ToList();
                var DrugGroup = WYNKContext.DrugGroup.ToList();
                var ItemBalance = WYNKContext.ItemBalance.ToList();

                if (ID == "All Stores")
                {
                    var AllStores = (from FIY in ItemBalance.Where(x => x.FYear == FFYID && x.CmpID == cmpid)
                                     select new
                                     {
                                         StoreName = StoreMaster.Where(x => x.StoreID == FIY.StoreID && x.CmpID == cmpid).Select(x => x.Storename).FirstOrDefault(),
                                         ItemDescription = DrugMaster.Where(x => x.ID == FIY.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                         GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(x => x.ID == FIY.ItemID).Select(x => x.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                                         UOM = DrugMaster.Where(x => x.ID == FIY.ItemID).Select(x => x.UOM).FirstOrDefault(),
                                         ClosingBalance = FIY.ClosingBalance,
                                         OPeningBalance = GetOpeningBalance(FIY.ItemID, TFYID, Convert.ToInt32(FIY.StoreID), cmpid),
                                     }).ToList();


                    if (AllStores.Count > 0)
                    {
                        return new
                        {
                            Success = true,
                            Message = "Data Found",
                            data = AllStores,
                        };
                    }
                    else
                    {
                        return new
                        {
                            Success = false,
                            Message = "No Data Found",
                        };
                    }

                }

                var res = (from FY in ItemBalance.Where(x => x.FYear == FFYID && x.StoreID == Convert.ToInt32(ID) && x.CmpID == cmpid)
                           select new
                           {
                               ItemDescription = DrugMaster.Where(x => x.ID == FY.ItemID).Select(x => x.Brand).FirstOrDefault(),
                               GenericName = DrugGroup.Where(dg => dg.ID == DrugMaster.Where(x => x.ID == FY.ItemID).Select(x => x.GenericName).FirstOrDefault()).Select(dg => dg.Description).FirstOrDefault(),
                               UOM = DrugMaster.Where(x => x.ID == FY.ItemID).Select(x => x.UOM).FirstOrDefault(),
                               ClosingBalance = FY.ClosingBalance,
                               OPeningBalance = GetOpeningBalance(FY.ItemID, TFYID, Convert.ToInt32(ID), cmpid),
                           }).ToList();


                if (res.Count > 0)
                {
                    return new
                    {
                        Success = true,
                        Message = "Data Found",
                        data = res,
                    };
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Data Found",
                    };
                }

            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = "Something Went Wrong"
                };
            }


        }

        public int GetOpeningBalance(int ItemID, int TFYID, int StoreID, int cmpid)
        {
            var ItemBalan = WYNKContext.ItemBalance.ToList();
            var OpeningBalance = ItemBalan.Where(x => x.StoreID == StoreID && x.FYear == TFYID && x.ItemID == ItemID && x.CmpID == cmpid).Select(x => x.OpeningBalance).FirstOrDefault();
            return Convert.ToInt32(OpeningBalance);
        }

        public dynamic Submit(int cmpid, int FFYID, int TFYID, string Store, int Createdby)
        {
            try
            {
                var ItemBalan = WYNKContext.ItemBalance.Where(x=>x.CmpID == cmpid).ToList();
                if (Store != "All Stores")
                {

                    var fromItemBalance = ItemBalan.Where(x => x.StoreID == Convert.ToInt32(Store) && x.FYear == FFYID && x.CmpID == cmpid).ToList();

                    if (fromItemBalance.Count == 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "No Items to Transfer"
                        };
                    }

                    foreach (var FItemBal in fromItemBalance.ToList())
                    {
                        var AlreadyExistingItem = ItemBalan.Where(x => x.FYear == TFYID && x.CmpID == cmpid && x.ItemID == FItemBal.ItemID && x.StoreID == Convert.ToInt32(Store)).FirstOrDefault();

                        if (AlreadyExistingItem != null)
                        {
                            AlreadyExistingItem.OpeningBalance = FItemBal.ClosingBalance;
                            var Receipts = AlreadyExistingItem.Rec01 + AlreadyExistingItem.Rec02 + AlreadyExistingItem.Rec03 + AlreadyExistingItem.Rec04 + AlreadyExistingItem.Rec05 + AlreadyExistingItem.Rec06 + AlreadyExistingItem.Rec07 + AlreadyExistingItem.Rec08 + AlreadyExistingItem.Rec09 + AlreadyExistingItem.Rec10 + AlreadyExistingItem.Rec11 + AlreadyExistingItem.Rec12;
                            var Issues = AlreadyExistingItem.Iss01 + AlreadyExistingItem.Iss02 + AlreadyExistingItem.Iss03 + AlreadyExistingItem.Iss04 + AlreadyExistingItem.Iss05 + AlreadyExistingItem.Iss06 + AlreadyExistingItem.Iss07 + AlreadyExistingItem.Iss08 + AlreadyExistingItem.Iss09 + AlreadyExistingItem.Iss10 + AlreadyExistingItem.Iss11 + AlreadyExistingItem.Iss12;
                            var ClosingBalance = (FItemBal.ClosingBalance + Receipts) - Issues;
                            AlreadyExistingItem.ClosingBalance = ClosingBalance;
                            AlreadyExistingItem.UpdatedBy = Createdby;
                            AlreadyExistingItem.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.ItemBalance.UpdateRange(AlreadyExistingItem);
                        }
                        else
                        {
                            var ItemBalance = new ItemBalance();
                            ItemBalance.ItemID = FItemBal.ItemID;
                            ItemBalance.UOMID = FItemBal.UOMID;
                            ItemBalance.FYear = TFYID;
                            ItemBalance.OpeningBalance = FItemBal.ClosingBalance;
                            ItemBalance.Rec01 = 0;
                            ItemBalance.Rec02 = 0;
                            ItemBalance.Rec03 = 0;
                            ItemBalance.Rec04 = 0;
                            ItemBalance.Rec05 = 0;
                            ItemBalance.Rec06 = 0;
                            ItemBalance.Rec07 = 0;
                            ItemBalance.Rec08 = 0;
                            ItemBalance.Rec09 = 0;
                            ItemBalance.Rec10 = 0;
                            ItemBalance.Rec11 = 0;
                            ItemBalance.Rec12 = 0;
                            ItemBalance.Iss01 = 0;
                            ItemBalance.Iss02 = 0;
                            ItemBalance.Iss03 = 0;
                            ItemBalance.Iss04 = 0;
                            ItemBalance.Iss05 = 0;
                            ItemBalance.Iss06 = 0;
                            ItemBalance.Iss07 = 0;
                            ItemBalance.Iss08 = 0;
                            ItemBalance.Iss09 = 0;
                            ItemBalance.Iss10 = 0;
                            ItemBalance.Iss11 = 0;
                            ItemBalance.Iss12 = 0;
                            ItemBalance.CmpID = cmpid;
                            ItemBalance.ClosingBalance = FItemBal.ClosingBalance;
                            ItemBalance.StoreID = FItemBal.StoreID;
                            ItemBalance.CreatedUTC = DateTime.UtcNow;
                            ItemBalance.CreatedBy = Createdby;
                            WYNKContext.ItemBalance.Add(ItemBalance);

                        }
                    }
                    WYNKContext.SaveChanges();

                    return new
                    {
                        Success = true,
                        Message = "Saved Succesfully"

                    };
                }
                else
                {
                    var fromItemBalance = ItemBalan.Where(x => x.FYear == FFYID && x.CmpID == cmpid).ToList();

                    if (fromItemBalance.Count == 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "No Items to Transfer"
                        };
                    }

                    foreach (var FItemBal in fromItemBalance.ToList())
                    {
                        var AlreadyExistingItem = ItemBalan.Where(x => x.FYear == TFYID && x.CmpID == cmpid && x.ItemID == FItemBal.ItemID && x.StoreID == FItemBal.StoreID).FirstOrDefault();

                        if (AlreadyExistingItem != null)
                        {
                            AlreadyExistingItem.OpeningBalance = FItemBal.ClosingBalance;
                            var Receipts = AlreadyExistingItem.Rec01 + AlreadyExistingItem.Rec02 + AlreadyExistingItem.Rec03 + AlreadyExistingItem.Rec04 + AlreadyExistingItem.Rec05 + AlreadyExistingItem.Rec06 + AlreadyExistingItem.Rec07 + AlreadyExistingItem.Rec08 + AlreadyExistingItem.Rec09 + AlreadyExistingItem.Rec10 + AlreadyExistingItem.Rec11 + AlreadyExistingItem.Rec12;
                            var Issues = AlreadyExistingItem.Iss01 + AlreadyExistingItem.Iss02 + AlreadyExistingItem.Iss03 + AlreadyExistingItem.Iss04 + AlreadyExistingItem.Iss05 + AlreadyExistingItem.Iss06 + AlreadyExistingItem.Iss07 + AlreadyExistingItem.Iss08 + AlreadyExistingItem.Iss09 + AlreadyExistingItem.Iss10 + AlreadyExistingItem.Iss11 + AlreadyExistingItem.Iss12;
                            var ClosingBalance = (FItemBal.ClosingBalance + Receipts) - Issues;
                            AlreadyExistingItem.ClosingBalance = ClosingBalance;
                            AlreadyExistingItem.UpdatedBy = Createdby;
                            AlreadyExistingItem.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.ItemBalance.UpdateRange(AlreadyExistingItem);
                        }
                        else
                        {
                            var ItemBalance = new ItemBalance();
                            ItemBalance.ItemID = FItemBal.ItemID;
                            ItemBalance.UOMID = FItemBal.UOMID;
                            ItemBalance.FYear = TFYID;
                            ItemBalance.OpeningBalance = FItemBal.ClosingBalance;
                            ItemBalance.Rec01 = 0;
                            ItemBalance.Rec02 = 0;
                            ItemBalance.Rec03 = 0;
                            ItemBalance.Rec04 = 0;
                            ItemBalance.Rec05 = 0;
                            ItemBalance.Rec06 = 0;
                            ItemBalance.Rec07 = 0;
                            ItemBalance.Rec08 = 0;
                            ItemBalance.Rec09 = 0;
                            ItemBalance.Rec10 = 0;
                            ItemBalance.Rec11 = 0;
                            ItemBalance.Rec12 = 0;
                            ItemBalance.Iss01 = 0;
                            ItemBalance.Iss02 = 0;
                            ItemBalance.Iss03 = 0;
                            ItemBalance.Iss04 = 0;
                            ItemBalance.Iss05 = 0;
                            ItemBalance.Iss06 = 0;
                            ItemBalance.Iss07 = 0;
                            ItemBalance.Iss08 = 0;
                            ItemBalance.Iss09 = 0;
                            ItemBalance.Iss10 = 0;
                            ItemBalance.Iss11 = 0;
                            ItemBalance.Iss12 = 0;
                            ItemBalance.CmpID = cmpid;
                            ItemBalance.ClosingBalance = FItemBal.ClosingBalance;
                            ItemBalance.StoreID = FItemBal.StoreID;
                            ItemBalance.CreatedUTC = DateTime.UtcNow;
                            ItemBalance.CreatedBy = Createdby;
                            WYNKContext.ItemBalance.Add(ItemBalance);

                        }

                    }
                    WYNKContext.SaveChanges();
                    return new
                    {
                        Success = true,
                        Message = "Saved Succesfully"

                    };
                }
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic GetOpticaLFinancialYearItemBalance(string ID, int cmpid, int FFYID, int TFYID)
        {
            try
            {
                var StoreMaster = CMPSContext.Storemasters.ToList();
                var BrandMaster = WYNKContext.Brand.ToList();
                var LensMaster = WYNKContext.Lensmaster.ToList();
                var LensTran = WYNKContext.Lenstrans.ToList();
                var UOM = CMPSContext.uommaster.ToList();
                var OpticalItemBalance = WYNKContext.OpticalBalance.ToList();

                if (ID == "All Stores")
                {
                    var AllStores = (from FIY in OpticalItemBalance.Where(x => x.FYID == FFYID && x.CmpID == cmpid)
                                     select new
                                     {
                                         StoreName = StoreMaster.Where(x => x.StoreID == FIY.StoreID && x.CmpID == cmpid).Select(x => x.Storename).FirstOrDefault(),
                                         LensType = LensMaster.Where(x => x.RandomUniqueID == LensTran.Where(lt => lt.ID == FIY.LTID).Select(lt => lt.LMID).FirstOrDefault()).Select(x => x.LensType).FirstOrDefault(),
                                         Brand = BrandMaster.Where(x => x.ID == LensTran.Where(lt => lt.ID == FIY.LTID).Select(lt => lt.Brand).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),

                                         Model = LensTran.Where(lt => lt.ID == FIY.LTID).Select(lt => lt.Model).FirstOrDefault(),
                                         UOM = UOM.Where(x => x.id == LensTran.Where(lt => lt.ID == FIY.LTID).Select(lt => lt.UOMID).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                                         ClosingBalance = FIY.ClosingBalance,
                                         OPeningBalance = GetOpeningBalance(FIY.LTID, TFYID, Convert.ToInt32(FIY.StoreID), cmpid),
                                     }).ToList();


                    if (AllStores.Count > 0)
                    {
                        return new
                        {
                            Success = true,
                            Message = "Data Found",
                            data = AllStores,
                        };
                    }
                    else
                    {
                        return new
                        {
                            Success = false,
                            Message = "No Data Found",
                        };
                    }

                }

                var res = (from FY in OpticalItemBalance.Where(x => x.FYID == FFYID && x.StoreID == Convert.ToInt32(ID) && x.CmpID == cmpid)
                           select new
                           {
                               LensType = LensMaster.Where(x => x.RandomUniqueID == LensTran.Where(lt => lt.ID == FY.LTID).Select(lt => lt.LMID).FirstOrDefault()).Select(x => x.LensType).FirstOrDefault(),
                               Brand = BrandMaster.Where(x => x.ID == LensTran.Where(lt => lt.ID == FY.LTID).Select(lt => lt.Brand).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),

                               Model = LensTran.Where(lt => lt.ID == FY.LTID).Select(lt => lt.Model).FirstOrDefault(),
                               UOM = UOM.Where(x => x.id == LensTran.Where(lt => lt.ID == FY.LTID).Select(lt => lt.UOMID).FirstOrDefault()).Select(x => x.Description).FirstOrDefault(),
                               ClosingBalance = FY.ClosingBalance,
                               OPeningBalance = GetOpticalOpeningBalance(FY.LTID, TFYID, Convert.ToInt32(FY.StoreID), cmpid),
                           }).ToList();


                if (res.Count > 0)
                {
                    return new
                    {
                        Success = true,
                        Message = "Data Found",
                        data = res,
                    };
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Data Found",
                    };
                }

            }
            catch (Exception ex)
            {
                return new
                {
                    Success = false,
                    Message = "Something Went Wrong"
                };
            }


        }
        public int GetOpticalOpeningBalance(int ItemID, int TFYID, int StoreID, int cmpid)
        {
            var ItemBalan = WYNKContext.OpticalBalance.ToList();
            var OpeningBalance = ItemBalan.Where(x => x.StoreID == StoreID && x.FYID == TFYID && x.LTID == ItemID && x.CmpID == cmpid).Select(x => x.OpeningBalance).FirstOrDefault();
            return Convert.ToInt32(OpeningBalance);
        }

        public dynamic SubmitOptical(int cmpid, int FFYID, int TFYID, string Store, int Createdby)
        {
            try
            {
                var ItemBalan = WYNKContext.OpticalBalance.ToList();

                if (Store != "All Stores")
                {

                    var fromItemBalance = ItemBalan.Where(x => x.StoreID == Convert.ToInt32(Store) && x.FYID == FFYID && x.CmpID == cmpid).ToList();

                    if (fromItemBalance.Count == 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "No Items to Transfer"
                        };
                    }

                    foreach (var FItemBal in fromItemBalance.ToList())
                    {
                        var AlreadyExistingItem = ItemBalan.Where(x => x.FYID == TFYID && x.CmpID == cmpid && x.LTID == FItemBal.LTID && x.StoreID == Convert.ToInt32(Store)).FirstOrDefault();

                        if (AlreadyExistingItem != null)
                        {
                            AlreadyExistingItem.OpeningBalance = FItemBal.ClosingBalance;
                            var Receipts = AlreadyExistingItem.REC01 + AlreadyExistingItem.REC02 + AlreadyExistingItem.REC03 + AlreadyExistingItem.REC04 + AlreadyExistingItem.REC05 + AlreadyExistingItem.REC06 + AlreadyExistingItem.REC07 + AlreadyExistingItem.REC08 + AlreadyExistingItem.REC09 + AlreadyExistingItem.REC10 + AlreadyExistingItem.REC11 + AlreadyExistingItem.REC12;
                            var Issues = AlreadyExistingItem.ISS01 + AlreadyExistingItem.ISS02 + AlreadyExistingItem.ISS03 + AlreadyExistingItem.ISS04 + AlreadyExistingItem.ISS05 + AlreadyExistingItem.ISS06 + AlreadyExistingItem.ISS07 + AlreadyExistingItem.ISS08 + AlreadyExistingItem.ISS09 + AlreadyExistingItem.ISS10 + AlreadyExistingItem.ISS11 + AlreadyExistingItem.ISS12;
                            var ClosingBalance = (FItemBal.ClosingBalance + Receipts) - Issues;
                            AlreadyExistingItem.ClosingBalance = ClosingBalance;
                            AlreadyExistingItem.UpdatedBy = Createdby;
                            AlreadyExistingItem.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.OpticalBalance.UpdateRange(AlreadyExistingItem);
                        }
                        else
                        {
                            var ItemBalance = new OpticalBalance();
                            ItemBalance.LTID = FItemBal.LTID;
                            ItemBalance.UOMID = FItemBal.UOMID;
                            ItemBalance.FYID = TFYID;
                            ItemBalance.OpeningBalance = FItemBal.ClosingBalance;
                            ItemBalance.REC01 = 0;
                            ItemBalance.REC02 = 0;
                            ItemBalance.REC03 = 0;
                            ItemBalance.REC04 = 0;
                            ItemBalance.REC05 = 0;
                            ItemBalance.REC06 = 0;
                            ItemBalance.REC07 = 0;
                            ItemBalance.REC08 = 0;
                            ItemBalance.REC09 = 0;
                            ItemBalance.REC10 = 0;
                            ItemBalance.REC11 = 0;
                            ItemBalance.REC12 = 0;
                            ItemBalance.ISS01 = 0;
                            ItemBalance.ISS02 = 0;
                            ItemBalance.ISS03 = 0;
                            ItemBalance.ISS04 = 0;
                            ItemBalance.ISS05 = 0;
                            ItemBalance.ISS06 = 0;
                            ItemBalance.ISS07 = 0;
                            ItemBalance.ISS08 = 0;
                            ItemBalance.ISS09 = 0;
                            ItemBalance.ISS10 = 0;
                            ItemBalance.ISS11 = 0;
                            ItemBalance.ISS12 = 0;
                            ItemBalance.CmpID = cmpid;
                            ItemBalance.ClosingBalance = FItemBal.ClosingBalance;
                            ItemBalance.StoreID = FItemBal.StoreID;
                            ItemBalance.CreatedUTC = DateTime.UtcNow;
                            ItemBalance.CreatedBy = Createdby;
                            WYNKContext.OpticalBalance.Add(ItemBalance);
                        }
                    }
                    WYNKContext.SaveChanges();

                    return new
                    {
                        Success = true,
                        Message = "Saved Succesfully"

                    };
                }
                else
                {
                    var fromItemBalance = ItemBalan.Where(x => x.FYID == FFYID && x.CmpID == cmpid).ToList();

                    if (fromItemBalance.Count == 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "No Items to Transfer"
                        };
                    }

                    foreach (var FItemBal in fromItemBalance.ToList())
                    {
                        var AlreadyExistingItem = ItemBalan.Where(x => x.FYID == TFYID && x.CmpID == cmpid && x.LTID == FItemBal.LTID && x.StoreID == FItemBal.StoreID).FirstOrDefault();

                        if (AlreadyExistingItem != null)
                        {
                            AlreadyExistingItem.OpeningBalance = FItemBal.ClosingBalance;
                            var Receipts = AlreadyExistingItem.REC01 + AlreadyExistingItem.REC02 + AlreadyExistingItem.REC03 + AlreadyExistingItem.REC04 + AlreadyExistingItem.REC05 + AlreadyExistingItem.REC06 + AlreadyExistingItem.REC07 + AlreadyExistingItem.REC08 + AlreadyExistingItem.REC09 + AlreadyExistingItem.REC10 + AlreadyExistingItem.REC11 + AlreadyExistingItem.REC12;
                            var Issues = AlreadyExistingItem.ISS01 + AlreadyExistingItem.ISS02 + AlreadyExistingItem.ISS03 + AlreadyExistingItem.ISS04 + AlreadyExistingItem.ISS05 + AlreadyExistingItem.ISS06 + AlreadyExistingItem.ISS07 + AlreadyExistingItem.ISS08 + AlreadyExistingItem.ISS09 + AlreadyExistingItem.ISS10 + AlreadyExistingItem.ISS11 + AlreadyExistingItem.ISS12;
                            var ClosingBalance = (FItemBal.ClosingBalance + Receipts) - Issues;
                            AlreadyExistingItem.ClosingBalance = ClosingBalance;
                            AlreadyExistingItem.UpdatedBy = Createdby;
                            AlreadyExistingItem.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.OpticalBalance.UpdateRange(AlreadyExistingItem);
                        }
                        else
                        {
                            var ItemBalance = new OpticalBalance();
                            ItemBalance.LTID = FItemBal.LTID;
                            ItemBalance.UOMID = FItemBal.UOMID;
                            ItemBalance.FYID = TFYID;
                            ItemBalance.OpeningBalance = FItemBal.ClosingBalance;
                            ItemBalance.REC01 = 0;
                            ItemBalance.REC02 = 0;
                            ItemBalance.REC03 = 0;
                            ItemBalance.REC04 = 0;
                            ItemBalance.REC05 = 0;
                            ItemBalance.REC06 = 0;
                            ItemBalance.REC07 = 0;
                            ItemBalance.REC08 = 0;
                            ItemBalance.REC09 = 0;
                            ItemBalance.REC10 = 0;
                            ItemBalance.REC11 = 0;
                            ItemBalance.REC12 = 0;
                            ItemBalance.ISS01 = 0;
                            ItemBalance.ISS02 = 0;
                            ItemBalance.ISS03 = 0;
                            ItemBalance.ISS04 = 0;
                            ItemBalance.ISS05 = 0;
                            ItemBalance.ISS06 = 0;
                            ItemBalance.ISS07 = 0;
                            ItemBalance.ISS08 = 0;
                            ItemBalance.ISS09 = 0;
                            ItemBalance.ISS10 = 0;
                            ItemBalance.ISS11 = 0;
                            ItemBalance.ISS12 = 0;
                            ItemBalance.CmpID = cmpid;
                            ItemBalance.ClosingBalance = FItemBal.ClosingBalance;
                            ItemBalance.StoreID = FItemBal.StoreID;
                            ItemBalance.CreatedUTC = DateTime.UtcNow;
                            ItemBalance.CreatedBy = Createdby;
                            WYNKContext.OpticalBalance.Add(ItemBalance);
                        }

                    }
                    WYNKContext.SaveChanges();
                    return new
                    {
                        Success = true,
                        Message = "Saved Succesfully"

                    };
                }


            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                    Message = "Something Went Wrong"

                };
            }
        }


    }



}





