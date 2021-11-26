using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;



namespace WYNK.Data.Repository.Implementation
{
    class IndentRepository : RepositoryBase<IndentViewModel>, IIndentRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public IndentRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic Insertindent(IndentViewModel Indentdata)
        {
            var DD = new IndentViewModel();
            var Indata = new OTIndentModel();
            var indtran = new OTIndenttranModel();

            var Indentrunningnumber = CMPSContext.NumberControl.Where(x => x.Description == "Indent").Select(x => x.RunningNumber).FirstOrDefault();
            var Indentprefi = CMPSContext.NumberControl.Where(x => x.Description == "Indent").Select(x => x.Prefix).FirstOrDefault();
            var Insuffix = CMPSContext.NumberControl.Where(x => x.Description == "Indent").Select(x => x.Suffix).FirstOrDefault();
            if (Indentrunningnumber != null)
            {
                using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ir = Indentrunningnumber + 1;


                        Indata.CreatedBy = Convert.ToInt32(Indentdata.Userid);
                        Indata.CreatedUTC = DateTime.UtcNow;
                        Indata.IndentRunningNumber = Indentprefi + ir + Insuffix;
                        Indata.IndentRaisedBy = Indentdata.Indentdetails.OTRaisedBY;
                        Indata.IsCancelled = false;
                        Indata.StoreID = CMPSContext.Storemasters.Where(x => x.Storename == Indentdata.Indentdetails.Storename).Select(x => x.StoreID).FirstOrDefault(); ;
                        var opid = WYNKContext.OperationTheatre.Where(x => x.OTDescription == Indentdata.Indentdetails.OperationtheatreID).Select(x => x.OTID).FirstOrDefault();
                        Indata.OTID = Convert.ToInt32(Indentdata.Indentdetails.OperationtheatreID);
                        Indata.OTIndentDate = Convert.ToDateTime(Indentdata.Indentdetails.OTIndentdate);
                        Indata.TCID = Convert.ToInt32(Indentdata.transactionid);
                        WYNKContext.OTIndent.Add(Indata);
                        WYNKContext.SaveChanges();

                        var indid = WYNKContext.OTIndent.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();


                        var rn = new Number_Control();
                        rn = CMPSContext.NumberControl.Where(x => x.Description == "Indent").FirstOrDefault();
                        rn.RunningNumber = ir;
                        rn.IsActive = true;
                        rn.IsDeleted = false;
                        CMPSContext.Entry(rn).State = EntityState.Modified;
                        CMPSContext.SaveChanges();
                        

                        foreach (var griditem in Indentdata.GridDeatils)
                        {

                            var id = WYNKContext.OTIndentTran.OrderByDescending(x => x.CreatedUTC).Select(x => x.ID).FirstOrDefault();
                            if (id != null)
                            {
                                indtran.ID = id + 1;
                            }
                            else
                            {
                                indtran.ID = 1;
                            }
                            indtran.CreatedBy = Convert.ToInt32(Indentdata.Userid);
                            indtran.OTIndentID = indid;
                            
                            var UOMID = CMPSContext.uommaster.Where(x => x.Description == griditem.UOM).Select(x => x.id).FirstOrDefault();


                            indtran.UOMID = UOMID;
                            indtran.ReceivedQty = null;
                            indtran.IndentQty = Convert.ToInt32(griditem.IndentQty);
                            var Drudig = WYNKContext.DrugMaster.Where(x => x.Brand == griditem.Brand).Select(x => x.ID).FirstOrDefault();
                            indtran.DrugID = Drudig;
                            indtran.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.OTIndentTran.Add(indtran);
                            WYNKContext.SaveChanges();

                        }
                        dbContextTransaction.Commit();
                        return new
                        {
                            Success = true,
                            Message = "Done",
                            Indentiddd = Indata.IndentRunningNumber,
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
                        Message = "Something Went Wrong"
                    };

                }
            }
            else
            {
                DD.message = "No";
                return DD;
            }

           

         
        }




        public dynamic GetdrugDetails (int id)
        {
            var Retun = new IndentViewModel();
            Retun.Indentdetails = new IndentTotalData();
            Retun.DrugssDetailsss = (from drug in WYNKContext.DrugMaster.Where(u => u.ID == id)

                                                 select new DrugssDetailss
                                                 {
                                                     ID = drug.ID,
                                                     Drugdroup = drug.DrugGroup,
                                                     Brand = drug.Brand,
                                                     Manufacture = CMPSContext.OneLineMaster.Where(x => x.OLMID == Convert.ToInt32(drug.Manufacturer)).Select(x => x.ParentDescription).FirstOrDefault(),
                                                     Generic = WYNKContext.DrugGroup.Where(x => x.ID == drug.GenericName).Select(x => x.Description).FirstOrDefault(),
                                                     UOM = drug.UOM,
                                                     OTRAISEDDATE =  DateTime.Now.Date,
                                                 }).ToList();




            return Retun;
        }


        public dynamic updateindent(IndentViewModel Indentdata)
        {
            var DD = new IndentViewModel();
            var Indata = new OTIndentModel();
            var indtran = new OTIndenttranModel();



            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var OtIndentID = WYNKContext.OTIndent.Where(x => x.IndentRunningNumber == Indentdata.indentnumbers).Select(x => x.ID).FirstOrDefault();


                    foreach (var griditem in Indentdata.GridDeatils)
                    {

                        var id = WYNKContext.OTIndentTran.OrderByDescending(x => x.CreatedUTC).Select(x => x.ID).FirstOrDefault();
                        if (id != null)
                        {
                            indtran.ID = id + 1;
                        }
                        else
                        {
                            indtran.ID = 1;
                        }
                        indtran.CreatedBy = Convert.ToInt32(Indentdata.Userid);
                        indtran.OTIndentID = OtIndentID;

                        var UOMID = CMPSContext.uommaster.Where(x => x.Description == griditem.UOM).Select(x => x.id).FirstOrDefault();


                        indtran.UOMID = UOMID;
                        indtran.ReceivedQty = null;
                        indtran.IndentQty = Convert.ToInt32(griditem.IndentQty);
                        var Drudig = WYNKContext.DrugMaster.Where(x => x.Brand == griditem.Brand).Select(x => x.ID).FirstOrDefault();
                        indtran.DrugID = Drudig;
                        indtran.CreatedUTC = DateTime.UtcNow;
                        WYNKContext.OTIndentTran.Add(indtran);
                        WYNKContext.SaveChanges();

                    }
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Done",                      
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
                    Message = "Something Went Wrong"
                };

            }



            return DD;
        }


        }
}














