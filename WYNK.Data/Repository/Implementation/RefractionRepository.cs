using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class RefractionRepository : RepositoryBase<RefractionMasterss>, IrefractionRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public RefractionRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetOneeyedDetails(string UIN, int cmpid)
        {

            var OcularPro = (from RE in WYNKContext.RegistrationExtension.Where(x => x.UIN == UIN && x.CMPID == cmpid)


                             select new
                             {
                                 AE = RE.Artificialeye,
                                 OD = RE.OD == true ? "OD" : string.Empty,
                                 OS = RE.OS == true ? "OS" : string.Empty,
                                 OU = RE.OU == true ? "OU" : string.Empty,

                             }).LastOrDefault();


            return OcularPro;

        }
        public dynamic paediatrics(int cmpid)
        {
            var paediatrics = CMPSContext.Setup.Where(x => x.CMPID == cmpid).Select(x => x.Pediatric).FirstOrDefault();

            return paediatrics;
        }
        public dynamic CategoryDetails(string uin)
        {

            var CategoryDetails = (from Co in CMPSContext.OneLineMaster.Where(X => X.ParentTag == "CAT" && X.IsActive == true && X.IsDeleted == false)

                                   select new
                                   {

                                       SubCategory = Co.OLMID,
                                       Description = "Visual Acuity",
                                       Ocular = "OD",
                                       DistSph = "",
                                       NearCyl = "",
                                       PinAxis = "",
                                       PowerGlass = "",
                                       N_V_DESC = "",
                                       OcularOS = "OS",
                                       DistSphOS = "",
                                       NearCylOS = "",
                                       PinAxisOS = "",
                                       PowerGlassOS = "",
                                       N_V_DESCOS = "",
                                       parentDesc = Co.ParentDescription,
                                       CreatedUTc = Co.CreatedUTC,
                                       Remarks = "",
                                       ChartType = "",
                                       OD = false,
                                       OS = false,
                                   }).ToList();





            try
            {

                return new
                {
                    CategoryDetails
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
        public dynamic InstrumentDetails(string uin)
        {

            var InstrumentDetails = (from Co in CMPSContext.OneLineMaster.Where(X => X.ParentTag == "IN" && X.IsActive == true && X.IsDeleted == false)

                                     select new
                                     {

                                         SubCategory = Co.OLMID,
                                         Description = "Refraction",
                                         Ocular = "OD",
                                         Sphdry = "",
                                         Cyldry = "",
                                         Axisdry = "",
                                         Sphwet = "",
                                         Cylwet = "",
                                         Axiswet = "",
                                         TypeDry = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Dry").Select(x => x.OLMID).FirstOrDefault(),
                                         OcularOS = "OS",
                                         Sphdryos = "",
                                         Cyldryos = "",
                                         Axisdryos = "",
                                         Sphwetos = "",
                                         Cylwetos = "",
                                         Axiswetos = "",
                                         TypeWet = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Wet").Select(x => x.OLMID).FirstOrDefault(),
                                         CreatedUTc = Co.CreatedUTC,
                                         OD = false,
                                         OS = false,
                                     }).ToList();





            try
            {

                return new
                {
                    InstrumentDetails
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
        public dynamic Insertrefraction(RefractionMasterss Addrefraction)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {




                    var regtid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                    var reg = Convert.ToInt32(WYNKContext.Refraction.Where(x => x.RegistrationTranID == regtid).Select(x => x.RegistrationTranID).LastOrDefault());


                    if (reg == 0)
                    {
                        if (Addrefraction.Refracion.Count() > 0)
                        {
                            var ar = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.CMPID).FirstOrDefault();
                            string cmpname = CMPSContext.Company.Where(x => x.CmpID == ar).Select(x => x.CompanyName).FirstOrDefault();
                            string username = CMPSContext.DoctorMaster.Where(u => u.EmailID == CMPSContext.Users.Where(x => x.Userid == Addrefraction.Createdby).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                            string userid = Convert.ToString(Addrefraction.Createdby);
                            ErrorLog oErrorLogs = new ErrorLog();
                            oErrorLogs.WriteErrorLogTitle(cmpname, "refraction", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : Add");

                            var one = CMPSContext.OneLineMaster.ToList();

                            var od = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory != 0).ToList();
                            var os = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory != 0).ToList();

                            var paediatricod = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory == 0).ToList();
                            var paediatricos = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory == 0).ToList();

                            if (od.Count() > 0)
                            {
                                foreach (var item in od.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.PowerGlass != "" || item.N_V_DESC != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.PowerGlass = item.PowerGlass;
                                        Ref.N_V_DESC = item.N_V_DESC;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }

                            }

                            if (paediatricod.Count() > 0)
                            {
                                foreach (var item in paediatricod.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && (item.Central != false || item.Steady != false || item.Maintained != false || item.Uncentral != false || item.Unsteady != false || item.Unmaintained != false))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.Central = item.Central;
                                        Ref.Steady = item.Steady;
                                        Ref.Maintained = item.Maintained;
                                        Ref.Uncentral = item.Uncentral;
                                        Ref.Unsteady = item.Unsteady;
                                        Ref.Unmaintained = item.Unmaintained;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (os.Count() > 0)
                            {
                                foreach (var item in os.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.PowerGlassOS != "" || item.N_V_DESCOS != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.PowerGlass = item.PowerGlassOS;
                                        Ref.N_V_DESC = item.N_V_DESCOS;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            if (paediatricos.Count() > 0)
                            {
                                foreach (var item in paediatricos.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && (item.CentralOS != false || item.SteadyOS != false || item.MaintainedOS != false || item.UncentralOS != false || item.UnsteadyOS != false || item.UnmaintainedOS != false))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Central = item.CentralOS;
                                        Ref.Steady = item.SteadyOS;
                                        Ref.Maintained = item.MaintainedOS;
                                        Ref.Uncentral = item.UncentralOS;
                                        Ref.Unsteady = item.UnsteadyOS;
                                        Ref.Unmaintained = item.UnmaintainedOS;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            var PGPod = Addrefraction.Refracion.Where(x => x.Description == "PGP").ToList();
                            var PGPos = Addrefraction.Refracion.Where(x => x.Description == "PGP").ToList();

                            if (PGPod.Count() > 0)
                            {
                                foreach (var item in PGPod.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Details != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Details = item.Details;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (PGPos.Count() > 0)
                            {
                                foreach (var item in PGPos.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Details != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Details = item.Details;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            var Amsod = Addrefraction.Refracion.Where(x => x.Description == "Amsler").ToList();
                            var Amsos = Addrefraction.Refracion.Where(x => x.Description == "Amsler").ToList();

                            if (Amsod.Count() > 0)
                            {
                                foreach (var item in Amsod.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && (item.A_n_OD != false || item.A_n_OD != false || item.Desc_Text != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.A_n_OD = item.A_n_OD;
                                        Ref.A_abn_OD = item.A_abn_OD;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (Amsos.Count() > 0)
                            {
                                foreach (var item in Amsos.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && (item.A_n_OS != false || item.A_abn_OS != false || item.Desc_TextOS != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.A_n_OS = item.A_n_OS;
                                        Ref.A_abn_OS = item.A_abn_OS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            var recod = Addrefraction.Refracion.Where(x => x.Description == "Refraction").ToList();
                            var recos = Addrefraction.Refracion.Where(x => x.Description == "Refraction").ToList();

                            if (recod.Count() > 0)
                            {
                                foreach (var item in recod.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && item.TypeDry == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault() && (item.Sphdry != "" || item.Cyldry != "" || item.Axisdry != ""))
                                    {

                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphdry;
                                        Ref.NearCyl = item.Cyldry;
                                        Ref.PinAxis = item.Axisdry;
                                        Ref.Type = item.TypeDry;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (recod.Count() > 0)
                            {
                                foreach (var item in recod.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && item.TypeWet == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault() && (item.Sphwet != "" || item.Cylwet != "" || item.Axiswet != ""))
                                    {

                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphwet;
                                        Ref.NearCyl = item.Cylwet;
                                        Ref.PinAxis = item.Axiswet;
                                        Ref.Type = item.TypeWet;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (recos.Count() > 0)
                            {
                                foreach (var item in recos.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && item.TypeDry == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault() && (item.Sphdryos != "" || item.Cyldryos != "" || item.Axisdryos != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphdryos;
                                        Ref.NearCyl = item.Cyldryos;
                                        Ref.PinAxis = item.Axisdryos;
                                        Ref.Type = item.TypeDry;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            if (recos.Count() > 0)
                            {
                                foreach (var item in recos.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && item.TypeWet == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault() && (item.Sphwetos != "" || item.Cylwetos != "" || item.Axiswetos != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphwetos;
                                        Ref.NearCyl = item.Cylwetos;
                                        Ref.PinAxis = item.Axiswetos;
                                        Ref.Type = item.TypeWet;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }


                            var cvod = Addrefraction.Refracion.Where(x => x.Description == "Color Vision").ToList();
                            var cvos = Addrefraction.Refracion.Where(x => x.Description == "Color Vision").ToList();

                            if (cvod.Count() > 0)
                            {
                                foreach (var item in cvod.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.Ocular == "OD" && (item.CV_normal != "" || item.CV_defective != "" || item.Desc_Text != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.CV_normal = item.CV_normal;
                                        Ref.CV_defective = item.CV_defective;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (cvos.Count() > 0)
                            {
                                foreach (var item in cvos.ToList())
                                {
                                    var Ref = new RefractionMas();

                                    if (item.OcularOS == "OS" && (item.CV_normalOS != "" || item.CV_defectiveOS != "" || item.Desc_TextOS != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.CV_normal = item.CV_normalOS;
                                        Ref.CV_defective = item.CV_defectiveOS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            var Acod = Addrefraction.Refracion.Where(x => x.Description == "Acceptance").ToList();
                            var Acos = Addrefraction.Refracion.Where(x => x.Description == "Acceptance").ToList();


                            if (Acod.Count() > 0)
                            {
                                foreach (var item in Acod.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Distance Vision";

                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Remarks != ""))
                                    {

                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (Acod.Count() > 0)
                            {
                                foreach (var item in Acod.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Near Vision";
                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOD != "" || item.AddNVOD != ""))
                                    {

                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (Acos.Count() > 0)
                            {
                                foreach (var item in Acos.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Distance Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Remarks != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            if (Acos.Count() > 0)
                            {
                                foreach (var item in Acos.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Near Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOS != "" || item.AddNVOS != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            var finod = Addrefraction.Refracion.Where(x => x.Description == "Final Prescription").ToList();
                            var finos = Addrefraction.Refracion.Where(x => x.Description == "Final Prescription").ToList();


                            if (finod.Count() > 0)
                            {
                                foreach (var item in finod.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Distance Vision";

                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Remarks != ""))
                                    {

                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (finod.Count() > 0)
                            {
                                foreach (var item in finod.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Near Vision";
                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOD != "" || item.AddNVOD != ""))
                                    {

                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                            }

                            if (finos.Count() > 0)
                            {
                                foreach (var item in finos.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Distance Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Remarks != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                            if (finos.Count() > 0)
                            {
                                foreach (var item in finos.ToList())
                                {
                                    var Ref = new RefractionMas();
                                    item.DVName = "Near Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOS != "" || item.AddNVOS != ""))
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }

                        }
                    }
                    else
                    {

                        var ar = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.CMPID).FirstOrDefault();
                        string cmpname = CMPSContext.Company.Where(x => x.CmpID == ar).Select(x => x.CompanyName).FirstOrDefault();
                        string username = CMPSContext.DoctorMaster.Where(u => u.EmailID == CMPSContext.Users.Where(x => x.Userid == Addrefraction.Createdby).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                        string userid = Convert.ToString(Addrefraction.Createdby);
                        ErrorLog oErrorLogs = new ErrorLog();
                        oErrorLogs.WriteErrorLogTitle(cmpname, "refraction", "User name :", username, "User ID :", Convert.ToString(userid), "Mode : update");


                        var one = CMPSContext.OneLineMaster.ToList();
                        var od = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory != 0).ToList();
                        var os = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory != 0).ToList();

                        var paediatricod = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory == 0).ToList();
                        var paediatricos = Addrefraction.Refracion.Where(x => x.Description == "Visual Acuity" && x.SubCategory == 0).ToList();

                        if (od.Count() > 0)
                        {
                            foreach (var item in od.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.ID != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.ID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.PowerGlass = item.PowerGlass;
                                        Ref.N_V_DESC = item.N_V_DESC;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {

                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.PowerGlass = item.PowerGlass;
                                        Ref.N_V_DESC = item.N_V_DESC;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (paediatricod.Count() > 0)
                        {
                            foreach (var item in paediatricod.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.ID != 0)
                                {

                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.ID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.Central = item.Central;
                                        Ref.Steady = item.Steady;
                                        Ref.Maintained = item.Maintained;
                                        Ref.Uncentral = item.Uncentral;
                                        Ref.Unsteady = item.Unsteady;
                                        Ref.Unmaintained = item.Unmaintained;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }

                                else
                                {

                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.Central = item.Central;
                                        Ref.Steady = item.Steady;
                                        Ref.Maintained = item.Maintained;
                                        Ref.Uncentral = item.Uncentral;
                                        Ref.Unsteady = item.Unsteady;
                                        Ref.Unmaintained = item.Unmaintained;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }

                            }
                        }

                        if (os.Count() > 0)
                        {
                            foreach (var item in os.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.IDOS != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.IDOS).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.PowerGlass = item.PowerGlassOS;
                                        Ref.N_V_DESC = item.N_V_DESCOS;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }


                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.PowerGlass = item.PowerGlassOS;
                                        Ref.N_V_DESC = item.N_V_DESCOS;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }
                            }
                        }

                        if (paediatricos.Count() > 0)
                        {
                            foreach (var item in paediatricos.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.IDOS != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.IDOS).FirstOrDefault();
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Central = item.CentralOS;
                                        Ref.Steady = item.SteadyOS;
                                        Ref.Maintained = item.MaintainedOS;
                                        Ref.Uncentral = item.UncentralOS;
                                        Ref.Unsteady = item.UnsteadyOS;
                                        Ref.Unmaintained = item.UnmaintainedOS;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }

                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Central = item.CentralOS;
                                        Ref.Steady = item.SteadyOS;
                                        Ref.Maintained = item.MaintainedOS;
                                        Ref.Uncentral = item.UncentralOS;
                                        Ref.Unsteady = item.UnsteadyOS;
                                        Ref.Unmaintained = item.UnmaintainedOS;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }
                            }
                        }

                        var PGPod = Addrefraction.Refracion.Where(x => x.Description == "PGP").ToList();
                        var PGPos = Addrefraction.Refracion.Where(x => x.Description == "PGP").ToList();

                        if (PGPod.Count() > 0)
                        {
                            foreach (var item in PGPod.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.ID != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.ID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Details = item.Details;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Details = item.Details;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (PGPos.Count() > 0)
                        {
                            foreach (var item in PGPos.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.IDOS != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.IDOS).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Details = item.Details;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }


                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Details = item.Details;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }
                            }
                        }

                        var Amsod = Addrefraction.Refracion.Where(x => x.Description == "Amsler").ToList();
                        var Amsos = Addrefraction.Refracion.Where(x => x.Description == "Amsler").ToList();

                        if (Amsod.Count() > 0)
                        {
                            foreach (var item in Amsod.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.ID != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.ID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.A_n_OD = item.A_n_OD;
                                        Ref.A_abn_OD = item.A_abn_OD;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.A_n_OD = item.A_n_OD;
                                        Ref.A_abn_OD = item.A_abn_OD;
                                        Ref.Ocular = item.Ocular;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (Amsos.Count() > 0)
                        {
                            foreach (var item in Amsos.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.IDOS != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.IDOS).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.A_n_OS = item.A_n_OS;
                                        Ref.A_abn_OS = item.A_abn_OS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }


                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.A_n_OS = item.A_n_OS;
                                        Ref.A_abn_OS = item.A_abn_OS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }
                            }
                        }

                        var recod = Addrefraction.Refracion.Where(x => x.Description == "Refraction").ToList();
                        var recos = Addrefraction.Refracion.Where(x => x.Description == "Refraction").ToList();

                        if (recod.Count() > 0)
                        {
                            foreach (var item in recod.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Dry";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OD" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Refraction" && z.SubCategory == item.SubCategory).Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OD" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphdry;
                                        Ref.NearCyl = item.Cyldry;
                                        Ref.PinAxis = item.Axisdry;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphdry;
                                        Ref.NearCyl = item.Cyldry;
                                        Ref.PinAxis = item.Axisdry;
                                        Ref.Type = item.TypeDry;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (recod.Count() > 0)
                        {
                            foreach (var item in recod.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Wet";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OD" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Refraction" && z.SubCategory == item.SubCategory).Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OD" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphwet;
                                        Ref.NearCyl = item.Cylwet;
                                        Ref.PinAxis = item.Axiswet;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphwet;
                                        Ref.NearCyl = item.Cylwet;
                                        Ref.PinAxis = item.Axiswet;
                                        Ref.Type = item.TypeWet;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (recos.Count() > 0)
                        {
                            foreach (var item in recos.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Dry";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OS" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Refraction" && z.SubCategory == item.SubCategory).Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OS" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphdryos;
                                        Ref.NearCyl = item.Cyldryos;
                                        Ref.PinAxis = item.Axisdryos;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphdryos;
                                        Ref.NearCyl = item.Cyldryos;
                                        Ref.PinAxis = item.Axisdryos;
                                        Ref.Type = item.TypeDry;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (recos.Count() > 0)
                        {
                            foreach (var item in recos.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Wet";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OS" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Refraction" && z.SubCategory == item.SubCategory).Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OS" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphwetos;
                                        Ref.NearCyl = item.Cylwetos;
                                        Ref.PinAxis = item.Axiswetos;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphwetos;
                                        Ref.NearCyl = item.Cylwetos;
                                        Ref.PinAxis = item.Axiswetos;
                                        Ref.Type = item.TypeWet;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        var cvod = Addrefraction.Refracion.Where(x => x.Description == "Color Vision").ToList();
                        var cvos = Addrefraction.Refracion.Where(x => x.Description == "Color Vision").ToList();

                        if (cvod.Count() > 0)
                        {
                            foreach (var item in cvod.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.ID != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.ID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.CV_normal = item.CV_normal;
                                        Ref.CV_defective = item.CV_defective;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Ocular = item.Ocular;
                                        Ref.CV_normal = item.CV_normal;
                                        Ref.CV_defective = item.CV_defective;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }

                            }
                        }

                        if (cvos.Count() > 0)
                        {
                            foreach (var item in cvos.ToList())
                            {
                                var Ref = new RefractionMas();

                                if (item.IDOS != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == item.IDOS).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.CV_normal = item.CV_normalOS;
                                        Ref.CV_defective = item.CV_defectiveOS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }
                                }


                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.CV_normal = item.CV_normalOS;
                                        Ref.CV_defective = item.CV_defectiveOS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }
                            }
                        }


                        var Acod = Addrefraction.Refracion.Where(x => x.Description == "Acceptance").ToList();
                        var Acos = Addrefraction.Refracion.Where(x => x.Description == "Acceptance").ToList();

                        if (Acod.Count() > 0)
                        {
                            foreach (var item in Acod.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Distance Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OD" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Acceptance").Select(x => x.ID).FirstOrDefault();
                                if (res != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OD" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (Acod.Count() > 0)
                        {
                            foreach (var item in Acod.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Near Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OD" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Acceptance").Select(x => x.ID).FirstOrDefault();
                                if (res != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OD" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.Ocular = item.Ocular;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (Acos.Count() > 0)
                        {
                            foreach (var item in Acos.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Distance Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OS" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Acceptance").Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OS" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (Acos.Count() > 0)
                        {
                            foreach (var item in Acos.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Near Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OS" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Acceptance").Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OS" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }

                            }
                        }


                        var finod = Addrefraction.Refracion.Where(x => x.Description == "Final Prescription").ToList();
                        var finos = Addrefraction.Refracion.Where(x => x.Description == "Final Prescription").ToList();


                        if (finod.Count() > 0)
                        {
                            foreach (var item in finod.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Distance Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OD" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Final Prescription").Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OD" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (finod.Count() > 0)
                        {
                            foreach (var item in finod.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Near Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OD" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Final Prescription").Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OD" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.Ocular == "OD")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.Ocular = item.Ocular;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }

                        if (finos.Count() > 0)
                        {
                            foreach (var item in finos.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Distance Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OS" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Final Prescription").Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {

                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OS" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);

                                    }

                                }

                            }
                        }

                        if (finos.Count() > 0)
                        {
                            foreach (var item in finos.ToList())
                            {
                                var Ref = new RefractionMas();
                                item.DVName = "Near Vision";
                                int? OLMID = one.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(x => (int?)x.OLMID).FirstOrDefault();
                                int res = WYNKContext.Refraction.Where(z => z.Ocular == "OS" && z.RegistrationTranID == item.RegistrationTranID && z.Type == OLMID && z.Description == "Final Prescription").Select(x => x.ID).FirstOrDefault();

                                if (res != 0)
                                {
                                    if (item.OcularOS == "OS")
                                    {


                                        Ref = WYNKContext.Refraction.Where(x => x.ID == res && x.Ocular == "OS" && x.Type == OLMID).FirstOrDefault();

                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.UpdatedUTC = DateTime.UtcNow;
                                        Ref.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.Refraction.UpdateRange(Ref);
                                        ErrorLog oErrorLogstrans = new ErrorLog();
                                        object namestrs = Ref;
                                        oErrorLogstrans.WriteErrorLogArray("refraction", namestrs);


                                    }

                                }
                                else
                                {
                                    if (item.OcularOS == "OS")
                                    {
                                        var uinid = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.CMPID == Addrefraction.cmpid).Select(x => x.UIN).FirstOrDefault();
                                        Ref.UIN = uinid;
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Refraction.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("refraction", namestr);


                                    }

                                }

                            }
                        }


                    }

                    WYNKContext.SaveChanges();

                    var uin = WYNKContext.Refraction.Where(x => x.UIN == Addrefraction.UIN).Select(x => x.UIN).FirstOrDefault();
                    var pdate = WYNKContext.Refraction.Where(x => x.UIN == Addrefraction.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                    var pdatee = WYNKContext.Refraction.Where(x => x.RegistrationTranID == pdate).Select(x => x.CreatedUTC).LastOrDefault();
                    var refid = WYNKContext.Refraction.Where(x => x.UIN == Addrefraction.UIN).Select(x => x.RegistrationTranID).LastOrDefault();
                    var masteroptical = WYNKContext.Registration.Where(x => x.UIN == Addrefraction.UIN && x.IsDeleted == false).ToList();


                    if (Addrefraction.FINALPRESCRIPTION != null)
                    {
                        if (Addrefraction.FINALPRESCRIPTION.Count() > 0)
                        {
                            var ids = WYNKContext.OpticalPrescription.Where(x => x.RegistrationTranID == refid).ToList();
                            foreach (var item in ids.ToList())
                            {
                                if (ids != null && ids.Count != 0)
                                {
                                    ids.All(x => { x.RegistrationTranID = item.RegistrationTranID; return true; });
                                    var idsss = WYNKContext.OpticalPrescription.Where(x => x.RegistrationTranID == refid).ToList();
                                    if (idsss.Count != 0)
                                    {
                                        WYNKContext.OpticalPrescription.RemoveRange(ids);
                                        WYNKContext.SaveChanges();
                                    }
                                }
                            }

                            var ids1 = WYNKContext.OpticalPrescription.Where(x => x.RegistrationTranID == refid).ToList();

                            if (ids1.Count == 0)
                            {
                                var finod = Addrefraction.FINALPRESCRIPTION.Where(x => x.Description == "Final Prescription").ToList();
                                var finos = Addrefraction.FINALPRESCRIPTION.Where(x => x.Description == "Final Prescription").ToList();
                                var one = CMPSContext.OneLineMaster.ToList();

                                var opm = WYNKContext.OpticalPrescriptionmaster.Where(x => x.RegistrationID == refid).ToList();

                                if (opm.Count == 0)
                                {
                                    if (masteroptical.Count() > 0)
                                    {
                                        foreach (var item in masteroptical.ToList())
                                        {
                                            var Ref = new OpticalPrescriptionmaster();

                                            Ref.UIN = item.UIN;
                                            Ref.RegistrationID = refid;
                                            Ref.Name = item.Name;
                                            Ref.MiddleName = item.MiddleName;
                                            Ref.LastName = item.LastName;
                                            Ref.DateofBirth = item.DateofBirth;
                                            Ref.Gender = item.Gender;
                                            Ref.Phone = item.Phone;
                                            Ref.PrescriptionDate = pdatee;
                                            Ref.CMPID = Addrefraction.cmpid;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = Addrefraction.Createdby;
                                            WYNKContext.OpticalPrescriptionmaster.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }

                                    }
                                }


                                if (finod.Count() > 0)
                                {
                                    foreach (var item in finod.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Distance Vision";

                                        if (item.Ocular == "OD" && st == one.Where(z => z.ParentDescription == "Distance Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Remarks != ""))
                                        {
                                            Ref.RegistrationTranID = refid;
                                            Ref.UIN = uin;
                                            Ref.PrescriptionDate = pdatee;
                                            Ref.Ocular = item.Ocular;
                                            Ref.DistSph = item.DistSph;
                                            Ref.NearCyl = item.NearCyl;
                                            Ref.PinAxis = item.PinAxis;
                                            Ref.Add = item.Add;
                                            Ref.Remarks = item.Remarks;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.CMPID = Addrefraction.cmpid;
                                            Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription == "Distance Vision" && item.DVName == "Distance Vision").Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = Addrefraction.Createdby;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }
                                    }

                                }

                                if (finod.Count() > 0)
                                {
                                    foreach (var item in finod.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Near Vision";
                                        if (item.Ocular == "OD" && st == one.Where(z => z.ParentDescription == "Near Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOD != "" || item.AddNVOD != ""))
                                        {

                                            Ref.RegistrationTranID = refid;
                                            Ref.UIN = uin;
                                            Ref.PrescriptionDate = pdatee;
                                            Ref.Ocular = item.Ocular;
                                            Ref.DistSph = item.DistSphNVOD;
                                            Ref.Add = item.AddNVOD;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.Remarks = item.Remarks;
                                            Ref.CMPID = Addrefraction.cmpid;
                                            Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription == "Near Vision" && item.DVName == "Near Vision").Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = Addrefraction.Createdby;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }
                                    }

                                }

                                if (finos.Count() > 0)
                                {
                                    foreach (var item in finos.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Distance Vision";

                                        if (item.OcularOS == "OS" && st == one.Where(z => z.ParentDescription == "Distance Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Remarks != ""))
                                        {

                                            Ref.RegistrationTranID = refid;
                                            Ref.UIN = uin;
                                            Ref.PrescriptionDate = pdatee;
                                            Ref.Ocular = item.OcularOS;
                                            Ref.DistSph = item.DistSphOS;
                                            Ref.NearCyl = item.NearCylOS;
                                            Ref.PinAxis = item.PinAxisOS;
                                            Ref.Add = item.AddOS;
                                            Ref.Remarks = item.Remarks;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.CMPID = Addrefraction.cmpid;
                                            Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription == "Distance Vision" && item.DVName == "Distance Vision").Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = Addrefraction.Createdby;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }

                                    }
                                }

                                if (finos.Count() > 0)
                                {
                                    foreach (var item in finos.ToList())
                                    {
                                        var Ref = new OpticalPrescriptionn();
                                        var st = item.DVName = "Near Vision";

                                        if (item.OcularOS == "OS" && st == one.Where(z => z.ParentDescription == "Near Vision").Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOS != "" || item.AddNVOS != ""))
                                        {

                                            Ref.RegistrationTranID = refid;
                                            Ref.UIN = uin;
                                            Ref.PrescriptionDate = pdatee;
                                            Ref.Ocular = item.OcularOS;
                                            Ref.DistSph = item.DistSphNVOS;
                                            Ref.Add = item.AddNVOS;
                                            Ref.PD = item.PD;
                                            Ref.MPDOD = item.MPDOD;
                                            Ref.MPDOS = item.MPDOS;
                                            Ref.Remarks = item.Remarks;
                                            Ref.CMPID = Addrefraction.cmpid;
                                            Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription == "Near Vision" && item.DVName == "Near Vision").Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                            Ref.CreatedUTC = DateTime.UtcNow;
                                            Ref.CreatedBy = Addrefraction.Createdby;
                                            WYNKContext.OpticalPrescription.AddRange(Ref);
                                            ErrorLog oErrorLogstran = new ErrorLog();
                                            object namestr = Ref;
                                            oErrorLogstran.WriteErrorLogArray("OpticalPrescription", namestr);

                                        }

                                    }
                                }


                            }
                        }
                    }

                    if (Addrefraction.VISUALACUITY != null)
                    {
                        if (Addrefraction.VISUALACUITY.Count() > 0)
                        {

                            var od = Addrefraction.VISUALACUITY.Where(x => x.Ocular == "OD" && x.Description == "Visual Acuity").ToList();
                            var os = Addrefraction.VISUALACUITY.Where(x => x.OcularOS == "OS" && x.Description == "Visual Acuity").ToList();

                            if (od.Count() > 0)
                            {
                                foreach (var item in od.ToList())
                                {
                                    var Ref = new VISUALACUITY();

                                    if (item.Ocular == "OD" && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.PowerGlass != "" || item.N_V_DESC != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.PowerGlass = item.PowerGlass;
                                        Ref.N_V_DESC = item.N_V_DESC;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        Ref.RefTag = Addrefraction.Tag;
                                        WYNKContext.VISUALACUITY.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("VISUALACUITY", namestr);

                                    }

                                }
                            }

                            if (os.Count() > 0)
                            {
                                foreach (var item in os.ToList())
                                {
                                    var Ref = new VISUALACUITY();

                                    if (item.OcularOS == "OS" && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.PowerGlassOS != "" || item.N_V_DESCOS != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.PowerGlass = item.PowerGlassOS;
                                        Ref.N_V_DESC = item.N_V_DESCOS;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        Ref.RefTag = Addrefraction.Tag;
                                        WYNKContext.VISUALACUITY.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("VISUALACUITY", namestr);

                                    }



                                }
                            }

                        }
                    }

                    if (Addrefraction.paediatricvisualacuity != null)
                    {
                        if (Addrefraction.paediatricvisualacuity.Count() > 0)
                        {

                            var paediatricod = Addrefraction.paediatricvisualacuity.Where(x => x.Description == "Visual Acuity" && x.Ocular == "OD").ToList();
                            var paediatricos = Addrefraction.paediatricvisualacuity.Where(x => x.Description == "Visual Acuity" && x.OcularOS == "OS").ToList();

                            if (paediatricod.Count() > 0)
                            {
                                foreach (var item in paediatricod.ToList())
                                {
                                    var Ref = new VISUALACUITY();

                                    if (item.Ocular == "OD" && (item.Central != false || item.Steady != false || item.Maintained != false || item.Uncentral != false || item.Unsteady != false || item.Unmaintained != false))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.Central = item.Central;
                                        Ref.Steady = item.Steady;
                                        Ref.Maintained = item.Maintained;
                                        Ref.Uncentral = item.Uncentral;
                                        Ref.Unsteady = item.Unsteady;
                                        Ref.Unmaintained = item.Unmaintained;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        Ref.RefTag = Addrefraction.Tag;
                                        WYNKContext.VISUALACUITY.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("VISUALACUITY", namestr);

                                    }
                                }
                            }

                            if (paediatricos.Count() > 0)
                            {
                                foreach (var item in paediatricos.ToList())
                                {
                                    var Ref = new VISUALACUITY();


                                    if (item.OcularOS == "OS" && (item.CentralOS != false || item.SteadyOS != false || item.MaintainedOS != false || item.UncentralOS != false || item.UnsteadyOS != false || item.UnmaintainedOS != false))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.Central = item.CentralOS;
                                        Ref.Steady = item.SteadyOS;
                                        Ref.Maintained = item.MaintainedOS;
                                        Ref.Uncentral = item.UncentralOS;
                                        Ref.Unsteady = item.UnsteadyOS;
                                        Ref.Unmaintained = item.UnmaintainedOS;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.ChartType = item.ChartType;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        Ref.RefTag = Addrefraction.Tag;
                                        WYNKContext.VISUALACUITY.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("VISUALACUITY", namestr);

                                    }

                                }
                            }
                        }
                    }

                    if (Addrefraction.PGP != null)
                    {
                        if (Addrefraction.PGP.Count() > 0)
                        {
                            var PGPod = Addrefraction.PGP.Where(x => x.Ocular == "OD" && x.Description == "PGP").ToList();
                            var PGPos = Addrefraction.PGP.Where(x => x.OcularOS == "OS" && x.Description == "PGP").ToList();


                            if (PGPod.Count() > 0)
                            {
                                foreach (var item in PGPod.ToList())
                                {
                                    var PG = new PGP();

                                    if (item.Ocular == "OD" && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Details != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        PG.RegistrationTranID = regid;
                                        PG.Description = item.Description;
                                        PG.Ocular = item.Ocular;
                                        PG.DistSph = item.DistSph;
                                        PG.NearCyl = item.NearCyl;
                                        PG.PinAxis = item.PinAxis;
                                        PG.Add = item.Add;
                                        PG.Details = item.Details;
                                        PG.CreatedUTC = DateTime.UtcNow;
                                        PG.Date = DateTime.UtcNow;
                                        PG.CreatedBy = Addrefraction.Createdby;
                                        PG.RefTag = Addrefraction.Tag;
                                        WYNKContext.PGP.AddRange(PG);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = PG;
                                        oErrorLogstran.WriteErrorLogArray("PGP", namestr);

                                    }

                                }
                            }

                            if (PGPos.Count() > 0)
                            {
                                foreach (var item in PGPos.ToList())
                                {
                                    var PG = new PGP();

                                    if (item.OcularOS == "OS" && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Details != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        PG.RegistrationTranID = regid;
                                        PG.Description = item.Description;
                                        PG.Ocular = item.OcularOS;
                                        PG.DistSph = item.DistSphOS;
                                        PG.NearCyl = item.NearCylOS;
                                        PG.PinAxis = item.PinAxisOS;
                                        PG.Add = item.AddOS;
                                        PG.Details = item.Details;
                                        PG.Date = DateTime.UtcNow;
                                        PG.CreatedUTC = DateTime.UtcNow;
                                        PG.CreatedBy = Addrefraction.Createdby;
                                        PG.RefTag = Addrefraction.Tag;
                                        WYNKContext.PGP.AddRange(PG);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = PG;
                                        oErrorLogstran.WriteErrorLogArray("PGP", namestr);

                                    }


                                }
                            }

                        }
                    }

                    if (Addrefraction.REFRACTIONEXT != null)
                    {
                        if (Addrefraction.REFRACTIONEXT.Count() > 0)
                        {
                            var recod = Addrefraction.REFRACTIONEXT.Where(x => x.Description == "Refraction").ToList();
                            var recos = Addrefraction.REFRACTIONEXT.Where(x => x.Description == "Refraction").ToList();
                            var one = CMPSContext.OneLineMaster.ToList();


                            if (recod.Count() > 0)
                            {
                                foreach (var item in recod.ToList())
                                {
                                    var Ref = new REFRACTIONEXT();

                                    if (item.Ocular == "OD" && item.TypeDry == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault() && (item.Sphdry != "" || item.Cyldry != "" || item.Axisdry != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphdry;
                                        Ref.NearCyl = item.Cyldry;
                                        Ref.PinAxis = item.Axisdry;
                                        Ref.Type = item.TypeDry;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.RefTag = Addrefraction.Tag;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.REFRACTIONEXT.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("REFRACTIONEXT", namestr);

                                    }
                                }

                            }

                            if (recod.Count() > 0)
                            {
                                foreach (var item in recod.ToList())
                                {
                                    var Ref = new REFRACTIONEXT();

                                    if (item.Ocular == "OD" && item.TypeWet == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault() && (item.Sphwet != "" || item.Cylwet != "" || item.Axiswet != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.Sphwet;
                                        Ref.NearCyl = item.Cylwet;
                                        Ref.PinAxis = item.Axiswet;
                                        Ref.Type = item.TypeWet;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.RefTag = Addrefraction.Tag;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.REFRACTIONEXT.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("REFRACTIONEXT", namestr);

                                    }
                                }

                            }

                            if (recos.Count() > 0)
                            {
                                foreach (var item in recos.ToList())
                                {
                                    var Ref = new REFRACTIONEXT();

                                    if (item.OcularOS == "OS" && item.TypeDry == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault() && (item.Sphdryos != "" || item.Cyldryos != "" || item.Axisdryos != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphdryos;
                                        Ref.NearCyl = item.Cyldryos;
                                        Ref.PinAxis = item.Axisdryos;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.Type = item.TypeDry;
                                        Ref.RefTag = Addrefraction.Tag;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.REFRACTIONEXT.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("REFRACTIONEXT", namestr);

                                    }

                                }
                            }

                            if (recos.Count() > 0)
                            {
                                foreach (var item in recos.ToList())
                                {
                                    var Ref = new REFRACTIONEXT();

                                    if (item.OcularOS == "OS" && item.TypeWet == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault() && (item.Sphwetos != "" || item.Cylwetos != "" || item.Axiswetos != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.SubCategory = item.SubCategory;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.Sphwetos;
                                        Ref.NearCyl = item.Cylwetos;
                                        Ref.PinAxis = item.Axiswetos;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.Type = item.TypeWet;
                                        Ref.RefTag = Addrefraction.Tag;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.REFRACTIONEXT.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("REFRACTIONEXT", namestr);

                                    }

                                }
                            }


                        }
                    }

                    if (Addrefraction.ACCEPTANCE != null)
                    {
                        if (Addrefraction.ACCEPTANCE.Count() > 0)
                        {
                            var Acod = Addrefraction.ACCEPTANCE.Where(x => x.Description == "Acceptance").ToList();
                            var Acos = Addrefraction.ACCEPTANCE.Where(x => x.Description == "Acceptance").ToList();
                            var one = CMPSContext.OneLineMaster.ToList();

                            if (Acod.Count() > 0)
                            {
                                foreach (var item in Acod.ToList())
                                {
                                    var Ref = new ACCEPTANCE();
                                    item.DVName = "Distance Vision";

                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Remarks != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.ACCEPTANCE.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("ACCEPTANCE", namestr);

                                    }
                                }

                            }

                            if (Acod.Count() > 0)
                            {
                                foreach (var item in Acod.ToList())
                                {
                                    var Ref = new ACCEPTANCE();
                                    item.DVName = "Near Vision";
                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOD != "" || item.AddNVOD != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.ACCEPTANCE.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("ACCEPTANCE", namestr);

                                    }
                                }

                            }

                            if (Acos.Count() > 0)
                            {
                                foreach (var item in Acos.ToList())
                                {
                                    var Ref = new ACCEPTANCE();
                                    item.DVName = "Distance Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Remarks != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.ACCEPTANCE.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("ACCEPTANCE", namestr);

                                    }

                                }
                            }

                            if (Acos.Count() > 0)
                            {
                                foreach (var item in Acos.ToList())
                                {
                                    var Ref = new ACCEPTANCE();
                                    item.DVName = "Near Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOS != "" || item.AddNVOS != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.ACCEPTANCE.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("ACCEPTANCE", namestr);

                                    }

                                }
                            }


                        }
                    }

                    if (Addrefraction.FINALPRESCRIPTION != null)
                    {
                        if (Addrefraction.FINALPRESCRIPTION.Count() > 0)
                        {
                            var finod = Addrefraction.FINALPRESCRIPTION.Where(x => x.Description == "Final Prescription").ToList();
                            var finos = Addrefraction.FINALPRESCRIPTION.Where(x => x.Description == "Final Prescription").ToList();
                            var one = CMPSContext.OneLineMaster.ToList();

                            if (finod.Count() > 0)
                            {
                                foreach (var item in finod.ToList())
                                {
                                    var Ref = new FINALPRESCRIPTION();
                                    item.DVName = "Distance Vision";

                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSph != "" || item.NearCyl != "" || item.PinAxis != "" || item.Add != "" || item.Remarks != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSph;
                                        Ref.NearCyl = item.NearCyl;
                                        Ref.PinAxis = item.PinAxis;
                                        Ref.Add = item.Add;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.FINALPRESCRIPTION.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("FINALPRESCRIPTION", namestr);

                                    }
                                }

                            }

                            if (finod.Count() > 0)
                            {
                                foreach (var item in finod.ToList())
                                {
                                    var Ref = new FINALPRESCRIPTION();
                                    item.DVName = "Near Vision";
                                    if (item.Ocular == "OD" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOD != "" || item.AddNVOD != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.DistSph = item.DistSphNVOD;
                                        Ref.Add = item.AddNVOD;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.FINALPRESCRIPTION.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("FINALPRESCRIPTION", namestr);

                                    }
                                }

                            }

                            if (finos.Count() > 0)
                            {
                                foreach (var item in finos.ToList())
                                {
                                    var Ref = new FINALPRESCRIPTION();
                                    item.DVName = "Distance Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphOS != "" || item.NearCylOS != "" || item.PinAxisOS != "" || item.AddOS != "" || item.Remarks != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphOS;
                                        Ref.NearCyl = item.NearCylOS;
                                        Ref.PinAxis = item.PinAxisOS;
                                        Ref.Add = item.AddOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.Type = item.DV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.DV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.FINALPRESCRIPTION.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("FINALPRESCRIPTION", namestr);

                                    }

                                }
                            }

                            if (finos.Count() > 0)
                            {
                                foreach (var item in finos.ToList())
                                {
                                    var Ref = new FINALPRESCRIPTION();
                                    item.DVName = "Near Vision";

                                    if (item.OcularOS == "OS" && item.DVName == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.ParentDescription).FirstOrDefault() && (item.DistSphNVOS != "" || item.AddNVOS != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.DistSph = item.DistSphNVOS;
                                        Ref.Add = item.AddNVOS;
                                        Ref.PD = item.PD;
                                        Ref.MPDOD = item.MPDOD;
                                        Ref.MPDOS = item.MPDOS;
                                        Ref.Remarks = item.Remarks;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.Type = item.NV == 0 ? one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals(item.DVName.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault() : item.NV;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.FINALPRESCRIPTION.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("FINALPRESCRIPTION", namestr);

                                    }

                                }
                            }


                        }
                    }

                    if (Addrefraction.Amsler != null)
                    {
                        if (Addrefraction.Amsler.Count() > 0)
                        {

                            var Amsod = Addrefraction.Amsler.Where(x => x.Ocular == "OD" && x.Description == "Amsler").ToList();
                            var Amsos = Addrefraction.Amsler.Where(x => x.OcularOS == "OS" && x.Description == "Amsler").ToList();


                            if (Amsod.Count() > 0)
                            {
                                foreach (var item in Amsod.ToList())
                                {
                                    var Ams = new Amsler();

                                    if (item.Ocular == "OD" && (item.A_n_OD != false || item.A_n_OD != false || item.Desc_Text != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ams.RegistrationTranID = regid;
                                        Ams.Description = item.Description;
                                        Ams.Ocular = item.Ocular;
                                        Ams.A_n_OD = item.A_n_OD;
                                        Ams.A_abn_OD = item.A_abn_OD;
                                        Ams.Desc_Text = item.Desc_Text;
                                        Ams.Date = DateTime.Now;
                                        Ams.CreatedUTC = DateTime.UtcNow;
                                        Ams.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Amsler.AddRange(Ams);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ams;
                                        oErrorLogstran.WriteErrorLogArray("Amsler", namestr);
                                    }

                                }
                            }

                            if (Amsos.Count() > 0)
                            {
                                foreach (var item in Amsos.ToList())
                                {
                                    var Ams = new Amsler();

                                    if (item.OcularOS == "OS" && (item.A_n_OS != false || item.A_abn_OS != false || item.Desc_TextOS != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ams.RegistrationTranID = regid;
                                        Ams.Description = item.Description;
                                        Ams.Ocular = item.Ocular;
                                        Ams.A_n_OS = item.A_n_OS;
                                        Ams.A_abn_OS = item.A_abn_OS;
                                        Ams.Desc_Text = item.Desc_TextOS;
                                        Ams.Date = DateTime.Now;
                                        Ams.CreatedUTC = DateTime.UtcNow;
                                        Ams.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.Amsler.AddRange(Ams);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ams;
                                        oErrorLogstran.WriteErrorLogArray("Amsler", namestr);
                                    }


                                }
                            }



                        }
                    }

                    if (Addrefraction.ColorVision != null)
                    {
                        if (Addrefraction.ColorVision.Count() > 0)
                        {

                            var cvod = Addrefraction.ColorVision.Where(x => x.Description == "Color Vision").ToList();
                            var cvos = Addrefraction.ColorVision.Where(x => x.Description == "Color Vision").ToList();

                            if (cvod.Count() > 0)
                            {
                                foreach (var item in cvod.ToList())
                                {
                                    var Ref = new ColorVision();

                                    if (item.Ocular == "OD" && (item.CV_normal != "" || item.CV_defective != "" || item.Desc_Text != ""))
                                    {

                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.Ocular;
                                        Ref.CV_normal = item.CV_normal;
                                        Ref.CV_defective = item.CV_defective;
                                        Ref.Desc_Text = item.Desc_Text;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.ColorVision.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("ColorVision", namestr);

                                    }

                                }
                            }

                            if (cvos.Count() > 0)
                            {
                                foreach (var item in cvos.ToList())
                                {
                                    var Ref = new ColorVision();

                                    if (item.OcularOS == "OS" && (item.CV_normalOS != "" || item.CV_defectiveOS != "" || item.Desc_TextOS != ""))
                                    {
                                        var regid = WYNKContext.RegistrationTran.Where(x => x.UIN == Addrefraction.UIN && x.CmpID == Addrefraction.cmpid).Select(x => x.RegistrationTranID).LastOrDefault();
                                        Ref.RegistrationTranID = regid;
                                        Ref.Description = item.Description;
                                        Ref.Ocular = item.OcularOS;
                                        Ref.CV_normal = item.CV_normalOS;
                                        Ref.CV_defective = item.CV_defectiveOS;
                                        Ref.Desc_Text = item.Desc_TextOS;
                                        Ref.Date = DateTime.UtcNow;
                                        Ref.CreatedUTC = DateTime.UtcNow;
                                        Ref.CreatedBy = Addrefraction.Createdby;
                                        WYNKContext.ColorVision.AddRange(Ref);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = Ref;
                                        oErrorLogstran.WriteErrorLogArray("ColorVision", namestr);

                                    }


                                }
                            }
                        }
                    }

                    var Srid = Convert.ToInt32(WYNKContext.SquintTran.Where(x => x.RID == regtid).OrderByDescending(x => x.CreatedUTC).Select(r => r.RID).FirstOrDefault());

                    if (Srid == 0)
                    {
                        if (Addrefraction.SquintTran != null)
                        {
                            if (Addrefraction.SquintTran.Count() > 0)
                            {
                                foreach (var item in Addrefraction.SquintTran.ToList())
                                {
                                    var ST = new SquintTran();

                                    ST.RID = regtid;
                                    ST.IsDVOD = item.IsDVOD;
                                    ST.IsDVOS = item.IsDVOS;
                                    ST.IsDVOU = item.IsDVOU;
                                    ST.Date = DateTime.UtcNow;
                                    ST.SquintType = item.SquintType;
                                    ST.SquintDiagnosisDescription = item.SquintDiagnosisDescription;
                                    ST.CreatedUTC = DateTime.UtcNow;
                                    ST.CreatedBy = Addrefraction.Createdby;
                                    ST.IsActive = true;
                                    WYNKContext.SquintTran.AddRange(ST);
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = ST;
                                    oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                                }
                            }
                        }


                    }

                    else
                    {

                        if (Addrefraction.SquintTran != null)
                        {
                            if (Addrefraction.SquintTran.Count() > 0)
                            {
                                foreach (var item in Addrefraction.SquintTran.ToList())
                                {
                                    var ST = new SquintTran();

                                    if (item.ID != 0)
                                    {
                                        ST = WYNKContext.SquintTran.Where(x => x.ID == item.ID).FirstOrDefault();
                                        ST.RID = regtid;
                                        ST.IsDVOD = item.IsDVOD;
                                        ST.IsDVOS = item.IsDVOS;
                                        ST.IsDVOU = item.IsDVOU;
                                        ST.Date = DateTime.UtcNow;
                                        ST.SquintType = item.SquintType;
                                        ST.SquintDiagnosisDescription = item.SquintDiagnosisDescription;
                                        ST.UpdatedUTC = DateTime.UtcNow;
                                        ST.UpdatedBy = Addrefraction.updatedby;
                                        WYNKContext.SquintTran.UpdateRange(ST);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = ST;
                                        oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                                    }
                                    else
                                    {
                                        ST.RID = regtid;
                                        ST.IsDVOD = item.IsDVOD;
                                        ST.IsDVOS = item.IsDVOS;
                                        ST.IsDVOU = item.IsDVOU;
                                        ST.Date = DateTime.UtcNow;
                                        ST.SquintType = item.SquintType;
                                        ST.SquintDiagnosisDescription = item.SquintDiagnosisDescription;
                                        ST.CreatedUTC = DateTime.UtcNow;
                                        ST.CreatedBy = Addrefraction.Createdby;
                                        ST.IsActive = true;
                                        WYNKContext.SquintTran.AddRange(ST);
                                        ErrorLog oErrorLogstran = new ErrorLog();
                                        object namestr = ST;
                                        oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                                    }
                                }
                            }
                        }



                    }

                    if (Addrefraction.SquintTranDelete != null)
                    {
                        if (Addrefraction.SquintTranDelete.Count() > 0)
                        {
                            foreach (var item in Addrefraction.SquintTranDelete.ToList())
                            {

                                var master = WYNKContext.SquintTran.Where(x => x.ID == item.ID).FirstOrDefault();
                                master.IsActive = false;
                                WYNKContext.Entry(master).State = EntityState.Modified;
                                ErrorLog oErrorLogstran = new ErrorLog();
                                object namestr = master;
                                oErrorLogstran.WriteErrorLogArray("SquintTran", namestr);
                            }
                        }
                    }


                    if (Addrefraction.fname == "Refraction")
                    {
                        var roledescription = Addrefraction.Role;
                        var cmpids = Convert.ToInt32(Addrefraction.CMPID);
                        var Regtranid = Convert.ToInt32(Addrefraction.Registrationtranid);
                        var doctorid = Convert.ToInt32(Addrefraction.Userid);
                        var usid = Convert.ToInt32(Addrefraction.usid);
                        var utctimes = CMPSContext.Setup.Where(x => x.CMPID == cmpids).Select(x => x.UTCTime).FirstOrDefault();

                        TimeSpan utctime = TimeSpan.Parse(utctimes);

                        var CurDate = DateTime.UtcNow;
                        var times1 = CurDate.Hour + ":" + CurDate.Minute + ":" + CurDate.Second;
                        var times2 = utctime;
                        var fulltime = TimeSpan.Parse(times1) + times2;
                        DateTime time = DateTime.Today.Add(fulltime);
                        var curfullh = time.Hour;
                        var curfullm = time.Minute;
                        string displayTimes = curfullh + ":" + curfullm;

                        if (Addrefraction.Role != "Doctor" && Addrefraction.Role != "Optometrist")
                        {
                            var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Refraction").Select(x => x.ID).FirstOrDefault();
                            var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid != role).ToList();

                            if (masters != null && masters.Count != 0)
                            {

                                masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                                WYNKContext.Workflowtran.UpdateRange(masters);

                            }
                            var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == true && x.Doctorid == 0 && x.status == true && x.Workflowmasterid == role).ToList();
                            if (master != null && master.Count != 0)
                            {
                                master.All(x => { x.Checkintime = displayTimes; return true; });
                                WYNKContext.Workflowtran.UpdateRange(master);

                            }
                            else
                            {

                                var workflow = new Workflowtran();
                                workflow.Workflowmasterid = role;
                                workflow.Regtranid = Regtranid;
                                workflow.Reception = true;
                                workflow.Doctor = false;
                                workflow.Optometrist = false;
                                workflow.Checkintime = displayTimes;
                                workflow.Checkouttime = "";
                                workflow.CMPID = cmpids;
                                workflow.Date = DateTime.UtcNow.Date;
                                workflow.Createdutc = DateTime.UtcNow;
                                workflow.Createdby = usid;
                                workflow.status = true;
                                workflow.Doctorid = 0;
                                WYNKContext.Workflowtran.Add(workflow);
                                WYNKContext.SaveChanges();
                            }


                        }

                        else
                        {
                            var role = WYNKContext.Workflowmaster.Where(x => x.Description == "Refraction").Select(x => x.ID).FirstOrDefault();
                            var masters = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid != role).ToList();


                            if (masters != null && masters.Count != 0)
                            {

                                masters.All(x => { x.Checkouttime = displayTimes; x.status = false; return true; });
                                WYNKContext.Workflowtran.UpdateRange(masters);

                            }
                            var master = WYNKContext.Workflowtran.Where(x => x.Regtranid == Regtranid && x.CMPID == cmpids && x.Reception == false && x.Doctorid != 0 && x.status == true && x.Workflowmasterid == role).ToList();
                            if (master != null && master.Count != 0)
                            {
                                master.All(x => { x.Checkintime = displayTimes; return true; });
                                WYNKContext.Workflowtran.UpdateRange(master);

                            }
                            else
                            {


                                var workflow = new Workflowtran();
                                workflow.Workflowmasterid = role;
                                workflow.Regtranid = Regtranid;
                                workflow.Reception = false;
                                if (Addrefraction.Role == "Doctor")
                                {
                                    workflow.Doctor = true;
                                }
                                if (Addrefraction.Role == "Optometrist")
                                {
                                    workflow.Optometrist = true;
                                }
                                workflow.Checkintime = displayTimes;
                                workflow.Checkouttime = "";
                                workflow.CMPID = cmpids;
                                workflow.Date = DateTime.UtcNow.Date;
                                workflow.Createdutc = DateTime.UtcNow;
                                workflow.Createdby = usid;
                                workflow.status = true;
                                workflow.Doctorid = doctorid;
                                WYNKContext.Workflowtran.Add(workflow);
                                WYNKContext.SaveChanges();
                            }


                        }
                    }


                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

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
                return new
                {
                    Success = false,
                };

            }

        }
        public dynamic docname(string uin, int ID)
        {
            var v = new RefractionMasterss();

            var patient = WYNKContext.PatientAssign.ToList();
            var doctormas = CMPSContext.DoctorMaster.ToList();

            var RE = WYNKContext.RegistrationTran.Where(x => x.UIN == uin).Select(x => x.RegistrationTranID).LastOrDefault();
            var DRID = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == RE).ToList();


            if (DRID.Count > 0)
            {

                foreach (var item in DRID.ToList())
                {

                    var d = WYNKContext.PatientAssign.Where(x => x.ID == item.ID).Select(x => x.DoctorID).FirstOrDefault();

                    v.Registerationno = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.StaffIdentification).FirstOrDefault();

                    if (v.Registerationno == "D" || v.Registerationno == "A")
                    {
                        v.Docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                        v.Regno = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.RegistrationNumber).FirstOrDefault();
                    }
                    if (v.Registerationno == "O")
                    {
                        v.OptoName = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                    }

                    if (v.Registerationno == "V")
                    {
                        v.VCName = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                    }

                }

            }

            else
            {

                v.docnam = CMPSContext.DoctorMaster.Where(x => x.DoctorID == ID).Select(x => x.Firstname + " " + x.MiddleName + " " + x.LastName).FirstOrDefault();
                v.docregg = CMPSContext.DoctorMaster.Where(x => x.DoctorID == ID).Select(x => x.RegistrationNumber).FirstOrDefault();

            }


            try
            {

                return new
                {
                    Doctorname = v.Docname,
                    Registerationno = v.Regno,
                    Optometristname = v.OptoName,
                    VCTname = v.VCName,

                    Docnamee = v.docnam,
                    Docrgg = v.docregg,


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
        public dynamic GetrefractionDetails(string UIN, int CMPID)
        {
            var refractionDetails = new RefractionMasterss();
            var one = CMPSContext.OneLineMaster.ToList();
            var company = CMPSContext.Company.ToList();
            refractionDetails.SquintTraninfo = new List<SquintTraninfo>();
            refractionDetails.Refracion = new List<Refraction>();
            refractionDetails.VISUALACUITY = new List<VISUALACUITYMas>();
            refractionDetails.visualacuitypaediatric = new List<visualacuitypaediatric>();
            refractionDetails.PGP = new List<PGPMas>();
            refractionDetails.REFRACTIONEXT = new List<REFRACTIONEXTMast>();
            refractionDetails.ACCEPTANCE = new List<ACCEPTANCEMas>();
            refractionDetails.FINALPRESCRIPTION = new List<FINALPRESCRIPTIONMas>();
            refractionDetails.ColorVision = new List<ColorVisionMas>();
            refractionDetails.Amsler = new List<Amslermas>();
            refractionDetails.SquintTran = new List<SquintTran>();
            refractionDetails.SquintTranDelete = new List<SquintTranDelete>();
            var cmpid = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CmpID).FirstOrDefault();

            var TranID = WYNKContext.Refraction.Where(u => u.UIN == UIN && cmpid == company.Where(z => z.CmpID == CMPID).Select(s => s.CmpID).FirstOrDefault()).OrderByDescending(x => x.CreatedUTC).Select(x => x.RegistrationTranID).FirstOrDefault();

            refractionDetails.VISUALACUITY = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Visual Acuity" && x.SubCategory != 0).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.SubCategory)
                                              select new VISUALACUITYMas
                                              {
                                                  Remarks = res.Select(x => x.Remarks).FirstOrDefault(),
                                                  ChartType = res.Select(x => x.ChartType).FirstOrDefault() != "" ? res.Select(x => x.ChartType).FirstOrDefault() : string.Empty,
                                                  Description = res.Select(x => x.Description).FirstOrDefault(),
                                                  SubCategory = res.Select(x => x.SubCategory).FirstOrDefault(),
                                                  ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                                  DistSph = res.Where(x => x.Ocular == "OD").Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                  NearCyl = res.Where(x => x.Ocular == "OD").Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                  PinAxis = res.Where(x => x.Ocular == "OD").Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                  N_V_DESC = res.Where(x => x.Ocular == "OD").Select(x => x.N_V_DESC).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.N_V_DESC).FirstOrDefault() : string.Empty,
                                                  PowerGlass = res.Where(x => x.Ocular == "OD").Select(x => x.PowerGlass).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.PowerGlass).FirstOrDefault() : string.Empty,
                                                  Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                                  IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                                  DistSphOS = res.Where(x => x.Ocular == "OS").Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                  NearCylOS = res.Where(x => x.Ocular == "OS").Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                  PinAxisOS = res.Where(x => x.Ocular == "OS").Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                  N_V_DESCOS = res.Where(x => x.Ocular == "OS").Select(x => x.N_V_DESC).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.N_V_DESC).FirstOrDefault() : string.Empty,
                                                  PowerGlassOS = res.Where(x => x.Ocular == "OS").Select(x => x.PowerGlass).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.PowerGlass).FirstOrDefault() : string.Empty,
                                                  OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                                  CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                                  RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),

                                              }).ToList();

            refractionDetails.PGP = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "PGP").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)
                                     select new PGPMas
                                     {
                                         Details = res.Select(x => x.Details).FirstOrDefault(),
                                         Description = res.Select(x => x.Description).FirstOrDefault(),
                                         ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                         DistSph = res.Where(x => x.Ocular == "OD").Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                         NearCyl = res.Where(x => x.Ocular == "OD").Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                         PinAxis = res.Where(x => x.Ocular == "OD").Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                         Add = res.Where(x => x.Ocular == "OD").Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Add).FirstOrDefault() : string.Empty,
                                         Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                         IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                         DistSphOS = res.Where(x => x.Ocular == "OS").Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                         NearCylOS = res.Where(x => x.Ocular == "OS").Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                         PinAxisOS = res.Where(x => x.Ocular == "OS").Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                         AddOS = res.Where(x => x.Ocular == "OS").Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Add).FirstOrDefault() : string.Empty,
                                         OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                         CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                         Subcategory = res.Select(x => x.SubCategory).FirstOrDefault() != null ? res.Select(x => x.SubCategory).FirstOrDefault() : 0,
                                         RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),
                                     }).ToList();

            refractionDetails.visualacuitypaediatric = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Visual Acuity" && x.SubCategory == 0).OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)
                                                        select new visualacuitypaediatric
                                                        {
                                                            Remarks = res.Select(x => x.Remarks).FirstOrDefault(),
                                                            ChartType = res.Select(x => x.ChartType).FirstOrDefault() != "" ? res.Select(x => x.ChartType).FirstOrDefault() : string.Empty,
                                                            Description = res.Select(x => x.Description).FirstOrDefault(),
                                                            ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                                            Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                                            Central = res.Where(x => x.Ocular == "OD").Select(x => x.Central).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.Central).FirstOrDefault() : false,
                                                            Steady = res.Where(x => x.Ocular == "OD").Select(x => x.Steady).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.Steady).FirstOrDefault() : false,
                                                            Maintained = res.Where(x => x.Ocular == "OD").Select(x => x.Maintained).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.Maintained).FirstOrDefault() : false,
                                                            Uncentral = res.Where(x => x.Ocular == "OD").Select(x => x.Uncentral).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.Uncentral).FirstOrDefault() : false,
                                                            Unsteady = res.Where(x => x.Ocular == "OD").Select(x => x.Unsteady).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.Unsteady).FirstOrDefault() : false,
                                                            Unmaintained = res.Where(x => x.Ocular == "OD").Select(x => x.Unmaintained).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.Unmaintained).FirstOrDefault() : false,
                                                            IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                                            OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                                            CentralOS = res.Where(x => x.Ocular == "OS").Select(x => x.Central).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.Central).FirstOrDefault() : false,
                                                            SteadyOS = res.Where(x => x.Ocular == "OS").Select(x => x.Steady).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.Steady).FirstOrDefault() : false,
                                                            MaintainedOS = res.Where(x => x.Ocular == "OS").Select(x => x.Maintained).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.Maintained).FirstOrDefault() : false,
                                                            UncentralOS = res.Where(x => x.Ocular == "OS").Select(x => x.Uncentral).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.Uncentral).FirstOrDefault() : false,
                                                            UnsteadyOS = res.Where(x => x.Ocular == "OS").Select(x => x.Unsteady).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.Unsteady).FirstOrDefault() : false,
                                                            UnmaintainedOS = res.Where(x => x.Ocular == "OS").Select(x => x.Unmaintained).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.Unmaintained).FirstOrDefault() : false,
                                                            CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                                            RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),
                                                        }).ToList();

            refractionDetails.Amsler = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Amsler").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)
                                        select new Amslermas
                                        {
                                            Description = res.Select(x => x.Description).FirstOrDefault(),
                                            ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                            A_n_OD = res.Where(x => x.Ocular == "OD").Select(x => x.A_n_OD).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.A_n_OD).FirstOrDefault() : false,
                                            A_abn_OD = res.Where(x => x.Ocular == "OD").Select(x => x.A_abn_OD).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OD").Select(x => x.A_abn_OD).FirstOrDefault() : false,
                                            Desc_Text = res.Where(x => x.Ocular == "OD").Select(x => x.Desc_Text).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Desc_Text).FirstOrDefault() : string.Empty,
                                            Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                            IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                            A_n_OS = res.Where(x => x.Ocular == "OS").Select(x => x.A_n_OS).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.A_n_OS).FirstOrDefault() : false,
                                            A_abn_OS = res.Where(x => x.Ocular == "OS").Select(x => x.A_abn_OS).FirstOrDefault() != false ? res.Where(x => x.Ocular == "OS").Select(x => x.A_abn_OS).FirstOrDefault() : false,
                                            Desc_TextOS = res.Where(x => x.Ocular == "OS").Select(x => x.Desc_Text).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Desc_Text).FirstOrDefault() : string.Empty,
                                            OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                            CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                            Subcategory = res.Select(x => x.SubCategory).FirstOrDefault() != null ? res.Select(x => x.SubCategory).FirstOrDefault() : 0,
                                            RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),
                                        }).ToList();

            refractionDetails.REFRACTIONEXT = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Refraction").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.SubCategory)
                                               select new REFRACTIONEXTMast
                                               {
                                                   Description = res.Select(x => x.Description).FirstOrDefault(),
                                                   SubCategory = res.Select(x => x.SubCategory).FirstOrDefault(),
                                                   ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                                   Sphdry = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                   Cyldry = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                   Axisdry = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                   Sphwet = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                   Cylwet = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                   Axiswet = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                   Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                                   TypeDry = one.Where(x => x.ParentDescription == "Dry").Select(x => x.OLMID).FirstOrDefault(),
                                                   IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                                   Sphdryos = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                   Cyldryos = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                   Axisdryos = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                   Sphwetos = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                   Cylwetos = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                   Axiswetos = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                   OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                                   TypeWet = one.Where(x => x.ParentDescription == "Wet").Select(x => x.OLMID).FirstOrDefault(),
                                                   CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                                   RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),

                                               }).ToList();

            refractionDetails.ACCEPTANCE = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Acceptance").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)
                                            select new ACCEPTANCEMas
                                            {
                                                Description = res.Select(x => x.Description).FirstOrDefault(),
                                                ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                                DistSph = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                NearCyl = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                PinAxis = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                Add = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                DistSphNVOD = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                AddNVOD = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                                DV = one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault(),
                                                IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                                DistSphOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                NearCylOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                PinAxisOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                AddOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                DistSphNVOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                AddNVOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                                NV = one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault(),
                                                Remarks = res.Select(x => x.Remarks).FirstOrDefault(),
                                                PD = res.Select(x => x.PD).FirstOrDefault(),
                                                MPDOD = res.Select(x => x.MPDOD).FirstOrDefault(),
                                                MPDOS = res.Select(x => x.MPDOS).FirstOrDefault(),
                                                CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                                Subcategory = res.Select(x => x.SubCategory).FirstOrDefault() != null ? res.Select(x => x.SubCategory).FirstOrDefault() : 0,
                                                RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),

                                            }).ToList();


            refractionDetails.FINALPRESCRIPTION = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Final Prescription").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)
                                                   select new FINALPRESCRIPTIONMas
                                                   {
                                                       Description = res.Select(x => x.Description).FirstOrDefault(),
                                                       ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                                       DistSph = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                       NearCyl = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                       PinAxis = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                       Add = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                       DistSphNVOD = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                       AddNVOD = res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                       Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                                       DV = one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault(),
                                                       IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                                       DistSphOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                       NearCylOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.NearCyl).FirstOrDefault() : string.Empty,
                                                       PinAxisOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.PinAxis).FirstOrDefault() : string.Empty,
                                                       AddOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Distance Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                       DistSphNVOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.DistSph).FirstOrDefault() : string.Empty,
                                                       AddNVOS = res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS" && x.Type == one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault()).Select(x => x.Add).FirstOrDefault() : string.Empty,
                                                       OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                                       NV = one.Where(z => z.ParentDescription.Replace(" ", string.Empty).Equals("Near Vision".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(s => s.OLMID).FirstOrDefault(),
                                                       Remarks = res.Select(x => x.Remarks).FirstOrDefault(),
                                                       PD = res.Select(x => x.PD).FirstOrDefault(),
                                                       MPDOD = res.Select(x => x.MPDOD).FirstOrDefault(),
                                                       MPDOS = res.Select(x => x.MPDOS).FirstOrDefault(),
                                                       CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                                       Subcategory = res.Select(x => x.SubCategory).FirstOrDefault() != null ? res.Select(x => x.SubCategory).FirstOrDefault() : 0,
                                                       RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),
                                                   }).ToList();

            refractionDetails.ColorVision = (from res in WYNKContext.Refraction.Where(x => x.RegistrationTranID == TranID && x.Description == "Color Vision").OrderByDescending(x => x.CreatedUTC).GroupBy(x => x.CreatedBy)

                                             select new ColorVisionMas
                                             {
                                                 Description = res.Select(x => x.Description).FirstOrDefault(),
                                                 ID = res.Where(x => x.Ocular == "OD").Select(x => x.ID).FirstOrDefault(),
                                                 CV_normal = res.Where(x => x.Ocular == "OD").Select(x => x.CV_normal).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.CV_normal).FirstOrDefault() : string.Empty,
                                                 CV_defective = res.Where(x => x.Ocular == "OD").Select(x => x.CV_defective).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.CV_defective).FirstOrDefault() : string.Empty,
                                                 Desc_Text = res.Where(x => x.Ocular == "OD").Select(x => x.Desc_Text).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Desc_Text).FirstOrDefault() : string.Empty,
                                                 Ocular = res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OD").Select(x => x.Ocular).FirstOrDefault() : "OD",
                                                 IDOS = res.Where(x => x.Ocular == "OS").Select(x => x.ID).FirstOrDefault(),
                                                 CV_normalOS = res.Where(x => x.Ocular == "OS").Select(x => x.CV_normal).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.CV_normal).FirstOrDefault() : string.Empty,
                                                 CV_defectiveOS = res.Where(x => x.Ocular == "OS").Select(x => x.CV_defective).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.CV_defective).FirstOrDefault() : string.Empty,
                                                 Desc_TextOS = res.Where(x => x.Ocular == "OS").Select(x => x.Desc_Text).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Desc_Text).FirstOrDefault() : string.Empty,
                                                 OcularOS = res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() != null ? res.Where(x => x.Ocular == "OS").Select(x => x.Ocular).FirstOrDefault() : "OS",
                                                 CreatedUTc = res.Select(x => x.CreatedUTC).FirstOrDefault(),
                                                 Subcategory = res.Select(x => x.SubCategory).FirstOrDefault() != null ? res.Select(x => x.SubCategory).FirstOrDefault() : 0,
                                                 RegistrationTranID = res.Select(x => x.RegistrationTranID).FirstOrDefault(),
                                             }).ToList();






            var RID = WYNKContext.SquintTran.Where(w => w.RID == WYNKContext.RegistrationTran.Where(q => q.UIN == WYNKContext.Registration.Where(u => u.UIN == UIN).Select(x => x.UIN).FirstOrDefault()).OrderByDescending(s => s.CreatedUTC).Select(s => s.RegistrationTranID).FirstOrDefault()).OrderByDescending(t => t.CreatedUTC).Select(a => a.RID).FirstOrDefault();

            refractionDetails.SquintTraninfo = (from sq in WYNKContext.SquintTran.Where(x => x.RID == RID && x.IsActive == true)

                                                select new SquintTraninfo
                                                {



                                                    IsDVOD = sq.IsDVOD,
                                                    IsDVOS = sq.IsDVOS,
                                                    IsDVOU = sq.IsDVOU,
                                                    OD = sq.IsDVOD,
                                                    OS = sq.IsDVOS,
                                                    OU = sq.IsDVOU,
                                                    Date = sq.Date,
                                                    SquintDiagnosisDescription = sq.SquintDiagnosisDescription,
                                                    SquintType = sq.SquintType,
                                                    ID = sq.ID,
                                                    IsActive = sq.IsActive
                                                }).ToList();
            refractionDetails.UIN = WYNKContext.Refraction.Where(x => x.UIN == UIN).Select(x => x.UIN).LastOrDefault();
            int r = refractionDetails.UIN != null ? WYNKContext.RegistrationTran.Where(f => f.UIN == refractionDetails.UIN).Select(h => h.RegistrationTranID).LastOrDefault() : 0;

            refractionDetails.TransID = r != 0 ? WYNKContext.Refraction.Where(x => x.RegistrationTranID == r).Select(d => d.RegistrationTranID).LastOrDefault() : 0;

            return refractionDetails;
        }
        public dynamic Getrefractionprint(string UIN, int CMPID, string Time)

        {

            var refractionDetailsprint = new RefractionMasterss();
            var doc = CMPSContext.Users.ToList();
            var re = WYNKContext.Refraction.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);
            refractionDetailsprint.RefractionMas = new List<RefractionMas>();

            refractionDetailsprint.TransID = WYNKContext.Refraction.Where(u => u.UIN == UIN).Select(x => x.RegistrationTranID).LastOrDefault();

            var d = WYNKContext.PatientAssign.Where(x => x.RegistrationTranID == refractionDetailsprint.TransID).Select(x => x.DoctorID).FirstOrDefault();
            var v = doc.Where(x => x.Userid == re.Where(c => c.RegistrationTranID == refractionDetailsprint.TransID).Select(g => g.CreatedBy).FirstOrDefault()).Select(x => x.Username).FirstOrDefault();
            var f = CMPSContext.DoctorMaster.Where(x => x.EmailID == v).Select(x => x.DoctorID).FirstOrDefault();

            if (d != 0)
            {
                refractionDetailsprint.docnamefirst = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.Firstname).FirstOrDefault();
                refractionDetailsprint.docnamesecond = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.MiddleName).FirstOrDefault();
                refractionDetailsprint.docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.LastName).FirstOrDefault();
                refractionDetailsprint.docreg = CMPSContext.DoctorMaster.Where(x => x.DoctorID == d).Select(x => x.RegistrationNumber).FirstOrDefault();

            }
            else
            {
                refractionDetailsprint.docnamefirst = CMPSContext.DoctorMaster.Where(x => x.DoctorID == f).Select(x => x.Firstname).FirstOrDefault();
                refractionDetailsprint.docnamesecond = CMPSContext.DoctorMaster.Where(x => x.DoctorID == f).Select(x => x.MiddleName).FirstOrDefault();
                refractionDetailsprint.docname = CMPSContext.DoctorMaster.Where(x => x.DoctorID == f).Select(x => x.LastName).FirstOrDefault();
                refractionDetailsprint.docreg = CMPSContext.DoctorMaster.Where(x => x.DoctorID == f).Select(x => x.RegistrationNumber).FirstOrDefault();

            }

            refractionDetailsprint.UINID = WYNKContext.Refraction.Where(x => x.UIN == UIN).Select(x => x.UIN).FirstOrDefault();
            refractionDetailsprint.Date = WYNKContext.Refraction.Where(x => x.RegistrationTranID == refractionDetailsprint.TransID).Select(x => x.CreatedUTC.Add(ts)).FirstOrDefault();

            refractionDetailsprint.Name = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.Name).FirstOrDefault();
            refractionDetailsprint.SecondName = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.MiddleName).FirstOrDefault();
            refractionDetailsprint.LastName = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.LastName).FirstOrDefault();

            refractionDetailsprint.Age = PasswordEncodeandDecode.ToAgeString(WYNKContext.Registration.Where(x => x.UIN == refractionDetailsprint.UINID).Select(x => x.DateofBirth).FirstOrDefault());
            refractionDetailsprint.Gender = WYNKContext.Registration.Where(x => x.UIN == UIN).Select(x => x.Gender).FirstOrDefault();
            refractionDetailsprint.Description = WYNKContext.allergy.Where(s => s.ID == WYNKContext.AllergyTran.Where(x => x.UIN == UIN && x.IsActive == true).Select(x => x.Description).FirstOrDefault()).Select(r => r.ParentDescription).FirstOrDefault() != null ? WYNKContext.allergy.Where(s => s.ID == WYNKContext.AllergyTran.Where(x => x.UIN == UIN && x.IsActive == true).Select(x => x.Description).FirstOrDefault()).Select(r => r.ParentDescription).FirstOrDefault() : string.Empty;
            refractionDetailsprint.Address = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address1).FirstOrDefault() : string.Empty;
            refractionDetailsprint.Address1 = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address2).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address2).FirstOrDefault() : string.Empty;
            refractionDetailsprint.Address2 = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address3).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Address3).FirstOrDefault() : string.Empty;
            refractionDetailsprint.phone = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Phone1).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Phone1).FirstOrDefault() : string.Empty;
            refractionDetailsprint.web = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Website).FirstOrDefault() != null ? CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.Website).FirstOrDefault() : string.Empty;
            refractionDetailsprint.Compnayname = CMPSContext.Company.Where(x => x.CmpID == CMPID).Select(x => x.CompanyName).FirstOrDefault();
            refractionDetailsprint.RefractionMas = WYNKContext.Refraction.Where(x => x.RegistrationTranID == refractionDetailsprint.TransID && x.Description == "Final Prescription").ToList();

            if (refractionDetailsprint.RefractionMas.Count() > 0)
            {
                foreach (var item in refractionDetailsprint.RefractionMas.ToList())

                {

                    if (item.Type == one.Where(z => z.ParentDescription == "Distance Vision").Select(s => s.OLMID).FirstOrDefault())
                    {


                        if (item.Ocular == "OD")
                        {
                            refractionDetailsprint.dsph = item.DistSph != null || item.DistSph != "" ? item.DistSph : string.Empty;
                            refractionDetailsprint.nearcyl = item.NearCyl != null || item.NearCyl != "" ? item.NearCyl : string.Empty;
                            refractionDetailsprint.pinaxsis = item.PinAxis != null || item.PinAxis != "" ? item.PinAxis : string.Empty;
                            refractionDetailsprint.add = item.Add != null || item.Add != "" ? item.Add : string.Empty;

                        }

                        if (item.Ocular == "OS")
                        {
                            refractionDetailsprint.dsphos = item.DistSph != null || item.DistSph != "" ? item.DistSph : string.Empty;
                            refractionDetailsprint.nearcylos = item.NearCyl != null || item.NearCyl != "" ? item.NearCyl : string.Empty;
                            refractionDetailsprint.pinaxsisos = item.PinAxis != null || item.PinAxis != "" ? item.PinAxis : string.Empty;
                            refractionDetailsprint.addos = item.Add != null || item.Add != "" ? item.Add : string.Empty;
                        }


                    }
                    if (item.Type == one.Where(z => z.ParentDescription == "Near Vision").Select(s => s.OLMID).FirstOrDefault())
                    {
                        if (item.Ocular == "OD")
                        {

                            refractionDetailsprint.DsphOD = item.DistSph != null || item.DistSph != "" ? item.DistSph : string.Empty;
                            refractionDetailsprint.addOD = item.Add != null || item.Add != "" ? item.Add : string.Empty;

                        }
                        if (item.Ocular == "OS")

                        {

                            refractionDetailsprint.DsphOS = item.DistSph != null || item.DistSph != "" ? item.DistSph : string.Empty;
                            refractionDetailsprint.AddOS = item.Add != null || item.Add != "" ? item.Add : string.Empty;
                        }

                    }

                    refractionDetailsprint.Remark = item.Remarks != null || item.Remarks != "" ? item.Remarks : string.Empty;
                    refractionDetailsprint.PD = item.PD != null || item.PD != "" ? item.PD : string.Empty;
                    refractionDetailsprint.MPDOD = item.MPDOD != null || item.MPDOD != "" ? item.MPDOD : string.Empty;
                    refractionDetailsprint.MPDOS = item.MPDOS != null || item.MPDOS != "" ? item.MPDOS : string.Empty;


                }
            }

            try
            {

                return new
                {

                    dsphod = refractionDetailsprint.dsph,
                    addod = refractionDetailsprint.add,
                    pRemark = refractionDetailsprint.Remark,
                    pPD = refractionDetailsprint.PD,
                    MPD = refractionDetailsprint.MPDOD,
                    MPS = refractionDetailsprint.MPDOS,
                    pinaxsisod = refractionDetailsprint.pinaxsis,
                    nearcylod = refractionDetailsprint.nearcyl,
                    dsphoss = refractionDetailsprint.dsphos,
                    addoss = refractionDetailsprint.addos,
                    pinaxsisoss = refractionDetailsprint.pinaxsisos,
                    nearcyloss = refractionDetailsprint.nearcylos,
                    DsphODD = refractionDetailsprint.DsphOD,
                    addODD = refractionDetailsprint.addOD,
                    DsphOSS = refractionDetailsprint.DsphOS,
                    AddOSS = refractionDetailsprint.AddOS,
                    UinNo = refractionDetailsprint.UINID,
                    Daterec = refractionDetailsprint.Date,
                    Patientname = refractionDetailsprint.Name,
                    Patientage = refractionDetailsprint.Age,
                    Patientgender = refractionDetailsprint.Gender,
                    patientdesc = refractionDetailsprint.Description,
                    Addressss = refractionDetailsprint.Address,
                    Addressss1 = refractionDetailsprint.Address1,
                    Addressss2 = refractionDetailsprint.Address2,
                    phoness = refractionDetailsprint.phone,
                    webb = refractionDetailsprint.web,
                    company = refractionDetailsprint.Compnayname,
                    Doctname = refractionDetailsprint.docname,
                    Doctreg = refractionDetailsprint.docreg,
                    final = refractionDetailsprint.RefractionMas,
                    first = refractionDetailsprint.docnamefirst,
                    second = refractionDetailsprint.docnamesecond,
                    patsec = refractionDetailsprint.SecondName,
                    patlat = refractionDetailsprint.LastName

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
        public dynamic GetHistoryvisualDetails(string UIN, string Time)
        {
            var HDetails = new RefractionMasterss();
            HDetails.HistoryVisualacuity = new List<HistoryVisualacuity>();

            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var vs = WYNKContext.VISUALACUITY.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);

            var TID = WYNKContext.Refraction.Where(u => u.UIN == UIN).GroupBy(x => x.RegistrationTranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qry = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory != 0)
                          join use in user on
                          va.CreatedBy equals use.Userid
                          join doc in docmas on use.Username equals doc.EmailID
                          join usr in userrole on
                          use.Userid equals usr.UserID


                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              Tag = use.ReferenceTag,
                              VDescription = va.Description != null ? va.Description : string.Empty,
                              SubCategory = va.SubCategory != null ? onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                              DistSph = va.DistSph != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.DistSph)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              NearCyl = va.NearCyl != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.NearCyl)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              VPowerGlass = va.PowerGlass != "" ? va.PowerGlass : string.Empty,
                              NVDESC = va.N_V_DESC != "" ? va.N_V_DESC : string.Empty,
                              PinAxis = va.PinAxis != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.PinAxis)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              createdby = va.CreatedBy,
                              Ocularr = va.Ocular,
                              rid = va.RegistrationTranID,
                              ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                          };

                var qryAdmin = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory != 0)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID


                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   Tag = use.ReferenceTag,
                                   VDescription = va.Description != null ? va.Description : string.Empty,
                                   SubCategory = va.SubCategory != null ? onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                                   DistSph = va.DistSph != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.DistSph)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   NearCyl = va.NearCyl != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.NearCyl)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   VPowerGlass = va.PowerGlass != "" ? va.PowerGlass : string.Empty,
                                   NVDESC = va.N_V_DESC != "" ? va.N_V_DESC : string.Empty,
                                   PinAxis = va.PinAxis != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.PinAxis)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   createdby = va.CreatedBy,
                                   Ocularr = va.Ocular,
                                   rid = va.RegistrationTranID,
                                   ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                               };

                var grpres = qry.Count() > 0 ? qry.ToList() : qryAdmin.ToList();

                if (grpres.Count != 0)
                {

                    HDetails.HistoryVisualacuity1 = (from res in grpres.GroupBy(item => new { item.SubCategory, item.createdby })
                                                     select new HistoryVisualacuity1
                                                     {
                                                         Description = res.Select(x => x.VDescription).LastOrDefault(),
                                                         SubCategory = res.Where(x => x.Ocularr == "OD").Select(x => x.SubCategory).LastOrDefault(),
                                                         SubCatgry = res.Where(x => x.Ocularr == "OS").Select(x => x.SubCategory).LastOrDefault(),
                                                         DistSph = res.Where(x => x.Ocularr == "OD").Select(x => x.DistSph).LastOrDefault(),
                                                         NearCyl = res.Where(x => x.Ocularr == "OD").Select(x => x.NearCyl).LastOrDefault(),
                                                         PinAxis = res.Where(x => x.Ocularr == "OD").Select(x => x.PinAxis).LastOrDefault(),
                                                         N_V_DESC = res.Where(x => x.Ocularr == "OD").Select(x => x.NVDESC).LastOrDefault(),
                                                         PowerGlass = res.Where(x => x.Ocularr == "OD").Select(x => x.VPowerGlass).LastOrDefault(),
                                                         DistSphh = res.Where(x => x.Ocularr == "OS").Select(x => x.DistSph).LastOrDefault(),
                                                         NearCyll = res.Where(x => x.Ocularr == "OS").Select(x => x.NearCyl).LastOrDefault(),
                                                         PinAxiss = res.Where(x => x.Ocularr == "OS").Select(x => x.PinAxis).LastOrDefault(),
                                                         N_V_DESCC = res.Where(x => x.Ocularr == "OS").Select(x => x.NVDESC).LastOrDefault(),
                                                         PowerGlasss = res.Where(x => x.Ocularr == "OS").Select(x => x.VPowerGlass).LastOrDefault(),
                                                         Name = res.Select(x => x.Name).LastOrDefault(),
                                                         Role = res.Select(x => x.Role).LastOrDefault(),
                                                         Date = res.Select(x => x.VDate).LastOrDefault(),
                                                         ChartType = res.Select(x => x.ChartType).LastOrDefault(),
                                                         Remarks = res.Select(x => x.Remarks).LastOrDefault(),

                                                     }).ToList();

                    foreach (var itm in HDetails.HistoryVisualacuity1.ToList())
                    {
                        var Hisv = new HistoryVisualacuity();

                        Hisv.Description = itm.Description;
                        Hisv.SubCategory = itm.SubCategory;
                        Hisv.SubCatgry = itm.SubCatgry;
                        Hisv.DistSph = itm.DistSph;
                        Hisv.NearCyl = itm.NearCyl;
                        Hisv.PinAxis = itm.PinAxis;
                        Hisv.N_V_DESC = itm.N_V_DESC;
                        Hisv.PowerGlass = itm.PowerGlass;
                        Hisv.DistSphh = itm.DistSphh;
                        Hisv.NearCyll = itm.NearCyll;
                        Hisv.PinAxiss = itm.PinAxiss;
                        Hisv.N_V_DESCC = itm.N_V_DESCC;
                        Hisv.PowerGlasss = itm.PowerGlasss;
                        Hisv.Name = itm.Name;
                        Hisv.Role = itm.Role;
                        Hisv.Date = itm.Date;
                        Hisv.Remarks = itm.Remarks;
                        Hisv.ChartType = itm.ChartType;
                        HDetails.HistoryVisualacuity.Add(Hisv);

                    }

                }

            }



            return HDetails;
        }
        public dynamic GetHistoryvisualacuitypaediatric(string UIN, string Time)
        {
            var HDetails = new RefractionMasterss();
            HDetails.Historyvisualacuitypaediatric = new List<Historyvisualacuitypaediatric>();

            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var vs = WYNKContext.VISUALACUITY.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);
            var TID = WYNKContext.Refraction.Where(u => u.UIN == UIN).GroupBy(x => x.RegistrationTranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qry = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory == 0)
                          join use in user on
                          va.CreatedBy equals use.Userid
                          join doc in docmas on use.Username equals doc.EmailID
                          join usr in userrole on
                           use.Userid equals usr.UserID

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              VDescription = va.Description != null ? va.Description : string.Empty,
                              VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                              Central = va.Central != false ? va.Central : false,
                              Steady = va.Steady != false ? va.Steady : false,
                              Maintained = va.Maintained != false ? va.Maintained : false,
                              Uncentral = va.Uncentral != false ? va.Uncentral : false,
                              Unsteady = va.Unsteady != false ? va.Unsteady : false,
                              Unmaintained = va.Unmaintained != false ? va.Unmaintained : false,
                              CentralOS = va.Central != false ? va.Central : false,
                              SteadyOS = va.Steady != false ? va.Steady : false,
                              MaintainedOS = va.Maintained != false ? va.Maintained : false,
                              UncentralOS = va.Uncentral != false ? va.Uncentral : false,
                              UnsteadyOS = va.Unsteady != false ? va.Unsteady : false,
                              UnmaintainedOS = va.Unmaintained != false ? va.Unmaintained : false,
                              createdby = va.CreatedBy,
                              Ocularr = va.Ocular,
                              rid = va.RegistrationTranID,
                              ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                              Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                          };

                var qryAdmin = from va in vs.Where(u => u.RegistrationTranID == rid && u.SubCategory == 0)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                               use.Userid equals userole.UserID

                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   VDescription = va.Description != null ? va.Description : string.Empty,
                                   VOcular = va.Ocular != null ? va.Ocular : string.Empty,
                                   Central = va.Central != false ? va.Central : false,
                                   Steady = va.Steady != false ? va.Steady : false,
                                   Maintained = va.Maintained != false ? va.Maintained : false,
                                   Uncentral = va.Uncentral != false ? va.Uncentral : false,
                                   Unsteady = va.Unsteady != false ? va.Unsteady : false,
                                   Unmaintained = va.Unmaintained != false ? va.Unmaintained : false,
                                   CentralOS = va.Central != false ? va.Central : false,
                                   SteadyOS = va.Steady != false ? va.Steady : false,
                                   MaintainedOS = va.Maintained != false ? va.Maintained : false,
                                   UncentralOS = va.Uncentral != false ? va.Uncentral : false,
                                   UnsteadyOS = va.Unsteady != false ? va.Unsteady : false,
                                   UnmaintainedOS = va.Unmaintained != false ? va.Unmaintained : false,
                                   createdby = va.CreatedBy,
                                   Ocularr = va.Ocular,
                                   rid = va.RegistrationTranID,
                                   ChartType = va.ChartType != "" ? onelinemaster.Where(x => x.OLMID == Convert.ToInt32(va.ChartType)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                   Remarks = va.Remarks != "" ? va.Remarks : string.Empty,
                               };




                var grpres = qry.Count() > 0 ? qry.ToList() : qryAdmin.ToList();

                if (grpres.Count != 0)
                {

                    HDetails.Historyvisualacuitypaediatric1 = (from res in grpres.GroupBy(x => x.createdby)
                                                               select new Historyvisualacuitypaediatric1
                                                               {
                                                                   Description = res.Select(x => x.VDescription).LastOrDefault(),
                                                                   Central = res.Where(x => x.Ocularr == "OD").Select(x => x.Central).LastOrDefault(),
                                                                   Steady = res.Where(x => x.Ocularr == "OD").Select(x => x.Steady).LastOrDefault(),
                                                                   Maintained = res.Where(x => x.Ocularr == "OD").Select(x => x.Maintained).LastOrDefault(),
                                                                   Uncentral = res.Where(x => x.Ocularr == "OD").Select(x => x.Uncentral).LastOrDefault(),
                                                                   Unsteady = res.Where(x => x.Ocularr == "OD").Select(x => x.Unsteady).LastOrDefault(),
                                                                   Unmaintained = res.Where(x => x.Ocularr == "OD").Select(x => x.Unmaintained).LastOrDefault(),
                                                                   CentralOS = res.Where(x => x.Ocularr == "OS").Select(x => x.CentralOS).LastOrDefault(),
                                                                   SteadyOS = res.Where(x => x.Ocularr == "OS").Select(x => x.SteadyOS).LastOrDefault(),
                                                                   MaintainedOS = res.Where(x => x.Ocularr == "OS").Select(x => x.MaintainedOS).LastOrDefault(),
                                                                   UncentralOS = res.Where(x => x.Ocularr == "OS").Select(x => x.UncentralOS).LastOrDefault(),
                                                                   UnsteadyOS = res.Where(x => x.Ocularr == "OS").Select(x => x.UnsteadyOS).LastOrDefault(),
                                                                   UnmaintainedOS = res.Where(x => x.Ocularr == "OS").Select(x => x.UnmaintainedOS).LastOrDefault(),
                                                                   Name = res.Select(x => x.Name).LastOrDefault(),
                                                                   Role = res.Select(x => x.Role).LastOrDefault(),
                                                                   Date = res.Select(x => x.VDate).LastOrDefault(),
                                                                   ChartType = res.Select(x => x.ChartType).LastOrDefault(),
                                                                   Remarks = res.Select(x => x.Remarks).LastOrDefault(),
                                                               }).ToList();



                    foreach (var itm in HDetails.Historyvisualacuitypaediatric1.ToList())
                    {
                        var Hisv = new Historyvisualacuitypaediatric();

                        Hisv.Description = itm.Description;
                        Hisv.Central = itm.Central;
                        Hisv.Steady = itm.Steady;
                        Hisv.Maintained = itm.Maintained;
                        Hisv.Uncentral = itm.Uncentral;
                        Hisv.Unsteady = itm.Unsteady;
                        Hisv.Unmaintained = itm.Unmaintained;
                        Hisv.CentralOS = itm.CentralOS;
                        Hisv.SteadyOS = itm.SteadyOS;
                        Hisv.MaintainedOS = itm.MaintainedOS;
                        Hisv.UncentralOS = itm.UncentralOS;
                        Hisv.UnsteadyOS = itm.UnsteadyOS;
                        Hisv.UnmaintainedOS = itm.UnmaintainedOS;
                        Hisv.Name = itm.Name;
                        Hisv.Role = itm.Role;
                        Hisv.Date = itm.Date;
                        Hisv.Remarks = itm.Remarks;
                        Hisv.ChartType = itm.ChartType;
                        HDetails.Historyvisualacuitypaediatric.Add(Hisv);

                    }

                }

            }



            return HDetails;
        }
        public dynamic GetHistorypgpDetails(string UIN, string Time)
        {
            var HDetails = new RefractionMasterss();
            HDetails.Historypgp = new List<Historypgp>();

            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var pgp = WYNKContext.PGP.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);
            var TID = WYNKContext.Refraction.Where(u => u.UIN == UIN).GroupBy(x => x.RegistrationTranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qryAdmin = from va in pgp.Where(u => u.RegistrationTranID == rid)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID

                               select new
                               {

                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   Descriptionn = va.Description,
                                   Ocularr = va.Ocular,
                                   DistSphh = va.DistSph,
                                   NearCyll = va.NearCyl,
                                   PinAxiss = va.PinAxis,
                                   Addd = va.Add,
                                   Detailss = va.Details,
                                   createdby = va.CreatedBy,

                               };
                var qry = from va in WYNKContext.PGP.Where(u => u.RegistrationTranID == rid)
                          join us in user on
                          va.CreatedBy equals us.Userid
                          join doc in docmas on
                          us.Username equals doc.EmailID
                          join usr in userrole on
                          us.Userid equals usr.UserID

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              Descriptionn = va.Description,
                              Ocularr = va.Ocular,
                              DistSphh = va.DistSph,
                              NearCyll = va.NearCyl,
                              PinAxiss = va.PinAxis,
                              Addd = va.Add,
                              Detailss = va.Details,
                              createdby = va.CreatedBy,
                          };

                var grpres = qry.Count() > 0 ? qry.ToList() : qryAdmin.ToList();

                if (grpres.Count != 0)
                {
                    HDetails.Historypgp1 = (from res in grpres.GroupBy(x => x.createdby)
                                            select new Historypgp1
                                            {
                                                Description = res.Select(x => x.Descriptionn).LastOrDefault(),
                                                DistSph = res.Where(x => x.Ocularr == "OD").Select(x => x.DistSphh).LastOrDefault(),
                                                NearCyl = res.Where(x => x.Ocularr == "OD").Select(x => x.NearCyll).LastOrDefault(),
                                                PinAxis = res.Where(x => x.Ocularr == "OD").Select(x => x.PinAxiss).LastOrDefault(),
                                                Add = res.Where(x => x.Ocularr == "OD").Select(x => x.Addd).LastOrDefault(),
                                                DistSph1 = res.Where(x => x.Ocularr == "OS").Select(x => x.DistSphh).LastOrDefault(),
                                                NearCyl1 = res.Where(x => x.Ocularr == "OS").Select(x => x.NearCyll).LastOrDefault(),
                                                PinAxis1 = res.Where(x => x.Ocularr == "OS").Select(x => x.PinAxiss).LastOrDefault(),
                                                Add1 = res.Where(x => x.Ocularr == "OS").Select(x => x.Addd).LastOrDefault(),
                                                Name = res.Select(x => x.Name).LastOrDefault(),
                                                Role = res.Select(x => x.Role).LastOrDefault(),
                                                Date = res.Select(x => x.VDate).LastOrDefault(),
                                                Details = res.Select(x => x.Detailss).LastOrDefault(),
                                            }).ToList();

                    foreach (var itm in HDetails.Historypgp1.ToList())
                    {
                        var Hisp = new Historypgp();

                        Hisp.Description = itm.Description;
                        Hisp.DistSph = itm.DistSph;
                        Hisp.NearCyl = itm.NearCyl;
                        Hisp.PinAxis = itm.PinAxis;
                        Hisp.Add = itm.Add;
                        Hisp.DistSph1 = itm.DistSph1;
                        Hisp.NearCyl1 = itm.NearCyl1;
                        Hisp.PinAxis1 = itm.PinAxis1;
                        Hisp.Add1 = itm.Add1;
                        Hisp.Details = itm.Details;
                        Hisp.Name = itm.Name;
                        Hisp.Role = itm.Role;
                        Hisp.Date = itm.Date;
                        HDetails.Historypgp.Add(Hisp);

                    }
                }




            }

            return HDetails;
        }
        public dynamic GetHistorycvDetails(string UIN, string Time)
        {
            var HDetails = new RefractionMasterss();
            HDetails.Historycv = new List<Historycv>();

            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var cv = WYNKContext.ColorVision.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);
            var TID = WYNKContext.Refraction.Where(u => u.UIN == UIN).GroupBy(x => x.RegistrationTranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qryAdmin = from va in cv.Where(u => u.RegistrationTranID == rid)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID

                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   Descriptionn = va.Description,
                                   Ocularr = va.Ocular,
                                   cvnormal = va.CV_normal,
                                   cvdefective = va.CV_defective,
                                   desc = va.Desc_Text,
                                   createdby = va.CreatedBy,

                               };


                var qry = from va in WYNKContext.ColorVision.Where(u => u.RegistrationTranID == rid)
                          join us in user on
                          va.CreatedBy equals us.Userid
                          join doc in docmas on
                          us.Username equals doc.EmailID
                          join usr in userrole on
                          us.Userid equals usr.UserID

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              Descriptionn = va.Description,
                              Ocularr = va.Ocular,
                              cvnormal = va.CV_normal,
                              cvdefective = va.CV_defective,
                              desc = va.Desc_Text,
                              createdby = va.CreatedBy,

                          };

                var grpres = qry.Count() > 0 ? qry.ToList() : qryAdmin.ToList();

                if (grpres.Count != 0)
                {
                    HDetails.Historycv1 = (from res in grpres.GroupBy(x => x.createdby)
                                           select new Historycv1
                                           {
                                               Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).LastOrDefault(),
                                               Name = res.Select(x => x.Name).LastOrDefault(),
                                               Role = res.Select(x => x.Role).LastOrDefault(),
                                               Date = res.Where(x => x.Ocularr == "OD").Select(x => x.VDate).LastOrDefault(),
                                               CV_normal = res.Where(x => x.Ocularr == "OD").Select(x => x.cvnormal).LastOrDefault(),
                                               CV_defective = res.Where(x => x.Ocularr == "OD").Select(x => x.cvdefective).LastOrDefault(),
                                               Desc_Text = res.Where(x => x.Ocularr == "OD").Select(x => x.desc).LastOrDefault(),
                                               CV_normal1 = res.Where(x => x.Ocularr == "OS").Select(x => x.cvnormal).LastOrDefault(),
                                               CV_defective1 = res.Where(x => x.Ocularr == "OS").Select(x => x.cvdefective).LastOrDefault(),
                                               Desc_Text1 = res.Where(x => x.Ocularr == "OS").Select(x => x.desc).LastOrDefault(),
                                           }).ToList();

                    foreach (var itm in HDetails.Historycv1.ToList())
                    {
                        var Hiscv = new Historycv();

                        Hiscv.Description = itm.Description;
                        Hiscv.CV_normal = itm.CV_normal;
                        Hiscv.CV_defective = itm.CV_defective;
                        Hiscv.Desc_Text = itm.Desc_Text;
                        Hiscv.CV_normal1 = itm.CV_normal1;
                        Hiscv.CV_defective1 = itm.CV_defective1;
                        Hiscv.Desc_Text1 = itm.Desc_Text1;
                        Hiscv.Name = itm.Name;
                        Hiscv.Role = itm.Role;
                        Hiscv.Date = itm.Date;
                        HDetails.Historycv.Add(Hiscv);

                    }

                }

            }

            return HDetails;
        }
        public dynamic GetHistoryrefraDetails(string UIN, string Time)
        {
            var HDetails = new RefractionMasterss();
            HDetails.Historyrefraction = new List<Historyrefraction>();

            var user = CMPSContext.Users.ToList();
            var docmas = CMPSContext.DoctorMaster.ToList();
            var userrole = CMPSContext.UserVsRole.ToList();
            var onelinemaster = CMPSContext.OneLineMaster.ToList();
            var Rec = WYNKContext.REFRACTIONEXT.ToList();
            TimeSpan ts = TimeSpan.Parse(Time);
            var TID = WYNKContext.Refraction.Where(u => u.UIN == UIN).GroupBy(x => x.RegistrationTranID).ToList();

            foreach (var i in TID.ToList())
            {
                var rid = i.Key;

                var qryAdmin = from va in Rec.Where(u => u.RegistrationTranID == rid)
                               join use in user on
                               va.CreatedBy equals use.Userid
                               join userole in userrole on
                                use.Userid equals userole.UserID

                               select new
                               {
                                   VDate = va.Date.Add(ts),
                                   Name = use.Username,
                                   Role = userole.RoleDescription,
                                   Descriptionn = va.Description,
                                   Ocularr = va.Ocular,
                                   createdby = va.CreatedBy,
                                   disp = va.DistSph,
                                   near = va.NearCyl,
                                   pin = va.PinAxis,
                                   typee = va.Type,
                                   SubCategory = va.SubCategory != null ? onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                               };

                var qry = from va in WYNKContext.REFRACTIONEXT.Where(u => u.RegistrationTranID == rid)
                          join us in user on
                          va.CreatedBy equals us.Userid
                          join doc in docmas on
                          us.Username equals doc.EmailID
                          join usr in userrole on
                          us.Userid equals usr.UserID

                          select new
                          {
                              VDate = va.Date.Add(ts),
                              Name = doc.Firstname + " " + doc.MiddleName + " " + doc.LastName,
                              Role = usr.RoleDescription,
                              Descriptionn = va.Description,
                              Ocularr = va.Ocular,
                              createdby = va.CreatedBy,
                              disp = va.DistSph,
                              near = va.NearCyl,
                              pin = va.PinAxis,
                              typee = va.Type,
                              SubCategory = va.SubCategory != null ? onelinemaster.Where(x => x.OLMID == va.SubCategory).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                          };

                var grpres = qry.Count() > 0 ? qry.ToList() : qryAdmin.ToList();

                if (grpres.Count != 0)
                {
                    HDetails.Historyrefraction1 = (from res in grpres.GroupBy(item => new { item.SubCategory, item.createdby })
                                                   select new Historyrefraction1
                                                   {
                                                       Description = res.Where(x => x.Ocularr == "OD").Select(x => x.Descriptionn).LastOrDefault(),
                                                       Name = res.Select(x => x.Name).LastOrDefault(),
                                                       Role = res.Select(x => x.Role).LastOrDefault(),
                                                       Date = res.Where(x => x.Ocularr == "OD").Select(x => x.VDate).LastOrDefault(),
                                                       SubCategory = res.Where(x => x.Ocularr == "OD").Select(x => x.SubCategory).LastOrDefault(),
                                                       SubCatgry = res.Where(x => x.Ocularr == "OS").Select(x => x.SubCategory).LastOrDefault(),
                                                       DistSph = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).LastOrDefault()).Select(x => x.disp).LastOrDefault(),
                                                       NearCyl = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).LastOrDefault()).Select(x => x.near).LastOrDefault(),
                                                       PinAxis = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).LastOrDefault()).Select(x => x.pin).LastOrDefault(),
                                                       DistSph1 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).LastOrDefault()).Select(x => x.disp).LastOrDefault(),
                                                       NearCyl1 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).LastOrDefault()).Select(x => x.near).LastOrDefault(),
                                                       PinAxis1 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Wet").Select(s => s.OLMID).LastOrDefault()).Select(x => x.pin).LastOrDefault(),
                                                       DistSph2 = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).LastOrDefault()).Select(x => x.disp).LastOrDefault(),
                                                       NearCyl2 = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).LastOrDefault()).Select(x => x.near).LastOrDefault(),
                                                       PinAxis2 = res.Where(x => x.Ocularr == "OD" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).LastOrDefault()).Select(x => x.pin).LastOrDefault(),
                                                       DistSph3 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).LastOrDefault()).Select(x => x.disp).LastOrDefault(),
                                                       NearCyl3 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).LastOrDefault()).Select(x => x.near).LastOrDefault(),
                                                       PinAxis3 = res.Where(x => x.Ocularr == "OS" && x.typee == onelinemaster.Where(z => z.ParentDescription == "Dry").Select(s => s.OLMID).LastOrDefault()).Select(x => x.pin).LastOrDefault(),
                                                   }).ToList();

                    foreach (var itm in HDetails.Historyrefraction1.ToList())
                    {
                        var Hisref = new Historyrefraction();

                        Hisref.Description = itm.Description;
                        Hisref.SubCategory = itm.SubCategory;
                        Hisref.SubCatgry = itm.SubCatgry;
                        Hisref.DistSph = itm.DistSph;
                        Hisref.NearCyl = itm.NearCyl;
                        Hisref.PinAxis = itm.PinAxis;
                        Hisref.DistSph1 = itm.DistSph1;
                        Hisref.NearCyl1 = itm.NearCyl1;
                        Hisref.PinAxis1 = itm.PinAxis1;
                        Hisref.DistSph2 = itm.DistSph2;
                        Hisref.NearCyl2 = itm.NearCyl2;
                        Hisref.PinAxis2 = itm.PinAxis2;
                        Hisref.DistSph3 = itm.DistSph3;
                        Hisref.NearCyl3 = itm.NearCyl3;
                        Hisref.PinAxis3 = itm.PinAxis3;
                        Hisref.Name = itm.Name;
                        Hisref.Role = itm.Role;
                        Hisref.Date = itm.Date;
                        HDetails.Historyrefraction.Add(Hisref);

                    }

                }




            }

            return HDetails;
        }
        public dynamic Insertcategory(RefractionMasterss Addcategory)
        {


            var refractionDetails = new RefractionMasterss();
            refractionDetails.Refracion = new List<Refraction>();
            refractionDetails.VISUALACUITY = new List<VISUALACUITYMas>();

            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = Addcategory.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "category").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "CAT";
            onelinemaster.CreatedBy = Addcategory.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractioncategory()
        {
            var getData = new RefractionMasterss();

            getData.categoryhis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "CAT" && x.IsDeleted == false)
                                   select new categoryhis
                                   {
                                       ID = details.OLMID,
                                       Description = details.ParentDescription,
                                       IsActive = details.IsActive == true ? "Active" : "InActive",
                                       Active = details.IsActive,
                                   }).ToList();

            return getData;
        }
        public dynamic updatecategory(RefractionMasterss Upcategory, int ID)
        {
            var refractionDetails = new RefractionMasterss();
            refractionDetails.Refracion = new List<Refraction>();
            refractionDetails.VISUALACUITY = new List<VISUALACUITYMas>();
            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).FirstOrDefault();

            onelinemaster.ParentDescription = Upcategory.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = Upcategory.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = Upcategory.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic Deletecategory(int ID)
        {
            var refractionDetails = new RefractionMasterss();
            refractionDetails.Refracion = new List<Refraction>();
            refractionDetails.VISUALACUITY = new List<VISUALACUITYMas>();
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == ID).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }
        public dynamic InsertDistance(RefractionMasterss Adddistance)
        {
            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = Adddistance.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "V/A Distancevision").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "ADV";
            onelinemaster.CreatedBy = Adddistance.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractiondistance()
        {
            var getData = new RefractionMasterss();

            getData.distancehis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "ADV" && x.IsDeleted == false).OrderBy(x => x.Amount)
                                   select new distancehis
                                   {
                                       ID = details.OLMID,
                                       Description = details.ParentDescription,
                                       IsActive = details.IsActive == true ? "Active" : "InActive",
                                       Active = details.IsActive,
                                   }).ToList();

            return getData;
        }
        public dynamic updateDistance(RefractionMasterss Updistance, int IDD)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDD).FirstOrDefault();

            onelinemaster.ParentDescription = Updistance.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = Updistance.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = Updistance.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic DeleteDistance(int IDD)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDD).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }
        public dynamic InsertNear(RefractionMasterss Addnear)
        {
            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = Addnear.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Nearvision").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "NV";
            onelinemaster.CreatedBy = Addnear.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractionnear()
        {
            var getData = new RefractionMasterss();

            getData.distancenear = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "NV" && x.IsDeleted == false).OrderBy(x => x.OLMID)
                                    select new distancenear
                                    {
                                        ID = details.OLMID,
                                        Description = details.ParentDescription,
                                        IsActive = details.IsActive == true ? "Active" : "InActive",
                                        Active = details.IsActive,
                                    }).ToList();

            return getData;
        }
        public dynamic updateNear(RefractionMasterss UpNear, int IDN)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDN).FirstOrDefault();

            onelinemaster.ParentDescription = UpNear.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = UpNear.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = UpNear.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic DeleteNear(int IDN)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDN).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }
        public dynamic Insertpinhole(RefractionMasterss AddPinhole)
        {
            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = AddPinhole.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "V/A Distancevision").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "ADV";
            onelinemaster.CreatedBy = AddPinhole.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractionpinhole()
        {
            var getData = new RefractionMasterss();

            getData.distancepinhole = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "ADV" && x.IsDeleted == false).OrderBy(x => x.Amount)
                                       select new distancepinhole
                                       {
                                           ID = details.OLMID,
                                           Description = details.ParentDescription,
                                           IsActive = details.IsActive == true ? "Active" : "InActive",
                                           Active = details.IsActive,
                                       }).ToList();

            return getData;
        }
        public dynamic updatepinhole(RefractionMasterss UpPinhole, int IDP)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDP).FirstOrDefault();

            onelinemaster.ParentDescription = UpPinhole.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = UpPinhole.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = UpPinhole.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic DeletePinhole(int IDP)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDP).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }
        public dynamic Insertinstrutment(RefractionMasterss Addinstrutment)
        {
            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = Addinstrutment.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Instrument").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "IN";
            onelinemaster.CreatedBy = Addinstrutment.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractioninstrutment()
        {
            var getData = new RefractionMasterss();

            getData.instrutmenthis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "IN" && x.IsDeleted == false)
                                      select new instrutmenthis
                                      {
                                          ID = details.OLMID,
                                          Description = details.ParentDescription,
                                          IsActive = details.IsActive == true ? "Active" : "InActive",
                                          Active = details.IsActive,
                                      }).ToList();

            return getData;
        }
        public dynamic updateinstrutment(RefractionMasterss Upinstrutment, int IDI)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDI).FirstOrDefault();

            onelinemaster.ParentDescription = Upinstrutment.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = Upinstrutment.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = Upinstrutment.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic Deleteinstrutment(int IDI)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDI).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }
        public dynamic Insertstrabismus(RefractionMasterss Addstrabismus)
        {
            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = Addstrabismus.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "Squinttype").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "SQTYPE";
            onelinemaster.CreatedBy = Addstrabismus.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractionstrabismus()
        {
            var getData = new RefractionMasterss();

            getData.strabismushis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "SQTYPE" && x.IsDeleted == false)
                                     select new strabismushis
                                     {
                                         ID = details.OLMID,
                                         Description = details.ParentDescription,
                                         IsActive = details.IsActive == true ? "Active" : "InActive",
                                         Active = details.IsActive,
                                     }).ToList();

            return getData;
        }
        public dynamic updatestrabismus(RefractionMasterss Upstrabismus, int IDstrabismus)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDstrabismus).FirstOrDefault();

            onelinemaster.ParentDescription = Upstrabismus.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = Upstrabismus.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = Upstrabismus.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic Deletestrabismus(int IDstrabismus)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDstrabismus).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }

        public dynamic Insertcharttype(RefractionMasterss Addcharttype)
        {
            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = Addcharttype.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "ChartType").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "CT";
            onelinemaster.CreatedBy = Addcharttype.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,

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
        public dynamic refractioncharttype()
        {
            var getData = new RefractionMasterss();

            getData.charttypehis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "CT" && x.IsDeleted == false).OrderBy(x => x.Amount)
                                    select new charttypehis
                                    {
                                        ID = details.OLMID,
                                        Description = details.ParentDescription,
                                        IsActive = details.IsActive == true ? "Active" : "InActive",
                                        Active = details.IsActive,
                                    }).ToList();

            return getData;
        }
        public dynamic updatecharttype(RefractionMasterss upcharttype, int ICT)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == ICT).FirstOrDefault();

            onelinemaster.ParentDescription = upcharttype.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = upcharttype.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = upcharttype.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
                if (CMPSContext.SaveChanges() > 0)
                    return new
                    {
                        Success = true,
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
        public dynamic Deletecharttype(int ICT)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == ICT).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = CommonRepository.saved
                    };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Success = false,
                Message = CommonRepository.Missing
            };

        }



    }
}