
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    class Allergyrepository : RepositoryBase<AllergyViewmodel>, IAllergyrepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public Allergyrepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public dynamic Insertallergy(AllergyViewmodel AddType)
        {
            var allergy = new allergy();

            allergy.ParentDescription = AddType.allergy.ParentDescription;
            allergy.ParentID = 0;
            allergy.ParentTag = "0";
            allergy.CreatedBy = AddType.allergy.CreatedBy;
            allergy.CreatedUTC = DateTime.UtcNow;
            allergy.IsActive = true;
            allergy.IsDeleted = false;
            WYNKContext.allergy.AddRange(allergy);

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
        public dynamic allergyrecord()
        {
            var getData = new AllergyViewmodel();

            getData.allergyhis = (from details in WYNKContext.allergy.Where(x => x.IsDeleted == false && x.ParentID == 0)
                                      select new allergyhis
                                      {
                                          ID = details.ID,
                                          Description = details.ParentDescription,
                                          IsActive = details.IsActive == true ? "Active" : "InActive",
                                          Active = details.IsActive,
                                      }).ToList();

            return getData;
        }
        public dynamic updateallergy(AllergyViewmodel UpType, int IDT)
        {

            var allergy = new allergy();

            allergy = WYNKContext.allergy.Where(x => x.ID == IDT).FirstOrDefault();

            allergy.ParentDescription = UpType.allergy.ParentDescription;
            allergy.IsActive = UpType.allergy.IsActive;
            allergy.UpdatedBy = UpType.allergy.UpdatedBy;
            allergy.UpdatedUTC = DateTime.UtcNow;
            WYNKContext.allergy.UpdateRange(allergy);

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
        public dynamic Deleteallergy(int IDT)
        {
            var stoMas = WYNKContext.allergy.Where(x => x.ID == IDT).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = false; return true; });
                WYNKContext.allergy.UpdateRange(stoMas);

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
        public dynamic InsertDescription(AllergyViewmodel AddDescription)
        {
            var allergy = new allergy();

            allergy.ParentDescription = AddDescription.allergy.ParentDescription;
            allergy.ParentID = AddDescription.allergy.ParentID;
            allergy.ParentTag = WYNKContext.allergy.Where(x => x.ID == AddDescription.allergy.ParentID).Select(x => x.ParentDescription).FirstOrDefault();
            allergy.CreatedBy = AddDescription.allergy.CreatedBy;
            allergy.CreatedUTC = DateTime.UtcNow;
            allergy.IsActive = true;
            allergy.IsDeleted = false;
            WYNKContext.allergy.AddRange(allergy);

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
        public dynamic Descriptionrecord(int DescID)
        {
            var getData = new AllergyViewmodel();

            getData.Descriptionhis = (from details in WYNKContext.allergy.Where(x => x.IsDeleted == false && x.ParentID == DescID)
                                      select new Descriptionhis
                                      {
                                          ID = details.ID,
                                          Description = details.ParentDescription,
                                          IsActive = details.IsActive == true ? "Active" : "InActive",
                                          Active = details.IsActive,
                                      }).ToList();

            return getData;
        }
        public dynamic updateDescription(AllergyViewmodel UpDescription, int IDD)
        {

            var allergy = new allergy();

            allergy = WYNKContext.allergy.Where(x => x.ID == IDD).FirstOrDefault();

            allergy.ParentDescription = UpDescription.allergy.ParentDescription;
            allergy.IsActive = UpDescription.allergy.IsActive;
            allergy.UpdatedBy = UpDescription.allergy.UpdatedBy;
            allergy.UpdatedUTC = DateTime.UtcNow;
            WYNKContext.allergy.UpdateRange(allergy);

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
        public dynamic DeleteDescription(int IDD)
        {
            var stoMas = WYNKContext.allergy.Where(x => x.ID == IDD).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = false; return true; });
                WYNKContext.allergy.UpdateRange(stoMas);

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














