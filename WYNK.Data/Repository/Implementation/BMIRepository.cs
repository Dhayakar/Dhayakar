
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class BMIRepository : RepositoryBase<BMIViewM>, IBMIRepository  
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public BMIRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertData(BMIViewM BMI)
        {
            try
            {
                var bmi = new BMI();
                bmi.Category = BMI.BMI.Category;
                bmi.FromRange = BMI.BMI.FromRange;
                bmi.ToRange = BMI.BMI.ToRange;
                bmi.CreatedUTC = DateTime.UtcNow;
                bmi.CreatedBy = BMI.BMI.CreatedBy;
                WYNKContext.BMI.AddRange(bmi);
                try
                {
                    if (WYNKContext.SaveChanges() > 0)
                        return new
                        {
                            Success = true,
                            Message = "Added successfully"
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


            public dynamic UpdateData(BMIViewM BMI, int ID)
        {
            try
            {
               
                 var bmi = new BMI();
                bmi = WYNKContext.BMI.Where(x => x.ID == ID).FirstOrDefault();
                bmi.Category = BMI.BMI.Category;
                bmi.FromRange = BMI.BMI.FromRange;
                bmi.ToRange = BMI.BMI.ToRange;
                bmi.UpdatedUTC = DateTime.UtcNow;
                bmi.UpdatedBy = BMI.BMI.UpdatedBy;
                WYNKContext.BMI.UpdateRange(bmi);

                try
                {
                    if (WYNKContext.SaveChanges() > 0)
                        return new
                        {
                            Success = true,
                            Message = "update successfully"
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
}
}
