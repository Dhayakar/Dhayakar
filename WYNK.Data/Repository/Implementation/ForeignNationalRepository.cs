using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class ForeignNationalRepository : RepositoryBase<ForeignNationalViewModel>, IForeignNationalRepository
    {

        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public ForeignNationalRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic InsertForeignNational(ForeignNationalViewModel AddForeignNational)
        {
            try
            {
                var FN = new ForeignNational();
                FN.IsForeignNational = AddForeignNational.ForeignNational.IsForeignNational;
                FN.NormalFee = AddForeignNational.ForeignNational.NormalFee;
                FN.SuperSpecialityFee = AddForeignNational.ForeignNational.SuperSpecialityFee;
                FN.CreatedUTC = DateTime.UtcNow;
                FN.CreatedBy = AddForeignNational.ForeignNational.CreatedBy;
                FN.CmpID = AddForeignNational.ForeignNational.CmpID;
                FN.IsActive = false;
                WYNKContext.ForeignNational.Add(FN);
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
        public dynamic getForeignNational(int CmpID)
        {
            var ForeignNational = new ForeignNationalViewModel();

            ForeignNational.ForeignNationaldetials = (from FM in WYNKContext.ForeignNational.Where(x => x.IsActive == true && x.CmpID == CmpID)
                                    select new ForeignNationaldetials
                                    {
                                        ID=FM.ID,
                                       IsForeignNational=FM.IsForeignNational,
                                       NormalFee=FM.NormalFee,
                                       SuperSpecialityFee=FM.SuperSpecialityFee,
                                       IsActive=FM.IsActive,
                                    }).ToList();

            return ForeignNational;
        }

        public dynamic UpdateForeignNational(ForeignNationalViewModel ForeignNationalUpdate, int FNID,int Cmpid)
        {
            try
            {
                var FN = new ForeignNational();
                FN = WYNKContext.ForeignNational.Where(x => x.ID == FNID&& x.CmpID == Cmpid).FirstOrDefault();
                FN.IsForeignNational = ForeignNationalUpdate.ForeignNational.IsForeignNational;
                FN.NormalFee = ForeignNationalUpdate.ForeignNational.NormalFee;
                FN.SuperSpecialityFee = ForeignNationalUpdate.ForeignNational.SuperSpecialityFee;
                FN.UpdatedUTC = DateTime.UtcNow;
                FN.UpdatedBy = ForeignNationalUpdate.ForeignNational.UpdatedBy;
                WYNKContext.Entry(FN).State = EntityState.Modified;
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
