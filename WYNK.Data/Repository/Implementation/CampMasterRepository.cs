using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class CampMasterRepository : RepositoryBase<campmasterViewModel>, ICampMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public CampMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }
        public dynamic AddOrganization(string ID)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var olm = new OneLine_Masters();
                    olm.ParentDescription = ID;
                    olm.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CAMPORGANIZATION").Select(x => x.OLMID).FirstOrDefault();
                    olm.ParentTag = "CMPORG";
                    olm.CreatedBy = 1;
                    olm.IsActive = true;
                    olm.IsDeleted = false;
                    olm.CreatedUTC = DateTime.UtcNow;
                    CMPSContext.OneLineMaster.Add(olm);
                    CMPSContext.SaveChanges();
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

        public dynamic UpdateOrganization(string ID, string status, string Desc)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {

                    var detsails = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(ID) && x.ParentTag == "CMPORG").FirstOrDefault();
                    detsails.ParentDescription = Desc;
                    detsails.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CAMPORGANIZATION").Select(x => x.OLMID).FirstOrDefault();
                    detsails.ParentTag = "CMPORG";
                    detsails.UpdatedBy = 1;
                    if (status == "true")
                    {
                        detsails.IsActive = true;
                        detsails.IsDeleted = false;
                    }
                    else
                    {
                        detsails.IsActive = false;
                        detsails.IsDeleted = false;
                    }
                    detsails.UpdatedUTC = DateTime.UtcNow;
                    CMPSContext.Entry(detsails).State = EntityState.Modified;                    
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

        public dynamic DeleteOrganization(string ID, int valueolmid)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var olm = new OneLine_Masters();
                    var detsails = CMPSContext.OneLineMaster.Where(x => x.OLMID == valueolmid && x.ParentTag == "CMPORG").FirstOrDefault();
                    detsails.ParentDescription = ID;
                    detsails.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CAMPORGANIZATION").Select(x => x.OLMID).FirstOrDefault();
                    detsails.ParentTag = "CMPORG";
                    detsails.UpdatedBy = 1;
                    detsails.IsActive = false;
                    detsails.IsDeleted = true;
                    detsails.UpdatedUTC = DateTime.UtcNow;
                    CMPSContext.Entry(detsails).State = EntityState.Modified;
                    
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

        public dynamic Getallorganization()
        {
            var dataa = new campmasterViewModel();
            dataa.campognizationdetails = new List<campognizationdetails>();
            var lm = CMPSContext.LocationMaster.AsNoTracking().ToList();
            var coo = WYNKContext.CAMPORGANIZATION.AsNoTracking().ToList();
            var olm = CMPSContext.OneLineMaster.AsNoTracking().ToList();

            dataa.campognizationdetails = (from co in coo
                                           select new campognizationdetails
                                           {
                                               OrganizationID = co.ID,
                                               OrganizationName = co.OrganizationName,
                                               OrganizationType = olm.Where(x => x.OLMID == co.OrganizationType).Select(x => x.ParentDescription).FirstOrDefault(),
                                               Address1 = co.Address1,
                                               Address2 = co.Address2,
                                               Address3 = co.Address3,
                                               Location = getlocationvalue(co.Location),
                                               State = lm.Where(x => x.ID == co.State).Select(x => x.ParentDescription).FirstOrDefault(),
                                               City = lm.Where(x => x.ID == co.City).Select(x => x.ParentDescription).FirstOrDefault(),
                                               Country = lm.Where(x => x.ID == co.Country).Select(x => x.ParentDescription).FirstOrDefault(),
                                               ContactPerson = co.ContactPerson,
                                               Phone = co.Phone,
                                               EMailID = co.EMailID,
                                               Website = co.Website,
                                               Isactive = co.IsActive,
                                               Normalactive = getlocationvactive(co.IsActive),
                                           }).ToList();
            return dataa;

        }
        public dynamic Getallcampmasterdata()
        {
            var dataa = new campmasterViewModel();
            dataa.campmasterdetails = new List<campmasterdetails>();
            var lm = CMPSContext.LocationMaster.AsNoTracking().ToList();
            var coo = WYNKContext.CAMP.AsNoTracking().ToList();
            var coorg = WYNKContext.CAMPORGANIZATION.AsNoTracking().ToList();
            var olm = CMPSContext.OneLineMaster.AsNoTracking().ToList();
            dataa.campmasterdetails = (from co in coo
                                           select new campmasterdetails
                                           {
                                               CampMastreID = co.CampID,
                                               CampMastreName = co.CampName,
                                               CampMastreorganisedby = coorg.Where(x => x.ID == co.Organisedby).Select(x => x.OrganizationName).FirstOrDefault(),
                                               CampMastrefrom = co.Campfrom.ToString("dd-MMM-yyyy"),
                                               CampMastreto = co.Campto.ToString("dd-MMM-yyyy"),
                                               CampMastrelocation = getlocationvalue(co.Location),
                                               CampMastrestate = lm.Where(x => x.ID == co.State).Select(x => x.ParentDescription).FirstOrDefault(),
                                               CampMastrecity = lm.Where(x => x.ID == co.City).Select(x => x.ParentDescription).FirstOrDefault(),
                                               CampMastrecountry = lm.Where(x => x.ID == co.Country).Select(x => x.ParentDescription).FirstOrDefault(),
                                               Isactive = co.IsActive,
                                               Normalactive = getlocationvactive(co.IsActive),
                                           }).ToList();
            return dataa;

        }

        public dynamic getlocationvactive(bool loc)
        {
            var d = loc;
            var c = "";
            if (loc == true)
            {
                c = "Yes";
            }
            else
            {
                c = "No";
            }
            return c;
        }


        public dynamic getlocationvalue(int? loc)
        {
            var d = loc;
            var c = "";
            if(loc != null)
            {
                c = CMPSContext.LocationMaster.Where(x => x.ID == loc).Select(x => x.ParentDescription).FirstOrDefault();
            }
            else
            {
                c = null;
            }
            return c;
        }
        public dynamic SubmitOrganizationData(campmasterViewModel Fundus)
        {

            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var data = new Camporganization_Model();

                    var id = WYNKContext.CAMPORGANIZATION.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
                    if (id == 0)
                    {
                        data.ID = 1;

                    }
                    else
                    {
                        data.ID = id + 1;
                    }
                    data.OrganizationName = Fundus.OrganizationName;
                    data.Address1 = Fundus.Address1;
                    data.Address2 = Fundus.Address2;
                    data.Address3 = Fundus.Address3;
                    if (Fundus.Location != null)
                    {
                        data.Location = Convert.ToInt32(Fundus.Location);
                    }
                    data.City = Convert.ToInt32(Fundus.City);
                    data.State = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.State && x.ParentTag == "State").Select(x => x.ID).FirstOrDefault();
                    data.Country = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.Country && x.ParentTag == "COUNTRY").Select(x => x.ID).FirstOrDefault();
                    data.ContactPerson = Fundus.ContactPerson;
                    data.Phone = Convert.ToInt64(Fundus.Phone);
                    data.EMailID = Fundus.EMailID;
                    data.Website = Fundus.Website;
                    data.OrganizationType = Convert.ToInt32(Fundus.OrganizationType);
                    data.CreatedUTC = DateTime.UtcNow;
                    data.Createdby = 1;
                    data.IsActive = true;
                    WYNKContext.CAMPORGANIZATION.Add(data);
                    WYNKContext.SaveChanges();
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
                    ErrorLog oErrorLogs = new ErrorLog();
                    if (ex.Message == null)
                    {
                        oErrorLogs.WriteErrorLog("CampOrganization : ", ex.InnerException.Message);
                    }
                    else
                    {
                        oErrorLogs.WriteErrorLog("CampOrganization : ", ex.Message);
                    }
                }

            }
            return new
            {
                Success = false,

            };

        }
        public dynamic SubmitCampmasterData(campmasterViewModel Fundus)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var data = new CampMaster_Model();
                    var id = WYNKContext.CAMP.OrderByDescending(x => x.CampID).Select(x => x.CampID).FirstOrDefault();
                    if (id == 0)
                    {
                        data.CampID = 1;

                    }
                    else
                    {
                        data.CampID = id + 1;
                    }
                    data.CampName = Fundus.CampMastreName;
                    data.Campfrom = Fundus.CampMastrefrom.AddDays(1);
                    data.Campto = Fundus.CampMastreto.AddDays(1);
                    data.Organisedby = Convert.ToInt32(Fundus.CampMastreorganisedby);
                    if (Fundus.CampMastrelocation != null)
                    {
                        data.Location = Convert.ToInt32(Fundus.CampMastrelocation);
                    }
                    data.City = Convert.ToInt32(Fundus.CampMastrecity);
                    data.State = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.CampMastrestate && x.ParentTag == "State").Select(x => x.ID).FirstOrDefault();
                    data.Country = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.CampMastrecountry && x.ParentTag == "COUNTRY").Select(x => x.ID).FirstOrDefault();
                    data.CreatedUTC = DateTime.UtcNow;
                    data.Createdby = 1;
                    data.IsActive = true;
                    WYNKContext.CAMP.Add(data);
                    WYNKContext.SaveChanges();
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
                    ErrorLog oErrorLogs = new ErrorLog();
                    if (ex.Message == null)
                    {
                        oErrorLogs.WriteErrorLog("CampMaster : ", ex.InnerException.Message);
                    }
                    else
                    {
                        oErrorLogs.WriteErrorLog("CampMaster : ", ex.Message);
                    }
                }

            }
            return new
            {
                Success = false,

            };

        }
        public dynamic Updatecampmasterdata(campmasterViewModel Fundus)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var data = WYNKContext.CAMP.Where(x => x.CampID == Convert.ToInt32(Fundus.CampMastreID)).FirstOrDefault();
                    data.CampName = Fundus.CampMastreName;
                    var fdate = data.Campfrom.Date;

                    if(Fundus.CampMastrefrom.Date == fdate)
                    {
                        data.Campfrom = Fundus.CampMastrefrom;
                    }
                    else
                    {
                        data.Campfrom = Fundus.CampMastrefrom.AddDays(1);
                    }
                    var tdate = data.Campto.Date;

                    if (Fundus.CampMastreto.Date == tdate)
                    {
                        data.Campto = Fundus.CampMastreto;
                    }
                    else
                    {
                        data.Campto = Fundus.CampMastreto.AddDays(1);
                    }
           
                    data.Organisedby = Convert.ToInt32(Fundus.CampMastreorganisedby);
                    if (Fundus.CampMastrelocation != null)
                    {
                        data.Location = Convert.ToInt32(Fundus.CampMastrelocation);
                    }
                    data.City = Convert.ToInt32(Fundus.CampMastrecity);
                    data.State = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.CampMastrestate && x.ParentTag == "State").Select(x => x.ID).FirstOrDefault();
                    data.Country = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.CampMastrecountry && x.ParentTag == "COUNTRY").Select(x => x.ID).FirstOrDefault();
                    data.CreatedUTC = DateTime.UtcNow;
                    data.Createdby = 1;
                    data.IsActive =Convert.ToBoolean(Fundus.IsActive);
                    WYNKContext.Entry(data).State = EntityState.Modified;
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
                    ErrorLog oErrorLogs = new ErrorLog();
                    if (ex.Message == null)
                    {
                        oErrorLogs.WriteErrorLog("CampMaster : ", ex.InnerException.Message);
                    }
                    else
                    {
                        oErrorLogs.WriteErrorLog("CampMaster : ", ex.Message);
                    }
                }

            }
            return new
            {
                Success = false,

            };

        }
        public dynamic UpdateOrganizationData(campmasterViewModel Fundus)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {                   
                    var data = WYNKContext.CAMPORGANIZATION.Where(x => x.ID == Convert.ToInt32(Fundus.OrganizationID)).FirstOrDefault();                
                    data.OrganizationName = Fundus.OrganizationName;
                    data.Address1 = Fundus.Address1;
                    data.Address2 = Fundus.Address2;
                    data.Address3 = Fundus.Address3;
                    if (Fundus.Location != null)
                    {
                        data.Location = Convert.ToInt32(Fundus.Location);
                    }
                    data.City = Convert.ToInt32(Fundus.City);
                    data.State = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.State && x.ParentTag == "State").Select(x => x.ID).FirstOrDefault();
                    data.Country = CMPSContext.LocationMaster.Where(x => x.ParentDescription == Fundus.Country && x.ParentTag == "COUNTRY").Select(x => x.ID).FirstOrDefault();
                    data.ContactPerson = Fundus.ContactPerson;
                    data.Phone = Convert.ToInt64(Fundus.Phone);
                    data.EMailID = Fundus.EMailID;
                    data.Website = Fundus.Website;
                    data.OrganizationType = Convert.ToInt32(Fundus.OrganizationType);
                    data.UpdatedUTC = DateTime.UtcNow;
                    data.Updatedby = 1;
                    data.IsActive =Convert.ToBoolean(Fundus.IsActive);
                    WYNKContext.Entry(data).State = EntityState.Modified;
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
                    ErrorLog oErrorLogs = new ErrorLog();
                    if (ex.Message == null)
                    {
                        oErrorLogs.WriteErrorLog("CampOrganization : ", ex.InnerException.Message);
                    }
                    else
                    {
                        oErrorLogs.WriteErrorLog("CampOrganization : ", ex.Message);
                    }
                }

            }
            return new
            {
                Success = false,

            };

        }

    }
}
