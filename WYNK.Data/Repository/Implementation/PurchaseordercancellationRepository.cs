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
    class PurchaseordercancellationRepository : RepositoryBase<Purchaseordercancellation>, IPurchaseordercancellationRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public PurchaseordercancellationRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic purchaseordercancel(int id, int TransactionId, string Time)
        {
            var cancel = new Purchaseordercancellation();

            var vendor = CMPSContext.VendorMaster.ToList();
            var purchase = WYNKContext.PurchaseOrder.ToList();
            var purchasetran = WYNKContext.PurchaseOrderTrans.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);

            var TID = purchase.Where(x => x.CMPID == id && x.TransactionID != TransactionId).Select(x => x.TransactionID).FirstOrDefault();

            var purchaseordercan = (from c in purchase.Where(c => c.IsCancelled != true && c.CMPID == id && c.TransactionID == TID)

                                    select new
                                    {
                                        RandomUniqueID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.RandomUniqueID).FirstOrDefault(),
                                        PONumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PONumber).FirstOrDefault(),
                                        PODate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault(),
                                        QuotationDate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null,
                                        QuotationNumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault(),
                                        DeliveryName = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault(),
                                        VendorName = vendor.Where(x => x.ID == c.VendorID).Select(x => x.Name).FirstOrDefault(),
                                        IsCancelled = Enum.GetName(typeof(cancel), c.IsCancelled),
                                    }).ToList();

            var pur1 = WYNKContext.PurchaseOrderTrans.Where(x => x.PORecdQty != 0).Select(x => x.RandomUniqueID).ToList();

            var purchaseordercancel = (from c in purchase
                                       where !pur1.Contains(c.RandomUniqueID)
                                       where c.TransactionID == TransactionId && c.IsCancelled == true && c.CMPID == id
                                       select new
                                       {
                                           RandomUniqueID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.RandomUniqueID).FirstOrDefault(),
                                           PONumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PONumber).FirstOrDefault(),
                                           PODate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault(),
                                           QuotationDate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null,
                                           QuotationNumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault(),
                                           DeliveryName = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault(),
                                           VendorName = vendor.Where(x => x.ID == c.VendorID).Select(x => x.Name).FirstOrDefault(),
                                           IsCancelled = Enum.GetName(typeof(cancel), c.IsCancelled),
                                           ParentPONumber = purchasetran.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.ParentPONumber).FirstOrDefault(),
                                           ParentPODate = purchasetran.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.ParentPODate).FirstOrDefault() != null ? purchasetran.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.ParentPODate.Value.Subtract(ts)).FirstOrDefault() : (DateTime?)null,

                                       }).ToList();


            try
            {

                return new
                {

                    Getpurchaseordercan = purchaseordercan,
                    Getpurchaseordercancel = purchaseordercancel,
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

        public dynamic purchaseordercancellation(int id, int TransactionId, string Time)
        {
            var cancel = new Purchaseordercancellation();

            var vendor = CMPSContext.VendorMaster.ToList();
            var purchase = WYNKContext.PurchaseOrder.ToList();
            var purchasetran = WYNKContext.PurchaseOrderTrans.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);

            var pur1 = WYNKContext.PurchaseOrderTrans.Where(x => x.PORecdQty != 0).Select(x => x.RandomUniqueID).ToList();


            var purchaseordercan1 = (from c in purchase
                                     where !pur1.Contains(c.RandomUniqueID)
                                     where c.TransactionID == TransactionId && c.IsCancelled == true && c.CMPID == id


                                     select new
                                     {
                                         RandomUniqueID = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.RandomUniqueID).FirstOrDefault(),
                                         PONumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PONumber).FirstOrDefault(),
                                         PODate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault(),
                                         QuotationDate = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null,
                                         QuotationNumber = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault(),
                                         DeliveryName = purchase.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault(),
                                         VendorName = vendor.Where(x => x.ID == c.VendorID).Select(x => x.Name).FirstOrDefault(),
                                         IsCancelled = Enum.GetName(typeof(cancel), c.IsCancelled),
                                         ParentPONumber = purchasetran.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.ParentPONumber).FirstOrDefault(),
                                         ParentPODate = purchasetran.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.ParentPODate).FirstOrDefault() != null ? purchasetran.Where(x => x.RandomUniqueID == c.RandomUniqueID).Select(x => x.ParentPODate.Value.Subtract(ts)).FirstOrDefault() : (DateTime?)null,

                                     }).ToList();

            try
            {

                return new
                {

                    Getpurchaseordercancelled = purchaseordercan1,
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
        public dynamic Getpurchasecal(string RandomUniqueID, string Time)
        {

            var purchasebinding = new Purchaseordercancellation();
            var potrans = WYNKContext.PurchaseOrderTrans.ToList();
            var drug = WYNKContext.DrugMaster.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var location = CMPSContext.LocationMaster.ToList();
            var tax = CMPSContext.TaxMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);


            var PODate = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault();
            var QuotationNumber = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault() : string.Empty;
            var QuotationDate = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null;
            var VendorID = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault();
            var Name = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Name).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Name).FirstOrDefault() : string.Empty;
            var Address1 = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var Address2 = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() : 0;
            var VendorLocationID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() : 0;
            if (VendorLocationID != 0)
            {
                var CityID = CMPSContext.LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                purchasebinding.Location = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchasebinding.city = location.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchasebinding.state = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchasebinding.country = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                var VendorCityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (VendorCityID != 0)
                {
                    var StateID = CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    purchasebinding.state = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchasebinding.city = location.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchasebinding.country = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                }
            }
            var PhoneNo = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() : string.Empty;
            var GSTNo = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() : string.Empty;
            var DeliveryName = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault() : string.Empty;
            var DeliveryAddress1 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress1).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress1).FirstOrDefault() : string.Empty;
            var DeliveryAddress2 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress2).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress2).FirstOrDefault() : string.Empty;
            var DeliveryLocationID = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryLocationID).FirstOrDefault();
            var purchaseLocationID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "loc").Select(x => x.ID).FirstOrDefault() : 0;
            if (purchaseLocationID != 0)
            {
                var purchaseCityID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID).Select(x => x.ID).FirstOrDefault() : 0;
                var purchaseStateID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var purchaseCountryID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                purchasebinding.LocationName = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchasebinding.CityName = location.Where(x => x.ID == purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchasebinding.StateName = location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchasebinding.CountryName = location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                purchasebinding.purchaseCityID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (purchasebinding.purchaseCityID != 0)
                {
                    var purchaseStateID = CMPSContext.LocationMaster.Where(x => x.ID == purchasebinding.purchaseCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchasebinding.purchaseCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var purchaseCountryID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    purchasebinding.StateName = location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchasebinding.CityName = location.Where(x => x.ID == purchasebinding.purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchasebinding.purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchasebinding.CountryName = location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                }
            }


            var GrossProductValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.GrossProductValue).FirstOrDefault();
            var TotalDiscountValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalDiscountValue).FirstOrDefault();
            var TotalTaxValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalTaxValue).FirstOrDefault();
            var GetCESSAmount = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.CESSAmount).FirstOrDefault();
            var GetAdditionalCESSAmount = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.AdditionalCESSAmount).FirstOrDefault();
            var TotalPOValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();
            var Tremandcond = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() : string.Empty;
            var Valid = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Validity).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Validity).FirstOrDefault() : string.Empty;
            var Deliver = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Delivery).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Delivery).FirstOrDefault() : string.Empty;
            var payment = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PaymentandTerms).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PaymentandTerms).FirstOrDefault() : string.Empty;
            var Iscancel = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.IsCancelled).FirstOrDefault();
            var Pponumber = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PONumber).FirstOrDefault();

            var PurchaseOrderTransdetailscancel = (from PoTrans in potrans.Where(x => x.RandomUniqueID == RandomUniqueID && x.IsDeleted == false)
                                                   join Dur in drug on PoTrans.ItemID equals Dur.ID
                                                   join UOM in uom on PoTrans.UOMID equals UOM.id
                                                   select new
                                                   {
                                                       ItemID = PoTrans.ItemID,
                                                       Itemname = drug.Where(x => x.ID == PoTrans.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                                       ItemQty = PoTrans.ItemQty,
                                                       UOMID = PoTrans.UOMID,
                                                       UOMname = uom.Where(x => x.id == PoTrans.UOMID).Select(x => x.Description).FirstOrDefault(),
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

                                                       Grossamount = PoTrans.ItemQty * PoTrans.ItemRate - PoTrans.ItemQty * PoTrans.ItemRate * PoTrans.DiscountPercentage / 100,
                                                       Totalamount = (PoTrans.ItemQty * PoTrans.ItemRate - PoTrans.ItemQty * PoTrans.ItemRate * PoTrans.DiscountPercentage / 100) +
                                                       (PoTrans.GSTTaxValue) + (PoTrans.CESSAmount) + (PoTrans.AddCESSAmount),
                                                       PONUM = Pponumber,
                                                       PONUMDate = PODate,
                                                       TaxDescription = tax.Where(a => a.ID == drug.Where(x => x.ID == PoTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.TaxDescription).FirstOrDefault(),
                                                       CESSDescription = tax.Where(a => a.ID == drug.Where(x => x.ID == PoTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.CESSDescription).FirstOrDefault(),
                                                       AdditionalCESSDescription = tax.Where(a => a.ID == drug.Where(x => x.ID == PoTrans.ItemID).Select(s => s.TaxID).FirstOrDefault()).Select(e => e.AdditionalCESSDescription).FirstOrDefault(),

                                                   }).ToList();


            try
            {

                return new
                {

                    PooNumberr = Pponumber,
                    PooDatee = PODate,
                    QoNoo = QuotationNumber,
                    QoDatee = QuotationDate,
                    VNamee = Name,
                    VAddresss1 = Address1,
                    VAddresss2 = Address2,
                    VLocationn = purchasebinding.Location,
                    Vcity = purchasebinding.city,
                    Vstate = purchasebinding.state,
                    Vcountry = purchasebinding.country,
                    VPhonenoo = PhoneNo,
                    VGstnoo = GSTNo,
                    PDeliveryy = DeliveryName,
                    PDeliveryaddd1 = DeliveryAddress1,
                    PDeliveryaddd2 = DeliveryAddress2,
                    PLocationn = purchasebinding.LocationName,
                    PLocationncity = purchasebinding.CityName,
                    PLocationnstate = purchasebinding.StateName,
                    PLocationncountry = purchasebinding.CountryName,
                    PGrosss = GrossProductValue,
                    PDiscountt = TotalDiscountValue,
                    PTotalaxx = TotalTaxValue,
                    PTotalpoo = TotalPOValue,
                    GetCESSAmountt = GetCESSAmount,
                    GetAdditionalCESSAmountt = GetAdditionalCESSAmount,
                    Termm = Tremandcond,
                    Validd = Valid,
                    Deliverr = Deliver,
                    paymentts = payment,
                    cancell = Iscancel,
                    VIDD = VendorID,
                    LID = DeliveryLocationID,
                    LLID = purchasebinding.purchaseCityID,
                    Potransdetailss = PurchaseOrderTransdetailscancel.ToList(),
                    Gettotalamount = PurchaseOrderTransdetailscancel.Select(x => x.ItemValue).ToList().Sum(),
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
        public dynamic Insertpurchaseordercancel(Purchaseordercancellation cancelpurchase, int cmpid, int TransactionTypeid, string Time)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    TimeSpan ts = TimeSpan.Parse(Time);

                    var purchaseordermas = new PurchaseOrder();

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
                    purchaseordermas.TransactionID = cancelpurchase.PurchaseOrdercancel.TransactionID;
                    purchaseordermas.PONumber = cancelpurchase.PurchaseOrdercancel.PONumber;
                    purchaseordermas.PODate = DateTime.UtcNow;
                    var Datee = DateTime.Now;
                    purchaseordermas.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= Datee && Convert.ToDateTime(b.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                    purchaseordermas.QuotationNumber = cancelpurchase.PurchaseOrdercancel.QuotationNumber;

                    if (cancelpurchase.PurchaseOrdercancel.QuotationDate != null)
                    {
                        var pur = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == cancelpurchase.PurchaseOrdercancel.RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault();
                        if (pur != null)
                        {
                            purchaseordermas.QuotationDate = cancelpurchase.PurchaseOrdercancel.QuotationDate.Value.Subtract(ts);
                        }
                        else
                        {
                            purchaseordermas.QuotationDate = cancelpurchase.PurchaseOrdercancel.QuotationDate;
                        }
                    }

                    purchaseordermas.VendorID = cancelpurchase.PurchaseOrdercancel.VendorID;
                    purchaseordermas.DepartmentID = 0;
                    purchaseordermas.DeliveryName = cancelpurchase.PurchaseOrdercancel.DeliveryName;
                    purchaseordermas.DeliveryAddress1 = cancelpurchase.PurchaseOrdercancel.DeliveryAddress1;
                    purchaseordermas.DeliveryAddress2 = cancelpurchase.PurchaseOrdercancel.DeliveryAddress2;
                    purchaseordermas.DeliveryLocationID = cancelpurchase.PurchaseOrdercancel.DeliveryLocationID;
                    purchaseordermas.GrossProductValue = cancelpurchase.PurchaseOrdercancel.GrossProductValue;
                    purchaseordermas.TotalDiscountValue = cancelpurchase.PurchaseOrdercancel.TotalDiscountValue;
                    purchaseordermas.TotalTaxValue = cancelpurchase.PurchaseOrdercancel.TotalTaxValue;
                    purchaseordermas.TotalCGSTTaxValue = cancelpurchase.PurchaseOrdercancel.TotalTaxValue / 2;
                    purchaseordermas.TotalSGSTTaxValue = cancelpurchase.PurchaseOrdercancel.TotalTaxValue / 2;
                    purchaseordermas.TotalPOValue = cancelpurchase.PurchaseOrdercancel.TotalPOValue;
                    purchaseordermas.CESSAmount = cancelpurchase.PurchaseOrdercancel.CESSAmount;
                    purchaseordermas.AdditionalCESSAmount = cancelpurchase.PurchaseOrdercancel.AdditionalCESSAmount;
                    purchaseordermas.POStatus = purchasestatus.Cancel.ToString();
                    purchaseordermas.TermsConditions = cancelpurchase.PurchaseOrdercancel.TermsConditions;
                    purchaseordermas.Validity = cancelpurchase.PurchaseOrdercancel.Validity;
                    purchaseordermas.Delivery = cancelpurchase.PurchaseOrdercancel.Delivery;
                    purchaseordermas.PaymentandTerms = cancelpurchase.PurchaseOrdercancel.PaymentandTerms;
                    purchaseordermas.CMPID = cancelpurchase.PurchaseOrdercancel.CMPID;
                    purchaseordermas.CreatedBy = cancelpurchase.PurchaseOrdercancel.CreatedBy;
                    purchaseordermas.CreatedUTC = DateTime.UtcNow;
                    purchaseordermas.IsCancelled = true;
                    WYNKContext.PurchaseOrder.AddRange(purchaseordermas);

                    if (cancelpurchase.PurchaseOrderTransdetailscancel.Count() > 0)
                    {

                        foreach (var item in cancelpurchase.PurchaseOrderTransdetailscancel.ToList())

                        {
                            var purchasetrans = new PurchaseOrderTrans();

                            purchasetrans.RandomUniqueID = RandomUniqueID;
                            purchasetrans.ItemID = item.ItemID;
                            purchasetrans.ItemQty = item.ItemQty;
                            purchasetrans.POCancelledQty = item.ItemQty;
                            purchasetrans.ParentPONumber = item.PONUM;
                            purchasetrans.ParentPODate = item.PONUMDate;
                            purchasetrans.UOMID = item.UOMID;
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
                            purchasetrans.CreatedBy = purchaseordermas.CreatedBy;
                            purchasetrans.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.PurchaseOrderTrans.AddRange(purchasetrans);

                            var master = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == purchasetrans.RandomUniqueID).FirstOrDefault();

                            if (master != null)
                            {
                                master.IsCancelled = true;
                                master.POStatus = purchasestatus.Cancel.ToString();
                                WYNKContext.Entry(master).State = EntityState.Modified;
                            }

                        }

                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();


                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "GetRunningNo");

                    if (RunningNumber == cancelpurchase.PurchaseOrdercancel.PONumber)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionTypeid, cmpid, "UpdateRunningNo");

                        var purchaseno = WYNKContext.PurchaseOrder.Where(x => x.PONumber == cancelpurchase.PurchaseOrdercancel.PONumber).FirstOrDefault();
                        purchaseno.PONumber = GetRunningNumber;
                        WYNKContext.PurchaseOrder.UpdateRange(purchaseno);
                        WYNKContext.SaveChanges();
                    }



                    return new
                    {
                        Success = true,
                        RandomUniqueID = purchaseordermas.RandomUniqueID,
                        Status = purchaseordermas.POStatus,
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, purchase = cancelpurchase.PurchaseOrdercancel.PONumber };
                }

            }
        }

        public dynamic Getpurchaseprintt(string RandomUniqueID, int cmpid, string Time)
        {
            var purchaseprint = new Purchaseordercancellation();
            var potrans = WYNKContext.PurchaseOrderTrans.ToList();
            var drug = WYNKContext.DrugMaster.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var location = CMPSContext.LocationMaster.ToList();
            var tax = CMPSContext.TaxMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);

            var PONumber = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PONumber).FirstOrDefault();
            var PODate = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PODate.Add(ts)).FirstOrDefault();
            var ParentPONumber = WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.ParentPONumber).FirstOrDefault() != null ? WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.ParentPONumber).FirstOrDefault() : string.Empty;
            var ParentPODate = WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.ParentPODate).FirstOrDefault() != null ? WYNKContext.PurchaseOrderTrans.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.ParentPODate.Value.Subtract(ts)).FirstOrDefault() : (DateTime?)null;
            var QuotationNumber = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationNumber).FirstOrDefault() : string.Empty;
            var QuotationDate = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationDate).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.QuotationDate.Value.Add(ts)).FirstOrDefault() : (DateTime?)null;
            var VendorID = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.VendorID).FirstOrDefault();
            var Name = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Name).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Name).FirstOrDefault() : string.Empty;
            var Address1 = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var Address2 = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var LocationID = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.Location).FirstOrDefault() : 0;
            var VendorLocationID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "loc").Select(x => x.ParentID).FirstOrDefault() : null;
            if (VendorLocationID != null)
            {
                var CityID = CMPSContext.LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorLocationID).Select(x => x.ID).FirstOrDefault() : 0;
                var StateID = CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == CityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                purchaseprint.Location = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchaseprint.city = location.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchaseprint.state = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchaseprint.country = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                var VendorCityID = CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == LocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (VendorCityID != 0)
                {
                    var StateID = CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == VendorCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var CountryID = CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == StateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    purchaseprint.state = location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == StateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchaseprint.city = location.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == VendorCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchaseprint.country = location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == CountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                }
            }
            var PhoneNo = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.PhoneNo).FirstOrDefault() : string.Empty;
            var GSTNo = CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() != null ? CMPSContext.VendorMaster.Where(x => x.ID == VendorID).Select(x => x.GSTNo).FirstOrDefault() : string.Empty;
            var DeliveryName = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryName).FirstOrDefault() : string.Empty;
            var DeliveryAddress1 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress1).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress1).FirstOrDefault() : string.Empty;
            var DeliveryAddress2 = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress2).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryAddress2).FirstOrDefault() : string.Empty;
            var DeliveryLocationID = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.DeliveryLocationID).FirstOrDefault();
            var purchaseLocationID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "loc").Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "loc").Select(x => x.ParentID).FirstOrDefault() : null;
            if (purchaseLocationID != null)
            {
                var purchaseCityID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID).Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseLocationID).Select(x => x.ID).FirstOrDefault() : 0;
                var purchaseStateID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                var purchaseCountryID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                purchaseprint.LocationName = CMPSContext.LocationMaster.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == LocationID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchaseprint.CityName = location.Where(x => x.ID == purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchaseprint.StateName = location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                purchaseprint.CountryName = location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
            }
            else
            {
                purchaseprint.purchaseCityID = CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == DeliveryLocationID && x.ParentTag == "City").Select(x => x.ID).FirstOrDefault() : 0;
                if (purchaseprint.purchaseCityID != 0)
                {
                    var purchaseStateID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseprint.purchaseCityID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseprint.purchaseCityID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    var purchaseCountryID = CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() != 0 ? CMPSContext.LocationMaster.Where(x => x.ID == purchaseStateID).Select(x => x.ParentID).FirstOrDefault() : 0;
                    purchaseprint.StateName = location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseStateID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchaseprint.CityName = location.Where(x => x.ID == purchaseprint.purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseprint.purchaseCityID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                    purchaseprint.CountryName = location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() != null ? location.Where(x => x.ID == purchaseCountryID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty;
                }
            }

            var GrossProductValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.GrossProductValue).FirstOrDefault();
            var TotalDiscountValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalDiscountValue).FirstOrDefault();
            var TotalTaxValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalTaxValue).FirstOrDefault();
            var TotalPOValue = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TotalPOValue).FirstOrDefault();

            var GetCESSAmount = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.CESSAmount).FirstOrDefault();
            var GetAdditionalCESSAmount = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.AdditionalCESSAmount).FirstOrDefault();
            var Tremandcond = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.TermsConditions).FirstOrDefault() : string.Empty;
            var Valid = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Validity).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Validity).FirstOrDefault() : string.Empty;
            var Deliver = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Delivery).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.Delivery).FirstOrDefault() : string.Empty;
            var payment = WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PaymentandTerms).FirstOrDefault() != null ? WYNKContext.PurchaseOrder.Where(x => x.RandomUniqueID == RandomUniqueID).Select(x => x.PaymentandTerms).FirstOrDefault() : string.Empty;
            var Address = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            var cAddress1 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            var cAddress2 = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Address3).FirstOrDefault() : string.Empty;
            var phone = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Phone1).FirstOrDefault() : string.Empty;
            var web = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.Website).FirstOrDefault() : string.Empty;
            var Compnayname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();

            var PurchaseOrderTransdetailscann = (from PoTrans in potrans.Where(x => x.RandomUniqueID == RandomUniqueID)
                                                 join Dur in drug on PoTrans.ItemID equals Dur.ID
                                                 join UOM in uom on PoTrans.UOMID equals UOM.id
                                                 select new
                                                 {
                                                     ItemID = drug.Where(x => x.ID == PoTrans.ItemID).Select(x => x.Brand).FirstOrDefault(),
                                                     ItemQty = PoTrans.ItemQty,
                                                     UOMID = uom.Where(x => x.id == PoTrans.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                     ItemRate = PoTrans.ItemRate,
                                                     POCancelledQty = PoTrans.POCancelledQty,
                                                     ItemValue = PoTrans.ItemValue,
                                                     DiscountPercentage = PoTrans.DiscountPercentage,
                                                     DiscountAmount = PoTrans.DiscountAmount,
                                                     GSTPercentage = PoTrans.GSTPercentage,
                                                     GSTTaxValue = PoTrans.GSTTaxValue,
                                                     CESSPercentage = PoTrans.CESSPercentage,
                                                     CESSAmount = PoTrans.CESSAmount,
                                                     AdditionalCESSPercentage = PoTrans.AdditionalCESSPercentage,
                                                     AddCESSAmount = PoTrans.AddCESSAmount,
                                                     Grossamount = PoTrans.ItemQty * PoTrans.ItemRate - PoTrans.ItemQty * PoTrans.ItemRate * PoTrans.DiscountPercentage / 100,
                                                     Totalamount = (PoTrans.ItemQty * PoTrans.ItemRate - PoTrans.ItemQty * PoTrans.ItemRate * PoTrans.DiscountPercentage / 100) +
                                                     (PoTrans.GSTTaxValue) + (PoTrans.CESSAmount) + (PoTrans.AddCESSAmount),
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
                    ParentPONumber = ParentPONumber,
                    ParentPODate = ParentPODate,
                    QoNo = QuotationNumber,
                    QoDate = QuotationDate,
                    VName = Name,
                    VAddress1 = Address1,
                    VAddress2 = Address2,
                    VLocation = purchaseprint.Location,
                    Vcity = purchaseprint.city,
                    Vstate = purchaseprint.state,
                    Vcountry = purchaseprint.country,
                    VPhoneno = PhoneNo,
                    VGstno = GSTNo,
                    PDelivery = DeliveryName,
                    PDeliveryadd1 = DeliveryAddress1,
                    PDeliveryadd2 = DeliveryAddress2,
                    PLocation = purchaseprint.LocationName,
                    PLocationncity = purchaseprint.CityName,
                    PLocationnstate = purchaseprint.StateName,
                    PLocationncountry = purchaseprint.CountryName,
                    PGross = GrossProductValue,
                    PDiscount = TotalDiscountValue,
                    PTotalax = TotalTaxValue,
                    PTotalpo = TotalPOValue,
                    Term = Tremandcond,
                    Vallid = Valid,
                    DDevlier = Deliver,
                    PPPayment = payment,
                    Addresss = Address,
                    Addresss1 = cAddress1,
                    Addresss2 = cAddress2,
                    Phonee = phone,
                    Webb = web,
                    company = Compnayname,
                    PGetCESSAmountt = GetCESSAmount,
                    PGetAdditionalCESSAmountt = GetAdditionalCESSAmount,
                    Potransdetails = PurchaseOrderTransdetailscann.ToList(),
                    Gettotalamount = PurchaseOrderTransdetailscann.Select(x => x.ItemValue).ToList().Sum(),

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



