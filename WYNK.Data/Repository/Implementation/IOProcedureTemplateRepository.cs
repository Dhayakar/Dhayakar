
using System;
using System.Linq;

using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

using WYNK.Data.Common;

namespace WYNK.Data.Repository.Implementation
{
    class IOProcedureTemplateRepository : RepositoryBase<IOProcedureTemplateViewModel>, IIOProcedureTemplateRepository
    {
        

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public IOProcedureTemplateRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic Submit(IOProcedureTemplateViewModel submit, int userID, int icdspecialitycode)
        {
             using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
             {

                try
                {

                if (submit.IOProcedureTempDetails.Count >= 1)
                {
                    foreach (var item in submit.IOProcedureTempDetails.ToList())
                    {
                        var IoProcs = new IOProcedureTemplate();
                        IoProcs.ICDSpecialityCode = Convert.ToInt32(item.icdSpeciality);
                        IoProcs.SurgeryDescription = item.SurgeryDescription;
                        IoProcs.IsActive = true;
                        IoProcs.CreatedUTC = DateTime.UtcNow;
                        IoProcs.CreatedBy = userID;
                        WYNKContext.IOProcedureTemplate.Add(IoProcs);
                        WYNKContext.SaveChanges();

                        var IoProcTrans = new IOTemplateTran();
                        IoProcTrans.IOTemplateID = IoProcs.ID;
                        IoProcTrans.OTNotesDescription = item.OTNotesDescription;
                        IoProcTrans.UserInputType = item.UserInputType;
                        IoProcTrans.InputValue = item.InputValue;
                        IoProcTrans.IsActive = true;
                        IoProcTrans.CreatedUTC = DateTime.UtcNow;
                        IoProcTrans.CreatedBy = userID;
                        WYNKContext.IOTemplateTran.Add(IoProcTrans);
                        WYNKContext.SaveChanges();
                    }

                }
     
                  dbContextTransaction.Commit(); 


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
                    Message = "Something Went Wrong"
                };
              }
        }

        public dynamic Update(IOProcedureTemplateViewModel submit, int userID, int icdspecialitycode)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    if (submit.IOProcedureTempDetails.Count >= 1)
                    {
                        foreach (var item in submit.IOProcedureTempDetails.ToList())
                        {
                            var IoProcs = new IOProcedureTemplate();
                            var master = (item.SurgeryDescription == submit.SurgeryDescription);
                            if (master == false)
                            {
                                var count = WYNKContext.IOProcedureTemplate.Where(x => x.SurgeryDescription == item.SurgeryDescription).ToList();
                                if (count != null)
                                {

                                    count.All(x => { x.SurgeryDescription = submit.SurgeryDescription; return true; });
                                    WYNKContext.IOProcedureTemplate.UpdateRange(count);
                                }
                            }
                        }
                        WYNKContext.SaveChanges();

                    }
                    var UpdatedList = submit.IOProcedureTempDetails.Where(x => x.changed == true).ToList();
                    var AddList = submit.IOProcedureTempDetails.Where(x => x.Added == true).ToList();


                    if (UpdatedList.Count >= 1) {

                        foreach (var item in UpdatedList.ToList())
                        {
                            var IoProc = WYNKContext.IOProcedureTemplate.Where(x => x.ID == item.ID).FirstOrDefault();
                           
                            IoProc.SurgeryDescription = item.SurgeryDescription;
                            if (item.Status)
                            {
                                IoProc.IsActive = true;
                            }
                            else
                            {
                                IoProc.IsActive = false;
                            }
                            IoProc.UpdatedUTC = DateTime.UtcNow;
                            IoProc.UpdatedBy = userID;
                            WYNKContext.IOProcedureTemplate.UpdateRange(IoProc);
                            WYNKContext.SaveChanges();

                            var IoProcTran = WYNKContext.IOTemplateTran.Where(x => x.IOTemplateID == item.ID).FirstOrDefault();
                          
                            IoProcTran.OTNotesDescription = item.OTNotesDescription;
                            if (item.Status)
                            {
                                IoProcTran.IsActive = true;
                            }
                            else
                            {
                                IoProcTran.IsActive = false;
                            }
                            IoProcTran.UserInputType = item.UserInputType;
                            IoProcTran.InputValue = item.InputValue;
                            IoProcTran.UpdatedUTC = DateTime.UtcNow;
                            IoProcTran.UpdatedBy = userID;
                            WYNKContext.IOTemplateTran.UpdateRange(IoProcTran);
                            WYNKContext.SaveChanges();

                         }
                     }

                    

