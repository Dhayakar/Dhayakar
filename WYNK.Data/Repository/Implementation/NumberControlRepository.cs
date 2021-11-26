using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;



namespace WYNK.Data.Repository.Implementation
{
    class NumberControlRepository : RepositoryBase<NumberControlViewModel>, INumberControlRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public NumberControlRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public NumberControlViewModel getNumberControl(int CmpID, int Description)
        {
            var NumberControl = new NumberControlViewModel();
            var test = CMPSContext.NumberControl.Where(x => x.TransactionID == Description).FirstOrDefault();
            return NumberControl;

        }


        public dynamic InsertNum(NumberControlViewModel Numcol)

        {
            try
            {
                if (Numcol.NumColgrid.Count != 0)                 
                {
                    foreach (var item in Numcol.NumColgrid.ToList())
                    {
                        var NumberControl = CMPSContext.NumberControl.Where(x => x.VCID == item.VCID).FirstOrDefault();
                        NumberControl.Prefix = item.Prefix;
                        NumberControl.Suffix = item.Suffix;
                        if (item.Autonumber == "Yes") {NumberControl.Autonumber = true;}
                        else { NumberControl.Autonumber = false; }
                        NumberControl.RunningNumber = item.RunningNumber;
                        NumberControl.EffectiveFrom = item.EffectiveFrom;
                        if (item.IsActive == "Yes") { NumberControl.IsActive = true;}
                        else { NumberControl.IsActive = false; }
                        NumberControl.UpdatedUTC = DateTime.UtcNow;
                        NumberControl.UpdatedBy = Numcol.NumberControl.CreatedBy;
                        CMPSContext.Entry(NumberControl).State = EntityState.Modified;
                        CMPSContext.SaveChanges();
                    }
                    
                        return new
                        {
                            Success = true,
                        };
                }
                else
                {
                    var vcid = CMPSContext.NumberControl.Where(x => x.TransactionID == Numcol.NumberControl.TransactionID && x.EffectiveTo == null && x.IsActive == true && x.IsDeleted == false && x.CmpID == Numcol.NumberControl.CmpID).Select(x => x.VCID).FirstOrDefault();
                    if (vcid == 0)
                    {
                        var NumberControl = new Number_Control();
                        NumberControl.TransactionID = Numcol.NumberControl.TransactionID;
                        NumberControl.CmpID = Numcol.NumberControl.CmpID;
                        NumberControl.Prefix = Numcol.NumberControl.Prefix;
                        NumberControl.Suffix = Numcol.NumberControl.Suffix;
                        NumberControl.Autonumber = true;
                        NumberControl.RunningNumber = Numcol.NumberControl.RunningNumber;
                        NumberControl.EffectiveFrom = Numcol.NumberControl.EffectiveFrom.AddDays(1);
                        NumberControl.Description = Numcol.NumberControl.Description;
                        NumberControl.IsActive = Numcol.NumberControl.IsActive;
                        NumberControl.IsDeleted = false;
                        NumberControl.CreatedUTC = DateTime.UtcNow;
                        NumberControl.CreatedBy = Numcol.NumberControl.CreatedBy;
                        CMPSContext.NumberControl.Add(NumberControl);
                    }
                    else
                    {
                        var NumberControl = new Number_Control();
                        NumberControl = CMPSContext.NumberControl.Where(x => x.VCID == vcid).FirstOrDefault();
                        NumberControl.TransactionID = Numcol.NumberControl.TransactionID;
                        NumberControl.CmpID = Numcol.NumberControl.CmpID;
                        NumberControl.Prefix = Numcol.NumberControl.Prefix;
                        NumberControl.Suffix = Numcol.NumberControl.Suffix;
                        NumberControl.Autonumber = Numcol.NumberControl.Autonumber;
                        NumberControl.RunningNumber = Numcol.NumberControl.RunningNumber;
                        NumberControl.EffectiveFrom = Numcol.NumberControl.EffectiveFrom;
                        NumberControl.EffectiveTo = null;
                        NumberControl.Description = Numcol.NumberControl.Description;
                        NumberControl.IsActive = Numcol.NumberControl.IsActive;
                        NumberControl.UpdatedUTC = DateTime.UtcNow;
                        NumberControl.UpdatedBy = Numcol.NumberControl.UpdatedBy;
                        CMPSContext.Entry(NumberControl).State = EntityState.Modified;
                    }
                }

                if (CMPSContext.SaveChanges() > 0)
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




        public dynamic UpdateNum(NumberControlViewModel Numcol, int VCID)

        {
            try
            {
               
                var NumberControl = new Number_Control();
                NumberControl = CMPSContext.NumberControl.Where(x => x.VCID == VCID).FirstOrDefault();
                NumberControl.TransactionID = Numcol.NumberControl.TransactionID;
                NumberControl.CmpID = Numcol.NumberControl.CmpID;
                NumberControl.Prefix = Numcol.NumberControl.Prefix;
                NumberControl.Suffix = Numcol.NumberControl.Suffix;
                NumberControl.Autonumber = Numcol.NumberControl.Autonumber;
                NumberControl.RunningNumber = Numcol.NumberControl.RunningNumber;
                NumberControl.EffectiveFrom = Numcol.NumberControl.EffectiveFrom;
                if (Numcol.NumberControl.EffectiveTo == null)
                {

                }
                else
                {
                    NumberControl.EffectiveTo = Numcol.NumberControl.EffectiveTo.Value.AddDays(1);
                }

                NumberControl.Description = Numcol.NumberControl.Description;
                NumberControl.IsActive = Numcol.NumberControl.IsActive;
                NumberControl.UpdatedUTC = DateTime.UtcNow;
                NumberControl.UpdatedBy = Numcol.NumberControl.UpdatedBy;
                CMPSContext.Entry(NumberControl).State = EntityState.Modified;


                if (CMPSContext.SaveChanges() > 0)
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

        public dynamic DeleteNum(NumberControlViewModel Numcol, int VCID)

        {
            try
            {
                var NumberControl = new Number_Control();
                NumberControl = CMPSContext.NumberControl.Where(x => x.VCID == VCID).FirstOrDefault();
                NumberControl.IsDeleted = true;
                CMPSContext.Entry(NumberControl).State = EntityState.Modified;


                if (CMPSContext.SaveChanges() > 0)
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



        public NumberControlViewModel getgridNC(int CmpID)
        {
            var NumberControl = new NumberControlViewModel();


            NumberControl.NumColgrid = (from NumCol in CMPSContext.NumberControl.Where(x => x.IsDeleted == false && x.CmpID == CmpID && x.RunningNumber == 0)

                                 select new NumColgrid
                                 {
                                     VCID = NumCol.VCID,
                                     TransactionID = NumCol.TransactionID,
                                     DepartmentID = NumCol.DepartmentID,
                                     CmpID = NumCol.CmpID,
                                     Prefix = NumCol.Prefix,
                                     Suffix = NumCol.Suffix,
                                     Description = NumCol.Description,
                                     RunningNumber = NumCol.RunningNumber,
                                     EffectiveFrom = NumCol.EffectiveFrom,
                                     EffectiveTo = NumCol.EffectiveTo,
                                     Autonumber = Enum.GetName(typeof(ISactive), NumCol.Autonumber),
                                     IsActive = Enum.GetName(typeof(ISactive), NumCol.IsActive),
                                     IsDeleted = Enum.GetName(typeof(ISactive), NumCol.IsDeleted),
                               
                                 }

                ).ToList();
            return NumberControl;

        }


    }
}