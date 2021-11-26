using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    class SpecialityVSTestRepository : RepositoryBase<specialityvstest>, ISpecialityVSTestRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public SpecialityVSTestRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic UpdateTest(specialityvstest SpecialityVSTest)
        {



            var olm = new OneLine_Masters();

            olm.ParentDescription = SpecialityVSTest.OneLineMaster.ParentDescription;
            olm.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Investigation Test").Select(x => x.OLMID).FirstOrDefault();
            olm.ParentTag = "INV";
            olm.IsActive = true;
            olm.IsDeleted = false;
            olm.CreatedUTC = DateTime.UtcNow;
            olm.CreatedBy = SpecialityVSTest.OneLineMaster.CreatedBy;
            CMPSContext.OneLineMaster.Add(olm);


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };



        }

        public specialityvstest Getinvestigationvalues(int CmpID)
        {

            var sptest = new specialityvstest();
            sptest.SpecialityVSTest = new List<SpecialityVSTest>();

            var services = WYNKContext.Services.ToList();

            var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == CmpID).Select(x => x.parentDescription).FirstOrDefault();
            var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == CmpID).Select(x => x.ID).FirstOrDefault();

            sptest.Specialitydetials = (from SE in services.Where(u => u.ParentID == pid && u.ParentID != 0).OrderBy(u => u.Description)


                                        select new Specialitydetials
                                        {
                                            Itemdescription = SE.Description,

                                        }).ToList();


            return sptest;
        }

        public specialityvstest Getservicevalues(int CmpID)
        {

            var sptest = new specialityvstest();
            sptest.SpecialityVSTest = new List<SpecialityVSTest>();

            var services = WYNKContext.Services.Where(x => x.CMPID == CmpID).ToList();

            var a = (from SE in services.Where(u => u.ParentID != 0 && u.CMPID == CmpID && u.Tag != "Pkg").OrderBy(u => u.Description)


                     select new
                     {
                         Itemdescription = SE.Description,

                     }).ToList();


            var Parent0 = services.Where(u => u.ParentID == 0 && u.CMPID == CmpID).Select(x => x.ID).ToList();

            var ParentIds = (from SE in services.Where(u => u.CMPID == CmpID)
                             where Parent0.Contains(SE.ParentID)
                             group SE by SE.ParentID into g
                             select new
                             {
                                 ID = g.Key,
                             }).ToList();


            var res = (from Parent1 in Parent0
                       where !ParentIds.Any(
                                         x => x.ID == Parent1)
                       select Parent1).ToList();

            var result = (from SE in services.Where(u => u.CMPID == CmpID)
                          where res.Contains(SE.ID)
                          select new
                          {
                              Itemdescription = SE.Description
                          }).ToList();

             var c = a.Concat(result);
            sptest.Specialitydetials = (from SE in c.OrderBy(u => u.Itemdescription)


                                        select new Specialitydetials
                                        {
                                            Itemdescription = SE.Itemdescription,

                                        }).ToList();
            return sptest;
        }
        public specialityvstest GetSelectedspecdetials(int ID, int CmpID)
        {

            var sptest = new specialityvstest();
            var drugmaster = new DrugMaster();
            var vendormaster = new VendorMasterViewModel();
            var ICDSpec = WYNKContext.ICDSpecialityCode.ToList();
            var spvstest = WYNKContext.SpecialityVSTest.ToList();
            var oneline = CMPSContext.OneLineMaster.ToList();
            var services = WYNKContext.Services.ToList();
            var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == CmpID).Select(x => x.parentDescription).FirstOrDefault();
            var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == CmpID).Select(x => x.ID).FirstOrDefault();

            var v = WYNKContext.SpecialityVSTest.Where(x => x.SpecialityID == ID && x.IsActive == true).ToList();
            IList<Specialitydetials> Specialitydetials = new List<Specialitydetials>();


            foreach (var list in v)
            {
                var dd = new Specialitydetials();
                dd.Itemdescription = services.Where(x => x.ID == list.InvestigationID).Select(x => x.Description).FirstOrDefault();
                dd.Itemselect = true;
                Specialitydetials.Add(dd);
            }

            var cpm = Specialitydetials;
            sptest.Specialitydetials = cpm;

            sptest.NONSpecialitydetials = (from OLM in services.Where(x => x.ParentID == pid && x.ParentID != 0).OrderBy(x => x.Description)
                                           where cpm.All(a => a.Itemdescription != OLM.Description)
                                           select new NONSpecialitydetials
                                           {
                                               Itemdescription = OLM.Description,
                                               Itemselect = false,
                                           }).ToList();


            return sptest;
        }

        public specialityvstest GetSelectedservdetials(int ID, int CmpID)
        {

            var sptest = new specialityvstest();
            var drugmaster = new DrugMaster();
            var vendormaster = new VendorMasterViewModel();
            var ICDSpec = WYNKContext.ICDSpecialityCode.ToList();
            var spvstest = WYNKContext.SpecialityVSTest.ToList();
            var oneline = CMPSContext.OneLineMaster.ToList();
            var services = WYNKContext.Services.ToList();
            var v = WYNKContext.PackageMapping.Where(x => x.SpecialityID == ID && x.IsActive == true).ToList();
            IList<Specialitydetials> Specialitydetials = new List<Specialitydetials>();


            foreach (var list in v)
            {
                var dd = new Specialitydetials();
                dd.Itemdescription = services.Where(x => x.ID == list.ServiceID).Select(x => x.Description).FirstOrDefault();
                dd.Itemselect = true;
                Specialitydetials.Add(dd);
            }


            var a = (from SE in services.Where(u => u.ParentID != 0 && u.CMPID == CmpID && u.Tag != "Pkg").OrderBy(u => u.Description)


                     select new
                     {
                         Itemdescription = SE.Description,

                     }).ToList();


            var Parent0 = services.Where(u => u.ParentID == 0 && u.CMPID == CmpID).Select(x => x.ID).ToList();

            var ParentIds = (from SE in services.Where(u => u.CMPID == CmpID)
                             where Parent0.Contains(SE.ParentID)
                             group SE by SE.ParentID into g
                             select new
                             {
                                 ID = g.Key,
                             }).ToList();


            var res = (from Parent1 in Parent0
                       where !ParentIds.Any(
                                         x => x.ID == Parent1)
                       select Parent1).ToList();

            var result = (from SE in services.Where(u => u.CMPID == CmpID)
                          where res.Contains(SE.ID)
                          select new
                          {
                              Itemdescription = SE.Description
                          }).ToList();

            var c = a.Concat(result);
            var cpm = Specialitydetials;
            sptest.Specialitydetials = cpm;
            sptest.NONSpecialitydetials = (from SE in c.OrderBy(u => u.Itemdescription)
                                           where cpm.All(b => b.Itemdescription != SE.Itemdescription)

                                           select new NONSpecialitydetials
                                        {
                                            Itemdescription = SE.Itemdescription,

                                        }).ToList();


            return sptest;
        }

        public dynamic Insertspecialitydata(specialityvstest SpecialityVSTest)
        {
            if (SpecialityVSTest.SpecialityDetail.Count() != 0)
            {
                foreach (var item in SpecialityVSTest.SpecialityDetail.ToList())
                {
                    var specvstest = new SpecialityVSTest();
                    var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.parentDescription).FirstOrDefault();
                    var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var id = WYNKContext.Services.Where(x => x.Description == item.Description && x.ParentID == pid && x.ParentID != 0).Select(x => x.ID).FirstOrDefault();

                    var itemid = WYNKContext.SpecialityVSTest.OrderBy(x => x.CreatedUTC).Where(x => x.InvestigationID == id && x.SpecialityID == SpecialityVSTest.Code && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    specvstest.ID = itemid;
                    specvstest.CMPID = Convert.ToInt32(SpecialityVSTest.CompanyID);
                    specvstest.InvestigationID = id;

                    var specid = WYNKContext.ICDSpecialityCode.Where(x => x.ID == SpecialityVSTest.Code).Select(x => x.ID).FirstOrDefault();

                    specvstest.SpecialityID = SpecialityVSTest.Code;
                    specvstest.IsActive = false;
                    specvstest.IsDeleted = true;
                    specvstest.CreatedUTC = DateTime.UtcNow;
                    specvstest.UpdatedUTC = DateTime.UtcNow;
                    specvstest.CreatedBy = Convert.ToInt32(SpecialityVSTest.UserID);
                    specvstest.UpdatedBy = null;
                    WYNKContext.SpecialityVSTest.UpdateRange(specvstest);
                    WYNKContext.SaveChanges();
                }
            }




            if (SpecialityVSTest.SSpecialityDetail.Count() != 0)
            {
                foreach (var item in SpecialityVSTest.SSpecialityDetail.ToList())
                {
                    var Drugdetails = new Drug_Master();
                    var specvstest = new SpecialityVSTest();
                    var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.parentDescription).FirstOrDefault();
                    var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var id = WYNKContext.Services.Where(x => x.Description == item.Description && x.ParentID == pid && x.ParentID != 0).Select(x => x.ID).FirstOrDefault();
                    specvstest.CMPID = Convert.ToInt32(SpecialityVSTest.CompanyID);
                    specvstest.InvestigationID = id;
                    var specid = WYNKContext.SpecialityVSTest.Where(x => x.ID == SpecialityVSTest.Code).Select(x => x.ID).FirstOrDefault();

                    specvstest.SpecialityID = SpecialityVSTest.Code;
                    specvstest.IsActive = true;
                    specvstest.IsDeleted = false;
                    specvstest.CreatedUTC = DateTime.UtcNow;
                    specvstest.CreatedBy = Convert.ToInt32(SpecialityVSTest.UserID);
                    WYNKContext.SpecialityVSTest.Add(specvstest);
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


        public dynamic Insertpackagedata(specialityvstest SpecialityVSTest)
        {
            if (SpecialityVSTest.SpecialityDetail.Count() != 0)
            {
                foreach (var item in SpecialityVSTest.SpecialityDetail.ToList())
                {
                    var specvstest = new PackageMapping();

                    var id = WYNKContext.Services.Where(x => x.Description == item.Description && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.ID).FirstOrDefault();

                    var itemid = WYNKContext.PackageMapping.OrderBy(x => x.CreatedUTC).Where(x => x.ServiceID == id && x.SpecialityID == SpecialityVSTest.Code && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    specvstest.ID = itemid;
                    specvstest.CMPID = Convert.ToInt32(SpecialityVSTest.CompanyID);
                    specvstest.ServiceID = id;

                    var specid = WYNKContext.ICDSpecialityCode.Where(x => x.ID == SpecialityVSTest.Code).Select(x => x.ID).FirstOrDefault();

                    specvstest.SpecialityID = SpecialityVSTest.Code;
                    specvstest.IsActive = false;
                    specvstest.IsDeleted = true;
                    specvstest.CreatedUTC = DateTime.UtcNow;
                    specvstest.UpdatedUTC = DateTime.UtcNow;
                    specvstest.CreatedBy = Convert.ToInt32(SpecialityVSTest.UserID);
                    specvstest.UpdatedBy = null;
                    WYNKContext.PackageMapping.UpdateRange(specvstest);
                    WYNKContext.SaveChanges();
                }
            }




            if (SpecialityVSTest.SSpecialityDetail.Count() != 0)
            {
                foreach (var item in SpecialityVSTest.SSpecialityDetail.ToList())
                {
                    var Drugdetails = new Drug_Master();
                    var specvstest = new PackageMapping();

                    var id = WYNKContext.Services.Where(x => x.Description == item.Description && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    specvstest.CMPID = Convert.ToInt32(SpecialityVSTest.CompanyID);
                    specvstest.ServiceID = id;
                    var specid = WYNKContext.SpecialityVSTest.Where(x => x.ID == SpecialityVSTest.Code).Select(x => x.ID).FirstOrDefault();

                    specvstest.SpecialityID = SpecialityVSTest.Code;
                    specvstest.IsActive = true;
                    specvstest.IsDeleted = false;
                    specvstest.CreatedUTC = DateTime.UtcNow;
                    specvstest.CreatedBy = Convert.ToInt32(SpecialityVSTest.UserID);
                    WYNKContext.PackageMapping.Add(specvstest);
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

        ////////////////////////////////////////diagnosis vs test////////////////////////////////////////////////////


        public specialityvstest GetSelecteddiadetials(int ID, int CmpID)
        {

            var sptest = new specialityvstest();
            var drugmaster = new DrugMaster();
            var vendormaster = new VendorMasterViewModel();
            var ICDSpec = WYNKContext.ICDSpecialityCode.ToList();
            //var spvstest = WYNKContext.SpecialityVSTest.ToList();
            var oneline = CMPSContext.OneLineMaster.ToList();
            var services = WYNKContext.Services.ToList();
            var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == CmpID).Select(x => x.parentDescription).FirstOrDefault();
            var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == CmpID).Select(x => x.ID).FirstOrDefault();

            var v = WYNKContext.DiagnosisVSTest.Where(x => x.DiagnosisID == ID && x.IsActive == true).ToList();
            IList<Specialitydetials> Specialitydetials = new List<Specialitydetials>();


            foreach (var list in v)
            {
                var dd = new Specialitydetials();
                dd.Itemdescription = services.Where(x => x.ID == list.InvestigationID).Select(x => x.Description).FirstOrDefault();
                dd.Itemselect = true;
                Specialitydetials.Add(dd);
            }

            var cpm = Specialitydetials;
            sptest.Specialitydetials = cpm;

            sptest.NONSpecialitydetials = (from OLM in services.Where(x => x.ParentID == pid && x.ParentID != 0).OrderBy(x => x.Description)
                                           where cpm.All(a => a.Itemdescription != OLM.Description)
                                           select new NONSpecialitydetials
                                           {
                                               Itemdescription = OLM.Description,
                                               Itemselect = false,
                                           }).ToList();


            return sptest;
        }


        public dynamic Insertdiagnosisdata(specialityvstest SpecialityVSTest)
        {
            if (SpecialityVSTest.SpecialityDetail.Count() != 0)
            {
                foreach (var item in SpecialityVSTest.SpecialityDetail.ToList())
                {
                    var specvstest = new DiagnosisVSTest();
                    var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.parentDescription).FirstOrDefault();
                    var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var id = WYNKContext.Services.Where(x => x.Description == item.Description && x.ParentID == pid && x.ParentID != 0).Select(x => x.ID).FirstOrDefault();

                    var itemid = WYNKContext.DiagnosisVSTest.OrderBy(x => x.CreatedUTC).Where(x => x.InvestigationID == id && x.DiagnosisID == SpecialityVSTest.Code && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
                    specvstest.ID = itemid;
                    specvstest.CMPID = Convert.ToInt32(SpecialityVSTest.CompanyID);
                    specvstest.InvestigationID = id;

                    var specid = WYNKContext.SpecialityMaster.Where(x => x.ID == SpecialityVSTest.Code).Select(x => x.ID).FirstOrDefault();

                    specvstest.DiagnosisID = SpecialityVSTest.Code;
                    specvstest.IsActive = false;
                    specvstest.IsDeleted = true;
                    specvstest.CreatedUTC = DateTime.UtcNow;
                    specvstest.UpdatedUTC = DateTime.UtcNow;
                    specvstest.CreatedBy = Convert.ToInt32(SpecialityVSTest.UserID);
                    specvstest.UpdatedBy = null;
                    WYNKContext.DiagnosisVSTest.UpdateRange(specvstest);
                    WYNKContext.SaveChanges();
                }
            }




            if (SpecialityVSTest.SSpecialityDetail.Count() != 0)
            {
                foreach (var item in SpecialityVSTest.SSpecialityDetail.ToList())
                {
                    var Drugdetails = new Drug_Master();
                    var specvstest = new DiagnosisVSTest();
                    var parid = WYNKContext.ServiceMaster.Where(x => x.Tag == "INV" && x.IsActive == true && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.parentDescription).FirstOrDefault();
                    var pid = WYNKContext.Services.Where(x => x.ID == Convert.ToInt32(parid) && x.CMPID == Convert.ToInt32(SpecialityVSTest.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var id = WYNKContext.Services.Where(x => x.Description == item.Description && x.ParentID == pid && x.ParentID != 0).Select(x => x.ID).FirstOrDefault();
                    specvstest.CMPID = Convert.ToInt32(SpecialityVSTest.CompanyID);
                    specvstest.InvestigationID = id;
                    var specid = WYNKContext.SpecialityMaster.Where(x => x.ID == SpecialityVSTest.Code).Select(x => x.ID).FirstOrDefault();

                    specvstest.DiagnosisID = SpecialityVSTest.Code;
                    specvstest.IsActive = true;
                    specvstest.IsDeleted = false;
                    specvstest.CreatedUTC = DateTime.UtcNow;
                    specvstest.CreatedBy = Convert.ToInt32(SpecialityVSTest.UserID);
                    WYNKContext.DiagnosisVSTest.Add(specvstest);
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