                    if (AddList.Count >= 1)
                    {
                    foreach(var item in AddList.ToList())
                    {
                        var IoProcs = new IOProcedureTemplate();
                        IoProcs.ICDSpecialityCode = Convert.ToInt32(item.icdSpeciality);
                        IoProcs.SurgeryDescription = item.SurgeryDescription;
                        IoProcs.IsActive = true;
                        IoProcs.CreatedUTC = DateTime.UtcNow;
                        IoProcs.CreatedBy = userID;
                        WYNKContext.IOProcedureTemplate.Add(IoProcs);
                        WYNKContext.SaveChanges();

                        var IoProcTrans = new IOTemplateTran();
                        IoProcTrans.IOTemplateID = IoProcs.ID;
                        IoProcTrans.OTNotesDescription = item.OTNotesDescription;
                        IoProcTrans.UserInputType = item.UserInputType;
                        IoProcTrans.InputValue = item.InputValue;
                        IoProcTrans.IsActive = true;
                        IoProcTrans.CreatedUTC = DateTime.UtcNow;
                        IoProcTrans.CreatedBy = userID;
                        WYNKContext.IOTemplateTran.Add(IoProcTrans);
                        WYNKContext.SaveChanges();
                    }

                }




                 dbContextTransaction.Commit();

                var Data = (from IO in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == icdspecialitycode && x.SurgeryDescription == submit.SurgeryDescription)
                                group IO by IO.SurgeryDescription into IOgroup
                                select new
                                {
                                    SurgeryDescObj = (from IOt in WYNKContext.IOProcedureTemplate.Where(x => x.IsActive == true && x.ICDSpecialityCode == icdspecialitycode)
                                                      join IOtrans in WYNKContext.IOTemplateTran.Where(x => x.IsActive == true) on IOt.ID equals IOtrans.IOTemplateID
                                                      where (IOgroup.Select(x => x.ID)).Contains(IOtrans.IOTemplateID)
                                                      select new
                                                      {
                                                          ID = IOt.ID,
                                                          icdSpecialityDesc = WYNKContext.ICDSpecialityCode.Where(x => x.ID == IOgroup.Select(y => y.ICDSpecialityCode).FirstOrDefault()).Select(x => x.SpecialityDescription).FirstOrDefault(),
                                                          SurgeryDescription = IOgroup.Key,
                                                          OTNotesDescription = IOtrans.OTNotesDescription,
                                                          UserInputType = IOtrans.UserInputType,
                                                          InputValue = IOtrans.InputValue,
                                                          Status = IOt.IsActive,
                                                      }).ToList()

                                }).ToList();
       
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        Resultdata = Data
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
                    Message = "Something Went Wrong"
                };
            }

        }
        public dynamic DeleteIOTemplateTRan(int userID, int ID)
        {
            try
            {
                var IoTemptran = WYNKContext.IOTemplateTran.Where(x => x.IOTemplateID == ID).FirstOrDefault();
                IoTemptran.IsActive = false;
                IoTemptran.UpdatedUTC = DateTime.UtcNow;
                WYNKContext.IOTemplateTran.UpdateRange(IoTemptran);
                WYNKContext.SaveChanges();

                return new
                {
                    Success = true,
                };
            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }
        }

        public dynamic GetSurgeryDescriptions(int IcdSpecCode)
        {
            try
            {
                var SurgeryDescription = WYNKContext.ICDMaster.Where(x => x.SpecialityCode == IcdSpecCode).Select(x => new Dropdown { Text = x.ICDDESCRIPTION, Value = x.ICDDESCRIPTION }).OrderBy(x => x.Text).ToList();

                return new
                {
                    Success = true,
                    SurgeryDescription = SurgeryDescription
                };
            }
            catch (Exception)
            {

                return new
                {
                    Success = false,
                };
            }
        }
    }
}













