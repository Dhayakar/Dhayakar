using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class UomMasterRepository : RepositoryBase<UomMasterViewModel>, IUomMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public UomMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }




        public dynamic InsertUom(string UOMdep, int userID)
        {
            var uom = new UOM_Master();
            uom.Description = UOMdep;
            uom.CreatedUTC = DateTime.UtcNow;
            uom.CreatedBy = userID;
            uom.IsDeleted = false;
            CMPSContext.uommaster.Add(uom);
            CMPSContext.SaveChanges();
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Id = uom.Description,
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

        public dynamic UpdateUom(int UOMID, string UOMdep, int userID)
        {
            var uomupdate = new UOM_Master();
            uomupdate = CMPSContext.uommaster.Where(x => x.id == UOMID).FirstOrDefault();
            uomupdate.Description = UOMdep;
            uomupdate.UpdatedUTC = DateTime.UtcNow;
            uomupdate.UpdatedBy = userID;
            CMPSContext.Entry(uomupdate).State = EntityState.Modified;
            CMPSContext.SaveChanges();
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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

        public dynamic DeleteUom(int UOMID)
        {
            var uomupdate = new UOM_Master();
            uomupdate = CMPSContext.uommaster.Where(x => x.id == UOMID).FirstOrDefault();
            uomupdate.IsDeleted = true;
            CMPSContext.Entry(uomupdate).State = EntityState.Modified;
            CMPSContext.SaveChanges();
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
        public UomMasterViewModel GetUomDetails()

        {
            var UOM = new UomMasterViewModel();


            UOM.UOMMaster = (from oum in CMPSContext.uommaster.Where(u => u.IsDeleted == false)
                                select new UOMMaster
                                {
                                    ID = oum.id,
                                    Description = oum.Description,
                                 
                                }).ToList();
            
            return UOM;
        } 
    }

        
}













