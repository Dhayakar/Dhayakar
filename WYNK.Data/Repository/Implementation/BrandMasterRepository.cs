
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class BrandMasterRepository : RepositoryBase<BrandView>, IBrandMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public BrandMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public dynamic Insertbrand(BrandView Addbrand)
        {
         
            if (Addbrand.Brand.Count() > 0)
            {

                foreach (var item in Addbrand.Brand.ToList())
                {

                        var c = WYNKContext.Brand.Where(x => x.Description.Replace(" ", string.Empty).Equals(item.Description.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.BrandType.Replace(" ", string.Empty).Equals(item.BrandType.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                        if (c != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Already Exists",
                                Brand = item.Description,
                                Type = item.BrandType,
                            };

                        }

                    var brand = new Brand();

                    brand.BrandType = item.BrandType;
                    brand.Description = item.Description;
                    brand.CreatedBy = item.CreatedBy;
                    brand.CreatedUTC = DateTime.UtcNow;
                    brand.IsActive = true;
                    brand.cmpID = item.cmpID;
                    WYNKContext.Brand.AddRange(brand);
                }
            }

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

        public dynamic Fullbrandlist(int cmpid)
        {
            var getData = new BrandView();

            var Trantype = WYNKContext.Brand.ToList();


            getData.Brandfull = (from details in Trantype.Where(x => x.cmpID == cmpid && x.IsDeleted == false)
                                 select new Brandfull
                                 {
                                     ID = details.ID,
                                     Description = details.Description,
                                     BrandType = details.BrandType,
                                     IsActive = details.IsActive,
                                     IsActivename = details.IsActive == true ? "Active" : "InActive",
                                 }).ToList();

            return getData;
        }

        public dynamic updatebrand(BrandView Upbrand, int ID)
        {


            if (Upbrand.Brand.Count() > 0)
            {

                foreach (var item in Upbrand.Brand.ToList())
                {
                        var c = WYNKContext.Brand.Where(x => x.Description.Replace(" ", string.Empty).Equals(item.Description.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.BrandType.Replace(" ", string.Empty).Equals(item.BrandType.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.IsActive == item.IsActive).Select(y => y.ID).FirstOrDefault();

                        if (c != 0)
                        {
                            return new
                            {
                                Success = false,
                                Message = "Already Exists",
                                Brand = item.Description,
                                Type = item.BrandType,
                            };

                        }

                    var brand = new Brand();

                    brand = WYNKContext.Brand.Where(x => x.ID == ID).FirstOrDefault();

                    brand.BrandType = item.BrandType;
                    brand.Description = item.Description;
                    brand.UpdatedBy = item.CreatedBy;
                    brand.UpdatedUTC = DateTime.UtcNow;
                    brand.IsActive = item.IsActive;
                    brand.cmpID = item.cmpID;
                    WYNKContext.Brand.UpdateRange(brand);
                }
            }



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

        public dynamic Deletebrand(int ID)
        {
            var stoMas = WYNKContext.Brand.Where(x => x.ID == ID).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                WYNKContext.Brand.UpdateRange(stoMas);

            }

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
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
