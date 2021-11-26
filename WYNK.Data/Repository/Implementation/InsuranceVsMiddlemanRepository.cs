
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class InsuranceVsMiddlemanRepository : RepositoryBase<InsuranceVsMiddlemanViewModel>, IInsuranceVsMiddlemanRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public InsuranceVsMiddlemanRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public InsuranceVsMiddlemanViewModel GetMiddlemanDetails(int MiddlemanName)
        {
            var MiddlemanViewModel = new InsuranceVsMiddlemanViewModel();
            var LocationMaster = CMPSContext.LocationMaster.ToList();
            var MMID = WYNKContext.Insurance.Where(x => x.ID == MiddlemanName).Select(x => x.ID).FirstOrDefault();
            MiddlemanViewModel.GetMiddleman = (from INS in WYNKContext.Insurance.Where(x => x.ID == MMID && x.InsuranceCategory == 1 && x.IsActive==false)
                                                             select new GetMiddleman
                                                             {
                                                                 Name= INS.Name,
                                                                 Address1 = INS.Address1,
                                                                 Address2 = INS.Address2,
                                                                 Address3 = INS.Address3,
                                                                 LocationId = INS.LocationId,
                                                                 Pincode = INS.Pincode,
                                                                 LocationName = LocationMaster.Where(X => X.ID == Convert.ToInt16(INS.LocationId)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                             }).ToList();
            var ins = WYNKContext.Insurance.Where(x => x.InsuranceCategory == 0 && x.IsActive == false).ToList();
            var Insurancevsmiddloeman = WYNKContext.InsuranceVsMiddlemen.Where(x =>x.MiddleMenID == MiddlemanName && x.IsActive == false).ToList();
            var Sample = new List<SampleGetINvsMiddleman>();
            var Sampledata = new List<SampleGetINvsMiddlemantotal>();
            foreach (var item in Insurancevsmiddloeman)
            {
                var Lisytdata = new SampleGetINvsMiddleman();
                Lisytdata.ID =  item.IID;

                var newd = new SampleGetINvsMiddlemantotal();
                newd.ID = WYNKContext.Insurance.Where(x => x.ID == Lisytdata.ID && x.InsuranceCategory !=1).Select(x => x.ID).FirstOrDefault();
                newd.Name = WYNKContext.Insurance.Where(x => x.ID == Lisytdata.ID && x.InsuranceCategory != 1).Select(x => x.Name).FirstOrDefault();
                newd.amount = WYNKContext.InsuranceVsMiddlemen.Where(x => x.IID == Lisytdata.ID && x.MiddleMenID == MiddlemanName).Select(x => x.Amount).FirstOrDefault();
                newd.IvsMID = WYNKContext.InsuranceVsMiddlemen.Where(x => x.IID == Lisytdata.ID && x.MiddleMenID == MiddlemanName).Select(x => x.ID).FirstOrDefault();
                newd.status = true;
                Sampledata.Add(newd);
            }
            var Orgsample = (from g in Sampledata
                             select new
                             {   Id=g.ID,
                                 Orgname = g.Name,
                                 Status = true,
                                 Amount = g.amount,
                                 IvsMID=g.IvsMID,
                                 CB=false
                             }).ToList();
            var Orgdataq = (from d in ins
                            where Sampledata.All(a => a.ID != d.ID)
                            select new
                            {
                                Id = d.ID,
                                Orgname = d.Name,
                                Status = false,
                                Amount = Convert.ToDecimal(0),
                                IvsMID = 0,
                                CB=true
                            }).ToList();
            var mergeddata = Orgsample.Concat(Orgdataq);
                                          MiddlemanViewModel.temp1 = (from oo in Orgsample
                                                                      select new temp1
                                                                      {
                                                                          ID = oo.Id,
                                                                          IvsMID = oo.IvsMID,
                                                                          Name = oo.Orgname,
                                                                          checkeda = oo.Status,
                                                                          Amount = oo.Amount,
                                                                          CB= oo.CB,
                                                                      }).ToList();

            MiddlemanViewModel.OriginalSampleGetINvsMiddlemantotal = (from gg in mergeddata
                                                                      select new OriginalSampleGetINvsMiddlemantotal
                                                                      {
                                                                          ID = gg.Id,
                                                                          IvsMID =  gg.IvsMID,
                                                                          Name = gg.Orgname,
                                                                          checkeda =  gg.Status,
                                                                          Amount = gg.Amount,
                                                                          CB= gg.CB,
                                                                      }).ToList();

            return MiddlemanViewModel;
        }
        public InsuranceVsMiddlemanViewModel GetInsuranceDetails()
        {
            var MiddlemanViewModel = new InsuranceVsMiddlemanViewModel();
           
            MiddlemanViewModel.GetInsurance = (from INS in WYNKContext.Insurance.Where(x =>x.InsuranceCategory == 0 && x.IsActive == false)

                                               select new GetInsurance
                                               {
                                                   ID= INS.ID,
                                                   Name = INS.Name,
                                                   Amount = 0,
                                                   CB=true,
                                               }).ToList();

            return MiddlemanViewModel;
        }
        public dynamic InsertInsuranceVsMiddleman(InsuranceVsMiddlemanViewModel AddInsuranceVsMiddleman, int MMID,int userroleID)
        {
            try
            {
                var Insurance = WYNKContext.Insurance.ToList();
                foreach (var item in AddInsuranceVsMiddleman.InsertIvsM.ToList())
                {
                    var InsuranceVsMiddleman1 = new InsuranceVsMiddlemen();
                    InsuranceVsMiddleman1.IID = Insurance.Where(x => x.Name == item.Name).Select(x => x.ID).FirstOrDefault();
                    InsuranceVsMiddleman1.MiddleMenID = MMID;
                    InsuranceVsMiddleman1.Amount = item.Amount;
                    InsuranceVsMiddleman1.IsActive = false;
                    InsuranceVsMiddleman1.CreatedBy = userroleID;
                    InsuranceVsMiddleman1.CreatedUTC = DateTime.UtcNow;
                    WYNKContext.InsuranceVsMiddlemen.AddRange(InsuranceVsMiddleman1);
                    
                }
                WYNKContext.SaveChanges();
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


        public dynamic UpdateInsuranceVsMiddleman(InsuranceVsMiddlemanViewModel InsuranceVsMiddlemanUpdate, int MMID,int userroleID)
        {
            try
            {
                var Insurance = WYNKContext.Insurance.ToList();
               
                var InsuranceVsMiddlemen1 = WYNKContext.InsuranceVsMiddlemen.Where(x => x.MiddleMenID == MMID).ToList();
                if (InsuranceVsMiddlemen1.Count!=0)
                {
                    WYNKContext.InsuranceVsMiddlemen.RemoveRange(InsuranceVsMiddlemen1);
                    WYNKContext.SaveChanges();
                }
                    foreach (var item in InsuranceVsMiddlemanUpdate.UpdateIvsM.ToList())
                    {
                        var InsuranceVsMiddleman1 = new InsuranceVsMiddlemen();
                        InsuranceVsMiddleman1.IID = Insurance.Where(x => x.Name == item.Name).Select(x => x.ID).FirstOrDefault();
                        InsuranceVsMiddleman1.MiddleMenID = MMID;
                        InsuranceVsMiddleman1.Amount = item.Amount;
                        InsuranceVsMiddleman1.IsActive = false;
                        InsuranceVsMiddleman1.CreatedBy = userroleID;
                        InsuranceVsMiddleman1.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.InsuranceVsMiddlemen.AddRange(InsuranceVsMiddleman1);
                    }
                    WYNKContext.SaveChanges();   
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
      
        }
}
