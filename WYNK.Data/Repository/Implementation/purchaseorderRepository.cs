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
    class purchaseorderRepository : RepositoryBase<purchaseorderview>, IpurchaseorderRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public purchaseorderRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetvenderDetails(int Name, int cmpid)
        {
            var VenderDetails = new purchaseorderview();
            VenderDetails.Vendername = new List<Vendername>();
            var location = CMPSContext.LocationMaster.ToList();

            VenderDetails.PurchaseOrder = new PurchaseOrder();

            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == Name && x.CmpID == cmpid).Select(x => x.Location).FirstOrDefault();
            var CityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentID).FirstOrDefault();
            var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault();
            var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault();
            var ParentDescriptionstatee = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            var ParentDescriptioncountryy = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

            VenderDetails.Vendername = (from REF in CMPSContext.VendorMaster.Where(u => u.ID == Name && u.CmpID == cmpid)
                                        select new Vendername
                                        {
                                            ID = REF.ID,
                                            Address1 = REF.Address1 != null ? REF.Address1 : string.Empty,
                                            Address2 = REF.Address2 != null ? REF.Address2 : string.Empty,
                                            Location = location.Where(x => x.ID == REF.Location).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == REF.Location).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                            City = location.Where(x => x.ID == location.Where(y => y.ID == REF.Location).Select(y => y.ParentID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == location.Where(y => y.ID == REF.Location).Select(y => y.ParentID).FirstOrDefault()).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                            PhoneNo = REF.PhoneNo != null ? REF.PhoneNo : string.Empty,
                                            GSTNo = REF.GSTNo != null ? REF.GSTNo : string.Empty,
                                            DLNo = REF.DLNo != null ? REF.DLNo : string.Empty,
                                            DLNoDate = REF.DLNoDate != null ? REF.DLNoDate.Value : (DateTime?)null,
                                        }).ToList();


            try
            {

                return new
                {

                    state = ParentDescriptionstatee,
                    country = ParentDescriptioncountryy,
                    venderdetails = VenderDetails.Vendername.ToList(),


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
        public dynamic Getcompantdetails(int cmpid)
        {

            var company = CMPSContext.Company.Where(x => x.CmpID == cmpid).ToList();

            return company;
        }
        public dynamic GetlocationDetails(int id)
        {

            var locDetails = new purchaseorderview();

            var ID = CMPSContext.LocationMaster.Where(x => x.ID == id && x.ParentTag == "loc").Select(f => f.ParentID).FirstOrDefault();

            if (ID != null)
            {

                var v = CMPSContext.LocationMaster.Where(x => x.ID == ID).Select(x => x.ParentDescription).FirstOrDefault();
                var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
                locDetails.ParentDescriptionstatee = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault() != null ? CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

                var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstatee).Select(x => x.ParentID).FirstOrDefault();
                locDetails.ParentDescriptioncountryy = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault() != null ? CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

            }
            else
            {
                var v = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
                var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
                locDetails.ParentDescriptionstatee = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault() != null ? CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

                var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstatee).Select(x => x.ParentID).FirstOrDefault();
                locDetails.ParentDescriptioncountryy = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault() != null ? CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }



            try
            {

                return new
                {

                    ParentDescriptionstate = locDetails.ParentDescriptionstatee,
                    ParentDescriptioncountry = locDetails.ParentDescriptioncountryy,

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
        public dynamic GetdrugDetails(int ID, int cmpid)

        {
            var DrugDetails = new purchaseorderview();
            DrugDetails.purchaseorderdetails = new List<purchaseorderdetails>();

            var taxid = WYNKContext.DrugMaster.Where(x => x.ID == ID).Select(x => x.TaxID).FirstOrDefault();
            var uom = CMPSContext.uommaster.ToList();
            var DM = WYNKContext.DrugMaster.ToList();
            var taxmaster = CMPSContext.TaxMaster.ToList();
            DrugDetails.purchaseorderdetails = (from REF in DM.Where(x => x.ID == ID && x.Cmpid == cmpid)

                                                select new purchaseorderdetails
                                                {
                                                    Drugname = REF.Brand,
                                                    DrugID = Convert.ToString(REF.ID),
                                                    purchaseid = uom.Where(x => x.Description == REF.UOM).Select(x => x.id).FirstOrDefault() != 0 ? uom.Where(x => x.Description == REF.UOM).Select(x => x.id).FirstOrDefault() : 0,
                                                    PurchaseUOM = uom.Where(x => x.Description == REF.UOM).Select(x => x.Description).FirstOrDefault() != null ? uom.Where(x => x.Description == REF.UOM).Select(x => x.Description).FirstOrDefault() : string.Empty,
                                                    UnitPrice = REF.Rate,
                                                    GST = taxmaster.Where(x => x.ID == taxid).Select(x => x.GSTPercentage).FirstOrDefault() != null ? taxmaster.Where(x => x.ID == taxid).Select(x => x.GSTPercentage).FirstOrDefault() : 0,
                                                    CESS = taxmaster.Where(x => x.ID == taxid).Select(x => x.CESSPercentage).FirstOrDefault() != null ? taxmaster.Where(x => x.ID == taxid).Select(x => x.CESSPercentage).FirstOrDefault() : 0,
                                                    AdditionalCESS = taxmaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSPercentage).FirstOrDefault() != null ? taxmaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSPercentage).FirstOrDefault() : 0,
                                                    TaxDescription = taxmaster.Where(x => x.ID == taxid).Select(x => x.TaxDescription).FirstOrDefault() != null ? taxmaster.Where(x => x.ID == taxid).Select(x => x.TaxDescription).FirstOrDefault() : string.Empty,
                                                    CessDescription = taxmaster.Where(x => x.ID == taxid).Select(x => x.CESSDescription).FirstOrDefault() != null ? taxmaster.Where(x => x.ID == taxid).Select(x => x.CESSDescription).FirstOrDefault() : string.Empty,
                                                    AddcessDescription = taxmaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSDescription).FirstOrDefault() != null ? taxmaster.Where(x => x.ID == taxid).Select(x => x.AdditionalCESSDescription).FirstOrDefault() : string.Empty,
                                                }
                                        ).ToList();
            return DrugDetails;
        }
        public dynamic Insertpurchaseorder(purchaseorderview Addpurchase, int cmpid, int TransactionId, string Time)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                TimeSpan ts = TimeSpan.Parse(Time);

                try
                {

                    var purchaseordermas = new PurchaseOrder();
                    if (Addpurchase.PurchaseOrder.RandomUniqueID != "")
                    {

                        purchaseordermas = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == Addpurchase.PurchaseOrder.RandomUniqueID).FirstOrDefault();


                        if (Addpurchase.PurchaseOrder.QuotationNumber != null)
                        {

                            var list = WYNKContext.PurchaseOrder.Where(x => x.CMPID == cmpid && x.TransactionID == TransactionId && x.QuotationNumber != null).ToList();
                            if (list.Count() > 0)
                            {
                                var Q = list.Where(x => x.QuotationNumber.Replace(" ", string.Empty).Equals(Addpurchase.PurchaseOrder.QuotationNumber.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.RandomUniqueID.Replace(" ", string.Empty).Equals(Addpurchase.PurchaseOrder.RandomUniqueID.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(y => y.POID).FirstOrDefault();

                                if (Q != 0)
                                {
                                    purchaseordermas.QuotationNumber = Addpurchase.PurchaseOrder.QuotationNumber;
                                }
                                else
                                {
                                    var QQ = list.Where(x => x.QuotationNumber.Replace(" ", string.Empty).Equals(Addpurchase.PurchaseOrder.QuotationNumber.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(y => y.POID).FirstOrDefault();
                                    if (QQ != 0)
                                    {
                                        return new
                                        {
                                            Success = false,
                                            Message = "Quotation number already exists"
                                        };

                                    }

                                }

                            }
                        }


                        purchaseordermas.QuotationNumber = Addpurchase.PurchaseOrder.QuotationNumber;

                        if (Addpurchase.PurchaseOrder.QuotationDate != null)
                        {
                            var pur = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == Addpurchase.PurchaseOrder.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault();
                            if (pur != null)
                            {
                                purchaseordermas.QuotationDate = Addpurchase.PurchaseOrder.QuotationDate.Value.Subtract(ts);
                            }
                            else
                            {
                                purchaseordermas.QuotationDate = Addpurchase.PurchaseOrder.QuotationDate;
                            }
                        }

                        purchaseordermas.VendorID = Addpurchase.PurchaseOrder.VendorID;
                        purchaseordermas.DepartmentID = 0;
                        purchaseordermas.DeliveryName = Addpurchase.PurchaseOrder.DeliveryName;
                        purchaseordermas.DeliveryAddress1 = Addpurchase.PurchaseOrder.DeliveryAddress1;
                        purchaseordermas.DeliveryAddress2 = Addpurchase.PurchaseOrder.DeliveryAddress2;
                        purchaseordermas.DeliveryAddress3 = Addpurchase.PurchaseOrder.DeliveryAddress3;
                        purchaseordermas.DeliveryLocationID = Addpurchase.PurchaseOrder.DeliveryLocationID;
                        purchaseordermas.GrossProductValue = Addpurchase.PurchaseOrder.GrossProductValue;
                        purchaseordermas.TotalDiscountValue = Addpurchase.PurchaseOrder.TotalDiscountValue;
                        purchaseordermas.TotalTaxValue = Addpurchase.PurchaseOrder.TotalTaxValue;
                        purchaseordermas.TotalCGSTTaxValue = Addpurchase.PurchaseOrder.TotalTaxValue / 2;
                        purchaseordermas.TotalSGSTTaxValue = Addpurchase.PurchaseOrder.TotalTaxValue / 2;
                        purchaseordermas.TotalPOValue = Addpurchase.PurchaseOrder.TotalPOValue;
                        purchaseordermas.CESSAmount = Addpurchase.PurchaseOrder.CESSAmount;
                        purchaseordermas.AdditionalCESSAmount = Addpurchase.PurchaseOrder.AdditionalCESSAmount;
                        purchaseordermas.POStatus = purchasestatus.Open.ToString();
                        purchaseordermas.Validity = Addpurchase.PurchaseOrder.Validity;
                        purchaseordermas.Delivery = Addpurchase.PurchaseOrder.Delivery;
                        purchaseordermas.PaymentandTerms = Addpurchase.PurchaseOrder.PaymentandTerms;
                        purchaseordermas.TermsConditions = Addpurchase.PurchaseOrder.TermsConditions;
                        purchaseordermas.UpdatedUTC = DateTime.UtcNow;
                        purchaseordermas.UpdatedBy = Addpurchase.PurchaseOrder.CreatedBy;
                        WYNKContext.PurchaseOrder.UpdateRange(purchaseordermas);

                        if (Addpurchase.purchasedetails.Count() > 0)
                        {

                            foreach (var item in Addpurchase.purchasedetails.ToList())
                            {

                                var purchasetrans = new PurchaseOrderTrans();

                                if (item.POTranID != 0)
                                {

                                    purchasetrans = WYNKContext.PurchaseOrderTrans.Where(x => x.POTranID == item.POTranID).FirstOrDefault();
                                    purchasetrans.ItemID = Convert.ToInt32(item.ItemID);
                                    purchasetrans.ItemQty = item.ItemQty;
                                    purchasetrans.UOMID = Convert.ToInt32(item.UOMID);
                                    purchasetrans.ItemRate = item.ItemRate;
                                    purchasetrans.ItemValue = item.ItemValue;
                                    purchasetrans.DiscountPercentage = item.DiscountPercentage;
                                    purchasetrans.DiscountAmount = item.DiscountAmount;
                                    purchasetrans.GSTPercentage = item.GSTPercentage;
                                    purchasetrans.GSTTaxValue = item.GSTTaxValue;
                                    purchasetrans.SGSTPercentage = item.GSTPercentage / 2;
                                    purchasetrans.CGSTPercentage = item.GSTPercentage / 2;
                                    purchasetrans.SGSTTaxValue = item.GSTTaxValue / 2;
                                    purchasetrans.CGSTTaxValue = item.GSTTaxValue / 2;
                                    purchasetrans.CESSPercentage = item.CESSPercentage;
                                    purchasetrans.CESSAmount = item.CESSAmount;
                                    purchasetrans.AdditionalCESSPercentage = item.AdditionalCESSPercentage;
                                    purchasetrans.AddCESSAmount = item.AddCESSAmount;
                                    purchasetrans.IsDeleted = false;
                                    purchasetrans.UpdatedUTC = DateTime.UtcNow;
                                    purchasetrans.UpdatedBy = purchaseordermas.CreatedBy;
                                    WYNKContext.PurchaseOrderTrans.UpdateRange(purchasetrans);

                                }
                                else
                                {

                                    string RandomUniqueID = Addpurchase.PurchaseOrder.RandomUniqueID;
                                    purchasetrans.RandomUniqueID = RandomUniqueID;
                                    purchasetrans.ItemID = Convert.ToInt32(item.ItemID);
                                    purchasetrans.ItemQty = item.ItemQty;
                                    purchasetrans.UOMID = Convert.ToInt32(item.UOMID);
                                    purchasetrans.ItemRate = item.ItemRate;
                                    purchasetrans.ItemValue = item.ItemValue;
                                    purchasetrans.DiscountPercentage = item.DiscountPercentage;
                                    purchasetrans.DiscountAmount = item.DiscountAmount;
                                    purchasetrans.GSTPercentage = item.GSTPercentage;
                                    purchasetrans.GSTTaxValue = item.GSTTaxValue;
                                    purchasetrans.SGSTPercentage = item.GSTPercentage / 2;
                                    purchasetrans.CGSTPercentage = item.GSTPercentage / 2;
                                    purchasetrans.SGSTTaxValue = item.GSTTaxValue / 2;
                                    purchasetrans.CGSTTaxValue = item.GSTTaxValue / 2;
                                    purchasetrans.CESSPercentage = item.CESSPercentage;
                                    purchasetrans.CESSAmount = item.CESSAmount;
                                    purchasetrans.AdditionalCESSPercentage = item.AdditionalCESSPercentage;
                                    purchasetrans.AddCESSAmount = item.AddCESSAmount;
                                    purchasetrans.IsDeleted = false;
                                    purchasetrans.CreatedUTC = DateTime.UtcNow;
                                    purchasetrans.CreatedBy = purchaseordermas.CreatedBy;
                                    WYNKContext.PurchaseOrderTrans.AddRange(purchasetrans);
                                }


                            }
                        }

                    }

                    else
                    {

                        var Date = DateTime.Now;
                        var CurrentMonth = Date.Month;
                        var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                        if (FinancialYearId == 0)
                        {
                            dbContextTransaction.Rollback();
                            return new
                            {
                                Success = false,
                                Message = "Financial year doesn't exists",
                            };
                        }
                        purchaseordermas.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                        string RandomUniqueID = purchaseordermas.RandomUniqueID;
                        purchaseordermas.TransactionID = Addpurchase.PurchaseOrder.TransactionID;
                        purchaseordermas.CMPID = Addpurchase.PurchaseOrder.CMPID;
                        purchaseordermas.PONumber = Addpurchase.PurchaseOrder.PONumber;
                        var Datee = DateTime.Now;
                        purchaseordermas.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= Datee && Convert.ToDateTime(b.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                        purchaseordermas.PODate = Addpurchase.PurchaseOrder.PODate;

                        if (Addpurchase.PurchaseOrder.QuotationNumber != null)
                        {

                            var list = WYNKContext.PurchaseOrder.Where(x => x.CMPID == cmpid && x.TransactionID == TransactionId && x.QuotationNumber != null).ToList();
                            if (list.Count() > 0)
                            {

                                var Q = list.Where(x => x.QuotationNumber.Replace(" ", string.Empty).Equals(Addpurchase.PurchaseOrder.QuotationNumber.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(y => y.POID).FirstOrDefault();

                                if (Q != 0)
                                {
                                    return new
                                    {
                                        Success = false,
                                        Message = "Quotation number already exists"
                                    };

                                }
                            }
                        }

                        purchaseordermas.QuotationNumber = Addpurchase.PurchaseOrder.QuotationNumber;

                        if (Addpurchase.PurchaseOrder.QuotationDate != null)
                        {
                            purchaseordermas.QuotationDate = Addpurchase.PurchaseOrder.QuotationDate;
                        }

                        purchaseordermas.VendorID = Addpurchase.PurchaseOrder.VendorID;
                        purchaseordermas.DepartmentID = 0;
                        purchaseordermas.DeliveryName = Addpurchase.PurchaseOrder.DeliveryName;
                        purchaseordermas.DeliveryAddress1 = Addpurchase.PurchaseOrder.DeliveryAddress1;
                        purchaseordermas.DeliveryAddress2 = Addpurchase.PurchaseOrder.DeliveryAddress2;
                        purchaseordermas.DeliveryAddress3 = Addpurchase.PurchaseOrder.DeliveryAddress3;
                        purchaseordermas.DeliveryLocationID = Addpurchase.PurchaseOrder.DeliveryLocationID;
                        purchaseordermas.GrossProductValue = Addpurchase.PurchaseOrder.GrossProductValue;
                        purchaseordermas.TotalDiscountValue = Addpurchase.PurchaseOrder.TotalDiscountValue;
                        purchaseordermas.TotalTaxValue = Addpurchase.PurchaseOrder.TotalTaxValue;
                        purchaseordermas.TotalCGSTTaxValue = Addpurchase.PurchaseOrder.TotalTaxValue / 2;
                        purchaseordermas.TotalSGSTTaxValue = Addpurchase.PurchaseOrder.TotalTaxValue / 2;
                        purchaseordermas.TotalPOValue = Addpurchase.PurchaseOrder.TotalPOValue;
                        purchaseordermas.CESSAmount = Addpurchase.PurchaseOrder.CESSAmount;
                        purchaseordermas.AdditionalCESSAmount = Addpurchase.PurchaseOrder.AdditionalCESSAmount;
                        purchaseordermas.POStatus = purchasestatus.Open.ToString();
                        purchaseordermas.Validity = Addpurchase.PurchaseOrder.Validity;
                        purchaseordermas.Delivery = Addpurchase.PurchaseOrder.Delivery;
                        purchaseordermas.PaymentandTerms = Addpurchase.PurchaseOrder.PaymentandTerms;
                        purchaseordermas.TermsConditions = Addpurchase.PurchaseOrder.TermsConditions;
                        purchaseordermas.CreatedUTC = DateTime.UtcNow;
                        purchaseordermas.CreatedBy = Addpurchase.PurchaseOrder.CreatedBy;
                        WYNKContext.PurchaseOrder.AddRange(purchaseordermas);


                        if (Addpurchase.purchasedetails.Count() > 0)
                        {

                            foreach (var item in Addpurchase.purchasedetails.ToList())

                            {

                                var purchasetrans = new PurchaseOrderTrans();

                                purchasetrans.RandomUniqueID = RandomUniqueID;
                                purchasetrans.ItemID = Convert.ToInt32(item.ItemID);
                                purchasetrans.ItemQty = item.ItemQty;
                                purchasetrans.UOMID = Convert.ToInt32(item.UOMID);
                                purchasetrans.ItemRate = item.ItemRate;
                                purchasetrans.ItemValue = item.ItemValue;
                                purchasetrans.DiscountPercentage = item.DiscountPercentage;
                                purchasetrans.DiscountAmount = item.DiscountAmount;
                                purchasetrans.GSTPercentage = item.GSTPercentage;
                                purchasetrans.GSTTaxValue = item.GSTTaxValue;
                                purchasetrans.SGSTPercentage = item.GSTPercentage / 2;
                                purchasetrans.CGSTPercentage = item.GSTPercentage / 2;
                                purchasetrans.SGSTTaxValue = item.GSTTaxValue / 2;
                                purchasetrans.CGSTTaxValue = item.GSTTaxValue / 2;
                                purchasetrans.CESSPercentage = item.CESSPercentage;
                                purchasetrans.CESSAmount = item.CESSAmount;
                                purchasetrans.AdditionalCESSPercentage = item.AdditionalCESSPercentage;
                                purchasetrans.AddCESSAmount = item.AddCESSAmount;
                                purchasetrans.IsDeleted = false;
                                purchasetrans.CreatedUTC = DateTime.UtcNow;
                                purchasetrans.CreatedBy = purchaseordermas.CreatedBy;
                                WYNKContext.PurchaseOrderTrans.AddRange(purchasetrans);
                            }
                        }


                    }

                    if (Addpurchase.purchasedetailsDelete != null)
                    {
                        if (Addpurchase.purchasedetailsDelete.Count() > 0)
                        {
                            foreach (var item in Addpurchase.purchasedetailsDelete.ToList())
                            {

                                var master = WYNKContext.PurchaseOrderTrans.Where(x => x.POTranID == item.ID).FirstOrDefault();
                                master.IsDeleted = true;
                                WYNKContext.Entry(master).State = EntityState.Modified;
                            }
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    if (Addpurchase.PurchaseOrder.RandomUniqueID == "")
                    {
                        var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                        var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, Addpurchase.PurchaseOrder.CMPID, "GetRunningNo");

                        if (RunningNumber == Addpurchase.PurchaseOrder.PONumber)
                        {
                            commonRepos.GenerateRunningCtrlNoo(TransactionId, Addpurchase.PurchaseOrder.CMPID, "UpdateRunningNo");
                        }
                        else
                        {
                            var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, Addpurchase.PurchaseOrder.CMPID, "UpdateRunningNo");

                            var purchaseno = WYNKContext.PurchaseOrder.Where(x => x.PONumber == Addpurchase.PurchaseOrder.PONumber).FirstOrDefault();
                            purchaseno.PONumber = GetRunningNumber;
                            WYNKContext.PurchaseOrder.UpdateRange(purchaseno);
                            WYNKContext.SaveChanges();
                        }
                    }



                    return new
                    {

                        Success = true,
                        ponum = purchaseordermas.PONumber,
                        RandomUniqueID = purchaseordermas.RandomUniqueID,
                        companyname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault(),
                        companyaddress1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() : string.Empty,
                        companyaddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() : string.Empty,
                        companylocationID = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationID).FirstOrDefault() != 0 ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationID).FirstOrDefault() : 0,
                        companylocation = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault() : string.Empty,
                    };

                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, purchase = Addpurchase.PurchaseOrder.PONumber };
                }

            }
        }
        public dynamic Getpurchaseprint(string RandomUniqueID, int cmpid, string Time, string name, string PONumbers)
        {


            var purchase = new purchaseorderview();
            var potrans = WYNKContext.PurchaseOrderTrans.ToList();
            var drug = WYNKContext.DrugMaster.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var location = CMPSContext.LocationMaster.ToList();
            var tax = CMPSContext.TaxMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);

            var PONumber = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PONumber).FirstOrDefault();
            var PODate = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault();
            var QuotationNumber = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault();
            var QuotationDate = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null;

            var VendorID = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault();
            var Name = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Name).FirstOrDefault();
            var Address1 = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var Address2 = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() : 0;
            var VendorLocationID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() : 0;
            if (VendorLocationID != 0)
            {
                var CityID = CMPSContext.LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                purchase.Location = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchase.ParentDescriptioncity = location.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchase.ParentDescriptionstate = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchase.ParentDescriptioncountry = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                var VendorCityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (VendorCityID != 0)
                {
                    var StateID = CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    purchase.ParentDescriptioncity = location.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchase.ParentDescriptionstate = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchase.ParentDescriptioncountry = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                }
            }


            var PhoneNo = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() : string.Empty;
            var GSTNo = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() : string.Empty;

            var DeliveryName = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault() : string.Empty;
            var DeliveryAddress1 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress1).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress1).FirstOrDefault() : string.Empty;
            var DeliveryAddress2 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress2).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress2).FirstOrDefault() : string.Empty;

            var purchaseLocationID = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryLocationID).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryLocationID).FirstOrDefault() : 0;
            var DeliveryLocationID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() : 0;

            if (DeliveryLocationID != 0)
            {
                var DeliveryCityID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var DeliveryStateID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var DeliveryCountryID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                purchase.LocationName = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchase.DeliveryParentDescriptioncity = location.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchase.DeliveryParentDescriptionstate = location.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchase.DeliveryParentDescriptioncountry = location.Where(x => x.ID == DeliveryCountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == DeliveryCountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                var DeliveryCityID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (DeliveryCityID != 0)
                {
                    var DeliveryStateID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var DeliveryCountryID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    purchase.DeliveryParentDescriptioncity = location.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == DeliveryCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchase.DeliveryParentDescriptionstate = location.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == DeliveryStateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchase.DeliveryParentDescriptioncountry = location.Where(x => x.ID == DeliveryCountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == DeliveryCountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;

                }
            }

            var TotalPOValue1 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();
            var Valid = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Validity).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Validity).FirstOrDefault() : string.Empty;
            var Delivery = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Delivery).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Delivery).FirstOrDefault() : string.Empty;
            var payment = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PaymentandTerms).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PaymentandTerms).FirstOrDefault() : string.Empty;
            var Tremandcond = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() : string.Empty;
            var Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
            var Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var cAddress1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var cAddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault() : string.Empty;
            var phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault() : string.Empty;
            var web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault() : string.Empty;
            var companylocationID = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationID).FirstOrDefault() != 0 ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationID).FirstOrDefault() : 0;
            var companylocation = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.LocationName).FirstOrDefault() : string.Empty;
            var GrossProductValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.GrossProductValue).FirstOrDefault();
            var TotalDiscountValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalDiscountValue).FirstOrDefault();
            var TotalTaxValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalTaxValue).FirstOrDefault();
            var TotalPOValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();
            var GetCESSAmount = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.CESSAmount).FirstOrDefault();
            var GetAdditionalCESSAmount = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.AdditionalCESSAmount).FirstOrDefault();


            var PurchaseOrderTransdetails = (from PoTrans in potrans.Where(x => x.RandomUniqueID == RandomUniqueID && x.IsDeleted == false)
                                             join Dur in drug on PoTrans.ItemID equals Dur.ID
                                             join UOM in uom on PoTrans.UOMID equals UOM.id
                                             select new
                                             {
                                                 RandomUniqueID = PoTrans.RandomUniqueID,
                                                 POTranID = PoTrans.POTranID,
                                                 ItemDrug = drug.Where(x => x.ID == PoTrans.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                                 ItemID = Convert.ToString(PoTrans.ItemID),
                                                 UOMname = uom.Where(x => x.id == PoTrans.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                 UOMID = PoTrans.UOMID,
                                                 ItemQty = PoTrans.ItemQty,
                                                 ItemRate = PoTrans.ItemRate,
                                                 ItemValue = PoTrans.ItemValue,
                                                 DiscountPercentage = PoTrans.DiscountPercentage,
                                                 DiscountAmount = PoTrans.DiscountAmount,
                                                 GSTPercentage = PoTrans.GSTPercentage,
                                                 GSTTaxValue = PoTrans.GSTTaxValue,
                                                 CESSPercentage = PoTrans.CESSPercentage,
                                                 CESSAmount = PoTrans.CESSAmount,
                                                 AdditionalCESSPercentage = PoTrans.AdditionalCESSPercentage,
                                                 AddCESSAmount = PoTrans.AddCESSAmount,
                                                 GrossAmount = PoTrans.ItemQty * PoTrans.ItemRate - PoTrans.ItemQty * PoTrans.ItemRate * PoTrans.DiscountPercentage / 100,
                                                 TotalAmount = (PoTrans.ItemQty * PoTrans.ItemRate - PoTrans.ItemQty * PoTrans.ItemRate * PoTrans.DiscountPercentage / 100) +
                                                 (PoTrans.GSTTaxValue) + (PoTrans.CESSAmount) + (PoTrans.AddCESSAmount),
                                                 PORecd = potrans.Where(x => x.ParentPONumber == PONumbers && x.PORecdQty != 0).Select(s => s.PORecdQty != 0).FirstOrDefault(),
                                                 POCancel = potrans.Where(x => x.ParentPONumber == PONumbers && x.POCancelledQty != 0).Select(s => s.POCancelledQty != 0).FirstOrDefault(),
                                                 Status = name,
                                                 PORecdQty = PoTrans.PORecdQty,
                                                 TaxDescription = tax.Where(a => a.ID == drug.Where(x => x.ID == PoTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.TaxDescription).FirstOrDefault(),
                                                 CESSDescription = tax.Where(a => a.ID == drug.Where(x => x.ID == PoTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.CESSDescription).FirstOrDefault(),
                                                 AdditionalCESSDescription = tax.Where(a => a.ID == drug.Where(x => x.ID == PoTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.AdditionalCESSDescription).FirstOrDefault(),
                                             }).ToList();


            try
            {

                return new
                {

                    PooNumber = PONumber,
                    PooDate = PODate,
                    QoNo = QuotationNumber,
                    QoDate = QuotationDate,
                    VendorID = VendorID,
                    VName = Name,
                    VAddress1 = Address1,
                    VAddress2 = Address2,
                    VLocation = purchase.Location,
                    VParentDescriptioncity = purchase.ParentDescriptioncity,
                    VParentDescriptionstate = purchase.ParentDescriptionstate,
                    VParentDescriptioncountry = purchase.ParentDescriptioncountry,
                    VPhoneno = PhoneNo,
                    VGstno = GSTNo,
                    PDelivery = DeliveryName,
                    PDeliveryadd1 = DeliveryAddress1,
                    PDeliveryadd2 = DeliveryAddress2,
                    PLocation = purchase.LocationName,
                    PDeliveryParentDescriptioncity = purchase.DeliveryParentDescriptioncity,
                    PDeliveryParentDescriptionstate = purchase.DeliveryParentDescriptionstate,
                    PDeliveryParentDescriptioncountry = purchase.DeliveryParentDescriptioncountry,
                    PTotalpo1 = TotalPOValue1,
                    Term = Tremandcond,
                    Addresss = Address,
                    cAddress1 = cAddress1,
                    cAddress2 = cAddress2,
                    Phonee = phone,
                    Webb = web,
                    ComplocID = companylocationID,
                    Complocname = companylocation,
                    Validy = Valid,
                    Deliverr = Delivery,
                    payments = payment,
                    company = Compnayname,
                    PGross = GrossProductValue,
                    PDiscount = TotalDiscountValue,
                    PTotalpo = TotalPOValue,
                    PTotalax = TotalTaxValue,
                    GetCESSAmountt = GetCESSAmount,
                    GetAdditionalCESSAmountt = GetAdditionalCESSAmount,
                    Potransdetails = PurchaseOrderTransdetails.ToList(),

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
        public dynamic purchasehis(int cmpid, int TransactionId, string Time)
        {
            var history = new purchaseorderview();
            history.purchaseorderhist = new List<purchaseorderhist>();
            history.purchaseorderhist3 = new List<purchaseorderhist3>();
            history.purchaseorderhist4 = new List<purchaseorderhist4>();
            var vendor = CMPSContext.VendorMaster.ToList();
            var purchase = WYNKContext.PurchaseOrder.ToList();
            var purchasetran = WYNKContext.PurchaseOrderTrans.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);


            history.purchaseorderhist = (from c in purchase.Where(c => c.TransactionID == TransactionId && c.IsCancelled != true && c.CMPID == cmpid && c.POStatus == "Closed").OrderByDescending(x => x.CreatedUTC)
                                         select new purchaseorderhist
                                         {
                                             RandomUniqueID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.RandomUniqueID).FirstOrDefault(),
                                             PONumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PONumber).FirstOrDefault(),
                                             PODate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault(),
                                             QuotationDate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? purchase.Where(x => x.POID == c.POID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null,
                                             QuotationNumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault(),
                                             DeliveryName = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault(),
                                             VendorName = vendor.Where(x => x.ID == c.VendorID).Select(x => x.Name).FirstOrDefault(),
                                             VendorID = c.VendorID,
                                             Status = "RECEIVED",
                                             DeliverylocationID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryLocationID).FirstOrDefault(),


                                         }).ToList();

            history.purchaseorderhist5 = (from c in purchase.Where(c => c.TransactionID == TransactionId && c.IsCancelled != true && c.CMPID == cmpid).OrderByDescending(x => x.CreatedUTC)
                                          join UM in purchasetran
                                          on c.PONumber equals UM.ParentPONumber
                                          select new purchaseorderhist5
                                          {
                                              RandomUniqueID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.RandomUniqueID).FirstOrDefault(),
                                              PONumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PONumber).FirstOrDefault(),
                                              PODate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault(),
                                              QuotationDate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? purchase.Where(x => x.POID == c.POID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null,
                                              QuotationNumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault(),
                                              DeliveryName = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault(),
                                              VendorName = vendor.Where(x => x.ID == c.VendorID).Select(x => x.Name).FirstOrDefault(),
                                              VendorID = c.VendorID,
                                              Status = "Cancelled",
                                              DeliverylocationID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryLocationID).FirstOrDefault(),

                                          }).ToList();


            var op = purchase.Where(x => x.POStatus == "Open").ToList();

            foreach (var item in op.ToList())
            {
                var result = true;
                var POID = item.RandomUniqueID;
                var POT = purchasetran.Where(x => x.RandomUniqueID == POID).ToList();
                foreach (var item1 in POT.ToList())
                {
                    var porqty = item1.PORecdQty;

                    if (porqty != 0)
                    {
                        result = false;
                        break;
                    }
                }

                if (result == true)
                {
                    var qry = from Cust in purchase.Where(Cust => Cust.TransactionID == TransactionId && Cust.IsCancelled != true && Cust.CMPID == cmpid).OrderByDescending(x => x.CreatedUTC)
                              join Order in POT on Cust.RandomUniqueID equals Order.RandomUniqueID
                              group Cust by new
                              {
                                  purPOID = Cust.RandomUniqueID,
                                  purPONumber = Cust.PONumber,
                                  purQuotationDate = Cust.QuotationDate,
                                  purQuotationNumber = Cust.QuotationNumber,
                                  purDeliveryName = Cust.DeliveryName,
                                  purVendorName = vendor.Where(x => x.ID == Cust.VendorID).Select(x => x.Name).FirstOrDefault(),
                                  purPODate = Cust.PODate,
                                  VendorID = Cust.VendorID,
                                  DeliverylocationID = Cust.DeliveryLocationID,
                              } into grp

                              select new
                              {
                                  PurchasePOID = grp.Key.purPOID,
                                  PurchasePONumber = grp.Key.purPONumber,
                                  PurchasePODate = grp.Key.purPODate,
                                  PurchaseQuotationDate = grp.Key.purQuotationDate,
                                  PurchaseQuotationNumber = grp.Key.purQuotationNumber,
                                  PurchaseDeliveryName = grp.Key.purDeliveryName,
                                  PurchaseVendorName = grp.Key.purVendorName,
                                  VendorID = grp.Key.VendorID,
                                  DeliverylocationID = grp.Key.DeliverylocationID,

                              };


                    history.purchaseorderhist1 = (from pu in qry

                                                  select new purchaseorderhist1
                                                  {
                                                      RandomUniqueID = pu.PurchasePOID,
                                                      PONumber = pu.PurchasePONumber,
                                                      PODate = pu.PurchasePODate.Add(ts),
                                                      QuotationDate = pu.PurchaseQuotationDate != null ? pu.PurchaseQuotationDate.Value.Add(ts) : (DateTime?)null,
                                                      QuotationNumber = pu.PurchaseQuotationNumber,
                                                      DeliveryName = pu.PurchaseDeliveryName,
                                                      VendorName = pu.PurchaseVendorName,
                                                      Status = "Open",
                                                      VendorID = pu.VendorID,
                                                      DeliverylocationID = pu.DeliverylocationID

                                                  }).ToList();

                    foreach (var itm in history.purchaseorderhist1.ToList())
                    {
                        var purcha = new purchaseorderhist3();
                        purcha.RandomUniqueID = itm.RandomUniqueID;
                        purcha.PONumber = itm.PONumber;
                        purcha.PODate = itm.PODate;
                        purcha.QuotationDate = itm.QuotationDate;
                        purcha.QuotationNumber = itm.QuotationNumber;
                        purcha.DeliveryName = itm.DeliveryName;
                        purcha.VendorName = itm.VendorName;
                        purcha.Status = itm.Status;
                        purcha.VendorID = itm.VendorID;
                        purcha.DeliverylocationID = itm.DeliverylocationID;
                        history.purchaseorderhist3.Add(purcha);

                    }

                }

                else
                {

                    var qry = from Cust in purchase.Where(Cust => Cust.TransactionID == TransactionId && Cust.IsCancelled != true && Cust.CMPID == cmpid).OrderByDescending(x => x.CreatedUTC)
                              join Order in POT on Cust.RandomUniqueID equals Order.RandomUniqueID
                              group Cust by new
                              {
                                  purPOID = Cust.RandomUniqueID,
                                  purPONumber = Cust.PONumber,
                                  purQuotationDate = Cust.QuotationDate,
                                  purQuotationNumber = Cust.QuotationNumber,
                                  purDeliveryName = Cust.DeliveryName,
                                  purVendorName = vendor.Where(x => x.ID == Cust.VendorID).Select(x => x.Name).FirstOrDefault(),
                                  purPODate = Cust.PODate,
                                  VendorID = Cust.VendorID,
                                  DeliverylocationID = Cust.DeliveryLocationID,
                              } into grp

                              select new
                              {
                                  PurchasePOID = grp.Key.purPOID,
                                  PurchasePONumber = grp.Key.purPONumber,
                                  PurchasePODate = grp.Key.purPODate,
                                  PurchaseQuotationDate = grp.Key.purQuotationDate,
                                  PurchaseQuotationNumber = grp.Key.purQuotationNumber,
                                  PurchaseDeliveryName = grp.Key.purDeliveryName,
                                  PurchaseVendorName = grp.Key.purVendorName,
                                  VendorID = grp.Key.VendorID,
                                  DeliverylocationID = grp.Key.DeliverylocationID,
                              };

                    history.purchaseorderhist2 = (from pu in qry

                                                  select new purchaseorderhist2
                                                  {
                                                      RandomUniqueID = pu.PurchasePOID,
                                                      PONumber = pu.PurchasePONumber,
                                                      PODate = pu.PurchasePODate.Add(ts),
                                                      QuotationDate = pu.PurchaseQuotationDate != null ? pu.PurchaseQuotationDate.Value.Add(ts) : (DateTime?)null,
                                                      QuotationNumber = pu.PurchaseQuotationNumber,
                                                      DeliveryName = pu.PurchaseDeliveryName,
                                                      VendorName = pu.PurchaseVendorName,
                                                      Status = "PARTIALLY RECEIVED",
                                                      VendorID = pu.VendorID,
                                                      DeliverylocationID = pu.DeliverylocationID
                                                  }).ToList();




                    foreach (var itm in history.purchaseorderhist2.ToList())
                    {
                        var purcha = new purchaseorderhist4();
                        purcha.RandomUniqueID = itm.RandomUniqueID;
                        purcha.PONumber = itm.PONumber;
                        purcha.PODate = itm.PODate;
                        purcha.QuotationDate = itm.QuotationDate;
                        purcha.QuotationNumber = itm.QuotationNumber;
                        purcha.DeliveryName = itm.DeliveryName;
                        purcha.VendorName = itm.VendorName;
                        purcha.Status = itm.Status;
                        purcha.VendorID = itm.VendorID;
                        purcha.DeliverylocationID = itm.DeliverylocationID;
                        history.purchaseorderhist4.Add(purcha);

                    }


                }

            }




            return history;


        }


    }

}