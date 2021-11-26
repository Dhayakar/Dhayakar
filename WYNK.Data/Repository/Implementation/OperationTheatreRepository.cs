using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;


namespace WYNK.Data.Repository.Implementation
{
    class OperationTheatreRepository : RepositoryBase<OperationTheatres>, IOperationTheatreRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public OperationTheatreRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }





        public dynamic Insertoperation(OperationTheatres Operation)

        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {


                try
                {

                    var operationThe = new OperationTheatre();
                    operationThe.OTDescription = Operation.OperationTheatre.OTDescription;
                    operationThe.OTAddress1 = Operation.OperationTheatre.OTAddress1;
                    operationThe.OTAddress2 = Operation.OperationTheatre.OTAddress2;
                    operationThe.OTAddress3 = Operation.OperationTheatre.OTAddress3;
                    operationThe.OTLocation = Operation.OperationTheatre.OTLocation;
                    operationThe.OTCharge = Operation.OperationTheatre.OTCharge;
                    operationThe.GST = Operation.OperationTheatre.GST;
                    operationThe.CGST = Operation.OperationTheatre.CGST;
                    operationThe.SGST = Operation.OperationTheatre.SGST;
                    operationThe.CMPID = Operation.OperationTheatre.CMPID;
                    operationThe.CreatedBy = Operation.OperationTheatre.CreatedBy;
                    operationThe.CreatedUTC = DateTime.UtcNow;
                    operationThe.IsActive = true;
                    WYNKContext.OperationTheatre.AddRange(operationThe);
                    WYNKContext.SaveChanges();

                    if (Operation.OperationExtension.Count() > 0)
                    {
                        foreach (var item in Operation.OperationExtension.ToList())

                        {

                            var opten = new OperationExtension();

                            opten.OTID = operationThe.OTID;
                            opten.ICDCODE = item.ICDCODE;
                            opten.SpecialityDescription = item.SpecialityDescription;
                            opten.IsDeleted = false;
                            opten.CreatedUTC = DateTime.UtcNow;
                            opten.CreatedBy = operationThe.CreatedBy;
                            WYNKContext.OperationTheatreExtension.AddRange(opten);
                        }

                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();


                    return new
                    {

                        Success = true,
                        Message = "Saved successfully",
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"
                };
            }

        }

        public dynamic Updateoperation(OperationTheatres Operationup, int ID)
        {

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {


                try
                {

                    var operationThe = new OperationTheatre();

                    operationThe = WYNKContext.OperationTheatre.Where(x => x.OTID == ID).FirstOrDefault();

                    operationThe.OTDescription = Operationup.OperationTheatre.OTDescription;
                    operationThe.OTAddress1 = Operationup.OperationTheatre.OTAddress1;
                    operationThe.OTAddress2 = Operationup.OperationTheatre.OTAddress2;
                    operationThe.OTAddress3 = Operationup.OperationTheatre.OTAddress3;
                    operationThe.OTLocation = Operationup.OperationTheatre.OTLocation;
                    operationThe.OTCharge = Operationup.OperationTheatre.OTCharge;
                    operationThe.GST = Operationup.OperationTheatre.GST;
                    operationThe.CGST = Operationup.OperationTheatre.CGST;
                    operationThe.SGST = Operationup.OperationTheatre.SGST;
                    operationThe.IsActive = Operationup.OperationTheatre.IsActive;
                    operationThe.UpdatedUTC = DateTime.UtcNow;
                    operationThe.UpdatedBy = Operationup.OperationTheatre.UpdatedBy;
                    WYNKContext.OperationTheatre.UpdateRange(operationThe);
                    WYNKContext.SaveChanges();

                    if (Operationup.OperationExtension != null)
                    {
                        var ids = WYNKContext.OperationTheatreExtension.Where(x => x.OTID == ID).ToList();
                        foreach (var item in ids.ToList())
                        {
                            if (ids != null && ids.Count != 0)
                            {
                                ids.All(x => { x.OTID = item.OTID; return true; });
                                var idsss = WYNKContext.OperationTheatreExtension.Where(x => x.OTID == ID).ToList();
                                if (idsss.Count != 0)
                                {
                                    WYNKContext.OperationTheatreExtension.RemoveRange(ids);
                                    WYNKContext.SaveChanges();
                                }
                            }
                        }

                        var ids1 = WYNKContext.OperationTheatreExtension.Where(x => x.OTID == ID).ToList();

                        if (ids1.Count == 0)
                        {
                            foreach (var item in Operationup.OperationExtension.ToList())
                            {
                                var opten = new OperationExtension();
                                opten.OTID = operationThe.OTID;
                                opten.ICDCODE = item.ICDCODE;
                                opten.SpecialityDescription = item.SpecialityDescription;
                                opten.IsDeleted = false;
                                opten.CreatedUTC = DateTime.UtcNow;
                                opten.CreatedBy = operationThe.CreatedBy;
                                WYNKContext.OperationTheatreExtension.AddRange(opten);
                            }
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();


                    return new
                    {

                        Success = true,
                        Message = "Saved successfully",
                    };
                }

                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"
                };
            }

        }

        public dynamic Deleteoperation(int ID)
        {


            var stoMas = WYNKContext.OperationTheatre.Where(x => x.OTID == ID).ToList();



            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                WYNKContext.OperationTheatre.UpdateRange(stoMas);

            }

            var opt = WYNKContext.OperationTheatreExtension.Where(x => x.OTID == ID).ToList();

            if (opt != null)
            {
                opt.All(x => { x.IsDeleted = true; return true; });
                WYNKContext.OperationTheatreExtension.UpdateRange(opt);
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
