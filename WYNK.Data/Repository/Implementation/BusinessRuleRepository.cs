using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
  
        class BusinessRuleRepository : RepositoryBase<BusinessRuleViewModel>, IBusinessRuleRepository
        {

            private readonly WYNKContext _Wynkcontext;
            private readonly CMPSContext _Cmpscontext;
           

            public BusinessRuleRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

            public dynamic InsertBusinessRule(BusinessRuleViewModel BusinessRule, int CmpID,int BRID)
        {
            var BusinessRuleMaster = new BusinessRule();
            var BusinessRuleTranMaster = new BusinessRuleTran();
            var ICDMaster = WYNKContext.ICDMaster.Where(x => x.IsDeleted == false && x.IsActive==true).ToList();   
            var MDescription = WYNKContext.BussinessRule.Where(x => x.ModuleDescription == BusinessRule.BusinessRule.ModuleDescription && x.CMPID == CmpID).Select(x => x.ModuleDescription).FirstOrDefault();
            var BRTID = BRID;
            if (BRID !=0) 
            {
                BRTID = BRID;

            }
           else
            {
                BRTID = WYNKContext.BussinessRule.Where(x => x.ModuleDescription == BusinessRule.BusinessRule.ModuleDescription && x.CMPID == CmpID).Select(x => x.ID).FirstOrDefault();
            }

             
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {


                    if (BusinessRule.IcdSPYarray.Count != 0)
                    {
                        foreach (var item in BusinessRule.IcdSPYarray.ToList())
                        {
                            var BusinessR = new BusinessRule();
                            BusinessR.ModuleID = BusinessRule.BusinessRule.ModuleID;
                            BusinessR.ModuleDescription = BusinessRule.BusinessRule.ModuleDescription;
                            BusinessR.BRBasedOn = BusinessRule.BusinessRule.BRBasedOn;
                            BusinessR.CMPID = BusinessRule.BusinessRule.CMPID;
                            BusinessR.WEF = BusinessRule.BusinessRule.WEF.AddDays(1);
                            BusinessR.ICDCODE = Convert.ToString(item.id);
                            BusinessR.CreatedUTC = DateTime.UtcNow;
                            BusinessR.IsActive = true;
                            BusinessR.CreatedBy = BusinessRule.BusinessRule.CreatedBy;
                            WYNKContext.BussinessRule.Add(BusinessR);
                            WYNKContext.SaveChanges();

                            var brtid1 = BusinessR.ID;

                            foreach (var item1 in BusinessRule.BusinessRT.ToList())
                            {
                                var BusinessRT = new BusinessRuleTran();
                                BusinessRT.BRID = brtid1;
                                BusinessRT.From = item1.From;
                                BusinessRT.TO = item1.TO;
                                BusinessRT.NoofDays = item1.NODays;
                                BusinessRT.Amount = item1.Amount;
                                BusinessRT.CreatedUTC = DateTime.UtcNow;
                                BusinessRT.CreatedBy = BusinessRule.BusinessRule.CreatedBy;
                                BusinessRT.IsActive = true;
                                BusinessRT.EffectiveDate = DateTime.UtcNow;
                                WYNKContext.BussinessRuleTran.Add(BusinessRT);
                                WYNKContext.SaveChanges();
                            }

                        }

                    }

                    else 
                    {
                        if (BusinessRule.BusinessRule.ModuleDescription == MDescription && BRTID != 0)
                        {
                            foreach (var item in WYNKContext.BussinessRuleTran.Where(x => x.BRID == BRTID).ToList())
                            {
                                item.IsActive = false;
                                WYNKContext.Entry(item).State = EntityState.Modified;
                                WYNKContext.SaveChanges();
                            }
                        
                            var busR = WYNKContext.BussinessRule.Where(x => x.ID == BRTID).FirstOrDefault();
                            busR.WEF = BusinessRule.BusinessRule.WEF.AddDays(1);
                            WYNKContext.Entry(busR).State = EntityState.Modified;
                            WYNKContext.SaveChanges();

                            foreach (var item in BusinessRule.BusinessRT.ToList())
                            {
                                var BusinessRT = new BusinessRuleTran();
                                BusinessRT.BRID = BRTID;
                                BusinessRT.From = item.From;
                                BusinessRT.TO = item.TO;
                                BusinessRT.NoofDays = item.NODays;
                                BusinessRT.Amount = item.Amount;
                                BusinessRT.CreatedUTC = DateTime.UtcNow;
                                BusinessRT.CreatedBy = BusinessRule.BusinessRule.CreatedBy;
                                BusinessRT.IsActive = true;
                                BusinessRT.EffectiveDate = DateTime.UtcNow;
                                WYNKContext.BussinessRuleTran.Add(BusinessRT);
                                WYNKContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var BusinessR = new BusinessRule();
                            BusinessR.ModuleID = BusinessRule.BusinessRule.ModuleID;
                            BusinessR.ModuleDescription = BusinessRule.BusinessRule.ModuleDescription;
                            BusinessR.BRBasedOn = BusinessRule.BusinessRule.BRBasedOn;
                            BusinessR.CMPID = BusinessRule.BusinessRule.CMPID;
                            BusinessR.WEF = BusinessRule.BusinessRule.WEF.AddDays(1);
                            BusinessR.CreatedUTC = DateTime.UtcNow;
                            BusinessR.IsActive = true;
                            BusinessR.CreatedBy = BusinessRule.BusinessRule.CreatedBy;
                            WYNKContext.BussinessRule.Add(BusinessR);
                            WYNKContext.SaveChanges();
                            var BRID1 = BusinessR.ID;
                            foreach (var item in BusinessRule.BusinessRT.ToList())
                            {
                                var BusinessRT = new BusinessRuleTran();
                                BusinessRT.BRID = BRID1;
                                BusinessRT.From = item.From;
                                BusinessRT.TO = item.TO;
                                BusinessRT.NoofDays = item.NODays;
                                BusinessRT.Amount = item.Amount;
                                BusinessRT.CreatedUTC = DateTime.UtcNow;
                                BusinessRT.CreatedBy = BusinessRule.BusinessRule.CreatedBy;
                                BusinessRT.IsActive = true;
                                BusinessRT.EffectiveDate = DateTime.UtcNow;
                                WYNKContext.BussinessRuleTran.Add(BusinessRT);
                                WYNKContext.SaveChanges();
                            }

                        }
                    } 
                    
                    dbContextTransaction.Commit();
                    if (WYNKContext.SaveChanges() >= 0)
                        return new
                        {
                            Success = true,
                            Message = "Saved successfully",
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
                    Message = "some data are missing"
                };
            }

        }



        public BusinessRuleViewModel GetBRMDescription(int cmpid, int Descriptionid, DateTime EFDate)

        {
            var BRV = new BusinessRuleViewModel();
            BRV.ModuleDescription = WYNKContext.BussinessRule.Where(x => x.ModuleID == Descriptionid && x.CMPID == cmpid).
                                     Select(x => x.ModuleDescription).FirstOrDefault();

            BRV.icdspyModuleDescription =( from busr in WYNKContext.BussinessRule.Where(x => x.ModuleID ==  Descriptionid  && x.CMPID == cmpid)
                                           select new icdspyModuleDescription
                                           {

                                           ICDCODE=busr.ICDCODE,

                                           }).ToList();

            var BRID = WYNKContext.BussinessRule.Where(x => x.ID == Descriptionid && x.CMPID == cmpid).Select(x => x.ID).FirstOrDefault();
            var efdate = new DateTime(EFDate.Year, EFDate.Month, EFDate.Day, EFDate.Hour, EFDate.Minute, 0);          
                   BRV.BusinessRT1 = (from BRT in WYNKContext.BussinessRuleTran.Where(x =>x.BRID == BRID &&
                                      new DateTime(x.CreatedUTC.Year, x.CreatedUTC.Month, x.CreatedUTC.Day, x.CreatedUTC.Hour, x.CreatedUTC.Minute, 0)== efdate)
                                      select new BusinessRT1
                                     {
                                          From = BRT.From,
                                          TO = BRT.TO,
                                          Amount = BRT.Amount,
                                          NODays = BRT.NoofDays,
                                          WEF    = BRT.EffectiveDate,

                                      }).ToList();
            return BRV;
        }   
    }
}
