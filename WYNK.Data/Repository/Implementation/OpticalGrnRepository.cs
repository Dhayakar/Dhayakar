using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
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
    class OpticalGrnRepository : RepositoryBase<OpticalGrnDataView>, IOpticalGrnRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public OpticalGrnRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetGrn(int val, string Time)
        {
            var GetGrn = new OpticalGrnDataView();

            var Optical = WYNKContext.OpticalOrder.ToList();
            var Vendor = CMPSContext.VendorMaster.ToList();
            var Locations = CMPSContext.LocationMaster.ToList();

            GetGrn.GetOpticalGrn = new List<GetOpticalGrn>();
            TimeSpan ts = TimeSpan.Parse(Time);

            GetGrn.GetOpticalGrn = (from OO in Optical.Where(x => x.IsOrderExecuted == 0 && x.IsCancelled == false && x.CmpID == val)

                                    select new GetOpticalGrn
                                    {
                                        CreateUtc = OO.CreatedUTC,
                                        ID = OO.RandomUniqueID,
                                        OrderNumber = OO.OrderNumber,
                                        OrderDate = OO.OrderDate.Add(ts),
                                        RefNo = OO.RefNo,
                                        RefDate = OO.RefDate,
                                        VendorID = OO.VendorID,
                                        SupplierName = Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.Name).FirstOrDefault(),
                                        SupplierAddress = Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.Address1).FirstOrDefault(),
                                        SupplierAddress1 = OO.VendorID != 0 ? Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.Address2).FirstOrDefault() : string.Empty,
                                        Supplierlocation = Locations.Where(x => x.ID == Vendor.Where(v => v.ID == OO.VendorID).Select(j => j.Location).FirstOrDefault()).Select(r => r.ParentDescription != null ? r.ParentDescription : string.Empty).FirstOrDefault(),
                                        GstNo = OO.VendorID != 0 ? Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.GSTNo).FirstOrDefault() : string.Empty,
                                        PhoneNo = OO.VendorID != 0 ? Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.PhoneNo).FirstOrDefault() : string.Empty,
                                        DeliveryName = OO.DeliveryName,
                                        DeliveryAddress = OO.DeliveryAddress1,
                                        DeliveryAddress1 = OO.DeliveryAddress2 != null ? OO.DeliveryAddress2 : string.Empty,
                                        Deliverylocation = OO.DeliveryLocationID != 0 ? Locations.Where(X => X.ID == OO.DeliveryLocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                    }).ToList();

            return GetGrn;
        }
        public dynamic GetGrnloc(string val)
        {
            var GetGrn = new OpticalGrnDataView();
            var Optical = WYNKContext.OpticalOrder.ToList();
            var Locations = CMPSContext.LocationMaster.ToList();
            var op = Optical.Where(x => x.RandomUniqueID == val).Select(x => x.VendorID).FirstOrDefault();
            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == op).Select(x => x.Location).FirstOrDefault();
            var CityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentID).FirstOrDefault();
            var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault();
            var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault();
            GetGrn.Supplierlocation = LocationID != null ? Locations.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Suppliercity = CityID != null ? Locations.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.SupplierState = StateID != null ? Locations.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Suppliercountry = CountryID != null ? Locations.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            var Location = Optical.Where(x => x.RandomUniqueID == val).Select(x => x.DeliveryLocationID).FirstOrDefault();
            var City = CMPSContext.LocationMaster.Where(x => x.ID == Location).Select(x => x.ParentID).FirstOrDefault();
            var Statee = CMPSContext.LocationMaster.Where(x => x.ID == City).Select(x => x.ParentID).FirstOrDefault();
            var Country = CMPSContext.LocationMaster.Where(x => x.ID == Statee).Select(x => x.ParentID).FirstOrDefault();
            GetGrn.Deliverylocation = Location != null ? Locations.Where(x => x.ID == Location).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Deliverycity = City != null ? Locations.Where(x => x.ID == City).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.DeliveryState = Statee != null ? Locations.Where(x => x.ID == Statee).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Deliverycountry = Country != null ? Locations.Where(x => x.ID == Country).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;


            return GetGrn;
        }
        public dynamic GetGrntrns(string ID)
        {
            var optrns = new OpticalGrnDataView();

            var Opticaltrns = WYNKContext.OpticalOrderTran.ToList();
            var opt = WYNKContext.Lenstrans.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var lens = WYNKContext.Lensmaster.ToList();
            var lenstrns = WYNKContext.Lenstrans.ToList();
            var brand = WYNKContext.Brand.ToList();
            var one = CMPSContext.OneLineMaster.ToList();

            optrns.GetOpticalGrntrns = new List<GetOpticalGrntrns>();


            optrns.GetOpticalGrntrns = (from Optrs in Opticaltrns.Where(x => x.RandomUniqueID == ID && x.IsCancelled == false && x.OrderedQty != x.ReceivedQty)

                                        select new GetOpticalGrntrns
                                        {
                                            RandomUniqueID = Optrs.RandomUniqueID,
                                            LTID = Optrs.LTID,
                                            UOMID = Optrs.UOMID,
                                            UOMname = uom.Where(X => X.id == Optrs.UOMID).Select(x => x.Description).FirstOrDefault(),
                                            OrderedQty = Optrs.OrderedQty,
                                            ReceivedQty = Optrs.ReceivedQty,
                                            DiscountPercentage = Optrs.DiscountPercentage,
                                            Price = Optrs.Price,
                                            Value = (Optrs.OrderedQty - Optrs.ReceivedQty) * (Optrs.Price),
                                            ActualQuantity = Optrs.OrderedQty - Optrs.ReceivedQty,
                                            DiscountAmount = (Optrs.OrderedQty - Optrs.ReceivedQty) * (Optrs.Price) * Optrs.DiscountPercentage / 100,
                                            TotalCost = ((Optrs.OrderedQty - Optrs.ReceivedQty) * (Optrs.Price)) - ((Optrs.OrderedQty - Optrs.ReceivedQty) * (Optrs.Price) * Optrs.DiscountPercentage / 100),
                                            Type = lens.Where(x => x.RandomUniqueID == lenstrns.Where(s => s.ID == Optrs.LTID).Select(a => a.LMID).FirstOrDefault()).Select(f => f.LensType).FirstOrDefault(),
                                            Brand = brand.Where(x => x.ID == lenstrns.Where(s => s.ID == Optrs.LTID).Select(a => a.Brand).FirstOrDefault()).Select(f => f.Description).FirstOrDefault(),
                                            Color = lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Colour).FirstOrDefault(),
                                            Model = lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Model).FirstOrDefault(),
                                            Sph = lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Sph).FirstOrDefault() != null ? "Sph :" + " " + lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Sph).FirstOrDefault() + ";" : null,
                                            Cyl = lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Cyl).FirstOrDefault() != null ? "Cyl :" + " " + lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Cyl).FirstOrDefault() + ";" : null,
                                            Axis = lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Axis).FirstOrDefault() != null ? "Axis :" + " " + lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Axis).FirstOrDefault() + ";" : null,
                                            Add = lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Add).FirstOrDefault() != null ? "Add :" + " " + lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.Add).FirstOrDefault() + ";" : null,
                                            Fshpae = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameShapeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameShapeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                            Ftype = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameTypeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameTypeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                            Fstyle = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameStyleID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameStyleID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                            Fwidth = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameWidthID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LTID).Select(x => x.FrameWidthID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                        }).ToList();


            return optrns;
        }
        public dynamic Insertopticalgrn(OpticalGrnDataView Addopticalgrn, int cmpid, int TransactionTypeid)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var opticalmaster = new OpticalStockMaster();
                    var transcation = CMPSContext.TransactionType.ToList();
                    var lensmas = WYNKContext.Lensmaster.ToList();
                    var lenstrns = WYNKContext.Lenstrans.ToList();
                    var taxmas = CMPSContext.TaxMaster.ToList();
                    var optrns = WYNKContext.OpticalOrderTran.ToList();

                    opticalmaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string RandomUniqueID = opticalmaster.RandomUniqueID;
                    opticalmaster.TransactionID = Addopticalgrn.OpticalStockMaster.TransactionID;
                    opticalmaster.CMPID = cmpid;
                    opticalmaster.DocumentNumber = Addopticalgrn.OpticalStockMaster.DocumentNumber;
                    //var Datee = DateTime.Now;
                    opticalmaster.Fyear = Addopticalgrn.OpticalStockMaster.Fyear;
                    opticalmaster.DocumentDate = Addopticalgrn.OpticalStockMaster.DocumentDate;
                    opticalmaster.OpticalOrderID = Addopticalgrn.OpticalStockMaster.OpticalOrderID;
                    opticalmaster.StoreID = Addopticalgrn.OpticalStockMaster.StoreID;
                    opticalmaster.TransactionType = transcation.Where(X => X.TransactionID == TransactionTypeid).Select(x => x.Rec_Issue_type).FirstOrDefault();
                    opticalmaster.VendorID = Addopticalgrn.OpticalStockMaster.VendorID;
                    opticalmaster.DepartmentID = Addopticalgrn.OpticalStockMaster.DepartmentID;
                    opticalmaster.TotalPOValue = Addopticalgrn.OpticalStockMaster.TotalPOValue;
                    opticalmaster.TotalDiscountValue = Addopticalgrn.OpticalStockMaster.TotalDiscountValue;
                    opticalmaster.IsCancelled = false;
                    opticalmaster.TermsConditions = Addopticalgrn.OpticalStockMaster.TermsConditions;
                    opticalmaster.IsDeleted = false;
                    opticalmaster.CreatedUTC = DateTime.UtcNow;
                    opticalmaster.CreatedBy = Addopticalgrn.OpticalStockMaster.CreatedBy;
                    WYNKContext.OpticalStockMaster.AddRange(opticalmaster);


                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == Addopticalgrn.OpticalStockMaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(Addopticalgrn.OpticalStockMaster.CreatedBy);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Optical GRN", "User name :", username, "User ID :", userid, "Mode : Submit");

                    object names = opticalmaster;
                    oErrorLogs.WriteErrorLogArray("OpticalStockMaster", names);


                    if (Addopticalgrn.GetOpticalGrntrns.Count() > 0)
                    {
                        foreach (var item in Addopticalgrn.GetOpticalGrntrns.ToList())
                        {
                            var opticaltrans = new OpticalStockTran();
                            opticaltrans.RandomUniqueID = RandomUniqueID;
                            opticaltrans.LMIDID = item.LTID;
                            opticaltrans.ItemQty = item.ActualQuantity;
                            opticaltrans.UOMID = item.UOMID;
                            opticaltrans.ItemRate = item.Price;
                            opticaltrans.ItemValue = item.Value;
                            opticaltrans.DiscountPercentage = item.DiscountPercentage;
                            opticaltrans.DiscountAmount = item.DiscountAmount;
                            opticaltrans.POID = 5;
                            opticaltrans.IsDeleted = false;
                            opticaltrans.CreatedUTC = DateTime.UtcNow;
                            opticaltrans.CreatedBy = opticalmaster.CreatedBy;
                            WYNKContext.OpticalStockTran.AddRange(opticaltrans);
                            ErrorLog oErrorLogstran = new ErrorLog();
                            object namestr = opticaltrans;
                            oErrorLogstran.WriteErrorLogArray("OpticalStockTran", namestr);
                        }
                    }
                    if (Addopticalgrn.GetOpticalGrntrns.Count() > 0)
                    {
                        foreach (var item in Addopticalgrn.GetOpticalGrntrns.ToList())
                        {
                            var opticaltrans = new OpticalOrderTran();
                            var opid = WYNKContext.OpticalOrderTran.Where(x => x.RandomUniqueID == item.RandomUniqueID && x.LTID == item.LTID).ToList();
                            if (opid != null)
                            {
                                opid.All(x =>
                                {
                                    x.ReceivedQty = item.ActualQuantity + item.ReceivedQty;
                                    x.UpdatedUTC = DateTime.UtcNow;
                                    x.UpdatedBy = opticalmaster.CreatedBy;
                                    return true;
                                });
                                WYNKContext.OpticalOrderTran.UpdateRange(opid);
                                ErrorLog OpticalOrderTran = new ErrorLog();
                                object Optical = opid;
                                OpticalOrderTran.WriteErrorLogArray("OpticalOrderTran", Optical);
                                WYNKContext.SaveChanges();
                            }

                            var match = false;
                            var id = WYNKContext.OpticalOrderTran.Where(x => x.RandomUniqueID == item.RandomUniqueID).Select(x => x.OrderedQty).ToList();
                            var ids = WYNKContext.OpticalOrderTran.Where(x => x.RandomUniqueID == item.RandomUniqueID).Select(x => x.ReceivedQty).ToList();
                            if (id.SequenceEqual(ids))
                            {
                                match = true;
                            }
                            else
                            {
                                match = false;
                            }

                            if (match == true)
                            {
                                var opm = WYNKContext.OpticalOrder.Where(x => x.RandomUniqueID == item.RandomUniqueID).ToList();
                                if (opm != null)
                                {
                                    opm.All(x =>
                                    {
                                        x.IsOrderExecuted = 1;
                                        x.UpdatedUTC = DateTime.UtcNow;
                                        x.UpdatedBy = opticalmaster.CreatedBy;
                                        return true;
                                    });
                                    WYNKContext.OpticalOrder.UpdateRange(opm);
                                    object Oporder = opm;
                                    oErrorLogs.WriteErrorLogArray("OpticalOrder", Oporder);
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                    //var Date = DateTime.Now;
                    //var CurrentMonth = Date.Month;
                    //var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                    //if (FinancialYearId == 0)
                    //{
                    //    dbContextTransaction.Rollback();
                    //    return new
                    //    {
                    //        Success = false,
                    //        Message = "Financial year doesn't exists",
                    //    };
                    //}

                        if (Addopticalgrn.GetOpticalGrntrns.Count() > 0)
                        {
                            foreach (var item in Addopticalgrn.GetOpticalGrntrns.ToList())
                            {
                                //var ItemBalance = WYNKContext.OpticalBalance.Where(x => x.FYID == FinancialYearId && x.LTID == item.LTID && x.StoreID == Addopticalgrn.OpticalStockMaster.StoreID && x.CmpID == cmpid).FirstOrDefault();

                            var Date = DateTime.Now;
                            var CurrentMonth = Date.Month;
                            var FinancialYearId = WYNKContext.FinancialYear.Where(x => x.FYAccYear == Convert.ToInt32(Addopticalgrn.OpticalStockMaster.Fyear) && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
                            var ItemBalance = WYNKContext.OpticalBalance.Where(x => x.FYID == Convert.ToInt32(Addopticalgrn.OpticalStockMaster.Fyear) && x.LTID == item.LTID && x.StoreID == Addopticalgrn.OpticalStockMaster.StoreID && x.CmpID == cmpid).FirstOrDefault();


                            if (ItemBalance != null)
                                {
                                    switch (CurrentMonth)
                                    {
                                        case 1:
                                            ItemBalance.REC01 = ItemBalance.REC01 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 2:
                                            ItemBalance.REC02 = ItemBalance.REC02 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 3:
                                            ItemBalance.REC03 = ItemBalance.REC03 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 4:
                                            ItemBalance.REC04 = ItemBalance.REC04 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 5:
                                            ItemBalance.REC05 = ItemBalance.REC05 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 6:
                                            ItemBalance.REC06 = ItemBalance.REC06 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 7:
                                            ItemBalance.REC07 = ItemBalance.REC07 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 8:
                                            ItemBalance.REC08 = ItemBalance.REC08 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 9:
                                            ItemBalance.REC09 = ItemBalance.REC09 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 10:
                                            ItemBalance.REC10 = ItemBalance.REC10 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 11:
                                            ItemBalance.REC11 = ItemBalance.REC11 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                        case 12:
                                            ItemBalance.REC12 = ItemBalance.REC12 + Convert.ToInt32(item.ActualQuantity);
                                            break;
                                    }

                                    ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToInt32(item.ActualQuantity);
                                    ItemBalance.StoreID = Addopticalgrn.OpticalStockMaster.StoreID;
                                    ItemBalance.UpdatedBy = opticalmaster.CreatedBy;
                                    ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                    WYNKContext.OpticalBalance.UpdateRange(ItemBalance);
                                    object IB = ItemBalance;
                                    oErrorLogs.WriteErrorLogArray("OpticalBalance", names);
                                }
                                else
                                {
                                    var closebal = WYNKContext.OpticalBalance.Where(x => x.LTID == item.LTID && x.CmpID == cmpid).Sum(x => x.ClosingBalance);
                                    if (closebal != 0)
                                    {
                                        var storeid = WYNKContext.OpticalBalance.Where(x => x.LTID == item.LTID && x.StoreID != Addopticalgrn.OpticalStockMaster.StoreID && x.FYID == FinancialYearId).Select(x => x.StoreID).FirstOrDefault();
                                        if (storeid != 0)
                                        {
                                            var ItemBalance1 = new OpticalBalance();

                                            switch (CurrentMonth)
                                            {

                                                case 1:
                                                    ItemBalance1.REC01 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 2:
                                                    ItemBalance1.REC02 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 3:
                                                    ItemBalance1.REC03 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 4:
                                                    ItemBalance1.REC04 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 5:
                                                    ItemBalance1.REC05 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 6:
                                                    ItemBalance1.REC06 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 7:
                                                    ItemBalance1.REC07 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 8:
                                                    ItemBalance1.REC08 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 9:
                                                    ItemBalance1.REC09 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 10:
                                                    ItemBalance1.REC10 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 11:
                                                    ItemBalance1.REC11 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 12:
                                                    ItemBalance1.REC12 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                            }

                                            ItemBalance1.ISS01 = 0;
                                            ItemBalance1.ISS02 = 0;
                                            ItemBalance1.ISS03 = 0;
                                            ItemBalance1.ISS04 = 0;
                                            ItemBalance1.ISS05 = 0;
                                            ItemBalance1.ISS06 = 0;
                                            ItemBalance1.ISS07 = 0;
                                            ItemBalance1.ISS08 = 0;
                                            ItemBalance1.ISS09 = 0;
                                            ItemBalance1.ISS10 = 0;
                                            ItemBalance1.ISS11 = 0;
                                            ItemBalance1.ISS12 = 0;
                                            ItemBalance1.LTID = item.LTID;
                                            ItemBalance1.UOMID = item.UOMID;
                                            ItemBalance1.FYID = FinancialYearId;
                                            ItemBalance1.OpeningBalance = 0;
                                            ItemBalance1.StoreID = Addopticalgrn.OpticalStockMaster.StoreID;
                                            ItemBalance1.ClosingBalance = Convert.ToInt32(item.ActualQuantity);
                                            ItemBalance1.CreatedBy = opticalmaster.CreatedBy;
                                            ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                            ItemBalance1.CmpID = cmpid;
                                            WYNKContext.OpticalBalance.AddRange(ItemBalance1);
                                            object IB1 = ItemBalance1;
                                            oErrorLogs.WriteErrorLogArray("OpticalBalance", IB1);


                                        }
                                        else
                                        {

                                            var ItemBalance1 = new OpticalBalance();

                                            switch (CurrentMonth)
                                            {
                                                case 1:
                                                    ItemBalance1.REC01 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 2:
                                                    ItemBalance1.REC02 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 3:
                                                    ItemBalance1.REC03 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 4:
                                                    ItemBalance1.REC04 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 5:
                                                    ItemBalance1.REC05 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 6:
                                                    ItemBalance1.REC06 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 7:
                                                    ItemBalance1.REC07 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 8:
                                                    ItemBalance1.REC08 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 9:
                                                    ItemBalance1.REC09 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 10:
                                                    ItemBalance1.REC10 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 11:
                                                    ItemBalance1.REC11 = Convert.ToInt32(item.ActualQuantity);
                                                    break;
                                                case 12:
                                                    ItemBalance1.REC12 = Convert.ToInt32(item.ActualQuantity);
                                                    break;

                                            }

                                            ItemBalance1.ISS01 = 0;
                                            ItemBalance1.ISS02 = 0;
                                            ItemBalance1.ISS03 = 0;
                                            ItemBalance1.ISS04 = 0;
                                            ItemBalance1.ISS05 = 0;
                                            ItemBalance1.ISS06 = 0;
                                            ItemBalance1.ISS07 = 0;
                                            ItemBalance1.ISS08 = 0;
                                            ItemBalance1.ISS09 = 0;
                                            ItemBalance1.ISS10 = 0;
                                            ItemBalance1.ISS11 = 0;
                                            ItemBalance1.ISS12 = 0;
                                            ItemBalance1.LTID = 0;
                                            ItemBalance1.LTID = item.LTID;
                                            ItemBalance1.UOMID = item.UOMID;
                                            ItemBalance1.FYID = FinancialYearId;
                                            ItemBalance1.OpeningBalance = 0;
                                            ItemBalance1.StoreID = Addopticalgrn.OpticalStockMaster.StoreID;
                                            ItemBalance1.ClosingBalance = Convert.ToInt32(item.ActualQuantity);
                                            ItemBalance1.CreatedBy = opticalmaster.CreatedBy;
                                            ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                            ItemBalance1.CmpID = cmpid;
                                            WYNKContext.OpticalBalance.AddRange(ItemBalance1);
                                            object IB2 = ItemBalance1;
                                            oErrorLogs.WriteErrorLogArray("OpticalBalance", IB2);

                                        }
                                    }
                                    else
                                    {
                                        var ItemBalance1 = new OpticalBalance();
                                        switch (CurrentMonth)
                                        {
                                            case 1:
                                                ItemBalance1.REC01 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 2:
                                                ItemBalance1.REC02 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 3:
                                                ItemBalance1.REC03 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 4:
                                                ItemBalance1.REC04 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 5:
                                                ItemBalance1.REC05 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 6:
                                                ItemBalance1.REC06 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 7:
                                                ItemBalance1.REC07 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 8:
                                                ItemBalance1.REC08 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 9:
                                                ItemBalance1.REC09 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 10:
                                                ItemBalance1.REC10 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 11:
                                                ItemBalance1.REC11 = Convert.ToInt32(item.ActualQuantity);
                                                break;
                                            case 12:
                                                ItemBalance1.REC12 = Convert.ToInt32(item.ActualQuantity);
                                                break;

                                        }

                                        ItemBalance1.ISS01 = 0;
                                        ItemBalance1.ISS02 = 0;
                                        ItemBalance1.ISS03 = 0;
                                        ItemBalance1.ISS04 = 0;
                                        ItemBalance1.ISS05 = 0;
                                        ItemBalance1.ISS06 = 0;
                                        ItemBalance1.ISS07 = 0;
                                        ItemBalance1.ISS08 = 0;
                                        ItemBalance1.ISS09 = 0;
                                        ItemBalance1.ISS10 = 0;
                                        ItemBalance1.ISS11 = 0;
                                        ItemBalance1.ISS12 = 0;
                                        ItemBalance1.LTID = item.LTID;
                                        ItemBalance1.UOMID = item.UOMID;
                                        ItemBalance1.FYID = FinancialYearId;
                                        ItemBalance1.OpeningBalance = 0;
                                        ItemBalance1.StoreID = Addopticalgrn.OpticalStockMaster.StoreID;
                                        ItemBalance1.ClosingBalance = Convert.ToInt32(item.ActualQuantity);
                                        ItemBalance1.CreatedBy = opticalmaster.CreatedBy;
                                        ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                        ItemBalance1.CmpID = cmpid;
                                        WYNKContext.OpticalBalance.AddRange(ItemBalance1);
                                        object IB3 = ItemBalance1;
                                        oErrorLogs.WriteErrorLogArray("OpticalBalance", IB3);

                                    }
                                }
                            }
                        }



                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");

                    if (RunningNumber == Addopticalgrn.OpticalStockMaster.DocumentNumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");

                        var opticalno = WYNKContext.OpticalStockMaster.Where(x => x.DocumentNumber == Addopticalgrn.OpticalStockMaster.DocumentNumber).FirstOrDefault();
                        opticalno.DocumentNumber = GetRunningNumber;
                        WYNKContext.OpticalStockMaster.UpdateRange(opticalno);
                        WYNKContext.SaveChanges();
                    }



                    return new
                    {
                        Success = true,
                        RandomUniqueID = opticalmaster.RandomUniqueID
                    };
                }

                catch (Exception ex)
                {

                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = Addopticalgrn.OpticalStockMaster.DocumentNumber };
                }
            }
        }
        public dynamic GetGrndetails(int val, string Time)
        {
            var GetGrn = new OpticalGrnDataView();

            var Optical = WYNKContext.OpticalOrder.ToList();
            var Opticalstock = WYNKContext.OpticalStockMaster.ToList();
            var Vendor = CMPSContext.VendorMaster.ToList();
            var Locations = CMPSContext.LocationMaster.ToList();
            var store = CMPSContext.Storemasters.ToList();

            GetGrn.GetOpticalGrndetails = new List<GetOpticalGrndetails>();
            TimeSpan ts = TimeSpan.Parse(Time);


            GetGrn.GetOpticalGrndetails = (from OO in Optical.Where(u => u.CmpID == val)
                                           join OS in Opticalstock on OO.RandomUniqueID equals OS.OpticalOrderID

                                           select new GetOpticalGrndetails
                                           {
                                               CreateUtc = OO.CreatedUTC,
                                               RandomUniqueID = OS.RandomUniqueID,
                                               GrnNumber = OS.DocumentNumber,
                                               GrnDate = OS.DocumentDate.Value.Add(ts),
                                               OrderNumber = OO.OrderNumber,
                                               OrderDate = OO.OrderDate.Add(ts),
                                               RefNo = OO.RefNo,
                                               RefDate = OO.RefDate,
                                               store = store.Where(X => X.StoreID == OS.StoreID).Select(x => x.Storename).FirstOrDefault(),
                                               Termcondition = OS.TermsConditions,
                                               SupplierName = Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.Name).FirstOrDefault(),
                                               SupplierAddress = Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.Address1).FirstOrDefault(),
                                               SupplierAddress1 = OO.VendorID != 0 ? Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.Address2).FirstOrDefault() : string.Empty,
                                               GstNo = OO.VendorID != 0 ? Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.GSTNo).FirstOrDefault() : string.Empty,
                                               PhoneNo = OO.VendorID != 0 ? Vendor.Where(X => X.ID == OO.VendorID).Select(x => x.PhoneNo).FirstOrDefault() : string.Empty,
                                               Supplierlocation = Locations.Where(x => x.ID == Vendor.Where(v => v.ID == OO.VendorID).Select(j => j.Location).FirstOrDefault()).Select(r => r.ParentDescription != null ? r.ParentDescription : string.Empty).FirstOrDefault(),
                                               DeliveryName = OO.DeliveryName,
                                               DeliveryAddress = OO.DeliveryAddress1,
                                               DeliveryAddress1 = OO.DeliveryAddress2 != null ? OO.DeliveryAddress2 : string.Empty,
                                               Deliverylocation = OO.DeliveryLocationID != 0 ? Locations.Where(X => X.ID == OO.DeliveryLocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty
                                           }).ToList();

            return GetGrn;
        }
        public dynamic GetGrnlocation(string RandomUniqueID)
        {
            var GetGrn = new OpticalGrnDataView();
            var Optical = WYNKContext.OpticalOrder.ToList();
            var Opticalstock = WYNKContext.OpticalStockMaster.ToList();
            var Locations = CMPSContext.LocationMaster.ToList();
            var op = Opticalstock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault();
            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == op).Select(x => x.Location).FirstOrDefault();
            var CityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentID).FirstOrDefault();
            var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault();
            var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault();
            GetGrn.Supplierlocation = LocationID != 0 ? Locations.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Suppliercity = CityID != null ? Locations.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.SupplierState = StateID != null ? Locations.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Suppliercountry = CountryID != null ? Locations.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            var Location = Optical.Where(z => z.RandomUniqueID == Opticalstock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.OpticalOrderID).FirstOrDefault()).Select(z => z.DeliveryLocationID).FirstOrDefault();
            var City = CMPSContext.LocationMaster.Where(x => x.ID == Location).Select(x => x.ParentID).FirstOrDefault();
            var Statee = CMPSContext.LocationMaster.Where(x => x.ID == City).Select(x => x.ParentID).FirstOrDefault();
            var Country = CMPSContext.LocationMaster.Where(x => x.ID == Statee).Select(x => x.ParentID).FirstOrDefault();
            GetGrn.Deliverylocation = Location != 0 ? Locations.Where(x => x.ID == Location).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Deliverycity = City != null ? Locations.Where(x => x.ID == City).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.DeliveryState = Statee != null ? Locations.Where(x => x.ID == Statee).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Deliverycountry = Country != null ? Locations.Where(x => x.ID == Country).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            return GetGrn;
        }
        public dynamic GetGrntrnsdetails(string RandomUniqueID)
        {
            var optrns = new OpticalGrnDataView();

            var Opticalstocktrns = WYNKContext.OpticalStockTran.ToList();
            var OpticalStockMaster = WYNKContext.OpticalStockMaster.ToList();
            var Opticalorder = WYNKContext.OpticalOrder.ToList();
            var Opticalordertran = WYNKContext.OpticalOrderTran.ToList();
            var opt = WYNKContext.Lenstrans.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var lens = WYNKContext.Lensmaster.ToList();
            var lenstrns = WYNKContext.Lenstrans.ToList();
            var brand = WYNKContext.Brand.ToList();
            var one = CMPSContext.OneLineMaster.ToList();

            optrns.GetOpticalGrntrnsstock = new List<GetOpticalGrntrnsstock>();

            optrns.TotalPOValue = OpticalStockMaster.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();

            optrns.GetOpticalGrntrnsstock = (from Optrs in Opticalstocktrns.Where(x => x.RandomUniqueID == RandomUniqueID)

                                             select new GetOpticalGrntrnsstock
                                             {
                                                 UOMname = uom.Where(X => X.id == Optrs.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                 ItemQty = Optrs.ItemQty,
                                                 Itemrate = Optrs.ItemRate,
                                                 Itemvalue = Optrs.ItemValue,
                                                 OrderedQty = Opticalordertran.Where(q => q.RandomUniqueID == Opticalorder.Where(x => x.RandomUniqueID == OpticalStockMaster.Where(s => s.RandomUniqueID == RandomUniqueID).Select(s => s.OpticalOrderID).FirstOrDefault()).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(q => q.OrderedQty).FirstOrDefault(),
                                                 PendingQty = (Opticalordertran.Where(q => q.RandomUniqueID == Opticalorder.Where(x => x.RandomUniqueID == OpticalStockMaster.Where(s => s.RandomUniqueID == RandomUniqueID).Select(s => s.OpticalOrderID).FirstOrDefault()).Select(x => x.RandomUniqueID).FirstOrDefault()).Select(q => q.OrderedQty).FirstOrDefault() - Optrs.ItemQty),
                                                 DiscountAmount = Optrs.DiscountAmount,
                                                 DiscountPercentage = Optrs.DiscountPercentage,
                                                 TotalCost = Optrs.ItemValue - Optrs.DiscountAmount,
                                                 Type = lens.Where(x => x.RandomUniqueID == lenstrns.Where(s => s.ID == Optrs.LMIDID).Select(a => a.LMID).FirstOrDefault()).Select(f => f.LensType).FirstOrDefault(),
                                                 Brand = brand.Where(x => x.ID == lenstrns.Where(s => s.ID == Optrs.LMIDID).Select(a => a.Brand).FirstOrDefault()).Select(f => f.Description).FirstOrDefault(),
                                                 Color = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Colour).FirstOrDefault(),
                                                 Model = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Model).FirstOrDefault(),
                                                 Sph = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Sph).FirstOrDefault() != null ? "Sph :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Sph).FirstOrDefault() + ";" : null,
                                                 Cyl = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Cyl).FirstOrDefault() != null ? "Cyl :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Cyl).FirstOrDefault() + ";" : null,
                                                 Axis = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Axis).FirstOrDefault() != null ? "Axis :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Axis).FirstOrDefault() + ";" : null,
                                                 Add = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Add).FirstOrDefault() != null ? "Add :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Add).FirstOrDefault() + ";" : null,
                                                 Fshpae = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameShapeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameShapeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                 Ftype = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameTypeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameTypeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                 Fstyle = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameStyleID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameStyleID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                 Fwidth = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameWidthID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameWidthID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                             }).ToList();


            return optrns;
        }
        public dynamic Opticalgrnprint(string RandomUniqueID, int CMPID, string Time)
        {
            var company = CMPSContext.Company.ToList();
            var optical = WYNKContext.OpticalOrder.ToList();
            var stock = WYNKContext.OpticalStockMaster.ToList();
            var Locations = CMPSContext.LocationMaster.ToList();
            var Vendor = CMPSContext.VendorMaster.ToList();
            var opt = WYNKContext.Lenstrans.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var taxMaster = CMPSContext.TaxMaster.ToList();
            var lens = WYNKContext.Lensmaster.ToList();
            var lenstrns = WYNKContext.Lenstrans.ToList();
            var brand = WYNKContext.Brand.ToList();
            var Opticalstocktrns = WYNKContext.OpticalStockTran.ToList();
            var one = CMPSContext.OneLineMaster.ToList();

            var GetGrn = new OpticalGrnDataView();
            TimeSpan ts = TimeSpan.Parse(Time);

            GetGrn.Compnayname = company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();
            GetGrn.companyAddress = company.Where(x => x.CmpID == CMPID).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault();
            GetGrn.companyAddress1 = company.Where(x => x.CmpID == CMPID).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault();
            GetGrn.companyAddress2 = company.Where(x => x.CmpID == CMPID).Select(x => x.Address3 != null ? x.Address3 : string.Empty).FirstOrDefault();
            GetGrn.phone = company.Where(x => x.CmpID == CMPID).Select(x => x.Phone1 != null ? x.Phone1 : string.Empty).FirstOrDefault();
            GetGrn.web = company.Where(x => x.CmpID == CMPID).Select(x => x.Website != null ? x.Website : string.Empty).FirstOrDefault();

            GetGrn.OpOrdernumber = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.OrderNumber).FirstOrDefault();
            GetGrn.OpOrderDate = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.OrderDate).FirstOrDefault().Add(ts);
            GetGrn.Refnumber = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.RefNo).FirstOrDefault();
            GetGrn.RefDate = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.RefDate).FirstOrDefault();
            GetGrn.Documentnumber = stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.DocumentNumber).FirstOrDefault();
            GetGrn.DocumentDate = stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.DocumentDate).FirstOrDefault().Value.Add(ts);
            GetGrn.SupplierName = Vendor.Where(X => X.ID == stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault()).Select(x => x.Name).FirstOrDefault();
            GetGrn.SupplierAddress = Vendor.Where(X => X.ID == stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault()).Select(x => x.Address1 != null ? x.Address1 : string.Empty).FirstOrDefault();
            GetGrn.SupplierAddress1 = Vendor.Where(X => X.ID == stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault()).Select(x => x.Address2 != null ? x.Address2 : string.Empty).FirstOrDefault();
            GetGrn.GstNo = Vendor.Where(X => X.ID == stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault()).Select(x => x.GSTNo != null ? x.GSTNo : string.Empty).FirstOrDefault();
            GetGrn.PhoneNo = Vendor.Where(X => X.ID == stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault()).Select(x => x.PhoneNo != null ? x.PhoneNo : string.Empty).FirstOrDefault();
            GetGrn.DeliveryName = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.DeliveryName).FirstOrDefault();
            GetGrn.DeliveryAddress = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.DeliveryAddress1 != null ? x.DeliveryAddress1 : string.Empty).FirstOrDefault();
            GetGrn.DeliveryAddress1 = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.DeliveryAddress2 != null ? x.DeliveryAddress2 : string.Empty).FirstOrDefault();

            var op = stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault();
            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == op).Select(x => x.Location).FirstOrDefault();
            var CityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentID).FirstOrDefault();
            var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault();
            var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault();

            GetGrn.Supplierlocation = LocationID != 0 ? Locations.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Suppliercity = CityID != null ? Locations.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.SupplierState = StateID != null ? Locations.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Suppliercountry = CountryID != null ? Locations.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

            var Location = optical.Where(x => x.RandomUniqueID == stock.Where(y => y.RandomUniqueID == RandomUniqueID).Select(y => y.OpticalOrderID).FirstOrDefault()).Select(x => x.DeliveryLocationID).FirstOrDefault();
            var City = CMPSContext.LocationMaster.Where(x => x.ID == Location).Select(x => x.ParentID).FirstOrDefault();
            var Statee = CMPSContext.LocationMaster.Where(x => x.ID == City).Select(x => x.ParentID).FirstOrDefault();
            var Country = CMPSContext.LocationMaster.Where(x => x.ID == Statee).Select(x => x.ParentID).FirstOrDefault();

            GetGrn.Deliverylocation = Location != 0 ? Locations.Where(x => x.ID == Location).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Deliverycity = City != null ? Locations.Where(x => x.ID == City).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.DeliveryState = Statee != null ? Locations.Where(x => x.ID == Statee).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            GetGrn.Deliverycountry = Country != null ? Locations.Where(x => x.ID == Country).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

            GetGrn.TotalPOValue = stock.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();

            GetGrn.GetOpticalGrntrnsstockprint = (from Optrs in Opticalstocktrns.Where(x => x.RandomUniqueID == RandomUniqueID)

                                                  select new GetOpticalGrntrnsstockprint
                                                  {
                                                      UOMname = uom.Where(X => X.id == Optrs.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                      ItemQty = Optrs.ItemQty,
                                                      Itemrate = Optrs.ItemRate,
                                                      Itemvalue = Optrs.ItemValue,
                                                      DiscountAmount = Optrs.DiscountAmount,
                                                      DiscountPercentage = Optrs.DiscountPercentage,
                                                      TotalCost = Optrs.ItemValue - Optrs.DiscountAmount,
                                                      Type = lens.Where(x => x.RandomUniqueID == lenstrns.Where(s => s.ID == Optrs.LMIDID).Select(a => a.LMID).FirstOrDefault()).Select(f => f.LensType).FirstOrDefault(),
                                                      Brand = brand.Where(x => x.ID == lenstrns.Where(s => s.ID == Optrs.LMIDID).Select(a => a.Brand).FirstOrDefault()).Select(f => f.Description).FirstOrDefault(),
                                                      Color = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Colour).FirstOrDefault(),
                                                      Model = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Model).FirstOrDefault(),
                                                      Sph = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Sph).FirstOrDefault() != null ? "Sph :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Sph).FirstOrDefault() + ";" : null,
                                                      Cyl = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Cyl).FirstOrDefault() != null ? "Cyl :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Cyl).FirstOrDefault() + ";" : null,
                                                      Axis = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Axis).FirstOrDefault() != null ? "Axis :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Axis).FirstOrDefault() + ";" : null,
                                                      Add = lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Add).FirstOrDefault() != null ? "Add :" + " " + lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.Add).FirstOrDefault() + ";" : null,
                                                      Fshpae = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameShapeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Shape :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameShapeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                      Ftype = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameTypeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Type :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameTypeID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                      Fstyle = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameStyleID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Style :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameStyleID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                      Fwidth = one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameWidthID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() != null ? "Width :" + " " + one.Where(f => f.OLMID == lenstrns.Where(X => X.ID == Optrs.LMIDID).Select(x => x.FrameWidthID).FirstOrDefault()).Select(g => g.ParentDescription).FirstOrDefault() + ";" : null,
                                                  }).ToList();


            try
            {

                return new
                {
                    pcompanyAddress = GetGrn.companyAddress,
                    pcompanyAddress1 = GetGrn.companyAddress1,
                    pcompanyAddress2 = GetGrn.companyAddress2,
                    pphone = GetGrn.phone,
                    pweb = GetGrn.web,
                    pCompnayname = GetGrn.Compnayname,
                    pOpOrdernumber = GetGrn.OpOrdernumber,
                    pOpOrderDate = GetGrn.OpOrderDate,
                    pRefnumber = GetGrn.Refnumber,
                    pRefDate = GetGrn.RefDate,
                    pDocumentnumber = GetGrn.Documentnumber,
                    pDocumentDate = GetGrn.DocumentDate,
                    pSupplierName = GetGrn.SupplierName,
                    pSupplierAddress = GetGrn.SupplierAddress,
                    pSupplierAddress1 = GetGrn.SupplierAddress1,
                    pGstNo = GetGrn.GstNo,
                    pPhoneNo = GetGrn.PhoneNo,
                    pDeliveryName = GetGrn.DeliveryName,
                    pDeliveryAddress = GetGrn.DeliveryAddress,
                    pDeliveryAddress1 = GetGrn.DeliveryAddress1,
                    pSupplierlocation = GetGrn.Supplierlocation,
                    pSuppliercity = GetGrn.Suppliercity,
                    pSupplierState = GetGrn.SupplierState,
                    pSuppliercountry = GetGrn.Suppliercountry,
                    pDeliverylocation = GetGrn.Deliverylocation,
                    pDeliverycity = GetGrn.Deliverycity,
                    pDeliveryState = GetGrn.DeliveryState,
                    pDeliverycountry = GetGrn.Deliverycountry,
                    pTotalPOValue = GetGrn.TotalPOValue,
                    optransdetails = GetGrn.GetOpticalGrntrnsstockprint.ToList(),

                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
            };

        }







    }
}

