using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class VendorMasterRepository : RepositoryBase<VendorMasterViewModel>, IVendorMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public VendorMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public VendorMasterViewModel GetlocationDetails(int id)
        {

            var locDetails = new VendorMasterViewModel();

            var v = CMPSContext.LocationMaster.Where(x => x.ID == id).Select(x => x.ParentDescription).FirstOrDefault();
            var stateid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == v).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptionstate = CMPSContext.LocationMaster.Where(x => x.ID == stateid).Select(x => x.ParentDescription).FirstOrDefault();

            var countryid = CMPSContext.LocationMaster.Where(x => x.ParentDescription == locDetails.ParentDescriptionstate).Select(x => x.ParentID).FirstOrDefault();
            locDetails.ParentDescriptioncountry = CMPSContext.LocationMaster.Where(x => x.ID == countryid).Select(x => x.ParentDescription).FirstOrDefault();



          

            return locDetails;

        }


        public dynamic InsertVendorMas(VendorMasterViewModel VendorMas)

        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var VendorMasters = new Vendor_Masters();
                    VendorMasters.VendorCode = VendorMas.VendorMasters.VendorCode;
                    VendorMasters.VendorCategory = VendorMas.VendorMasters.VendorCategory;
                    VendorMasters.Name = VendorMas.VendorMasters.Name;
                    VendorMasters.Address1 = VendorMas.VendorMasters.Address1;
                    VendorMasters.Address2 = VendorMas.VendorMasters.Address2;
                    VendorMasters.Address3 = VendorMas.VendorMasters.Address3;
                    VendorMasters.Location = VendorMas.VendorMasters.Location;
                    VendorMasters.DLNo = VendorMas.VendorMasters.DLNo;
                    VendorMasters.DLNo1 = VendorMas.VendorMasters.DLNo1;
                    VendorMasters.DLNoDate = VendorMas.VendorMasters.DLNoDate.Value.AddDays(1);
                    VendorMasters.DLNoDate1 = VendorMas.VendorMasters.DLNoDate1;
                    VendorMasters.Landmark = VendorMas.VendorMasters.Landmark;
                    VendorMasters.FSSAINo = VendorMas.VendorMasters.FSSAINo;
                    VendorMasters.GSTNo = VendorMas.VendorMasters.GSTNo;
                    VendorMasters.MobileNo = VendorMas.VendorMasters.MobileNo;
                    VendorMasters.PhoneNo = VendorMas.VendorMasters.PhoneNo;
                    VendorMasters.ContactPerson = VendorMas.VendorMasters.ContactPerson;
                    VendorMasters.CreditDays = VendorMas.VendorMasters.CreditDays;
                    VendorMasters.Creditlimit = VendorMas.VendorMasters.Creditlimit;
                    VendorMasters.Leadtime = VendorMas.VendorMasters.Leadtime;
                    VendorMasters.CmpID = VendorMas.VendorMasters.CmpID;
                    VendorMasters.IsActive = true;
                    VendorMasters.CreatedUTC = DateTime.UtcNow;
                    VendorMasters.CreatedBy = VendorMas.VendorMasters.CreatedBy;
                    CMPSContext.VendorMaster.Add(VendorMasters);
                    dbContextTransaction.Commit();

                    if (CMPSContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            
                        };
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
                
            };
        }
        public dynamic UpdateVendorMas(VendorMasterViewModel venup, int ID)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var VendorMasters = new Vendor_Masters();
                    VendorMasters = CMPSContext.VendorMaster.Where(x => x.ID == ID).FirstOrDefault();
                    VendorMasters.VendorCode = venup.VendorMasters.VendorCode;
                    VendorMasters.VendorCategory = venup.VendorMasters.VendorCategory;
                    VendorMasters.Name = venup.VendorMasters.Name;
                    VendorMasters.Address1 = venup.VendorMasters.Address1;
                    VendorMasters.Address2 = venup.VendorMasters.Address2;
                    VendorMasters.Address3 = venup.VendorMasters.Address3;
                    VendorMasters.Location = venup.VendorMasters.Location;
                    VendorMasters.DLNo = venup.VendorMasters.DLNo;
                    VendorMasters.DLNo1 = venup.VendorMasters.DLNo1;
                    VendorMasters.DLNoDate = venup.VendorMasters.DLNoDate;
                    VendorMasters.DLNoDate1 = venup.VendorMasters.DLNoDate1;
                    VendorMasters.Landmark = venup.VendorMasters.Landmark;
                    VendorMasters.FSSAINo = venup.VendorMasters.FSSAINo;
                    VendorMasters.GSTNo = venup.VendorMasters.GSTNo;
                    VendorMasters.MobileNo = venup.VendorMasters.MobileNo;
                    VendorMasters.PhoneNo = venup.VendorMasters.PhoneNo;
                    VendorMasters.ContactPerson = venup.VendorMasters.ContactPerson;
                    VendorMasters.CreditDays = venup.VendorMasters.CreditDays;
                    VendorMasters.Creditlimit = venup.VendorMasters.Creditlimit;
                    VendorMasters.Leadtime = venup.VendorMasters.Leadtime;
                    VendorMasters.IsActive = venup.VendorMasters.IsActive;
                    VendorMasters.UpdatedUTC = DateTime.UtcNow;
                    VendorMasters.UpdatedBy = venup.VendorMasters.UpdatedBy;
                    CMPSContext.VendorMaster.UpdateRange(VendorMasters);
                    CMPSContext.SaveChanges();
                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            
                        };
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
                
            };
        }
        public dynamic DeleteVendorMas(VendorMasterViewModel venup, int ID)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var VenMas = CMPSContext.VendorMaster.Where(x => x.ID == ID).ToList();

                    if (VenMas != null)
                    {
                        VenMas.All(x => { x.IsDeleted = true; return true; });
                        VenMas.All(x => { x.IsActive = false; return true; });
                        CMPSContext.VendorMaster.UpdateRange(VenMas);
                    }
                    dbContextTransaction.Commit();
                    if (CMPSContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                      
                        };
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
             
            };
        }
   
        public dynamic Getvendordetials(int CompanyID)
        {
            var vendor = new VendorMasterViewModel();

            vendor.Vendordetials = (from VM in CMPSContext.VendorMaster.Where(x =>x.IsActive == true && x.CmpID == CompanyID).GroupBy(x =>x.VendorCode)
                                    
                                    select new Vendordetials
                                    {
                                        Itemdescription = VM.FirstOrDefault().Name,
                                    }).AsNoTracking().ToList();

            return vendor;
        }
        public dynamic GetItemdetials(int CompanyID)
        {
            var vendor = new VendorMasterViewModel();

            vendor.itemdetials = (from VM in WYNKContext.DrugMaster.Where(x => x.IsActive == true && x.Cmpid == CompanyID)
                                  select new Itemdetials
                                  {
                                      Itemdescription = VM.Brand,
                                  }).AsNoTracking().ToList();

            return vendor;
        }
        public dynamic GetSelectedvendordetials(int code, int CMPID)
        {            
            var vendormaster = new VendorMasterViewModel();
            var drug = WYNKContext.DrugMaster.Where(x =>x.Cmpid == CMPID).ToList();            
            var v = WYNKContext.ItemVendorMapping.Where(x => x.VendorID == code && x.IsActive ==  true && x.CMPID == CMPID).ToList();
            IList<Vendordetials> vendordetials = new List<Vendordetials>();
            IList<NONVendordetials> nONVendordetials = new List<NONVendordetials>();


            foreach (var list in v)
            {
                var dd = new Vendordetials();
                dd.Itemdescription = drug.Where(x => x.ID == list.ItemID).Select(x => x.Brand).FirstOrDefault();
                dd.Itemselect = true;
                vendordetials.Add(dd);
            }

            var cpm = vendordetials;
            vendormaster.Vendordetials = cpm;
            vendormaster.NONVendordetials = (from dm in drug
                                             where cpm.All(a => a.Itemdescription != dm.Brand) 
                                             select new NONVendordetials
                                             {
                                                 Itemdescription = dm.Brand,
                                                 Itemselect = false,
                                             }).ToList();

            return vendormaster;
        }
        public dynamic GetSelectedItemdetials(int code, int CMPID)
        {
            var vendormaster = new VendorMasterViewModel();
            var Vendors = CMPSContext.VendorMaster.Where(x =>x.CmpID == CMPID && x.IsActive == true).ToList();
            var v = WYNKContext.ItemVendorMapping.Where(x => x.ItemID == code && x.IsActive == true && x.CMPID == CMPID).ToList();
            IList<Itemdetials> itemdetials = new List<Itemdetials>();
            IList<NONItemdetials> NONItemdetials = new List<NONItemdetials>();

            foreach (var list in v)
            {
                var dd = new Itemdetials();
                dd.Itemdescription = Vendors.Where(x => x.VendorCode == list.VendorID).Select(x => x.Name).FirstOrDefault();
                dd.Vendorselect = true;
                itemdetials.Add(dd);
            }

            var cpm = itemdetials;
            vendormaster.itemdetials = cpm;
            vendormaster.NONItemdetials = (from dm in Vendors
                                           where cpm.All(a => a.Itemdescription != dm.Name)
                                             select new NONItemdetials
                                             {
                                                 Itemdescription = dm.Name,
                                                 Vendorselect = false,
                                             }).ToList();         
            return vendormaster;
        }
        public dynamic InsertVendorData(VendorMasterViewModel Insertvendordata)
        {
            if (Insertvendordata.unvendordetails.Count() != 0)
            {
                foreach (var item in Insertvendordata.unvendordetails.ToList())
                {
                    var Itemvendormapping = new ItemVendorMapping();
                    var id = WYNKContext.DrugMaster.Where(x => x.Brand == item.Description && x.Cmpid == Convert.ToInt32(Insertvendordata.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var itemid = WYNKContext.ItemVendorMapping.OrderBy(x =>x.CreatedUTC).Where(x => x.ItemID == id && x.VendorID ==  Insertvendordata.Code && x.IsActive ==  true).Select(x => x.ItemVendorID).FirstOrDefault();
                    Itemvendormapping.ItemVendorID = itemid;
                    Itemvendormapping.CMPID = Convert.ToInt32(Insertvendordata.CompanyID);
                    Itemvendormapping.ItemID = id;


                    Itemvendormapping.VendorID = Insertvendordata.Code;
                    Itemvendormapping.IsActive = false;
                    Itemvendormapping.IsDeleted = true;
                    Itemvendormapping.CreatedUTC = DateTime.UtcNow;
                    Itemvendormapping.UpdatedUTC = DateTime.UtcNow;
                    Itemvendormapping.CreatedBy = Convert.ToInt32(Insertvendordata.UserID);
                    Itemvendormapping.UpdatedBy = null;

                    WYNKContext.ItemVendorMapping.UpdateRange(Itemvendormapping);
                    WYNKContext.SaveChanges();
                }
            }




            if (Insertvendordata.VendorDetails.Count() != 0)
            {
                foreach (var item in Insertvendordata.VendorDetails.ToList())
                {
                    var Drugdetails = new Drug_Master();
                    var Itemvendormapping = new ItemVendorMapping();
                    var id = WYNKContext.DrugMaster.Where(x => x.Brand == item.Description && x.Cmpid == Convert.ToInt32(Insertvendordata.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    Drugdetails.ID = id;
                    Itemvendormapping.CMPID = Convert.ToInt32(Insertvendordata.CompanyID);
                    Itemvendormapping.ItemID = Drugdetails.ID;
                    var VendorID = CMPSContext.VendorMaster.Where(x => x.VendorCode == Insertvendordata.Code && x.CmpID == Convert.ToInt32(Insertvendordata.CompanyID)).Select(x => x.ID).FirstOrDefault();

                    Itemvendormapping.VendorID = Insertvendordata.Code;
                    Itemvendormapping.IsActive = true;
                    Itemvendormapping.IsDeleted = false;
                    Itemvendormapping.CreatedUTC = DateTime.UtcNow;
                    Itemvendormapping.CreatedBy = Convert.ToInt32(Insertvendordata.UserID);
                    WYNKContext.ItemVendorMapping.Add(Itemvendormapping);
                    WYNKContext.SaveChanges();
                }
            }
  


            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully"
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }
        public dynamic InsertItemData(VendorMasterViewModel InsertItemdata)
        {
            if (InsertItemdata.unitemDetails.Count() != 0)
            {
                foreach (var item in InsertItemdata.unitemDetails.ToList())
                {
                    var Itemvendormapping = new ItemVendorMapping();
                    var id = CMPSContext.VendorMaster.Where(x => x.Name == item.Description).Select(x => x.VendorCode).FirstOrDefault();
                    var drugid = WYNKContext.DrugMaster.Where(x => x.VendorCode == InsertItemdata.Code).Select(x => x.ID).FirstOrDefault();
                    var itemid = WYNKContext.ItemVendorMapping.OrderBy(x => x.CreatedUTC).Where(x => x.VendorID == id && x.ItemID == drugid && x.IsActive == true).Select(x => x.ItemVendorID).FirstOrDefault();
                    Itemvendormapping.ItemVendorID = itemid;
                    Itemvendormapping.CMPID = Convert.ToInt32(InsertItemdata.CompanyID);
                    Itemvendormapping.ItemID = drugid;
                    Itemvendormapping.VendorID = id;
                    Itemvendormapping.IsActive = false;
                    Itemvendormapping.IsDeleted = true;
                    Itemvendormapping.CreatedUTC = DateTime.UtcNow;
                    Itemvendormapping.UpdatedUTC = DateTime.UtcNow;
                    Itemvendormapping.CreatedBy = Convert.ToInt32(InsertItemdata.UserID);
                    Itemvendormapping.UpdatedBy = null;
                    WYNKContext.ItemVendorMapping.UpdateRange(Itemvendormapping);
                    WYNKContext.SaveChanges();
                }
            }




            if (InsertItemdata.ItemDetails.Count() != 0)
            {
                foreach (var item in InsertItemdata.ItemDetails.ToList())
                {
                    var Itemvendormapping = new ItemVendorMapping();
                    var id = CMPSContext.VendorMaster.Where(x => x.Name == item.Description).Select(x => x.VendorCode).FirstOrDefault();
                    Itemvendormapping.CMPID = Convert.ToInt32(InsertItemdata.CompanyID);
                    Itemvendormapping.ItemID = InsertItemdata.Code;
                    Itemvendormapping.VendorID = id;
                    Itemvendormapping.IsActive = true;
                    Itemvendormapping.IsDeleted = false;
                    Itemvendormapping.CreatedUTC = DateTime.UtcNow;
                    Itemvendormapping.CreatedBy = Convert.ToInt32(InsertItemdata.UserID);
                    WYNKContext.ItemVendorMapping.Add(Itemvendormapping);
                    WYNKContext.SaveChanges();
                }
            }




            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully"
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }
    }

}