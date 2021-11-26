using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class RoleVsAccessRepository : RepositoryBase<Rolevsaccesscontrol>, IRoleVsAccessRespository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public RoleVsAccessRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public dynamic GetAccessPrivilegesDetails(int RefID)


        {
            var AccessPrivilegesDetails = new Rolevsaccesscontrol();

            var Referecnceid = CMPSContext.UserVsRole.Where(x => x.UserRoleID == RefID).Select(x => x.UserName).FirstOrDefault();


            AccessPrivilegesDetails.ModuleNameGet = (from REF in CMPSContext.UserVsRole.Where(u => u.UserName == Referecnceid)

                                                     select new ModuleNameGet
                                                     {

                                                         RoleDesc = REF.RoleDescription,
                                                         UserName = REF.UserName,

                                                     }
            ).ToList();

            return AccessPrivilegesDetails;
        }
        public dynamic GetAccessModule(int CMPID)
        {
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();

            AccessPrivilegesDetailss.ModuleMasterget = (from REF in CMPSContext.ModuleMaster.Where(x => x.ParentModuleid != 0 && x.ModuleType == "Form")
                                                        select new ModuleMasterget
                                                        {
                                                            Desc = REF.ModuleDescription,
                                                            Code = REF.ModuleID,
                                                        }
            ).ToList();

            return AccessPrivilegesDetailss;
        }
        public dynamic GetAccessModulebasedonuser(string Role, int CMPID, string MViewaccess)
        {
            var AccessPrivilegesDetailss = new RegistrationMasterViewModel();
            List<ModuleMastergetuseraccess> moddetails = new List<ModuleMastergetuseraccess>();
            List<NonModuleMastergetuseraccess> nonmoddetails = new List<NonModuleMastergetuseraccess>();
            List<ModuleMasterdetails> NewMasterdetails = new List<ModuleMasterdetails>();

            var Totalmodules = CMPSContext.ModuleMaster.Where(x => x.ParentModuleid != 0 && x.ModuleType == "Form").ToList();
            var ModuleMaster = CMPSContext.ModuleMaster.OrderBy(x => x.Menuorder).Where(y => y.IsActive == true).ToList();


            var rolevalue = Convert.ToInt32(Role);
            if (MViewaccess == "all")
            {
                var fullacess = (from gf in ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ModuleDescription != "Menu-Workflow")
                                 select new DataOriginalModule()
                                 {
                                     MainDescription = gf.ModuleDescription,
                                     DataOriginalsubModule = (from s in ModuleMaster.Where(x => x.ParentModuleid == gf.ModuleID)
                                                              select new DataOriginalsubModule()
                                                              {
                                                                  subDescription = s.ModuleDescription,
                                                                  Moduletypes = s.ModuleType,
                                                                  DataOriginalformsModule = (from a in ModuleMaster.Where(x => x.ParentModuleid == s.ModuleID)
                                                                                             select new DataOriginalformsModule()
                                                                                             {
                                                                                                 formDescription = a.ModuleDescription,
                                                                                                 Moduletypes = a.ModuleType,
                                                                                                 All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                 Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                 Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                 Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                 Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                 Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                             }).OrderBy(x =>x.formDescription).ToList(),
                                                              }).OrderBy(x => x.subDescription).ToList(),
                                 }).OrderBy(x => x.MainDescription).ToList();
                AccessPrivilegesDetailss.DataOriginalModule.AddRange(fullacess);


                var parentmoduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ModuleDescription == "Menu-Workflow").Select(x => x.ModuleID).FirstOrDefault();
                AccessPrivilegesDetailss.Workflowaccerss = new List<Workflowaccerss>();
                var Moduledetails = ModuleMaster.Where(x => x.ParentModuleid == parentmoduleid).ToList();
                foreach (var item in Moduledetails)
                {
                    var Modid = item.ModuleID;
                    var roldedata = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == Modid && x.RoleID == rolevalue && x.CmpID == CMPID).FirstOrDefault();
                    if (roldedata != null)
                    {
                        if (roldedata.Add != false || roldedata.Edit != false || roldedata.Delete != false || roldedata.Print != false || roldedata.Query != false)
                        {
                            var data = new Workflowaccerss();
                            data.Add = roldedata.Add;
                            data.All = roldedata.All;
                            data.Edit = roldedata.Edit;
                            data.Delete = roldedata.Delete;
                            data.Export = roldedata.Print;
                            data.Update = roldedata.Query;
                            data.formDescription = item.ModuleDescription;
                            data.Moduletypes = item.ModuleType;
                            AccessPrivilegesDetailss.Workflowaccerss.Add(data);
                        }
                        else
                        {
                            var data = new Workflowaccerss();
                            data.Add = false;
                            data.All = false;
                            data.Edit = false;
                            data.Delete = false;
                            data.Export = false;
                            data.Update = false;
                            data.formDescription = item.ModuleDescription;
                            data.Moduletypes = item.ModuleType;
                            AccessPrivilegesDetailss.Workflowaccerss.Add(data);
                        }

                    }
                    else
                    {
                        var data = new Workflowaccerss();
                        data.Add = false;
                        data.All = false;
                        data.Edit = false;
                        data.Delete = false;
                        data.Export = false;
                        data.Update = false;
                        data.formDescription = item.ModuleDescription;
                        data.Moduletypes = item.ModuleType;
                        AccessPrivilegesDetailss.Workflowaccerss.Add(data);
                    }
                }


                AccessPrivilegesDetailss.WorkflowDataOriginalmainModule = (from gf in ModuleMaster.Where(x => x.ModuleType == "Findings-Main")
                                 select new WorkflowDataOriginalmainModule()
                                 {
                                     MainDescription = gf.ModuleDescription,
                                     workflowDataOriginalsubModule = (from s in ModuleMaster.Where(x => x.ParentModuleid == gf.ModuleID)
                                                              select new workflowDataOriginalsubModule()
                                                              {
                                                                  subDescription = s.ModuleDescription,
                                                                  Moduletypes = s.ModuleType,
                                                                  Parentmoduledescription = ModuleMaster.Where(x => x.ModuleID == s.ModuleID).Select(x => x.Parentmoduledescription).FirstOrDefault(),
                                                                  All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                  Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                  Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                  Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                  Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                  Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                  TransactionID = ModuleMaster.Where(x => x.ModuleID == s.ModuleID).Select(x => x.TransactionTypeID).FirstOrDefault(),                                                                  
                                                                  workflowFgformsModule = (from a in ModuleMaster.Where(x => x.ParentModuleid == s.ModuleID)
                                                                                             select new workflowFgformsModule()
                                                                                             {
                                                                                                 formDescription = a.ModuleDescription,
                                                                                                 Moduletypes = a.ModuleType,
                                                                                                 All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                 Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                 Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                 Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                 Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                 Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                             }).OrderBy(x => x.formDescription).ToList(),
                                                              }).OrderBy(x => x.subDescription).ToList(),
                                 }).OrderBy(x => x.MainDescription).ToList();                

            }
            else if (MViewaccess == "granted")
            {
                var Parentmainname = CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ModuleDescription == "Menu-Workflow").Select(x => x.ModuleID).FirstOrDefault();

                var rolemoduedata = (from rm in CMPSContext.RoleVsModule.Where(y => y.RoleID == rolevalue && y.CmpID == CMPID && y.ParentModuleID != Parentmainname
                                     && (y.Add == true || y.Edit == true || y.Print == true || y.Query == true || y.All == true))
                                     group new { rm } by new { rm.ModuleID } into g
                                     select new
                                     {
                                         Modid = g.FirstOrDefault().rm.ModuleID,
                                         parentid = g.FirstOrDefault().rm.ParentModuleID,
                                     }).ToList();

                var formresdata = (from v in ModuleMaster
                                   join b in rolemoduedata on v.ModuleID equals b.Modid

                                   select new
                                   {
                                       formdesc = v.ModuleDescription,
                                       Modid = v.ModuleID,
                                       parentid = v.ParentModuleid,
                                       parentsubid = b.parentid,
                                       Moduletype = v.ModuleType,
                                   }).ToList();

                var subresdata = (from g in ModuleMaster
                                  join l in formresdata on g.ModuleID equals l.parentid
                                  group new { g, l } by new { g.ModuleDescription } into p
                                  select new
                                  {
                                      subdesc = p.FirstOrDefault().g.ModuleDescription,
                                      modid = p.FirstOrDefault().g.ModuleID,
                                      parentmoid = p.FirstOrDefault().l.parentsubid,
                                      Moduletype = p.FirstOrDefault().g.ModuleType,
                                  }).ToList();

                var mainres = (from g in ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ParentModuleid == 0  && x.ModuleDescription != "Menu-Workflow")
                               join i in subresdata on g.ModuleID equals i.parentmoid
                               group new { g } by new { g.ModuleDescription } into r
                               select new
                               {
                                   Maindec = r.FirstOrDefault().g.ModuleDescription,
                                   modid = r.FirstOrDefault().g.ModuleID,

                               }).ToList();

                AccessPrivilegesDetailss.DataOriginalModule = new List<DataOriginalModule>();
                var fullacess = (from gf in mainres
                                 select new DataOriginalModule()
                                 {
                                     MainDescription = gf.Maindec,
                                     DataOriginalsubModule = (from s in subresdata.Where(x => x.parentmoid == gf.modid)
                                                              select new DataOriginalsubModule()
                                                              {
                                                                  subDescription = s.subdesc,
                                                                  Moduletypes = s.Moduletype,
                                                                  DataOriginalformsModule = (from a in formresdata.Where(x => x.parentid == s.modid)
                                                                                             select new DataOriginalformsModule()
                                                                                             {
                                                                                                 formDescription = a.formdesc,
                                                                                                 Moduletypes = a.Moduletype,
                                                                                                 Parentmoduledescription = ModuleMaster.Where(x => x.ModuleID == a.Modid).Select(x => x.Parentmoduledescription).FirstOrDefault(),
                                                                                                 All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                 Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                 Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                 Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                 Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                 Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.Modid && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                                 TransactionID = ModuleMaster.Where(x => x.ModuleID == a.Modid).Select(x => x.TransactionTypeID).FirstOrDefault(),
                                                                                             }).OrderBy(x => x.formDescription).ToList(),
                                                              }).OrderBy(x => x.subDescription).ToList(),
                                 }).OrderBy(x => x.MainDescription).ToList();

                AccessPrivilegesDetailss.DataOriginalModule.AddRange(fullacess);
                var parentmoduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ModuleDescription == "Menu-Workflow").Select(x => x.ModuleID).FirstOrDefault();
                AccessPrivilegesDetailss.Workflowaccerss = new List<Workflowaccerss>();
                var WorkFullmodules = CMPSContext.RoleVsModule.Where(x => x.CmpID == CMPID && x.RoleID == rolevalue
                && x.ParentModuleID == 0 && (x.Add == true || x.Edit == true || x.Print == true || x.Query == true || x.All == true)).Select(x => x.ModuleID).ToList();
                if (WorkFullmodules.Count() != 0)
                {
                    foreach (var item in WorkFullmodules)
                    {
                        var Roledata = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item).FirstOrDefault();

                        var data = new Workflowaccerss();
                        data.Add = Roledata.Add;
                        data.All = Roledata.All;
                        data.Edit = Roledata.Edit;
                        data.Delete = Roledata.Delete;
                        data.Export = Roledata.Print;
                        data.Update = Roledata.Query;
                        data.Moduletypes= CMPSContext.ModuleMaster.Where(x => x.ModuleID == item).Select(x => x.ModuleType).FirstOrDefault();
                        data.formDescription = CMPSContext.ModuleMaster.Where(x => x.ModuleID == item).Select(x => x.ModuleDescription).FirstOrDefault();
                        AccessPrivilegesDetailss.Workflowaccerss.Add(data);
                    }
                }


                AccessPrivilegesDetailss.WorkflowDataOriginalmainModule = (from gf in ModuleMaster.Where(x => x.ModuleType == "Findings-Main")
                                                                           select new WorkflowDataOriginalmainModule()
                                                                           {
                                                                               MainDescription = gf.ModuleDescription,
                                                                               workflowDataOriginalsubModule = (from s in ModuleMaster.Where(x => x.ParentModuleid == gf.ModuleID)
                                                                                                                select new workflowDataOriginalsubModule()
                                                                                                                {
                                                                                                                    subDescription = s.ModuleDescription,
                                                                                                                    Moduletypes = s.ModuleType,
                                                                                                                    Parentmoduledescription = ModuleMaster.Where(x => x.ModuleID == s.ModuleID).Select(x => x.Parentmoduledescription).FirstOrDefault(),
                                                                                                                    All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                                    Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                                    Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                                    Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                                    Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                                    Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                                                    TransactionID = ModuleMaster.Where(x => x.ModuleID == s.ModuleID).Select(x => x.TransactionTypeID).FirstOrDefault(),
                                                                                                                    workflowFgformsModule = (from a in ModuleMaster.Where(x => x.ParentModuleid == s.ModuleID)
                                                                                                                                             select new workflowFgformsModule()
                                                                                                                                             {
                                                                                                                                                 formDescription = a.ModuleDescription,
                                                                                                                                                 Moduletypes = a.ModuleType,
                                                                                                                                                 All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                                                                 Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                                                                 Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                                                                 Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                                                                 Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                                                                 Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                                                                             }).OrderBy(x => x.formDescription).ToList(),
                                                                                                                }).OrderBy(x => x.subDescription).ToList(),
                                                                           }).OrderBy(x => x.MainDescription).ToList();

            }
            else
            {

                var Fullmodules = CMPSContext.RoleVsModule.Where(x => x.CmpID == CMPID && x.RoleID == rolevalue && x.All != false).Select(x => x.ModuleID).ToList();
                var rejectedmodules = ModuleMaster.Where(x => Fullmodules.Contains(x.ModuleID));
                var filteredList = ModuleMaster.Except(rejectedmodules);
                var fullacesss = (from gf in filteredList.Where(x => x.ParentModuleid == 0 && x.ModuleDescription != "Menu-Workflow")
                                  where filteredList.Where(x => x.ParentModuleid == gf.ModuleID).ToList().Count() > 0
                                  select new DataOriginalModule()
                                  {
                                      MainDescription = gf.ModuleDescription,
                                      DataOriginalsubModule = (from s in filteredList.Where(x => x.ParentModuleid == gf.ModuleID)
                                                               where filteredList.Where(x => x.ParentModuleid == s.ModuleID).ToList().Count() > 0
                                                               select new DataOriginalsubModule()
                                                               {
                                                                   subDescription = s.ModuleDescription,
                                                                   Moduletypes = s.ModuleType,
                                                                   DataOriginalformsModule = (from a in filteredList.Where(x => x.ParentModuleid == s.ModuleID)
                                                                                              select new DataOriginalformsModule()
                                                                                              {
                                                                                                  formDescription = a.ModuleDescription,
                                                                                                  Moduletypes = a.ModuleType,
                                                                                                  All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                  Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                  Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                  Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                  Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                  Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),

                                                                                              }).OrderBy(x => x.formDescription).ToList(),
                                                               }).OrderBy(x => x.subDescription).ToList(),
                                  }).OrderBy(x => x.MainDescription).ToList();
                AccessPrivilegesDetailss.DataOriginalModule.AddRange(fullacesss);

                var parentmoduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.ModuleDescription == "Menu-Workflow").Select(x => x.ModuleID).FirstOrDefault();
                AccessPrivilegesDetailss.Workflowaccerss = new List<Workflowaccerss>();
                var Moduledetails = ModuleMaster.Where(x => x.ParentModuleid == parentmoduleid).ToList();
                foreach (var item in Moduledetails)
                {
                    var Modid = item.ModuleID;
                    var roldedata = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == Modid && x.RoleID == rolevalue && x.CmpID == CMPID).FirstOrDefault();
                    if (roldedata != null)
                    {
                        if (roldedata.All == false)
                        {
                            var data = new Workflowaccerss();
                            data.Add = roldedata.Add;
                            data.All = roldedata.All;
                            data.Edit = roldedata.Edit;
                            data.Delete = roldedata.Delete;
                            data.Export = roldedata.Print;
                            data.Update = roldedata.Query;
                            data.Moduletypes = item.ModuleType;
                            data.formDescription = item.ModuleDescription;
                            AccessPrivilegesDetailss.Workflowaccerss.Add(data);
                        }

                    }
                    else
                    {
                        var data = new Workflowaccerss();
                        data.Add = false;
                        data.All = false;
                        data.Edit = false;
                        data.Delete = false;
                        data.Export = false;
                        data.Update = false;
                        data.formDescription = item.ModuleDescription;
                        data.Moduletypes = item.ModuleType;
                        AccessPrivilegesDetailss.Workflowaccerss.Add(data);
                    }
                }


                AccessPrivilegesDetailss.WorkflowDataOriginalmainModule = (from gf in ModuleMaster.Where(x => x.ModuleType == "Findings-Main")
                                                                           select new WorkflowDataOriginalmainModule()
                                                                           {
                                                                               MainDescription = gf.ModuleDescription,
                                                                               workflowDataOriginalsubModule = (from s in ModuleMaster.Where(x => x.ParentModuleid == gf.ModuleID)
                                                                                                                select new workflowDataOriginalsubModule()
                                                                                                                {
                                                                                                                    subDescription = s.ModuleDescription,
                                                                                                                    Moduletypes = s.ModuleType,
                                                                                                                    Parentmoduledescription = ModuleMaster.Where(x => x.ModuleID == s.ModuleID).Select(x => x.Parentmoduledescription).FirstOrDefault(),
                                                                                                                    All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                                    Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                                    Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                                    Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                                    Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                                    Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == s.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                                                    TransactionID = ModuleMaster.Where(x => x.ModuleID == s.ModuleID).Select(x => x.TransactionTypeID).FirstOrDefault(),
                                                                                                                    workflowFgformsModule = (from a in ModuleMaster.Where(x => x.ParentModuleid == s.ModuleID)
                                                                                                                                             select new workflowFgformsModule()
                                                                                                                                             {
                                                                                                                                                 formDescription = a.ModuleDescription,
                                                                                                                                                 Moduletypes = a.ModuleType,
                                                                                                                                                 All = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.All).FirstOrDefault(),
                                                                                                                                                 Add = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Add).FirstOrDefault(),
                                                                                                                                                 Edit = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Edit).FirstOrDefault(),
                                                                                                                                                 Update = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Query).FirstOrDefault(),
                                                                                                                                                 Export = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Print).FirstOrDefault(),
                                                                                                                                                 Delete = CMPSContext.RoleVsModule.Where(x => x.ModuleID == a.ModuleID && x.CmpID == CMPID && x.RoleID == rolevalue).Select(x => x.Delete).FirstOrDefault(),
                                                                                                                                             }).OrderBy(x => x.formDescription).ToList(),
                                                                                                                }).OrderBy(x => x.subDescription).ToList(),
                                                                           }).OrderBy(x => x.MainDescription).ToList();

            }



            return AccessPrivilegesDetailss;
        }
        public dynamic GetUserDetails(string Roleuser, string CompanyID)
        {
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();
            var Roleid = CMPSContext.Role.Where(x => x.RoleID == Convert.ToInt32(Roleuser)).Select(x => x.RoleDescription).FirstOrDefault();
            var ccid = CMPSContext.Company.Where(x => x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.ParentID).FirstOrDefault();

            if (ccid == 0)
            {
                if (Roleid == "Doctor" || Roleid == "Vision")
                {
                    var roleIID = CMPSContext.Role.Where(x => x.RoleDescription == "Doctor").Select(x => x.RoleID).FirstOrDefault();


                    var res = (from cm in CMPSContext.Users.Where(x => x.ReferenceID == roleIID)
                               join qw in CMPSContext.DoctorMaster on cm.Username equals qw.EmailID
                               select qw.DoctorID).ToList();

                    AccessPrivilegesDetailss.AccessNames = (from Dm in CMPSContext.DoctorMaster.Where(x => x.CMPID == Convert.ToInt32(CompanyID)
                                                                     && x.IsActive == true && x.RoleID == roleIID && x.RegistrationNumber != null)
                                                            select new AccessNames
                                                            {
                                                                Roleids = Dm.DoctorID,
                                                                Roledescriptions = Dm.LastName,
                                                            }).ToList();

                }
                else if (Roleid == "Employee")
                {
                    var roleIID = CMPSContext.Role.Where(x => x.RoleDescription == "Employee").Select(x => x.RoleID).FirstOrDefault();
                    var res = (from cm in CMPSContext.Users.Where(x => x.ReferenceID == roleIID)
                               join qw in CMPSContext.EmployeeCommunication on cm.Username equals qw.EmailID
                               select qw.EmpID).ToList();

                    AccessPrivilegesDetailss.AccessNames = (from ee in CMPSContext.EmployeeCommunication
                                                            join Dm in CMPSContext.Employee.Where(x => x.CMPID == Convert.ToInt32(CompanyID)
                                                            && x.IsActive == true) on ee.EmpID equals Dm.EmployeeID
                                                            select new AccessNames
                                                            {
                                                                Roleids = Dm.EmployeeID,
                                                                Roledescriptions = Dm.FirstName,
                                                            }).ToList();
                }

                else if (Roleid == "Admin")
                {
                    AccessPrivilegesDetailss.AccessNames = (from us in CMPSContext.Users.Where(x => x.ReferenceTag == "A" && x.CMPID == Convert.ToInt32(CompanyID))

                                                            select new AccessNames
                                                            {
                                                                Roleids = us.Userid,
                                                                Roledescriptions = us.Username,
                                                            }).ToList();
                }
                else if (Roleid == "Reception")
                {
                    AccessPrivilegesDetailss.AccessNames = (from us in CMPSContext.Users.Where(x => x.ReferenceTag == "R" && x.CMPID == Convert.ToInt32(CompanyID))
                                                            join EM in CMPSContext.Employee.Where(x => x.CMPID == Convert.ToInt32(CompanyID)) on us.ReferenceID equals EM.EmployeeID
                                                            select new AccessNames
                                                            {
                                                                Roleids = us.Userid,
                                                                Roledescriptions = EM.FirstName,
                                                            }).ToList();
                }
                else if (Roleid == "Optometrist")
                {
                    var roleIID = CMPSContext.Role.Where(x => x.RoleDescription == "Optometrist").Select(x => x.RoleID).FirstOrDefault();
                    var res = (from cm in CMPSContext.Users.Where(x => x.ReferenceID == roleIID)
                               join qw in CMPSContext.DoctorMaster on cm.Username equals qw.EmailID
                               select qw.DoctorID).ToList();

                    AccessPrivilegesDetailss.AccessNames = (from Dm in CMPSContext.DoctorMaster.Where(x => x.CMPID == Convert.ToInt32(CompanyID)
                                                            && x.IsActive == true && x.RoleID == roleIID)

                                                            select new AccessNames
                                                            {
                                                                Roleids = Dm.DoctorID,
                                                                Roledescriptions = Dm.LastName,
                                                            }).ToList();
                }

            }

            AccessPrivilegesDetailss.ModuleMasterget = (from REF in CMPSContext.ModuleMaster

                                                        select new ModuleMasterget
                                                        {

                                                            Desc = REF.ModuleDescription,
                                                            Code = REF.ModuleID,


                                                        }).ToList();

            return AccessPrivilegesDetailss;
        }
        public dynamic InsertAccessPrivileges(Rolevsaccesscontrol Addaccessname)

        {
            var cmpid = Convert.ToInt32(Addaccessname.CmpID);
            var rolevalue = Addaccessname.Roleid;
            var Createdrole = Addaccessname.userdoctorid;
            foreach (var item in Addaccessname.UpdateModuleMaster)
            {
                var Moduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleDescription == item.Desc && x.ModuleType== item.ModType).Select(x => x.ModuleID).FirstOrDefault();
                var existingrolelistduplicate = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.RoleID == rolevalue && x.CmpID == cmpid && x.ModuleID == Moduleid).Select(x => x.RoleModuleID).FirstOrDefault();
                if (existingrolelistduplicate == 0)
                {
                    if (item.All == true || item.Add == true || item.Edit == true || item.Delete == true || item.Export == true || item.Update == true)
                    {

                        var submoduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleID == Moduleid).Select(x => x.ParentModuleid).FirstOrDefault();
                        var parentmoduleid = CMPSContext.ModuleMaster.Where(x => x.ModuleID == submoduleid).Select(x => x.ParentModuleid).FirstOrDefault();
                        var existingrolelist = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.RoleID == rolevalue && x.CmpID == cmpid && x.ModuleID == Moduleid).Select(x => x.RoleModuleID).FirstOrDefault();
                        if (existingrolelist != 0)
                        {
                            var roledata = CMPSContext.RoleVsModule.Where(x => x.RoleModuleID == existingrolelist).FirstOrDefault();
                            if (roledata.Add != item.Add)
                            {
                                roledata.Add = item.Add;
                            }
                            if (roledata.All != item.All)
                            {
                                roledata.All = item.All;
                            }
                            if (roledata.Edit != item.Edit)
                            {
                                roledata.Edit = item.Edit;
                            }
                            if (roledata.Delete != item.Delete)
                            {
                                roledata.Delete = item.Delete;
                            }
                            if (roledata.Query != item.Update)
                            {
                                roledata.Query = item.Update;
                            }
                            if (roledata.Print != item.Export)
                            {
                                roledata.Print = item.Export;
                            }
                            CMPSContext.RoleVsModule.UpdateRange(roledata);
                            CMPSContext.SaveChanges();

                        }
                        else
                        {
                            var rolevsmodule = new RoleVsModuleaccess();
                            rolevsmodule.CmpID = cmpid;
                            rolevsmodule.ModuleID = Moduleid;
                            rolevsmodule.UserRoleID = Createdrole;
                            rolevsmodule.RoleID = rolevalue;
                            rolevsmodule.Add = item.Add;
                            rolevsmodule.Edit = item.Edit;
                            rolevsmodule.Delete = item.Delete;
                            rolevsmodule.Query = item.Update;
                            rolevsmodule.Print = item.Export;
                            rolevsmodule.All = item.All;
                            rolevsmodule.MenuOrder = 0;
                            rolevsmodule.CreatedUTC = DateTime.UtcNow;
                            rolevsmodule.CreatedBy = Createdrole;
                            rolevsmodule.UpdatedUTC = null;
                            rolevsmodule.UpdatedBy = null;
                            rolevsmodule.ParentModuleID = Convert.ToInt32(parentmoduleid);
                            CMPSContext.RoleVsModule.AddRange(rolevsmodule);
                            CMPSContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    var roledata = CMPSContext.RoleVsModule.Where(x => x.RoleModuleID == existingrolelistduplicate).FirstOrDefault();
                    if (roledata.Add != item.Add)
                    {
                        roledata.Add = item.Add;
                    }
                    if (roledata.All != item.All)
                    {
                        roledata.All = item.All;
                    }
                    if (roledata.Edit != item.Edit)
                    {
                        roledata.Edit = item.Edit;
                    }
                    if (roledata.Delete != item.Delete)
                    {
                        roledata.Delete = item.Delete;
                    }
                    if (roledata.Query != item.Update)
                    {
                        roledata.Query = item.Update;
                    }
                    if (roledata.Print != item.Export)
                    {
                        roledata.Print = item.Export;
                    }
                    CMPSContext.RoleVsModule.UpdateRange(roledata);
                    CMPSContext.SaveChanges();
                }
            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)

                    return new
                    {

                        Success = true,
                        Message = "Saved successfully",


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


        public dynamic GetMenuaccess(int userDoctorID, int userroleID, string CompanyID, string Description)

        {
            var AccessPrivilegesDetailss = new Rolevsaccesscontrol();

            var Cruddetails = new List<CrudOperations>();
            var Moduleid = CMPSContext.ModuleMaster.Where(x => x.Parentmoduledescription == Description).Select(x => x.ModuleID).FirstOrDefault();
            var roleid = CMPSContext.UserVsRole.Where(x => x.UserID == userroleID).Select(x => x.UserRoleID).FirstOrDefault();
            var USERID = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.RoleID == userDoctorID && x.UserRoleID == roleid && x.CmpID == Convert.ToInt32(CompanyID)).ToList();


            if (USERID != null || USERID.Count != 0)
            {
                foreach (var item in USERID)
                {
                    var useradd = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item.ModuleID && x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.Add).FirstOrDefault();
                    var userEdit = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item.ModuleID && x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.Edit).FirstOrDefault();
                    var userUpdate = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item.ModuleID && x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.Query).FirstOrDefault();
                    var userDelete = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item.ModuleID && x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.Delete).FirstOrDefault();
                    var userPrint = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item.ModuleID && x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.Print).FirstOrDefault();
                    var userAll = CMPSContext.RoleVsModule.OrderByDescending(x => x.CreatedUTC).Where(x => x.ModuleID == item.ModuleID && x.CmpID == Convert.ToInt32(CompanyID)).Select(x => x.All).FirstOrDefault();

                    if (useradd == true)
                    {
                        var Listt = new CrudOperations();
                        Listt.Add = Convert.ToString(useradd);
                        Cruddetails.Add(Listt);
                    }
                    else if (userEdit == true)
                    {

                    }
                    else if (userUpdate == true)
                    {

                    }
                    else if (userDelete == true)
                    {

                    }
                    else if (userPrint == true)
                    {

                    }
                    else if (userAll == true)
                    {

                    }
                }
            }
            else
            {

            }

            return AccessPrivilegesDetailss;
        }


    }
}







