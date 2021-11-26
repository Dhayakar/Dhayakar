using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using System.Security.Cryptography;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class SlitLampRepository : RepositoryBase<SlitLampViewM>, ISlitLampRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;


        public SlitLampRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic GetDesc()
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.Description = new List<Description>();



            slitlamp.Description = (from D in WYNKContext.SlitLampMaster.Where(u => u.ParentTag == "SLIT" && u.IsActive == true && u.IsDeleted == false)
                                    select new Description
                                    {
                                        ID = D.ID,
                                        ParentDescription = D.ParentDescription,
                                        Righteye = "",
                                        Lefteye = "",
                                        Remarks = "",

                                    }).OrderBy(x => x.ParentDescription).ToList();

            return slitlamp;
        }

        public dynamic GetDescfi()
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.Descriptionslt = new List<Descriptionslt>();



            slitlamp.Descriptionslt = (from D in WYNKContext.SlitLampMaster.Where(u => u.ParentTag == "SLIT" && u.IsDeleted == false)
                                       select new Descriptionslt
                                       {

                                           ID = D.ID,
                                           Description = D.ParentDescription,
                                           IsActive = D.IsActive,
                                           IsActive1 = Enum.GetName(typeof(ISactive), D.IsActive),
                                           PID = D.ParentID,
                                           PTag = D.ParentTag,
                                       }).OrderBy(x => x.Description).ToList();

            return slitlamp;
        }

        public dynamic GetSubDescfi(int ID)
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.Descriptionslts = new List<Descriptionslts>();


            slitlamp.Descriptionslts = (from D in WYNKContext.SlitLampMaster.Where(u => u.IsDeleted == false && u.ParentID == ID)
                                        select new Descriptionslts
                                        {
                                            ID = D.ID,
                                            Description = D.ParentDescription,
                                            IsActive = D.IsActive,
                                            IsActive1 = Enum.GetName(typeof(ISactive), D.IsActive),
                                            PID = D.ParentID,
                                            PTag = D.ParentTag,
                                        }).OrderBy(x => x.Description).ToList();

            return slitlamp;
        }

        public dynamic GetDesctab()
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.Descriptiontb = new List<Descriptiontb>();


            var sltlamp = WYNKContext.SlitLampMaster.ToList();
            slitlamp.Descriptiontb = (from res in WYNKContext.SlitLampMaster.Where(u => u.ParentTag == "SLIT" && u.IsActive == true && u.IsDeleted == false).GroupBy(x => x.ParentDescription)
                                      select new Descriptiontb
                                      {
                                          ID = res.Select(x => x.ID).FirstOrDefault(),
                                          ParentDescription = res.Select(x => x.ParentDescription).FirstOrDefault(),
                                          subslit = (from Desc in sltlamp.Where(u => u.ID == res.Select(z => z.ID).FirstOrDefault() && u.IsActive == true && u.IsDeleted == false)
                                                     join SubDesc in sltlamp.Where(u => u.IsActive == true && u.IsDeleted == false) on Desc.ID equals SubDesc.ParentID
                                                     join ADDesc in sltlamp.Where(u => u.IsActive == true && u.IsDeleted == false) on SubDesc.ID equals ADDesc.ParentID into
                                                     st
                                                     from ADDesc in st.DefaultIfEmpty()
                                                     select new subslit
                                                     {
                                                         SubID = SubDesc != null ? SubDesc.ID : 0,
                                                         SubDescription = SubDesc != null ? SubDesc.ParentDescription : string.Empty,
                                                         PropID = ADDesc != null ? ADDesc.ID : 0,
                                                         Property = ADDesc != null ? ADDesc.ParentDescription : string.Empty,

                                                     }).OrderByDescending(x => x.SubID).ToList()
                                      }).OrderByDescending(x => x.ID).ToList();

            return slitlamp;
        }

        public dynamic InsertDesc(SlitLampViewM SlitLamp)

        {
            var slitlamp = new SlitLampMaster();


            if (SlitLamp.SlitLampMaster.ParentDescription != null)
            {


                var c = WYNKContext.SlitLampMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == 0 && x.ParentDescription.Equals(Convert.ToString(SlitLamp.SlitLampMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            slitlamp.ParentDescription = SlitLamp.SlitLampMaster.ParentDescription;
            slitlamp.ParentID = 0;
            slitlamp.ParentTag = "SLIT";
            slitlamp.IsDeleted = false;
            slitlamp.IsActive = true;
            slitlamp.CreatedUTC = DateTime.UtcNow;
            slitlamp.CreatedBy = SlitLamp.SlitLampMaster.CreatedBy;
            WYNKContext.SlitLampMaster.AddRange(slitlamp);
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



        public dynamic UpdateDesc(SlitLampViewM SlitLamp, int ID)
        {

            var slitlamp = new SlitLampMaster();


            if (SlitLamp.SlitLampMaster.ParentDescription != null)
            {
                if (SlitLamp.SlitLampMaster.IsActive == true)
                {

                    var c = WYNKContext.SlitLampMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == 0 && x.ParentDescription.Equals(Convert.ToString(SlitLamp.SlitLampMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(y => y.ID).FirstOrDefault();

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

            slitlamp = WYNKContext.SlitLampMaster.Where(x => x.ID == ID).FirstOrDefault();

            slitlamp.ParentDescription = SlitLamp.SlitLampMaster.ParentDescription;
            slitlamp.IsActive = SlitLamp.SlitLampMaster.IsActive;
            if (SlitLamp.SlitLampMaster.IsDeleted == null)
            {
                slitlamp.IsDeleted = false;
            }
            else
            {
                slitlamp.IsDeleted = SlitLamp.SlitLampMaster.IsDeleted;
            }
            slitlamp.UpdatedUTC = DateTime.UtcNow;
            slitlamp.UpdatedBy = SlitLamp.SlitLampMaster.UpdatedBy;
            WYNKContext.SlitLampMaster.UpdateRange(slitlamp);

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

        public dynamic Deletedes(SlitLampViewM SlitLamp, int ID)
        {

            var Description = new SlitLampMaster();
            Description = WYNKContext.SlitLampMaster.Where(x => x.ID == ID).FirstOrDefault();

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

        public dynamic Deletedess(SlitLampViewM SlitLamp, int ID)
        {

            var Description = new SlitLampMaster();
            Description = WYNKContext.SlitLampMaster.Where(x => x.ID == ID).FirstOrDefault();

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

        public dynamic Deletedesss(SlitLampViewM SlitLamp, int ID)
        {

            var Description = new SlitLampMaster();
            Description = WYNKContext.SlitLampMaster.Where(x => x.ID == ID).FirstOrDefault();

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


        public dynamic UpdateSubDesc(SlitLampViewM SlitLamp, int ID)
        {

            var slitlamp = new SlitLampMaster();


            if (SlitLamp.SlitLampMaster.ParentDescription != null)
            {

                var c = WYNKContext.SlitLampMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == SlitLamp.SlitLampMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(SlitLamp.SlitLampMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();


                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            slitlamp = WYNKContext.SlitLampMaster.Where(x => x.ID == ID).FirstOrDefault();

            slitlamp.ParentDescription = SlitLamp.SlitLampMaster.ParentDescription;
            slitlamp.IsActive = SlitLamp.SlitLampMaster.IsActive;
            if (SlitLamp.SlitLampMaster.IsDeleted == null)
            {
                slitlamp.IsDeleted = false;
            }
            else
            {
                slitlamp.IsDeleted = SlitLamp.SlitLampMaster.IsDeleted;
            }
            slitlamp.UpdatedUTC = DateTime.UtcNow;
            slitlamp.UpdatedBy = SlitLamp.SlitLampMaster.UpdatedBy;
            WYNKContext.SlitLampMaster.UpdateRange(slitlamp);

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




        public dynamic InsertSubdesc(SlitLampViewM SlitLamp)

        {
            var slitlamp = new SlitLampMaster();


            if (SlitLamp.SlitLampMaster.ParentDescription != null)
            {



                var c = WYNKContext.SlitLampMaster.Where(x => x.IsActive == true && x.ParentID == SlitLamp.SlitLampMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(SlitLamp.SlitLampMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            slitlamp.ParentDescription = SlitLamp.SlitLampMaster.ParentDescription;
            slitlamp.ParentID = SlitLamp.SlitLampMaster.ParentID;
            slitlamp.ParentTag = SlitLamp.SlitLampMaster.ParentTag;
            slitlamp.IsDeleted = false;
            slitlamp.IsActive = true;
            slitlamp.CreatedUTC = DateTime.UtcNow;
            slitlamp.CreatedBy = SlitLamp.SlitLampMaster.CreatedBy;
            WYNKContext.SlitLampMaster.AddRange(slitlamp);
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


        public dynamic InsertAddesc(SlitLampViewM SlitLamp)

        {
            var slitlamp = new SlitLampMaster();


            if (SlitLamp.SlitLampMaster.ParentDescription != null)
            {



                var c = WYNKContext.SlitLampMaster.Where(x => x.IsActive == true && x.ParentID == SlitLamp.SlitLampMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(SlitLamp.SlitLampMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();

                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            slitlamp.ParentDescription = SlitLamp.SlitLampMaster.ParentDescription;
            slitlamp.ParentID = SlitLamp.SlitLampMaster.ParentID;
            slitlamp.ParentTag = SlitLamp.SlitLampMaster.ParentTag;
            slitlamp.IsDeleted = false;
            slitlamp.IsActive = true;
            slitlamp.CreatedUTC = DateTime.UtcNow;
            slitlamp.CreatedBy = SlitLamp.SlitLampMaster.CreatedBy;
            WYNKContext.SlitLampMaster.AddRange(slitlamp);
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





        public dynamic UpdateaddDesc(SlitLampViewM SlitLamp, int ID)
        {

            var slitlamp = new SlitLampMaster();


            if (SlitLamp.SlitLampMaster.ParentDescription != null)
            {

                var c = WYNKContext.SlitLampMaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentID == SlitLamp.SlitLampMaster.ParentID && x.ParentDescription.Equals(Convert.ToString(SlitLamp.SlitLampMaster.ParentDescription), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID).FirstOrDefault();


                if (c != 0)
                {
                    return new
                    {
                        Success = false,
                        Message = "Description already Exists"
                    };

                }

            }

            slitlamp = WYNKContext.SlitLampMaster.Where(x => x.ID == ID).FirstOrDefault();

            slitlamp.ParentDescription = SlitLamp.SlitLampMaster.ParentDescription;
            slitlamp.IsActive = SlitLamp.SlitLampMaster.IsActive;
            if (SlitLamp.SlitLampMaster.IsDeleted == null)
            {
                slitlamp.IsDeleted = false;
            }
            else
            {
                slitlamp.IsDeleted = SlitLamp.SlitLampMaster.IsDeleted;
            }
            slitlamp.UpdatedUTC = DateTime.UtcNow;
            slitlamp.UpdatedBy = SlitLamp.SlitLampMaster.UpdatedBy;
            WYNKContext.SlitLampMaster.UpdateRange(slitlamp);

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


            var slitlamp = new SlitLampViewM();
            slitlamp.SubslitDescription = new List<SubslitDescription>();


            slitlamp.SubslitDescription = (from D in WYNKContext.SlitLampMaster.Where(u => u.IsDeleted == false && u.IsActive == true && u.ParentID == ID)
                                           select new SubslitDescription
                                           {
                                               ID = D.ID,
                                               SubDescription = D.ParentDescription

                                           }).OrderBy(x => x.SubDescription).ToList();

            return slitlamp;
        }

        public dynamic GetfullDesc(int ID)
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.DescriptionDtls = new List<DescriptionDtls>();

            var sltlamp = WYNKContext.SlitLampMaster.ToList();


            slitlamp.DescriptionDtls = (from Desc in sltlamp.Where(u => u.ID == ID && u.IsActive == true && u.IsDeleted == false)
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


            return slitlamp;
        }

        public dynamic GetadDesc(int ID)
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.AddslitDescription = new List<AddslitDescription>();


            slitlamp.AddslitDescription = (from D in WYNKContext.SlitLampMaster.Where(u => u.IsDeleted == false && u.IsActive == true && u.ParentID == ID)
                                           select new AddslitDescription
                                           {
                                               ID = D.ID,
                                               AdditionalDescription = D.ParentDescription

                                           }).OrderBy(x => x.AdditionalDescription).ToList();

            return slitlamp;
        }


        public dynamic GetaddDescfi(int ID)
        {


            var slitlamp = new SlitLampViewM();
            slitlamp.Descriptionsltss = new List<Descriptionsltss>();


            slitlamp.Descriptionsltss = (from D in WYNKContext.SlitLampMaster.Where(u => u.IsDeleted == false && u.ParentID == ID)
                                         select new Descriptionsltss
                                         {
                                             ID = D.ID,
                                             Description = D.ParentDescription,
                                             IsActive = D.IsActive,
                                             IsActive1 = Enum.GetName(typeof(ISactive), D.IsActive),
                                             PID = D.ParentID,
                                             PTag = D.ParentTag,
                                         }).OrderBy(x => x.Description).ToList();

            return slitlamp;
        }

    }
}
