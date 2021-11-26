
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class DiagnosisVSMedicineRepository : RepositoryBase<diagnosisvsmedicine>, IDiagnosisVSMedicineRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       
        public DiagnosisVSMedicineRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public diagnosisvsmedicine Getdruggvalues(int cmpid)
        {

            var diamed = new diagnosisvsmedicine();
            diamed.DiagnosisVSMedicine = new List<DiagnosisVSMedicine>();
                       
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var druggroup = WYNKContext.DrugGroup.ToList();

            diamed.Specialitydetialsdm = (from DM in drugmaster.Where(u => u.Cmpid == cmpid && u.IsActive == true && u.IsDeleted == false)
                             join DG in druggroup on DM.GenericName equals DG.ID


                             select new Specialitydetialsdm
                             {
                                            drugid = DM.ID,
                                            Itemdescription = DM.Brand,
                                            genericid = DG.ID,
                                            genericname = DG.Description,

                             }).ToList();


            return diamed;
        }

        public diagnosisvsmedicine GetSelectedmeddetials(int ID, int cmpid)
        {

            var diamed = new diagnosisvsmedicine();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var druggroup = WYNKContext.DrugGroup.ToList();
            var v = WYNKContext.DiagnosisVSMedicine.Where(x => x.SpecialityID == ID && x.IsActive == true && x.IsDeleted == false && x.CMPID == cmpid).ToList();
            IList<Specialitydetialsdm> Specialitydetialsdm = new List<Specialitydetialsdm>();


            foreach (var list in v)
            {
                var dd = new Specialitydetialsdm();
                dd.Itemdescription = drugmaster.Where(x => x.ID == list.DrugID && x.Cmpid == cmpid).Select(x => x.Brand).FirstOrDefault();
                dd.genericname = druggroup.Where(x => x.ID == list.GenericID && x.Cmpid == cmpid).Select(x => x.Description).FirstOrDefault();
                dd.Itemselect = true;
                Specialitydetialsdm.Add(dd);
            }

            var cpm = Specialitydetialsdm;
            diamed.Specialitydetialsdm = cpm;
            diamed.NONSpecialitydetialsdm = (from DM in drugmaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.Cmpid == cmpid).OrderBy(x => x.Brand)
                                             join DG in druggroup on DM.GenericName equals DG.ID
                                             where cpm.All(a => a.Itemdescription != DM.Brand)
                                           select new NONSpecialitydetialsdm
                                           {
                                               Itemdescription = DM.Brand,
                                               genericname = DG.Description,
                                               Itemselect = false,
                                           }).ToList();


            return diamed;
        }


        public diagnosisvsmedicine GetsubSelectedmeddetials(string ID, int cmpid)
        {

            var diamed = new diagnosisvsmedicine();
            var drugmaster = WYNKContext.DrugMaster.ToList();
            var druggroup = WYNKContext.DrugGroup.ToList();
            var v = WYNKContext.DiagnosisVSMedicine.Where(x => x.SubSpecialityID == ID && x.IsActive == true && x.IsDeleted == false && x.CMPID == cmpid).ToList();
            IList<Specialitydetialsdm> Specialitydetialsdm = new List<Specialitydetialsdm>();


            foreach (var list in v)
            {
                var dd = new Specialitydetialsdm();
                dd.Itemdescription = drugmaster.Where(x => x.ID == list.DrugID && x.Cmpid == cmpid).Select(x => x.Brand).FirstOrDefault();
                dd.genericname = druggroup.Where(x => x.ID == list.GenericID && x.Cmpid == cmpid).Select(x => x.Description).FirstOrDefault();
                dd.Itemselect = true;
                Specialitydetialsdm.Add(dd);
            }

            var cpm = Specialitydetialsdm;
            diamed.Specialitydetialsdm = cpm;
            diamed.NONSpecialitydetialsdm = (from DM in drugmaster.Where(x => x.IsActive == true && x.IsDeleted == false && x.Cmpid == cmpid).OrderBy(x => x.Brand)
                                             join DG in druggroup on DM.GenericName equals DG.ID
                                             where cpm.All(a => a.Itemdescription != DM.Brand)
                                             select new NONSpecialitydetialsdm
                                             {
                                                 Itemdescription = DM.Brand,
                                                 genericname = DG.Description,
                                                 Itemselect = false,
                                             }).ToList();


            return diamed;
        }


        public dynamic Insertdiagmeddata(diagnosisvsmedicine DiagnosisVSMedicine)
        {
            if (DiagnosisVSMedicine.SpecialityDetaildm.Count() != 0)
            {
                foreach (var item in DiagnosisVSMedicine.SpecialityDetaildm.ToList())
                {
                    var diavsmed = new DiagnosisVSMedicine();
                    var id = WYNKContext.DrugMaster.Where(x => x.Brand == item.Description && x.IsActive == true && x.IsDeleted == false && x.Cmpid == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var gid = WYNKContext.DrugGroup.Where(x => x.Description == item.Descriptionsub && x.IsDeleted == false && x.Cmpid == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    if (gid == null) {

                        gid = 0;
                    }

                    if (DiagnosisVSMedicine.SubCode == null)
                    {
                       
                    }
                    var itemid = WYNKContext.DiagnosisVSMedicine.OrderBy(x => x.CreatedUTC).Where(x => x.DrugID == id && x.SpecialityID == DiagnosisVSMedicine.Code && x.IsActive == true && x.SubSpecialityID == DiagnosisVSMedicine.SubCode && x.CMPID == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();


                    diavsmed.ID = itemid;
                    diavsmed.CMPID = Convert.ToInt32(DiagnosisVSMedicine.CompanyID);
                    diavsmed.DrugID = id;
                    diavsmed.GenericID = gid;

                    var specid = WYNKContext.DrugMaster.Where(x => x.ID == DiagnosisVSMedicine.Code && x.Cmpid == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();

                    diavsmed.SpecialityID = DiagnosisVSMedicine.Code;
                    diavsmed.SubSpecialityID = DiagnosisVSMedicine.SubCode;
                    diavsmed.IsActive = false;
                    diavsmed.IsDeleted = true;
                    diavsmed.CreatedUTC = DateTime.UtcNow;
                    diavsmed.UpdatedUTC = DateTime.UtcNow;
                    diavsmed.CreatedBy = Convert.ToInt32(DiagnosisVSMedicine.UserID);
                    diavsmed.UpdatedBy = null;

                    WYNKContext.DiagnosisVSMedicine.UpdateRange(diavsmed);
                    WYNKContext.SaveChanges();
                }
            }




            if (DiagnosisVSMedicine.SSpecialityDetaildm.Count() != 0)
            {
                foreach (var item in DiagnosisVSMedicine.SSpecialityDetaildm.ToList())
                {
                    
                    var ids = WYNKContext.DrugMaster.Where(x => x.Brand == item.Description && x.IsActive == true && x.IsDeleted == false && x.Cmpid == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();
                    var master = WYNKContext.DiagnosisVSMedicine.Where(x => x.DrugID == ids && x.IsActive == true && x.SpecialityID == DiagnosisVSMedicine.Code && x.CMPID == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).ToList();

                    if (master.Count != 0)
                    {

                        master.All(x =>
                        {
                            x.SubSpecialityID = DiagnosisVSMedicine.SubCode; return true;
                        });
                        WYNKContext.DiagnosisVSMedicine.UpdateRange(master);
                    }
                    else
                    {

                        var drugmas = new Drug_Master();
                        var druggrp = new Drug_Group();
                        var diavsmed = new DiagnosisVSMedicine();
                        var id = WYNKContext.DrugMaster.Where(x => x.Brand == item.Description && x.IsActive == true && x.IsDeleted == false && x.Cmpid == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();
                        var gid = WYNKContext.DrugGroup.Where(x => x.Description == item.Descriptionsub && x.IsDeleted == false && x.Cmpid == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();

                        diavsmed.CMPID = Convert.ToInt32(DiagnosisVSMedicine.CompanyID);
                        diavsmed.DrugID = id;
                        diavsmed.GenericID = gid;
                        var specid = WYNKContext.DiagnosisVSMedicine.Where(x => x.ID == DiagnosisVSMedicine.Code && x.CMPID == Convert.ToInt32(DiagnosisVSMedicine.CompanyID)).Select(x => x.ID).FirstOrDefault();

                        diavsmed.SpecialityID = DiagnosisVSMedicine.Code;
                        diavsmed.SubSpecialityID = DiagnosisVSMedicine.SubCode;
                        diavsmed.IsActive = true;
                        diavsmed.IsDeleted = false;
                        diavsmed.CreatedUTC = DateTime.UtcNow;
                        diavsmed.CreatedBy = Convert.ToInt32(DiagnosisVSMedicine.UserID);
                        WYNKContext.DiagnosisVSMedicine.Add(diavsmed);
                        WYNKContext.SaveChanges();
                    }
                }
            }



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






