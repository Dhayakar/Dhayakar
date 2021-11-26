using Microsoft.EntityFrameworkCore;
using CommonMailService.cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class TonometryRepository : RepositoryBase<TonomentryViewmodel>, ITonometryRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public TonometryRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic Inserttonometry(TonomentryViewmodel Addtonometry)
        {

            var Tonometry = new Tonometry();

            if (Addtonometry.Tonometry.Description != null)
            {

                var c = WYNKContext.Tonometry.Where(x => x.Description.Equals(Convert.ToString(Addtonometry.Tonometry.Description), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Tonometry name already Exists"
                    };

                }

            }

            Tonometry.Description = Addtonometry.Tonometry.Description;
            Tonometry.Cmpid = Addtonometry.Tonometry.Cmpid;
            Tonometry.IsActive = true;
            Tonometry.CreatedUTC = DateTime.UtcNow;
            Tonometry.CreatedBy = Addtonometry.Tonometry.CreatedBy;
            WYNKContext.Tonometry.AddRange(Tonometry);

            try
            {
                if (WYNKContext.SaveChanges() > 0)
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
        public dynamic Fulltonometrylist()
        {
            var getData = new TonomentryViewmodel();

            getData.Tonomrtryfull = (from details in WYNKContext.Tonometry
                                     select new Tonomrtryfull
                                     {
                                         ID = details.ID,
                                         Description = details.Description,
                                         IsActive = details.IsActive == true ? "Active" : "InActive",
                                         Active = details.IsActive,
                                     }).ToList();

            return getData;
        }

        public dynamic updatetonometry(TonomentryViewmodel Uptonometry, int ID)
        {

            var Tonometry = new Tonometry();


            var u = WYNKContext.Tonometry.Where(x => x.ID == ID && x.Description.Equals(Convert.ToString(Uptonometry.Tonometry.Description), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

            if (u != 0)
            {

                if (Uptonometry.Tonometry.Description != null)
                {
                    var c = WYNKContext.Tonometry.Where(x => x.ID == ID && x.Description.Equals(Convert.ToString(Uptonometry.Tonometry.Description), StringComparison.OrdinalIgnoreCase) && x.IsActive == Uptonometry.Tonometry.IsActive).Select(x => x.ID).FirstOrDefault();

                    if (c != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Tonometry name already exists"
                        };

                    }

                }
            }

            Tonometry = WYNKContext.Tonometry.Where(x => x.ID == ID).FirstOrDefault();

            Tonometry.Description = Uptonometry.Tonometry.Description;
            Tonometry.IsActive = Uptonometry.Tonometry.IsActive;
            Tonometry.UpdatedUTC = DateTime.UtcNow;
            Tonometry.UpdatedBy = Uptonometry.Tonometry.UpdatedBy;
            WYNKContext.Tonometry.UpdateRange(Tonometry);


            try
            {
                if (WYNKContext.SaveChanges() > 0)
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

    }
}
