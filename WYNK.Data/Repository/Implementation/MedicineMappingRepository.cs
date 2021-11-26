
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class MedicineMappingRepository : RepositoryBase<MedicineMapping>, IMedicineMappingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public MedicineMappingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public MedicineMapping Getvalues()
        {

            var memap = new MedicineMapping();
            memap.AlternativeDrug = new List<AlternativeDrug>();

            return memap;
        }


        public MedicineMapping Getchilddrug(int drugid, int cmpid)
        {

            var memap = new MedicineMapping();
            var dname = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.Brand).FirstOrDefault();
            var dgrp = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.DrugGroup).FirstOrDefault();

            var dchildid = WYNKContext.AlternativeDrug.Where(x => x.DrugID == drugid).Select(x => x.DrugMappingID).ToList();


            var drug = WYNKContext.DrugMaster.ToList();

            memap.Drgchld = (from DG in drug.Where(u => u.ID != drugid && u.DrugGroup == dgrp && u.IsDeleted == false && u.IsActive == true)


                             select new Drgchld
                             {
                                 drugid = DG.ID,
                                 drugname = DG.Brand,

                             }).ToList();


            return memap;
        }

        public MedicineMapping Getrecdtls(int drugid, int cmpid)
        {

            var memap = new MedicineMapping();
            var dname = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.Brand).FirstOrDefault();
            var dgrp = WYNKContext.DrugMaster.Where(x => x.ID == drugid).Select(x => x.DrugGroup).FirstOrDefault();

            var drug = WYNKContext.DrugMaster.ToList();
            var altmed = WYNKContext.AlternativeDrug.ToList();

            memap.Drghis = (from AD in altmed.Where(u =>u.IsDeleted == false && u.IsActive == true && u.DrugID == drugid)
                            join DG in drug
                            on AD.DrugID equals DG.ID

                            select new Drghis
                            {
                                drugid = AD.DrugID,
                                drugchildid = AD.DrugMappingID,
                                drugname = DG.Brand,

                            }).ToList();


            return memap;
        }


        public dynamic InsertMedicineMapping(MedicineMapping MedicineMapping, int userid, int drugid)
        {

            

            if (MedicineMapping.AlternativeDrug.Count() > 0)
            {
                foreach (var item in MedicineMapping.AlternativeDrug.ToList())
                    

                {

                    var master = WYNKContext.AlternativeDrug.Where(x => x.DrugID == item.DrugID).ToList();
                    if (master != null)
                    {

                        WYNKContext.AlternativeDrug.RemoveRange(master);
                        WYNKContext.SaveChanges();
                    }


                    var AltDrg = new AlternativeDrug();


                    AltDrg.DrugID = item.DrugID;
                    AltDrg.DrugMappingID = item.DrugMappingID;
                    AltDrg.CMPID = item.CMPID;
                    AltDrg.IsActive = true;
                    AltDrg.IsDeleted = false;
                    AltDrg.CreatedUTC = DateTime.UtcNow;
                    AltDrg.CreatedBy = userid;
                    WYNKContext.AlternativeDrug.AddRange(AltDrg);

                }
            }

            WYNKContext.SaveChanges();

            

            try
            {
                if (WYNKContext.SaveChanges() >= 0)
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully"
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






