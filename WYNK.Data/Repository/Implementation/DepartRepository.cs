
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    public class DepartRepository : RepositoryBase<DepartView>, IDepartRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public DepartRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic InsertPart(DepartView De)
        {
            var Dpart = new Department();
            Dpart.Description = De.Des.Description;
            Dpart.DepartmentIncharge = De.Des.DepartmentIncharge;
            Dpart.DepartmentLocation = De.Des.DepartmentLocation;
            Dpart.CreatedUTC = DateTime.UtcNow;
            Dpart.IsActive = CommonRepository.Active;
            Dpart.CMPID = De.Des.CMPID;
            Dpart.CreatedBy = De.Des.CreatedBy;
           
            CMPSContext.Department.Add(Dpart);
            CMPSContext.SaveChanges();
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved Successfully"
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
        

        public DepartView GetDepartDetail()
        {

            var ddetail = new DepartView();
            ddetail.DepartDetails = (from get in CMPSContext.Department.Where(x => x.IsDeleted == false)
                                     select new DepartDetails
                                     {

                                         ID = get.ID,
                                         Description = get.Description,
                                         DepartmentIncharge = get.DepartmentIncharge,
                                         DepartmentLocation = get.DepartmentLocation,
                                         IsActive = get.IsActive,
                                     
                                         

                                     }).ToList();
            

            return ddetail;


        }

        public dynamic UpdatePart(DepartView De1, int ID)
        {

            var Dpart1 = new Department();
            Dpart1 = CMPSContext.Department.Where(x => x.ID == ID).FirstOrDefault();
            Dpart1.Description = De1.Des.Description;
            Dpart1.DepartmentIncharge = De1.Des.DepartmentIncharge;
            Dpart1.DepartmentLocation = De1.Des.DepartmentLocation;
            Dpart1.IsActive = De1.Des.IsActive;
            Dpart1.CMPID = De1.Des.CMPID;
            Dpart1.UpdatedBy = De1.Des.UpdatedBy;
            Dpart1.UpdatedUTC = DateTime.UtcNow;
            CMPSContext.Department.UpdateRange(Dpart1);
            CMPSContext.SaveChanges();


            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Updated Successfully"
                    };

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return new
            {
                Succes = false,
                Message = "Some data are Missing"
            };
         
        }


        public dynamic deletepart(int? ID)
        {
            var Dpart2 = CMPSContext.Department.Where(x => x.ID == ID).ToList();
            if(Dpart2 != null)

            {

                Dpart2.All(x => { x.IsDeleted = true; x.IsActive = false; return true; });
                CMPSContext.Department.UpdateRange(Dpart2);


            }
            try
            {
                if (CMPSContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Delete successfully"
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







