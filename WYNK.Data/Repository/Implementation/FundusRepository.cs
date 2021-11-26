using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class FundusRepository : RepositoryBase<FundusViewM>, IFundusRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public FundusRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GetDesc()
        {


            var fundus = new FundusViewM();
            fundus.Descriptionfn = new List<Descriptionfn>();



            fundus.Descriptionfn = (from D in WYNKContext.FundusMaster.Where(u => u.ParentTag == "FUNDUS" && u.IsActive == true && u.IsDeleted == false)
                                    select new Descriptionfn
                                    {
                                        ID = D.ID,
                                        ParentDescription = D.ParentDescription,
                                        Righteye = "",
                                        Lefteye = "",
                                        Remarks = "",

                                    }).OrderBy(x => x.ParentDescription).ToList();

            return fundus;
        }

        public dynamic GetDescfif()
        {


            var fundus = new FundusViewM();
            fundus.Descriptionfu = new List<Descriptionfu>();



            fundus.Descriptionfu = (from D in WYNKContext.FundusMaster.Where(u => u.ParentTag == "FUNDUS" && u.IsDeleted == false)
                                    select new Descriptionfu
                                    {
                                        ID = D.ID,
                                        Description = D.ParentDescription,
                                        IsActive = D.IsActive,
                                        IsActive1 = Enum.GetName(typeof(ISactive), D.IsActive),
                                        PID = D.ParentID,
                                        PTag = D.ParentTag,

                                    }).OrderBy(x => x.Description).ToList();

            return fundus;
        }

        public dynamic GetDescfn()
        {


            var fundus = new FundusViewM();
            fundus.Descriptiontbfn = new List<Descriptiontbfn>();


            var funduss = WYNKContext.FundusMaster.ToList();
            fundus.Descriptiontbfn = (from res in WYNKContext.FundusMaster.Where(u => u.ParentTag == "FUNDUS" && u.IsActive == true && u.IsDeleted == false).GroupBy(x => x.ParentDescription)
                                      select new Descriptiontbfn
                                      {
                                          ID = res.Select(x => x.ID).FirstOrDefault(),
                                          ParentDescription = res.Select(x => x.ParentDescription).FirstOrDefault(),
                                          subfundus = (from Desc in funduss.Where(u => u.ID == res.Select(z => z.ID).FirstOrDefault() && u.IsActive == true && u.IsDeleted == false)
                                                       join SubDesc in funduss.Where(u => u.IsActive == true && u.IsDeleted == false) on Desc.ID equals SubDesc.ParentID
                                                       join ADDesc in funduss.Where(u => u.IsActive == true && u.IsDeleted == false) on SubDesc.ID equals ADDesc.ParentID into
                                                       st
                                                       from ADDesc in st.DefaultIfEmpty()
                                                       select new subfundus
                                                       {                                                           
                                                           SubID = SubDesc != null ? SubDesc.ID : 0,
                                                           SubDescription = SubDesc != null ? SubDesc.ParentDescription : string.Empty,
                                                           PropID = ADDesc != null ? ADDesc.ID : 0,
                                                           Property = ADDesc != null ? ADDesc.ParentDescription : string.Empty,

                                                       }).OrderByDescending(x => x.SubID).ToList()
                                      }).OrderByDescending(x => x.ID).ToList();

            return fundus;
        }


        public dynamic InsertDesc(FundusViewM Fundus)

        {
            var fundus = new FundusMaster();


            if (Fundus.FundusMaster.ParentDescription != null)
            {


                var c = WYNKContext.FundusMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == 0 && x.ParentDescription.Equals(Convert.ToString(Fundus.FundusMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            fundus.ParentDescription = Fundus.FundusMaster.ParentDescription;
            fundus.ParentID = 0;
            fundus.ParentTag = "FUNDUS";
            fundus.IsDeleted = false;
            fundus.IsActive = true;
            fundus.CreatedUTC = DateTime.UtcNow;
            fundus.CreatedBy = Fundus.FundusMaster.CreatedBy;
            WYNKContext.FundusMaster.AddRange(fundus);
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


        public dynamic GetSubDesc(int ID)
        {


            var fundus = new FundusViewM();
            fundus.SubslitDescription = new List<SubslitDescription>();


            fundus.SubslitDescription = (from D in WYNKContext.FundusMaster.Where(u => u.IsDeleted == false && u.IsActive == true && u.ParentID == ID)
                                         select new SubslitDescription
                                         {
                                             ID = D.ID,
                                             SubDescription = D.ParentDescription

                                         }).OrderBy(x => x.SubDescription).ToList();

            return fundus;
        }

        public dynamic InsertSubdesc(FundusViewM Fundus)

        {
            var fundus = new FundusMaster();


            if (Fundus.FundusMaster.ParentDescription != null)
            {



                var c = WYNKContext.FundusMaster.Where(x => x.IsActive == true && x.ParentID == Fundus.FundusMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(Fundus.FundusMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            fundus.ParentDescription = Fundus.FundusMaster.ParentDescription;
            fundus.ParentID = Fundus.FundusMaster.ParentID;
            fundus.ParentTag = Fundus.FundusMaster.ParentTag;
            fundus.IsDeleted = false;
            fundus.IsActive = true;
            fundus.CreatedUTC = DateTime.UtcNow;
            fundus.CreatedBy = Fundus.FundusMaster.CreatedBy;
            WYNKContext.FundusMaster.AddRange(fundus);
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

        public dynamic GetadDesc(int ID)
        {


            var fundus = new FundusViewM();
            fundus.AddslitDescription = new List<AddslitDescription>();


            fundus.AddslitDescription = (from D in WYNKContext.FundusMaster.Where(u => u.IsDeleted == false && u.IsActive == true && u.ParentID == ID)
                                         select new AddslitDescription
                                         {
                                             ID = D.ID,
                                             AdditionalDescription = D.ParentDescription

                                         }).OrderBy(x => x.AdditionalDescription).ToList();

            return fundus;
        }

        public dynamic GetfullDesc(int ID)
        {


            var fundus = new FundusViewM();
            fundus.DescriptionDtls = new List<DescriptionDtls>();

            var sltlamp = WYNKContext.FundusMaster.ToList();


            fundus.DescriptionDtls = (from Desc in sltlamp.Where(u => u.ID == ID && u.IsActive == true && u.IsDeleted == false)
                                      join SubDesc in sltlamp.Where(u => u.IsActive == true && u.IsDeleted == false) on Desc.ID equals SubDesc.ParentID
                                      join ADDesc in sltlamp.Where(u => u.IsActive == true && u.IsDeleted == false) on SubDesc.ID equals ADDesc.ParentID into
                                      st
                                      from ADDesc in st.DefaultIfEmpty()
                                      select new DescriptionDtls
                                      {
                                          Description = Desc.ParentDescription,
                                          SubDescription = SubDesc != null ? SubDesc.ParentDescription : string.Empty,
                                          AddDescription = ADDesc != null ? ADDesc.ParentDescription : string.Empty,

                                      }).OrderBy(x => x.SubDescription).ToList();

            return fundus;
        }



        public dynamic InsertAddesc(FundusViewM Fundus)

        {
            var fundus = new FundusMaster();


            if (Fundus.FundusMaster.ParentDescription != null)
            {



                var c = WYNKContext.FundusMaster.Where(x => x.IsActive == true && x.ParentID == Fundus.FundusMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(Fundus.FundusMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            fundus.ParentDescription = Fundus.FundusMaster.ParentDescription;
            fundus.ParentID = Fundus.FundusMaster.ParentID;
            fundus.ParentTag = Fundus.FundusMaster.ParentTag;
            fundus.IsDeleted = false;
            fundus.IsActive = true;
            fundus.CreatedUTC = DateTime.UtcNow;
            fundus.CreatedBy = Fundus.FundusMaster.CreatedBy;
            WYNKContext.FundusMaster.AddRange(fundus);
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

        public dynamic UpdateDesc(FundusViewM Fundus, int ID)
        {

            var fundus = new FundusMaster();


            if (Fundus.FundusMaster.ParentDescription != null)
            {

                if (Fundus.FundusMaster.IsActive == true)
                {
                    var c = WYNKContext.FundusMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == 0 && x.ParentDescription.Equals(Convert.ToString(Fundus.FundusMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                    if (c != 0)
                    {
                        return new
                        {
                            Success = false,
                            Message = "Description already Exists"
                        };

                    }
                }

            }

            fundus = WYNKContext.FundusMaster.Where(x => x.ID == ID).FirstOrDefault();

            fundus.ParentDescription = Fundus.FundusMaster.ParentDescription;
            fundus.IsActive = Fundus.FundusMaster.IsActive;
            if (Fundus.FundusMaster.IsDeleted == null)
            {
                fundus.IsDeleted = false;
            }
            else
            {
                fundus.IsDeleted = Fundus.FundusMaster.IsDeleted;
            }
            fundus.UpdatedUTC = DateTime.UtcNow;
            fundus.UpdatedBy = Fundus.FundusMaster.UpdatedBy;
            WYNKContext.FundusMaster.UpdateRange(fundus);

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

        public dynamic UpdateSubDesc(FundusViewM Fundus, int ID)
        {

            var fundus = new FundusMaster();


            if (Fundus.FundusMaster.ParentDescription != null)
            {

                var c = WYNKContext.FundusMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == Fundus.FundusMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(Fundus.FundusMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            fundus = WYNKContext.FundusMaster.Where(x => x.ID == ID).FirstOrDefault();

            fundus.ParentDescription = Fundus.FundusMaster.ParentDescription;
            fundus.IsActive = Fundus.FundusMaster.IsActive;
            if (Fundus.FundusMaster.IsDeleted == null)
            {
                fundus.IsDeleted = false;
            }
            else
            {
                fundus.IsDeleted = Fundus.FundusMaster.IsDeleted;
            }
            fundus.UpdatedUTC = DateTime.UtcNow;
            fundus.UpdatedBy = Fundus.FundusMaster.UpdatedBy;
            WYNKContext.FundusMaster.UpdateRange(fundus);

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

        public dynamic UpdateaddDesc(FundusViewM Fundus, int ID)
        {

            var fundus = new FundusMaster();


            if (Fundus.FundusMaster.ParentDescription != null)
            {

                var c = WYNKContext.FundusMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == Fundus.FundusMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(Fundus.FundusMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            fundus = WYNKContext.FundusMaster.Where(x => x.ID == ID).FirstOrDefault();

            fundus.ParentDescription = Fundus.FundusMaster.ParentDescription;
            fundus.IsActive = Fundus.FundusMaster.IsActive;
            if (Fundus.FundusMaster.IsDeleted == null)
            {
                fundus.IsDeleted = false;
            }
            else
            {
                fundus.IsDeleted = Fundus.FundusMaster.IsDeleted;
            }
            fundus.UpdatedUTC = DateTime.UtcNow;
            fundus.UpdatedBy = Fundus.FundusMaster.UpdatedBy;
            WYNKContext.FundusMaster.UpdateRange(fundus);

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

        public dynamic Deletedes(FundusViewM Fundus, int ID)
        {

            var Description = new FundusMaster();
            Description = WYNKContext.FundusMaster.Where(x => x.ID == ID).FirstOrDefault();

            Description.IsDeleted = true;
            Description.IsActive = false;

            WYNKContext.Entry(Description).State = EntityState.Modified;


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

        public dynamic Deletedess(FundusViewM Fundus, int ID)
        {

            var Description = new FundusMaster();
            Description = WYNKContext.FundusMaster.Where(x => x.ID == ID).FirstOrDefault();

            Description.IsDeleted = true;
            Description.IsActive = false;

            WYNKContext.Entry(Description).State = EntityState.Modified;


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

        public dynamic Deletedesss(FundusViewM Fundus, int ID)
        {

            var Description = new FundusMaster();
            Description = WYNKContext.FundusMaster.Where(x => x.ID == ID).FirstOrDefault();

            Description.IsDeleted = true;
            Description.IsActive = false;

            WYNKContext.Entry(Description).State = EntityState.Modified;


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

        public dynamic GetSubDescfi(int ID)
        {


            var fundus = new FundusViewM();
            fundus.Descriptionsltsf = new List<Descriptionsltsf>();


            fundus.Descriptionsltsf = (from D in WYNKContext.FundusMaster.Where(u => u.IsDeleted == false && u.ParentID == ID)
                                       select new Descriptionsltsf
                                       {
                                           ID = D.ID,
                                           Description = D.ParentDescription,
                                           IsActive = D.IsActive,
                                           IsActive1 = Enum.GetName(typeof(ISactive), D.IsActive),
                                           PID = D.ParentID,
                                           PTag = D.ParentTag,
                                       }).OrderBy(x => x.Description).ToList();

            return fundus;
        }

        public dynamic GetaddDescfi(int ID)
        {


            var fundus = new FundusViewM();
            fundus.Descriptionsltssf = new List<Descriptionsltssf>();


            fundus.Descriptionsltssf = (from D in WYNKContext.FundusMaster.Where(u => u.IsDeleted == false && u.ParentID == ID)
                                        select new Descriptionsltssf
                                        {
                                            ID = D.ID,
                                            Description = D.ParentDescription,
                                            IsActive = D.IsActive,
                                            IsActive1 = Enum.GetName(typeof(ISactive), D.IsActive),
                                            PID = D.ParentID,
                                            PTag = D.ParentTag,
                                        }).OrderBy(x => x.Description).ToList();

            return fundus;
        }

    }
}
