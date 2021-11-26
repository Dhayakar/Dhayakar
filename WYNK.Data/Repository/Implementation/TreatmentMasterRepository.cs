using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class TreatmentMasterRepository:RepositoryBase<TreatmentMasterDataView>,ITreatmentMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public TreatmentMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertTreatment(TreatmentMasterDataView Con)
        {
            var Trat = WYNKContext.TreatmentMaster.ToList();

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    foreach(var Arrays in Con.TableArray.ToList())
                    {
                        var TreatmentMaster = new TreatmentMasterModel();

                        TreatmentMaster.ICD_SPECIALITY_ID = Arrays.SpecialityID;
                        TreatmentMaster.TreatmentDescription = Arrays.Description;
                        TreatmentMaster.IsActive = true;
                        TreatmentMaster.CreatedUTC = DateTime.UtcNow;
                        TreatmentMaster.UpdatedUTC = null;
                        TreatmentMaster.CreatedBy = Con.TreatmentMasterModel.CreatedBy;
                        TreatmentMaster.UpdatedBy = null;
                        WYNKContext.TreatmentMaster.AddRange(TreatmentMaster);
                        WYNKContext.SaveChanges();
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)

                            return new
                            {

                                Success = true,
                                Message = CommonMessage.saved,

                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }

            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };
        }


        public TreatmentMasterDataView PopupSearch()
        {
            var treatment = WYNKContext.TreatmentMaster.ToList();
            var splode = WYNKContext.ICDSpecialityCode.ToList();

            var PopupSearch = new TreatmentMasterDataView();
            PopupSearch.GetDataArray = new List<GetDataArray>();
            PopupSearch.GetDataArray = (from TM in treatment.Where(x => x.IsActive == true)
                                        join SM in splode on
                                        TM.ICD_SPECIALITY_ID equals SM.ID

                                        select new GetDataArray
                                        {
                                            ID=TM.ID,
                                            SpecialityID1 = SM.SpecialityDescription,
                                            Description1 = TM.TreatmentDescription,
                                            IsActive1 = TM.IsActive,

                                     }).ToList();
            return PopupSearch;
        }


        public dynamic UpdateTratment(TreatmentMasterDataView Cons)
        {
            var Trat = WYNKContext.TreatmentMaster.ToList();

            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var Arrays in Cons.TableArray.ToList())
                    {
                        var TreatmentMaster = new TreatmentMasterModel();
                        TreatmentMaster = Trat.Where(x => x.ID == Arrays.ID).FirstOrDefault();
                        TreatmentMaster.ICD_SPECIALITY_ID = Arrays.SpecialityID;
                        TreatmentMaster.TreatmentDescription = Arrays.Description;
                        TreatmentMaster.IsActive = Arrays.IsActive;
                        TreatmentMaster.UpdatedUTC = DateTime.UtcNow;
                        TreatmentMaster.UpdatedBy = Cons.TreatmentMasterModel.UpdatedBy;
                        WYNKContext.Entry(TreatmentMaster).State = EntityState.Modified;
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)

                            return new
                            {

                                Success = true,
                                Message = CommonMessage.saved,

                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
            }

            return new
            {
                Success = false,
                Message = CommonMessage.Missing,
            };
        }


        public dynamic DeleteTreatment(TreatmentMasterDataView Cons1, int ID)
        {
                    var Treatments = WYNKContext.TreatmentMaster.ToList();

                    foreach (var Arrays in Cons1.TableArray.ToList())
                    {
                            var Treat = new TreatmentMasterModel();
                            Treat = Treatments.Where(x => x.ID == Arrays.ID).FirstOrDefault();
                            Treat.IsActive = false;
                            Treat.IsDeleted = true;
                            Treat.UpdatedUTC = DateTime.UtcNow;
                            WYNKContext.TreatmentMaster.UpdateRange(Treat);
                            WYNKContext.SaveChanges();
                    }

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Delete Successfully"
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                return new
                {
                    Success = false,
                    Message = CommonMessage.Missing,
                };
        }
    }
}
