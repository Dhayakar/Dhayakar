using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    class ModuleMasterRepository : RepositoryBase<ModuleMasterViewM>, IModuleMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public ModuleMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public ModuleMasterViewM getParentModuleName(string CMPID, string Type)
        {
            var getparentData = new ModuleMasterViewM();
            var ModuleMas = CMPSContext.ModuleMaster.Where(x => x.IsActive == true).ToList();

            if (Type == "Module")
            {
                getparentData.getparentName = (from details in CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Main-Module" && x.IsActive == true)
                                               select new getparentName
                                               {
                                                   ModuleID1 = details.ModuleID,
                                                   ModuleDescription1 = details.ModuleDescription,

                                               }).ToList();
            }
            else if (Type == "Form")
            {
                getparentData.getparentName = (from details in CMPSContext.ModuleMaster.Where(x => x.ModuleType == "Module" && x.IsActive == true)
                                               select new getparentName
                                               {
                                                   ModuleID1 = details.ModuleID,
                                                   ModuleDescription1 = details.ModuleDescription,

                                               }).ToList();
            }
            return getparentData;
        }


        public dynamic insertdata(ModuleMasterViewM moduleMaster)
        {
            try
            {
                var M_ModuleMaster = new ModuleMaster();
                var PID = CMPSContext.ModuleMaster.Where(x => x.ModuleDescription == moduleMaster.Description).Select(x => x.ModuleID).FirstOrDefault();
                var transactionid = moduleMaster.ModuleMaster.TransactionTypeID;
                if (moduleMaster.Tag != null && transactionid != null)
                {

                    M_ModuleMaster.ModuleDescription = moduleMaster.ModuleMaster.ModuleDescription;
                    M_ModuleMaster.ModuleType = moduleMaster.ModuleMaster.ModuleType;
                    M_ModuleMaster.Parentmoduledescription = moduleMaster.ModuleMaster.Parentmoduledescription;
                    M_ModuleMaster.TransactionTypeID = moduleMaster.ModuleMaster.TransactionTypeID;
                    M_ModuleMaster.Visible = Convert.ToBoolean(moduleMaster.ModuleMaster.Visible);
                    M_ModuleMaster.ParentModuleid = PID;
                    M_ModuleMaster.IsActive = true;
                    M_ModuleMaster.Tag = true;
                    CMPSContext.ModuleMaster.AddRange(M_ModuleMaster);
                    CMPSContext.SaveChanges();

                    var NN = new Number_Control();
                    NN.Autonumber = true;
                    NN.TransactionID = Convert.ToInt32(moduleMaster.ModuleMaster.TransactionTypeID);
                    NN.DepartmentID = 0;
                    NN.CmpID = moduleMaster.CMPUID;
                    NN.Prefix = null;
                    NN.RunningNumber = 0;
                    NN.Suffix = null;
                    NN.EffectiveFrom = DateTime.UtcNow;
                    NN.IsActive = true;
                    NN.IsDeleted = false;
                    NN.CreatedUTC = DateTime.UtcNow;
                    NN.CreatedBy = 0;
                    NN.Description = CMPSContext.TransactionType.OrderByDescending(x => x.Description).Where(x => x.TransactionID == NN.TransactionID).Select(x => x.Description).FirstOrDefault();
                    CMPSContext.NumberControl.AddRange(NN);
                    CMPSContext.SaveChanges();

                }
                else
                {
                    M_ModuleMaster.ModuleDescription = moduleMaster.ModuleMaster.ModuleDescription;
                    M_ModuleMaster.ModuleType = moduleMaster.ModuleMaster.ModuleType;
                    M_ModuleMaster.Parentmoduledescription = moduleMaster.ModuleMaster.Parentmoduledescription;
                    M_ModuleMaster.TransactionTypeID = moduleMaster.ModuleMaster.TransactionTypeID;
                    M_ModuleMaster.Visible = Convert.ToBoolean(moduleMaster.ModuleMaster.Visible);
                    M_ModuleMaster.ParentModuleid = PID;
                    M_ModuleMaster.IsActive = true;
                    M_ModuleMaster.Tag = false;
                    CMPSContext.ModuleMaster.AddRange(M_ModuleMaster);
                    CMPSContext.SaveChanges();
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

        public dynamic updatedata(ModuleMasterViewM moduleMaster, int ID)
        {
            try
            {
                var updatedata = new ModuleMaster();

                updatedata = CMPSContext.ModuleMaster.Where(x => x.ModuleID == ID).FirstOrDefault();
                updatedata.ModuleDescription = moduleMaster.ModuleMaster.ModuleDescription;
                updatedata.ModuleType = moduleMaster.ModuleMaster.ModuleType;
                updatedata.TransactionTypeID = moduleMaster.ModuleMaster.TransactionTypeID;
                updatedata.Parentmoduledescription = moduleMaster.ModuleMaster.Parentmoduledescription;
                updatedata.ParentModuleid = moduleMaster.ModuleMaster.ParentModuleid;
                updatedata.Visible = Convert.ToBoolean(moduleMaster.ModuleMaster.Visible);
                updatedata.IsActive = moduleMaster.ModuleMaster.IsActive;
                updatedata.Tag = moduleMaster.ModuleMaster.Tag;
                CMPSContext.ModuleMaster.Update(updatedata);

                try
                {
                    if (CMPSContext.SaveChanges() > 0)
                        return new
                        {
                            Success = true,
                            Message = "Updated successfully"
                        };
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
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

        public dynamic Getavilablemodules()
        {
            var data = new ModuleMasterViewM();
            data.Maindetails = (from mm in CMPSContext.ModuleMaster.Where(x => x.IsActive == true && x.ModuleType == "Main-Module")
                                select new Maindetails
                                {
                                    Value = mm.ModuleID,
                                    Text = mm.ModuleDescription,
                                }).ToList();


            return data;
        }








    }
}
