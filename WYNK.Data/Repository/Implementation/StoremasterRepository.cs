using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class StoremasterRepository : RepositoryBase<Storemasterviewmodel>, IStoremasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public StoremasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertStoreMas(Storemasterviewmodel StoreMas)

        {



            try
            {
                var StoreMasters = new Storemasters();

                StoreMasters.Storename = StoreMas.Storemaster.Storename;
                StoreMasters.StoreLocation = StoreMas.Storemaster.StoreLocation;
                StoreMasters.Address1 = StoreMas.Storemaster.Address1;
                StoreMasters.Address2 = StoreMas.Storemaster.Address2;
                StoreMasters.StoreKeeper = StoreMas.Storemaster.StoreKeeper;
                StoreMasters.CmpID = StoreMas.Storemaster.CmpID;
                StoreMasters.CatgType = StoreMas.Storemaster.CatgType;
                StoreMasters.CreatedUTC = DateTime.UtcNow;
                StoreMasters.CreatedBy = StoreMas.Storemaster.CreatedBy;
                StoreMasters.IsActive = CommonRepository.Active;
                CMPSContext.Storemasters.AddRange(StoreMasters);

                string cmpname = CMPSContext.Company.Where(x => x.CmpID == StoreMas.Storemaster.CmpID).Select(x => x.CompanyName).FirstOrDefault();
                string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == StoreMas.Storemaster.CreatedBy).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                string userid = Convert.ToString(StoreMas.Storemaster.CreatedBy);
                ErrorLog oErrorLogs = new ErrorLog();
                oErrorLogs.WriteErrorLogTitle(cmpname, "Store Master", "User name :", username, "User ID :", userid, "Mode : Sumbit");

                object names = StoreMasters;
                oErrorLogs.WriteErrorLogArray("Storemasters", names);
                CMPSContext.SaveChanges();

                return new
                {
                    Success = true,

                };
            }

            catch (Exception ex)
            {
                ErrorLog oErrorLog = new ErrorLog();
                oErrorLog.WriteErrorLog("Error :", ex.InnerException.Message.ToString());
                return new
                {
                    Success = false,
                };
            }

        }
        public dynamic UpdateStoreMas(Storemasterviewmodel storeup, int ID)
        {

            var StoreMasters = new Storemasters();

            StoreMasters = CMPSContext.Storemasters.Where(x => x.StoreID == ID).FirstOrDefault();

            StoreMasters.Storename = storeup.Storemaster.Storename;
            StoreMasters.StoreLocation = storeup.Storemaster.StoreLocation;
            StoreMasters.Address1 = storeup.Storemaster.Address1;
            StoreMasters.Address2 = storeup.Storemaster.Address2;
            StoreMasters.StoreKeeper = storeup.Storemaster.StoreKeeper;
            StoreMasters.IsActive = storeup.Storemaster.IsActive;
            StoreMasters.CatgType = storeup.Storemaster.CatgType;
            StoreMasters.UpdatedUTC = DateTime.UtcNow;
            StoreMasters.UpdatedBy = storeup.Storemaster.UpdatedBy;
            CMPSContext.Storemasters.UpdateRange(StoreMasters);
            CMPSContext.SaveChanges();



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
        public dynamic DeleteStoreMas(int ID)
        {
            var stoMas = CMPSContext.Storemasters.Where(x => x.StoreID == ID).ToList();
            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDelete = true; return true; });
                CMPSContext.Storemasters.UpdateRange(stoMas);

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


        public dynamic saveonelineStoreMas(string ID, int CreatedBy)
        {
            try
            {
                var name = CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(ID.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.IsActive == true).Select(x => x.OLMID).FirstOrDefault();

                if (name != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Department Already Exists",
                    };
                }

                var one = new OneLine_Masters();
                one.Amount = null;
                one.ParentDescription = ID;
                one.ParentID = 0;
                one.ParentTag = "DEPT";
                one.IsDeleted = false;
                one.IsActive = true;
                one.CreatedUTC = DateTime.UtcNow;
                one.CreatedBy = CreatedBy;
                CMPSContext.OneLineMaster.AddRange(one);
                CMPSContext.SaveChanges();

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
        public dynamic updatenelineStoreMas(string ID, string status, string OLMID, int UpdatedBy)
        {
            try
            {

                var name = CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(ID.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.IsActive == Convert.ToBoolean(status)).Select(x => x.OLMID).FirstOrDefault();

                if (name != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Department Already Exists",
                    };
                }

                var olmdata = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(OLMID)).FirstOrDefault();
                olmdata.ParentDescription = ID;
                olmdata.IsActive = Convert.ToBoolean(status);
                olmdata.UpdatedUTC = DateTime.UtcNow;
                olmdata.UpdatedBy = UpdatedBy;
                CMPSContext.OneLineMaster.UpdateRange(olmdata);
                CMPSContext.SaveChanges();
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
