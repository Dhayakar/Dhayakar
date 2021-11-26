using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class OneLineMasterRepository : RepositoryBase<OneLineMasterViewModel>, IOneLineMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public OneLineMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic InsertSlampwithvalue(OneLineMasterViewModel OLMName, int userroleID, int CMPID, int modvalue)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new OneLine_Masters();

                    if (OLMName.MastersName == "RoleTable")
                    {
                        var rl = new Role_Master();
                        rl.CreatedUTC = null;
                        rl.UpdatedUTC = null;
                        rl.IsDeleted = false;
                        rl.RoleDescription = OLMName.OneLineMaster.ParentDescription;
                        rl.CreatedBy = userroleID;
                        rl.Formpath = modvalue;
                        CMPSContext.Role.Add(rl);
                        CMPSContext.SaveChanges();
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

        public dynamic UdateSlampwithvalue(OneLineMasterViewModel OLMName, int OLMID, int userroleID, int CMPID, int modvalue)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new OneLine_Masters();
                    var rl = CMPSContext.Role.Where(x => x.RoleID == OLMID).FirstOrDefault();
                    rl.CreatedUTC = DateTime.UtcNow;
                    rl.IsDeleted = false;
                    rl.RoleDescription = OLMName.OneLineMaster.ParentDescription;
                    rl.CreatedBy = userroleID;
                    rl.Formpath = modvalue;
                    CMPSContext.Role.UpdateRange(rl);
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


        public dynamic DeleteSlampwithvalue(OneLineMasterViewModel OneLineMaster, int OLMID, int CMPID, int modvalue)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var rl = CMPSContext.Role.Where(x => x.RoleID == OLMID).FirstOrDefault();
                    rl.IsDeleted = true;
                    CMPSContext.Role.UpdateRange(rl);
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


        public dynamic InsertSlamp(OneLineMasterViewModel OLMName, int userroleID, int CMPID)
        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new OneLine_Masters();

                    var Descriptions = new SpecialityMaster();

                    if (OLMName.MastersName == "Common Complaints")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Common Complaints").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ApptComplaint";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }



                    else if (OLMName.MastersName == "Relation")
                    {
                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Relation").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "REL";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }

                    else if (OLMName.MastersName == "Position of Globe")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PositionOfGlobe").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "POG";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Grade of Thyroid Eye Disease")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "GradeOfThyroidEyeDisease").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "GOTED";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Palpable Mass Location")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassLocation").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PML";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Palpable Mass Shape")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassShape").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PMShape";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Palpable Mass Texture")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassTexture").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PMT";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Palpable Mass Size")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassSize").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PMS";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "RoomType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "RoomType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "RoomType";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "ChartType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ChartType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CT";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Tear meniscus height")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "TMH").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TMH";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }

                    else if (OLMName.MastersName == "Tear Breakup time")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "TBUT").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TBUT";
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }

                    else if (OLMName.MastersName == "Optical Brand")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Opticalbrand").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Opticalbrand";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "FrameType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameType";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "FrameShape")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameShape").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameShape";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "FrameWidth")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameWidth").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameWidth";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "FrameStyle")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameStyle").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameStyle";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "UOM")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "UOM").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "UOM";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }

                    else if (OLMName.MastersName == "Systemic Condition")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Systemic Condition").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "SystemicCondition";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }

                    else if (OLMName.MastersName == "MaritalStatus")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "MaritalStatus").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "MaritalStatus";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Ocular Complaints")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Ocular Complaints").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "OcularComplaints";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();

                    }

                    else if (OLMName.MastersName == "RoomStatus")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "RoomStatus").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "RoomStatus";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Title")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Title").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Title";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Services")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Services").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Services";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "SlitLamp")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "SlitLamp").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "SLIT";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Diagnosis")
                    {
                        if (OLMName.SpecialityMaster.ParentDescription != null)
                        {
                            var Dep = WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.IsDeleted == false
                       && x.ParentTag == "DIA" && x.ParentDescription.Equals(OLMName.SpecialityMaster.ParentDescription))
                           .Select(F => F.ID).FirstOrDefault();
                            if (Dep != 0)
                            {
                                return new
                                {
                                    Success = false,
                                    Message = "Description already exists"
                                };
                            }
                        }
                        if (OLMName.SpecialityMaster.Code != null)
                        {
                            var cod = WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.IsDeleted == false
                           && x.ParentTag == "DIA" && x.Code.Equals(OLMName.SpecialityMaster.Code)).Select(F => F.ID).FirstOrDefault();
                            if (cod != 0)
                            {
                                return new
                                {
                                    Success = false,
                                    Message = "Code already exists"
                                };
                            }
                        }

                        Descriptions.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Descriptions.Code = OLMName.OneLineMaster.Code;
                        Descriptions.ParentID = 0;
                        Descriptions.ParentTag = "DIA";
                        Descriptions.CreatedBy = userroleID;
                        Descriptions.CreatedUTC = DateTime.UtcNow;
                        Descriptions.IsActive = true;
                        Descriptions.IsDeleted = false;
                        WYNKContext.SpecialityMaster.Add(Descriptions);
                        WYNKContext.SaveChanges();


                    }
                    else if (OLMName.MastersName == "ServiceTag")
                    {

                        var STag = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == OLMName.OneLineMaster.ParentDescription && x.IsDeleted == false).Select(c => c.OLMID).FirstOrDefault();
                        if (STag != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Service Tag already exists"
                            };
                        }

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.Code = OLMName.OneLineMaster.Code;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ServiceTag").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "STag";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "ServiceTag")
                    {

                        var STag = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == OLMName.OneLineMaster.ParentDescription && x.IsDeleted == false).Select(c => c.OLMID).FirstOrDefault();
                        if (STag != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Service Tag already exists"
                            };
                        }

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.Code = OLMName.OneLineMaster.Code;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ServiceTag").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "STag";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Investigation Test")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.Amount = OLMName.OneLineMaster.Amount;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Investigation Test").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "INV";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Engagement")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Engagement type").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ET";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Speciality")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Speciality").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ST";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "ICD Group")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.Code = OLMName.OneLineMaster.Code;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ICD GROUP").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ICD GROUP";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "RoleTable")
                    {


                        var rl = new Role_Master();
                        rl.CreatedUTC = null;
                        rl.UpdatedUTC = null;
                        rl.IsDeleted = false;
                        rl.RoleDescription = OLMName.OneLineMaster.ParentDescription;
                        rl.CreatedBy = userroleID;
                        CMPSContext.Role.Add(rl);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Source Of Referral")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Source Of Referral").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "SOR";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Type Of Visits")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Type of Visit").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TOV";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Status")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Status").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "RegStatus";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Payment")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Payment Mode").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PayMode";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Opinion")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "OpinionStatus").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "OS";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Category")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "category").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CAT";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Instrument")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "instrument").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "IN";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "NearVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Nearvision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "NV";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "VA DistanceVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "V/A Distancevision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ADV";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "VA NearVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "V/A Nearvision").Select(x => x.OLMID).FirstOrDefault();

                        Description.ParentTag = "ADNV";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "VisionType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "VisionType").Select(x => x.OLMID).FirstOrDefault();

                        Description.ParentTag = "TY";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Index")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Index").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "IX";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Manufacturer")
                    {


                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Manufacturer").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Brand";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Frequency")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Frequency").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FY";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Drug Instruction")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Drug Instruction").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FD";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Dosage")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Dossage").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Dossage";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "DistanceVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Distance vision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ADV";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "CoverTestDistanceVision")
                    {
                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CoverTestDistanceVision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CTDV";
                        Description.CreatedBy = userroleID;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CreatedUTC = DateTime.UtcNow;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "CoverTestNearVision")
                    {
                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CoverTestNearVision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CTNV";
                        Description.CreatedBy = userroleID;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        Description.CreatedUTC = DateTime.UtcNow;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "DrugForm")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Form").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Form";
                        Description.CreatedBy = userroleID;
                        Description.CreatedUTC = DateTime.UtcNow;
                        Description.IsActive = true;
                        Description.IsDeleted = false;
                        CMPSContext.OneLineMaster.Add(Description);
                        CMPSContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Icd Group Speciality")
                    {
                        var IcdSpeciality = new ICDSpecialityCode();
                        IcdSpeciality.SpecialityDescription = OLMName.OneLineMaster.ParentDescription;
                        IcdSpeciality.CreatedUTC = DateTime.UtcNow;
                        IcdSpeciality.IsActive = true;
                        IcdSpeciality.CreatedBy = userroleID;
                        WYNKContext.ICDSpecialityCode.Add(IcdSpeciality);
                        WYNKContext.SaveChanges();

                    }

                    else if (OLMName.MastersName == "FDDT" || OLMName.MastersName == "Syringing" || OLMName.MastersName == "Regurgitation" || OLMName.MastersName == "Fluid" || OLMName.MastersName == "Stop"
                        || OLMName.MastersName == "Generic Master")
                    {
                        var SpecialityMaster = new SpecialityMaster();
                        SpecialityMaster.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        SpecialityMaster.ParentTag = OLMName.MastersName;
                        SpecialityMaster.CreatedUTC = DateTime.UtcNow;
                        SpecialityMaster.IsActive = true;
                        SpecialityMaster.CreatedBy = userroleID;
                        SpecialityMaster.CmpID = CMPID;
                        WYNKContext.SpecialityMaster.Add(SpecialityMaster);
                        WYNKContext.SaveChanges();
                    }

                    else if (OLMName.MastersName == "Drug Form")
                    {
                        var masters = WYNKContext.SpecialityMaster.Where(x => x.ParentTag.Replace(" ", string.Empty).ToLower().Contains(OLMName.OneLineMaster.ParentTag) && x.CmpID == CMPID && x.IsDeleted == false && x.IsActive == true).ToList();


                        if (masters != null && masters.Count != 0)
                        {

                            masters.All(x => { x.ParentDescription = OLMName.OneLineMaster.ParentDescription; return true; });
                            WYNKContext.SpecialityMaster.UpdateRange(masters);

                        }
                        else
                        {
                            var SpecialityMaster = new SpecialityMaster();
                            SpecialityMaster.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                            SpecialityMaster.ParentTag = OLMName.OneLineMaster.ParentTag;
                            SpecialityMaster.CreatedUTC = DateTime.UtcNow;
                            SpecialityMaster.IsActive = true;
                            SpecialityMaster.CreatedBy = userroleID;
                            SpecialityMaster.CmpID = CMPID;
                            WYNKContext.SpecialityMaster.Add(SpecialityMaster);
                        }
                        WYNKContext.SaveChanges();

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

        public dynamic UdateSlamp(OneLineMasterViewModel OLMName, int OLMID, int userroleID, int CMPID)


        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new OneLine_Masters();
                    Description = CMPSContext.OneLineMaster.Where(x => x.OLMID == OLMID).FirstOrDefault();

                    var Descriptions = new SpecialityMaster();
                    Descriptions = WYNKContext.SpecialityMaster.Where(x => x.ID == OLMID).FirstOrDefault();


                    if (OLMName.MastersName == "Relation")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Relation").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "REL";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    
                    else if (OLMName.MastersName == "Position of Globe")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PositionOfGlobe").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "POG";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "ServiceTag")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ServiceTag").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "STag";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Grade of Thyroid Eye Disease")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "GradeOfThyroidEyeDisease").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "GOTED";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Palpable Mass Location")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassLocation").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PML";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Palpable Mass Shape")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassShape").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PMShape";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Palpable Mass Texture")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassTexture").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PMT";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Palpable Mass Size")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "PalpableMassSize").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PMS";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "RoomType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "RoomType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "RoomType";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "ChartType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ChartType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CT";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "UOM")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "UOM").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "UOM";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "Tear meniscus height")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "TMH").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TMH";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "Tear Breakup time")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "TBUT").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TBUT";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "FrameType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameType";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "FrameShape")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameShape").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameShape";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "FrameWidth")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameWidth").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameWidth";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "FrameStyle")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameStyle").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FrameStyle";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Optical Brand")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Opticalbrand").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Opticalbrand";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "Systemic Condition")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Systemic Condition").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "SystemicCondition";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Optical Brand")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Opticalbrand").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Opticalbrand";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Ocular Complaints")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Ocular Complaints").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "OcularComplaints";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "MaritalStatus")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "MaritalStatus").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "MaritalStatus";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "RoomStatus")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "RoomStatus").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "RoomStatus";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }
                    else if (OLMName.MastersName == "Title")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Title").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Title";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "Services")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Services").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Services";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }


                    else if (OLMName.MastersName == "SlitLamp")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "SlitLamp").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "SLIT";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "Diagnosis")
                    {

                        Descriptions.ParentDescription = OLMName.SpecialityMaster.ParentDescription;
                        Descriptions.ParentID = 0;
                        Descriptions.Code = OLMName.SpecialityMaster.Code;
                        Descriptions.ParentTag = "DIA";
                        Descriptions.UpdatedBy = userroleID;
                        Descriptions.IsActive = OLMName.SpecialityMaster.IsActive;
                        Descriptions.UpdatedUTC = DateTime.UtcNow;
                        WYNKContext.Entry(Descriptions).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Investigation Test")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.Amount = OLMName.OneLineMaster.Amount;
                        Description.ParentID = 1189;
                        Description.ParentTag = "INV";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Engagement")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Engagement type").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ET";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Speciality")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Speciality").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ST";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "ICD Group")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.Code = OLMName.OneLineMaster.Code;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ICD GROUP").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ICD GROUP";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "RoleTable")
                    {

                        var rl = new Role_Master();
                        rl.CreatedUTC = DateTime.UtcNow;
                        rl.IsDeleted = false;
                        rl.RoleDescription = OLMName.OneLineMaster.ParentDescription;
                        rl.CreatedBy = userroleID;
                        CMPSContext.Role.UpdateRange(rl);
                        CMPSContext.SaveChanges();

                    }
                    else if (OLMName.MastersName == "Source Of Referral")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Source Of Referral").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "SOR";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Type Of Visits")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Type of Visit").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TOV";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Status")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Status").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "RegStatus";
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Payment")
                    {
                        Description.IsActive = OLMName.OneLineMaster.IsActive;

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Payment Mode").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "PayMode";
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Opinion")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "OpinionStatus").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "OS";
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Category")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "category").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CAT";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Instrument")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "instrument").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "IN";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "NearVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Nearvision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "NV";
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "VA DistanceVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "V/A Distancevision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ADV";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "VA NearVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "V/A Nearvision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "ADNV";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "VisionType")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "VisionType").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "TY";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }

                    else if (OLMName.MastersName == "Index")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Index").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "IX";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Manufacturer")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Brand").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Brand";
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }

                    else if (OLMName.MastersName == "Frequency")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Frequency").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FY";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Drug Instruction")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Drug Instruction").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "FD";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "Dosage")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Dossage").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "Dossage";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }
                    else if (OLMName.MastersName == "DistanceVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Distance vision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "DV";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;
                    }

                    else if (OLMName.MastersName == "CoverTestDistanceVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CoverTestDistanceVision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CTDV";
                        Description.UpdatedBy = userroleID;
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedUTC = DateTime.UtcNow;


                    }
                    else if (OLMName.MastersName == "CoverTestNearVision")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "CoverTestNearVision").Select(x => x.OLMID).FirstOrDefault();
                        Description.ParentTag = "CTNV";
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;


                    }
                    else if (OLMName.MastersName == "DrugForm")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Form").Select(x => x.OLMID).FirstOrDefault();
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.ParentTag = "Form";
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;


                    }
                    else if (OLMName.MastersName == "Common Complaints")
                    {

                        Description.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        Description.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Common Complaints").Select(x => x.OLMID).FirstOrDefault();
                        Description.IsActive = OLMName.OneLineMaster.IsActive;
                        Description.ParentTag = "ApptComplaint";
                        Description.UpdatedBy = userroleID;
                        Description.UpdatedUTC = DateTime.UtcNow;

                    }

                    else if (OLMName.MastersName == "Icd Group Speciality")
                    {
                        var IcdSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.ID == OLMID).FirstOrDefault();
                        IcdSpeciality.SpecialityDescription = OLMName.OneLineMaster.ParentDescription;
                        IcdSpeciality.UpdatedUTC = DateTime.UtcNow;
                        IcdSpeciality.IsActive = OLMName.OneLineMaster.IsActive;
                        IcdSpeciality.UpdatedBy = userroleID;
                        WYNKContext.ICDSpecialityCode.UpdateRange(IcdSpeciality);
                        WYNKContext.SaveChanges();
                    }


                    else if (OLMName.MastersName == "FDDT" || OLMName.MastersName == "Syringing" || OLMName.MastersName == "Regurgitation" || OLMName.MastersName == "Fluid" || OLMName.MastersName == "Stop" || OLMName.MastersName == "Generic Master")
                    {
                        var SpecialityMaster = WYNKContext.SpecialityMaster.Where(x => x.ID == OLMID).FirstOrDefault();
                        SpecialityMaster.ParentDescription = OLMName.OneLineMaster.ParentDescription;
                        SpecialityMaster.UpdatedUTC = DateTime.UtcNow;
                        SpecialityMaster.IsActive = OLMName.OneLineMaster.IsActive;
                        SpecialityMaster.UpdatedBy = userroleID;
                        WYNKContext.SpecialityMaster.UpdateRange(SpecialityMaster);
                        WYNKContext.SaveChanges();
                    }

                    else if (OLMName.MastersName != "Diagnosis")
                    {
                        CMPSContext.Entry(Description).State = EntityState.Modified;

                        dbContextTransaction.Commit();
                    }
                    if (CMPSContext.SaveChanges() >= 0 || WYNKContext.SaveChanges() >= 0)
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

        public dynamic DeleteSlamp(OneLineMasterViewModel OLMName, int OLMID, int CMPID)


        {
            using (var dbContextTransaction = CMPSContext.Database.BeginTransaction())
            {
                try
                {
                    var Description = new OneLine_Masters();
                    Description = CMPSContext.OneLineMaster.Where(x => x.OLMID == OLMID).FirstOrDefault();

                    var Descriptions = new SpecialityMaster();
                    Descriptions = WYNKContext.SpecialityMaster.Where(x => x.ID == OLMID).FirstOrDefault();

                    if (OLMName.MastersName == "Common Complaints")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;

                    }

                    else
                    if (OLMName.MastersName == "SlitLamp")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "ServiceTag")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else
                    if (OLMName.MastersName == "Position of Globe")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "Grade of Thyroid Eye Disease")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "Palpable Mass Location")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "Palpable Mass Shape")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "Palpable Mass Texture")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "Palpable Mass Size")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else
                    if (OLMName.MastersName == "RoomType")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "ChartType")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "FrameType")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "Tear meniscus height")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "Tear Breakup time")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "FrameShape")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "FrameWidth")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "FrameStyle")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Optical Brand")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "UOM")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }


                    else if (OLMName.MastersName == "MaritalStatus")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }



                    else if (OLMName.MastersName == "UOM")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Systemic Condition")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Ocular Complaints")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "RoomStatus")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Title")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Relation")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Services")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Diagnosis")
                    {
                        Descriptions.IsDeleted = true;
                        Descriptions.IsActive = false;
                        WYNKContext.Entry(Descriptions).State = EntityState.Modified;
                        WYNKContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "Investigation Test")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Engagement")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Speciality")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "ICD Group")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "RoleTable")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Source Of Referral")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Type Of Visits")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Status")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Payment")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Opinion")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Category")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Instrument")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "NearVision")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "VA DistanceVision")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "VA NearVision")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "VisionType")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }

                    else if (OLMName.MastersName == "Index")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Manufacturer")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Frequency")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Drug Instruction")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Dosage")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "DistanceVision")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "CoverTestDistanceVision")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "CoverTestNearVision")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "DrugForm")
                    {
                        Description.IsDeleted = true;
                        Description.IsActive = false;
                    }
                    else if (OLMName.MastersName == "Icd Group Speciality")
                    {
                        var IcdSpeciality = WYNKContext.ICDSpecialityCode.Where(x => x.ID == OLMID).FirstOrDefault();
                        IcdSpeciality.IsActive = false;
                        WYNKContext.ICDSpecialityCode.UpdateRange(IcdSpeciality);
                        WYNKContext.SaveChanges();
                    }
                    else if (OLMName.MastersName == "FDDT" || OLMName.MastersName == "Syringing" || OLMName.MastersName == "Regurgitation" || OLMName.MastersName == "Fluid" || OLMName.MastersName == "Stop" || OLMName.MastersName == "Generic Master")
                    {
                        var SpecialityMaster = WYNKContext.SpecialityMaster.Where(x => x.ID == OLMID && x.CmpID == CMPID).FirstOrDefault();
                        SpecialityMaster.IsActive = false;
                        SpecialityMaster.IsDeleted = true;
                        WYNKContext.SpecialityMaster.UpdateRange(SpecialityMaster);
                        WYNKContext.SaveChanges();
                    }


                    CMPSContext.Entry(Description).State = EntityState.Modified;
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

        public OneLineMasterViewModel GetDetails(string MasterName, int CMPID)

        {
            var OLM = new OneLineMasterViewModel();


            if (MasterName == "Common Complaints")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "ApptComplaint" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }


            else if (MasterName == "SlitLamp")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "SLIT" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "ServiceTag")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "STag" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Position of Globe")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "POG" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Grade of Thyroid Eye Disease")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "GOTED" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Palpable Mass Location")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "PML" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Palpable Mass Shape")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "PMShape" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Palpable Mass Texture")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "PMT" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Palpable Mass Size")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "PMS" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "ChartType")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "CT" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "RoomType")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "RoomType" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Tear meniscus height")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "TMH" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }

            else if (MasterName == "Tear Breakup time")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "TBUT" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }

            else if (MasterName == "Optical Brand")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "Opticalbrand" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "FrameType")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "FrameType" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "FrameShape")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "FrameShape" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "FrameWidth")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "FrameWidth" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "FrameStyle")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "FrameStyle" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "UOM")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "UOM" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                .ToList();
            }
            else if (MasterName == "MaritalStatus")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "MaritalStatus" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Systemic Condition")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "SystemicCondition" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).OrderByDescending(V => V.PDescription).ToList();
            }
            else if (MasterName == "Ocular Complaints")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.OrderBy(x => x.ParentDescription).Where(u => u.ParentTag == "OcularComplaints" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }

            else if (MasterName == "RoomStatus")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "RoomStatus" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Title")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "Title" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }

            else if (MasterName == "Relation")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "REL" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }


            else if (MasterName == "Services")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "Services" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Diagnosis")
            {
                OLM.OLMaster = (from olm in WYNKContext.SpecialityMaster.Where(u => u.ParentTag == "DIA" && u.IsDeleted == false && u.IsActive == true && u.CmpID == CMPID)
                                select new OLMMaster
                                {
                                    POLMID = olm.ID,
                                    PDescription = olm.ParentDescription,
                                    PCode = olm.Code,
                                    pIsActive = olm.IsActive,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Engagement")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "ET" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    pIsActive = olm.IsActive,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Speciality")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "ST" && u.IsDeleted == false && u.ParentID != 0)

                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "ICD Group")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "ICD GROUP" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                    PCode = olm.Code,
                                }).AsNoTracking()
                                 .ToList();
            }


            else if (MasterName == "Investigation Test")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "INV" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                    PAmount = olm.Amount,
                                }).AsNoTracking()
                                 .ToList();
            }


            else if (MasterName == "RoleTable")
            {
                OLM.OLMaster = (from olm in CMPSContext.Role.Where(u => u.IsDeleted == false)
                                select new OLMMaster
                                {
                                    POLMID = olm.RoleID,
                                    pIsActive = olm.IsDeleted,
                                    PDescription = olm.RoleDescription,

                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Source Of Referral")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "SOR" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Type Of Visits")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "TOV" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Status")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "RegStatus" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Payment")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "PayMode" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Opinion")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "OS" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Category")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "CAT" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Instrument")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "IN" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "NearVision")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "NV" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    pIsActive = olm.IsActive,
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "VA DistanceVision")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "ADV" && u.IsDeleted == false && u.ParentID != 0).OrderBy(u => u.Amount)

                                select new OLMMaster
                                {
                                    pIsActive = olm.IsActive,
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }

            else if (MasterName == "VisionType")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "TY" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Index")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "IX" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Manufacturer")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "Brand" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Frequency")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "FY" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    pIsActive = olm.IsActive,
                                    POLMID = olm.OLMID,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Drug Instruction")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "FD" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "Dosage")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "Dossage" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }
            else if (MasterName == "DistanceVision")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "DV" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                                 .ToList();
            }


            else if (MasterName == "CoverTestDistanceVision")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "CTDV" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                 .ToList();
            }
            else if (MasterName == "CoverTestNearVision")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "CTNV" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                 .ToList();
            }
            else if (MasterName == "DrugForm")
            {
                OLM.OLMaster = (from olm in CMPSContext.OneLineMaster.Where(u => u.ParentTag == "Form" && u.IsDeleted == false && u.ParentID != 0)
                                select new OLMMaster
                                {
                                    POLMID = olm.OLMID,
                                    pIsActive = olm.IsActive,
                                    PDescription = olm.ParentDescription,
                                    PID = olm.ParentID,
                                    PTag = olm.ParentTag,
                                }).AsNoTracking()
                 .ToList();
            }

            else if (MasterName == "Icd Group Speciality")
            {
                OLM.OLMaster = (from ol in WYNKContext.ICDSpecialityCode.Where(x => x.IsActive == true)
                                select new OLMMaster
                                {
                                    POLMID = ol.ID,
                                    pIsActive = ol.IsActive,
                                    PDescription = ol.SpecialityDescription,
                                }).AsNoTracking().ToList();

            }


            else if (MasterName == "FDDT" || MasterName == "Syringing" || MasterName == "Regurgitation" || MasterName == "Fluid" || MasterName == "Stop" || MasterName == "Generic Master")
            {
                OLM.OLMaster = (from ol in WYNKContext.SpecialityMaster.Where(x => x.IsActive == true && x.ParentTag == MasterName)
                                select new OLMMaster
                                {
                                    POLMID = ol.ID,
                                    pIsActive = ol.IsActive,
                                    PDescription = ol.ParentDescription,
                                }).AsNoTracking().ToList();

            }


            return OLM;

        }

    }

}