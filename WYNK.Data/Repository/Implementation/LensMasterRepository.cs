
using System;
using System.Collections.Generic;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;
using WYNK.Data.Repository.Operation;

namespace WYNK.Data.Repository.Implementation
{
    class LensMasterRepository : RepositoryBase<LensMatserDataView>, ILensMasterRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;

        public LensMasterRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic GettaxDetails(int ID)
        {
            var TaxDetails = new LensMatserDataView();
            TaxDetails.Taxname = new List<Taxname>();


            TaxDetails.Taxname = (from REF in CMPSContext.TaxMaster.Where(u => u.ID == ID)
                                  select new Taxname
                                  {
                                      GSTNo = REF.GSTPercentage,
                                      AdditionalCesspercentage = REF.AdditionalCESSPercentage,
                                      Cesspercentage = REF.CESSPercentage,
                                      CessDescription = REF.CESSDescription,
                                      AddtionalDescription = REF.AdditionalCESSDescription

                                  }).ToList();

            return TaxDetails;

        }
        public dynamic Insertlensmaster(LensMatserDataView Addlens)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var lensmaster = new Lensmaster();

                    lensmaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string RandomUniqueID = lensmaster.RandomUniqueID;
                    lensmaster.LensType = Addlens.Lensmaster.LensType;
                    lensmaster.CMPID = Addlens.Lensmaster.CMPID;
                    lensmaster.CreatedUTC = DateTime.UtcNow;
                    lensmaster.CreatedBy = Addlens.Lensmaster.CreatedBy;
                    WYNKContext.Lensmaster.Add(lensmaster);


                    if (Addlens.LensTranModel.Count() > 0)
                    {
                        foreach (var item in Addlens.LensTranModel.ToList())
                        {
                            var match = false;

                            var LT = (from a in WYNKContext.Lenstrans.Where(e => e.LMID == WYNKContext.Lensmaster.Where(x => x.LensType == Addlens.Lensmaster.LensType && x.CMPID == Addlens.Lensmaster.CMPID).Select(x => x.RandomUniqueID).FirstOrDefault())

                                      select new
                                      {
                                          Brand = a.Brand,
                                          Sph = a.Sph,
                                          Cyl = a.Cyl,
                                          Axis = a.Axis,
                                          Add = a.Add,

                                      }).ToList();

                            var UPLT = (from a in Addlens.LensTranModel
                                        select new
                                        {
                                            Brand = a.Brand,
                                            Sph = a.Sph,
                                            Cyl = a.Cyl,
                                            Axis = a.Axis,
                                            Add = a.Add,

                                        }).ToList();

                            var Frame = (from a in WYNKContext.Lenstrans.Where(e => e.LMID == WYNKContext.Lensmaster.Where(x => x.LensType == Addlens.Lensmaster.LensType && x.CMPID == Addlens.Lensmaster.CMPID).Select(x => x.RandomUniqueID).FirstOrDefault())
                                         select new
                                         {
                                             Brand = a.Brand,
                                             FrameShapeID = a.FrameShapeID,
                                             FrameStyleID = a.FrameStyleID,
                                             FrameTypeID = a.FrameTypeID,
                                             FrameWidthID = a.FrameWidthID,
                                         }).ToList();

                            var UPFRAME = (from a in Addlens.LensTranModel
                                           select new
                                           {
                                               Brand = a.Brand,
                                               FrameShapeID = a.FrameShapeID,
                                               FrameStyleID = a.FrameStyleID,
                                               FrameTypeID = a.FrameTypeID,
                                               FrameWidthID = a.FrameWidthID,

                                           }).ToList();



                            if (Addlens.Lensmaster.LensType == "Frame")
                            {
                                if (Frame.SequenceEqual(UPFRAME))
                                {
                                    match = true;
                                }
                                else
                                {
                                    match = false;
                                }
                            }
                            else
                            {
                                if (LT.SequenceEqual(UPLT))
                                {
                                    match = true;
                                }
                                else
                                {
                                    match = false;
                                }

                            }



                            if (match == true)
                            {
                                continue;
                            }
                            else
                            {
                                var lenstrans = new LensTranModel();

                                lenstrans.LMID = RandomUniqueID;
                                lenstrans.Index = item.Index;
                                lenstrans.Costprice = item.Costprice;
                                lenstrans.Sptaxinclusive = item.Sptaxinclusive;
                                lenstrans.Model = item.Model;
                                lenstrans.Size = item.Size;
                                lenstrans.Colour = item.Colour;
                                lenstrans.Sph = item.Sph;
                                lenstrans.Cyl = item.Cyl;
                                lenstrans.Axis = item.Axis;
                                lenstrans.Add = item.Add;
                                lenstrans.Brand = item.Brand;
                                lenstrans.Prize = item.Prize;
                                lenstrans.CESSAmount = item.CESSAmount;
                                lenstrans.ADDCESSAmount = item.ADDCESSAmount;
                                lenstrans.UOMID = item.UOMID;
                                lenstrans.GST = item.GST;
                                lenstrans.HSNNo = item.HSNNo;
                                lenstrans.TaxID = item.TaxID;
                                lenstrans.Description = item.Description;
                                lenstrans.FrameShapeID = item.FrameShapeID;
                                lenstrans.FrameStyleID = item.FrameStyleID;
                                lenstrans.FrameTypeID = item.FrameTypeID;
                                lenstrans.FrameWidthID = item.FrameWidthID;
                                lenstrans.CreatedUTC = DateTime.UtcNow;
                                lenstrans.CreatedBy = lensmaster.CreatedBy;
                                lenstrans.IsActive = true;
                                WYNKContext.Lenstrans.AddRange(lenstrans);
                                WYNKContext.SaveChanges();
                            }




                        }
                    }


                    dbContextTransaction.Commit();

                    return new
                    {
                        Success = true,
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
                };
            }
        }
        public dynamic Getlensfull(string RandomUniqueID, int cmpid, string name)
        {
            var TaxDetails = new LensMatserDataView();
            var Brand = WYNKContext.Brand.ToList();
            var uom = CMPSContext.uommaster.ToList();
            var Tax = CMPSContext.TaxMaster.ToList();
            var one = CMPSContext.OneLineMaster.ToList();
            TaxDetails.Taxnamelensmastertrans = new List<Taxnamelensmastertrans>();

            TaxDetails.Taxnamelensmastertrans = (from REF in WYNKContext.Lenstrans.Where(x => x.LMID == WYNKContext.Lensmaster.Where(y => y.CMPID == cmpid && y.LensType == name).Select(y => y.RandomUniqueID).FirstOrDefault()).Where(x => x.LMID == RandomUniqueID && x.IsActive == true)
                                                 select new Taxnamelensmastertrans
                                                 {
                                                     ID = REF.ID,

                                                     Type = name,
                                                     Sph = REF.Sph != null ? REF.Sph : null,
                                                     Cyl = REF.Cyl != null ? REF.Cyl : null,
                                                     Axis = REF.Axis != null ? REF.Axis : null,
                                                     Add = REF.Add != null ? REF.Add : null,
                                                     Indexname = REF.Index != null ? one.Where(x => x.OLMID == Convert.ToInt32(REF.Index)).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                                     Index = REF.Index != null ? REF.Index : null,
                                                     Model = REF.Model != null ? REF.Model : null,
                                                     Size = REF.Size,
                                                     Colour = REF.Colour,
                                                     Brandname = Brand.Where(x => x.ID == REF.Brand).Select(x => x.Description).FirstOrDefault(),
                                                     Brand = REF.Brand,
                                                     Prize = REF.Prize,
                                                     Costprice = REF.Costprice,
                                                     Sptaxinclusive = REF.Sptaxinclusive,
                                                     CESSAmount = REF.CESSAmount != null ? REF.CESSAmount : null,
                                                     ADDCESSAmount = REF.ADDCESSAmount != null ? REF.ADDCESSAmount : null,
                                                     UOMname = uom.Where(x => x.id == REF.UOMID).Select(x => x.Description).FirstOrDefault(),
                                                     UOMID = REF.UOMID,
                                                     GST = REF.GST != null ? REF.GST : null,
                                                     HSNNo = REF.HSNNo != null ? REF.HSNNo : null,
                                                     TaxID = REF.TaxID != null ? REF.TaxID : null,
                                                     TaxDescription = REF.TaxID != null ? Tax.Where(x => x.ID == REF.TaxID).Select(x => x.TaxDescription).FirstOrDefault() : string.Empty,
                                                     CessDescription = REF.TaxID != null ? Tax.Where(x => x.ID == REF.TaxID).Select(x => x.CESSDescription).FirstOrDefault() : string.Empty,
                                                     AddtionalDescription = REF.TaxID != null ? Tax.Where(x => x.ID == REF.TaxID).Select(x => x.AdditionalCESSDescription).FirstOrDefault() : string.Empty,
                                                     Description = REF.Description != null ? REF.Description : null,
                                                     FrameShape = REF.FrameShapeID != null ? one.Where(x => x.OLMID == REF.FrameShapeID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                                     FrameStyle = REF.FrameStyleID != null ? one.Where(x => x.OLMID == REF.FrameStyleID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                                     FrameType = REF.FrameTypeID != null ? one.Where(x => x.OLMID == REF.FrameTypeID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                                     FrameWidth = REF.FrameWidthID != null ? one.Where(x => x.OLMID == REF.FrameWidthID).Select(x => x.ParentDescription).FirstOrDefault() : string.Empty,
                                                     FrameShapeID = REF.FrameShapeID != null ? REF.FrameShapeID : null,
                                                     FrameStyleID = REF.FrameStyleID != null ? REF.FrameStyleID : null,
                                                     FrameTypeID = REF.FrameTypeID != null ? REF.FrameTypeID : null,
                                                     FrameWidthID = REF.FrameWidthID != null ? REF.FrameWidthID : null,
                                                     IsActive = REF.IsActive,
                                                 }).ToList();

            return TaxDetails;

        }
        public dynamic Updatelensmaster(LensMatserDataView uplens, string ID, int doctorID)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    if (uplens.LensTranModel.Count() > 0)
                    {
                        foreach (var item in uplens.LensTranModel.ToList())
                        {
                            var match = false;

                            if (item.ID != 0 && uplens.Lensmaster.LensType == "Lens" || uplens.Lensmaster.LensType == "Contactlens")
                            {


                                var LT = (from a in WYNKContext.Lenstrans.Where(e => e.LMID == WYNKContext.Lensmaster.Where(x => x.LensType == uplens.Lensmaster.LensType && x.CMPID == uplens.Lensmaster.CMPID).Select(x => x.RandomUniqueID).FirstOrDefault()).OrderByDescending(x => x.ID)

                                          select new
                                          {
                                              ID = a.ID,
                                              Brand = a.Brand,
                                              Sph = a.Sph,
                                              Cyl = a.Cyl,
                                              Axis = a.Axis,
                                              Add = a.Add,
                                              Description = a.Description,
                                              Colour = a.Colour,
                                              Prize = a.Prize,
                                              TaxID = a.TaxID,
                                              Index = a.Index,
                                              Model = a.Model,
                                              HSNNO = a.HSNNo,

                                          }).ToList();

                                var UPLT = (from a in uplens.LensTranModel.OrderByDescending(x => x.ID)
                                            select new
                                            {
                                                ID = a.ID,
                                                Brand = a.Brand,
                                                Sph = a.Sph,
                                                Cyl = a.Cyl,
                                                Axis = a.Axis,
                                                Add = a.Add,
                                                Description = a.Description,
                                                Colour = a.Colour,
                                                Prize = a.Prize + 0.00m,
                                                TaxID = a.TaxID,
                                                Index = a.Index,
                                                Model = a.Model,
                                                HSNNO = a.HSNNo,

                                            }).ToList();

                                if (LT.SequenceEqual(UPLT))
                                {
                                    match = true;
                                }
                                else
                                {
                                    match = false;
                                }


                            }

                            if (item.ID == 0 && uplens.Lensmaster.LensType == "Lens" || uplens.Lensmaster.LensType == "Contactlens")
                            {


                                var LT = (from a in WYNKContext.Lenstrans.Where(e => e.LMID == WYNKContext.Lensmaster.Where(x => x.LensType == uplens.Lensmaster.LensType && x.CMPID == uplens.Lensmaster.CMPID).Select(x => x.RandomUniqueID).FirstOrDefault())

                                          select new
                                          {
                                              Brand = a.Brand,
                                              Sph = a.Sph,
                                              Cyl = a.Cyl,
                                              Axis = a.Axis,
                                              Add = a.Add,
                                              Description = a.Description,
                                              Colour = a.Colour,
                                              Prize = a.Prize,
                                              TaxID = a.TaxID,
                                              Index = a.Index,
                                              Model = a.Model,
                                              HSNNO = a.HSNNo,

                                          }).ToList();

                                var UPLT = (from a in uplens.LensTranModel
                                            select new
                                            {
                                                Brand = a.Brand,
                                                Sph = a.Sph,
                                                Cyl = a.Cyl,
                                                Axis = a.Axis,
                                                Add = a.Add,
                                                Description = a.Description,
                                                Colour = a.Colour,
                                                Prize = a.Prize + 0.00m,
                                                TaxID = a.TaxID,
                                                Index = a.Index,
                                                Model = a.Model,
                                                HSNNO = a.HSNNo,
                                            }).ToList();

                                if (LT.SequenceEqual(UPLT))
                                {
                                    match = true;
                                }
                                else
                                {
                                    match = false;
                                }

                            }

                            if (item.ID != 0 && uplens.Lensmaster.LensType == "Frame")
                            {

                                var Frame = (from a in WYNKContext.Lenstrans.Where(e => e.LMID == WYNKContext.Lensmaster.Where(x => x.LensType == uplens.Lensmaster.LensType && x.CMPID == uplens.Lensmaster.CMPID).Select(x => x.RandomUniqueID).FirstOrDefault())
                                             select new
                                             {
                                                 Brand = a.Brand,
                                                 FrameShapeID = a.FrameShapeID,
                                                 FrameStyleID = a.FrameStyleID,
                                                 FrameTypeID = a.FrameTypeID,
                                                 FrameWidthID = a.FrameWidthID,
                                                 Description = a.Description,
                                                 Colour = a.Colour,
                                                 Prize = a.Prize + 0.00m,
                                                 TaxID = a.TaxID,
                                                 Index = a.Index,
                                                 Model = a.Model,
                                                 HSNNO = a.HSNNo,
                                             }).ToList();

                                var UPFRAME = (from a in uplens.LensTranModel
                                               select new
                                               {
                                                   Brand = a.Brand,
                                                   FrameShapeID = a.FrameShapeID,
                                                   FrameStyleID = a.FrameStyleID,
                                                   FrameTypeID = a.FrameTypeID,
                                                   FrameWidthID = a.FrameWidthID,
                                                   Description = a.Description,
                                                   Colour = a.Colour,
                                                   Prize = a.Prize + 0.00m,
                                                   TaxID = a.TaxID,
                                                   Index = a.Index,
                                                   Model = a.Model,
                                                   HSNNO = a.HSNNo,

                                               }).ToList();

                                if (uplens.Lensmaster.LensType == "Frame")
                                {
                                    if (Frame.SequenceEqual(UPFRAME))
                                    {
                                        match = true;
                                    }
                                    else
                                    {
                                        match = false;
                                    }
                                }
                            }

                            if (item.ID == 0 && uplens.Lensmaster.LensType == "Frame")
                            {

                                var Frame = (from a in WYNKContext.Lenstrans.Where(e => e.LMID == WYNKContext.Lensmaster.Where(x => x.LensType == uplens.Lensmaster.LensType && x.CMPID == uplens.Lensmaster.CMPID).Select(x => x.RandomUniqueID).FirstOrDefault())
                                             select new
                                             {
                                                 Brand = a.Brand,
                                                 FrameShapeID = a.FrameShapeID,
                                                 FrameStyleID = a.FrameStyleID,
                                                 FrameTypeID = a.FrameTypeID,
                                                 FrameWidthID = a.FrameWidthID,
                                                 Description = a.Description,
                                                 Colour = a.Colour,
                                                 Prize = a.Prize,
                                                 TaxID = a.TaxID,
                                                 Model = a.Model,
                                             }).ToList();

                                var UPFRAME = (from a in uplens.LensTranModel
                                               select new
                                               {
                                                   Brand = a.Brand,
                                                   FrameShapeID = a.FrameShapeID,
                                                   FrameStyleID = a.FrameStyleID,
                                                   FrameTypeID = a.FrameTypeID,
                                                   FrameWidthID = a.FrameWidthID,
                                                   Description = a.Description,
                                                   Colour = a.Colour,
                                                   Prize = a.Prize,
                                                   TaxID = a.TaxID,
                                                   Model = a.Model,

                                               }).ToList();
                                if (uplens.Lensmaster.LensType == "Frame")
                                {
                                    if (Frame.SequenceEqual(UPFRAME))
                                    {
                                        match = true;
                                    }
                                    else
                                    {
                                        match = false;
                                    }
                                }
                            }


                            if (match == true)
                            {
                                continue;
                            }
                            else
                            {
                                var lenstrans = new LensTranModel();
                                var LTID = item.ID;

                                if (LTID != 0)
                                {
                                    lenstrans = WYNKContext.Lenstrans.Where(x => x.ID == item.ID).FirstOrDefault();
                                    lenstrans.LMID = ID;
                                    lenstrans.Sph = item.Sph;
                                    lenstrans.Cyl = item.Cyl;
                                    lenstrans.Axis = item.Axis;
                                    lenstrans.Add = item.Add;
                                    lenstrans.Index = item.Index;
                                    lenstrans.Model = item.Model;
                                    lenstrans.Size = item.Size;
                                    lenstrans.Colour = item.Colour;
                                    lenstrans.Brand = item.Brand;
                                    lenstrans.Prize = item.Prize;
                                    lenstrans.Sptaxinclusive = item.Sptaxinclusive;
                                    lenstrans.Costprice = item.Costprice;
                                    lenstrans.CESSAmount = item.CESSAmount;
                                    lenstrans.ADDCESSAmount = item.ADDCESSAmount;
                                    lenstrans.UOMID = item.UOMID;
                                    lenstrans.GST = item.GST;
                                    lenstrans.HSNNo = item.HSNNo;
                                    lenstrans.TaxID = item.TaxID;
                                    lenstrans.Description = item.Description;
                                    lenstrans.FrameShapeID = item.FrameShapeID;
                                    lenstrans.FrameStyleID = item.FrameStyleID;
                                    lenstrans.FrameTypeID = item.FrameTypeID;
                                    lenstrans.FrameWidthID = item.FrameWidthID;
                                    lenstrans.UpdatedUTC = DateTime.UtcNow;
                                    lenstrans.UpdatedBy = doctorID;
                                    WYNKContext.Lenstrans.UpdateRange(lenstrans);
                                }
                                else
                                {
                                    lenstrans.LMID = ID;
                                    lenstrans.Sph = item.Sph;
                                    lenstrans.Cyl = item.Cyl;
                                    lenstrans.Axis = item.Axis;
                                    lenstrans.Add = item.Add;
                                    lenstrans.Index = item.Index;
                                    lenstrans.Model = item.Model;
                                    lenstrans.Size = item.Size;
                                    lenstrans.Colour = item.Colour;
                                    lenstrans.Brand = item.Brand;
                                    lenstrans.Sptaxinclusive = item.Sptaxinclusive;
                                    lenstrans.Costprice = item.Costprice;
                                    lenstrans.Prize = item.Prize;
                                    lenstrans.CESSAmount = item.CESSAmount;
                                    lenstrans.ADDCESSAmount = item.ADDCESSAmount;
                                    lenstrans.UOMID = item.UOMID;
                                    lenstrans.GST = item.GST;
                                    lenstrans.HSNNo = item.HSNNo;
                                    lenstrans.TaxID = item.TaxID;
                                    lenstrans.IsActive = item.IsActive;
                                    lenstrans.Description = item.Description;
                                    lenstrans.FrameShapeID = item.FrameShapeID;
                                    lenstrans.FrameStyleID = item.FrameStyleID;
                                    lenstrans.FrameTypeID = item.FrameTypeID;
                                    lenstrans.FrameWidthID = item.FrameWidthID;
                                    lenstrans.CreatedUTC = DateTime.UtcNow;
                                    lenstrans.CreatedBy = doctorID;
                                    lenstrans.IsActive = true;
                                    WYNKContext.Lenstrans.AddRange(lenstrans);
                                }
                            }
                        }
                    }

                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return new
                    {
                        Success = true,
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
                };
            }
        }
        public dynamic Getframe(string Name, int cmpid)
        {

            var Details = new LensMatserDataView();

            Details.ResData = WYNKContext.Lensmaster.Where(x => x.LensType == Name && x.CMPID == cmpid).Select(x => x.RandomUniqueID).FirstOrDefault();
            return Details;
        }
        public dynamic InsertFrameShape(LensMatserDataView AddFrameShape)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = AddFrameShape.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameShape").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "FrameShape";
            onelinemaster.CreatedBy = AddFrameShape.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);

            try
            {
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
        public dynamic lensmasterFrameShape()
        {
            var getData = new LensMatserDataView();

            getData.FrameShapehis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "FrameShape" && x.IsDeleted == false && x.ParentID != 0)
                                     select new FrameShapehis
                                     {
                                         ID = details.OLMID,
                                         Description = details.ParentDescription,
                                         IsActive = details.IsActive == true ? "Active" : "InActive",
                                         Active = details.IsActive,
                                     }).ToList();

            return getData;
        }
        public dynamic updateFrameShape(LensMatserDataView UpFrameShape, int IDD)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDD).FirstOrDefault();

            onelinemaster.ParentDescription = UpFrameShape.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = UpFrameShape.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = UpFrameShape.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = UpFrameShape.OneLineMaster.IsActive;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
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
        public dynamic DeleteFrameShape(int IDD)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDD).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
        public dynamic InsertFrameType(LensMatserDataView AddFrameType)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = AddFrameType.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameType").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "FrameType";
            onelinemaster.CreatedBy = AddFrameType.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);




            try
            {
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
        public dynamic lensmasterFrameType()
        {
            var getData = new LensMatserDataView();

            getData.FrameTypehis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "FrameType" && x.IsDeleted == false && x.ParentID != 0)
                                    select new FrameTypehis
                                    {
                                        ID = details.OLMID,
                                        Description = details.ParentDescription,
                                        IsActive = details.IsActive == true ? "Active" : "InActive",
                                        Active = details.IsActive,
                                    }).ToList();

            return getData;
        }
        public dynamic updateFrameType(LensMatserDataView UpFrameType, int IDT)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDT).FirstOrDefault();

            onelinemaster.ParentDescription = UpFrameType.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = UpFrameType.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = UpFrameType.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = UpFrameType.OneLineMaster.IsActive;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
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
        public dynamic DeleteFrameType(int IDD)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDD).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
        public dynamic InsertFrameStyle(LensMatserDataView AddFrameStyle)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster.ParentDescription = AddFrameStyle.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameStyle").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "FrameStyle";
            onelinemaster.CreatedBy = AddFrameStyle.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);







            try
            {
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
        public dynamic lensmasterFrameStyle()
        {
            var getData = new LensMatserDataView();

            getData.FrameStylehis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "FrameStyle" && x.IsDeleted == false && x.ParentID != 0)
                                     select new FrameStylehis
                                     {
                                         ID = details.OLMID,
                                         Description = details.ParentDescription,
                                         IsActive = details.IsActive == true ? "Active" : "InActive",
                                         Active = details.IsActive,
                                     }).ToList();

            return getData;
        }
        public dynamic updateFrameStyle(LensMatserDataView UpFrameStyle, int IDS)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDS).FirstOrDefault();

            onelinemaster.ParentDescription = UpFrameStyle.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = UpFrameStyle.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = UpFrameStyle.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = UpFrameStyle.OneLineMaster.IsActive;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
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
        public dynamic DeleteFrameStyle(int IDS)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDS).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
        public dynamic InsertFrameWidth(LensMatserDataView AddFrameWidth)
        {

            var onelinemaster = new OneLine_Masters();
            onelinemaster.ParentDescription = AddFrameWidth.OneLineMaster.ParentDescription;
            onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameWidth").Select(x => x.OLMID).FirstOrDefault();
            onelinemaster.ParentTag = "FrameWidth";
            onelinemaster.CreatedBy = AddFrameWidth.OneLineMaster.CreatedBy;
            onelinemaster.CreatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = true;
            onelinemaster.IsDeleted = false;
            CMPSContext.OneLineMaster.AddRange(onelinemaster);



            try
            {
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
        public dynamic lensmasterFrameWidth()
        {
            var getData = new LensMatserDataView();

            getData.FrameWidthhis = (from details in CMPSContext.OneLineMaster.Where(x => x.ParentTag == "FrameWidth" && x.IsDeleted == false && x.ParentID != 0)
                                     select new FrameWidthhis
                                     {
                                         ID = details.OLMID,
                                         Description = details.ParentDescription,
                                         IsActive = details.IsActive == true ? "Active" : "InActive",
                                         Active = details.IsActive,
                                     }).ToList();

            return getData;
        }
        public dynamic updateFrameWidth(LensMatserDataView UpFrameWidth, int IDW)
        {

            var onelinemaster = new OneLine_Masters();

            onelinemaster = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDW).FirstOrDefault();

            onelinemaster.ParentDescription = UpFrameWidth.OneLineMaster.ParentDescription;
            onelinemaster.IsActive = UpFrameWidth.OneLineMaster.IsActive;
            onelinemaster.UpdatedBy = UpFrameWidth.OneLineMaster.UpdatedBy;
            onelinemaster.UpdatedUTC = DateTime.UtcNow;
            onelinemaster.IsActive = UpFrameWidth.OneLineMaster.IsActive;
            CMPSContext.OneLineMaster.UpdateRange(onelinemaster);

            try
            {
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
        public dynamic DeleteFrameWidth(int IDW)
        {
            var stoMas = CMPSContext.OneLineMaster.Where(x => x.OLMID == IDW).ToList();

            if (stoMas != null)
            {
                stoMas.All(x => { x.IsDeleted = true; return true; });
                CMPSContext.OneLineMaster.UpdateRange(stoMas);

            }

            try
            {
                if (CMPSContext.SaveChanges() >= 0)
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
        public dynamic Deleteframelens(int ID, int Cmpid, string name)
        {

            try
            {

                var LT = WYNKContext.Lenstrans.Where(x => x.LMID == WYNKContext.Lensmaster.Where(y => y.CMPID == Cmpid && y.LensType == name).Select(y => y.RandomUniqueID).FirstOrDefault()).Where(x => x.ID == ID).FirstOrDefault();
                LT.IsActive = false;
                WYNKContext.Lenstrans.UpdateRange(LT);
                WYNKContext.SaveChanges();
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
        public dynamic InsertUploadedExceldata(LensMatserDataView UpdateExceldatas, int CmpId, int Createdby)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                var getData = new LensMatserDataView();
                var one = CMPSContext.OneLineMaster.ToList();
                IList<Lensarray> Lensframearray = new List<Lensarray>();

                if (UpdateExceldatas.Lensarray.Count > 0)
                {


                    int Uploaded = 0;
                    int Duplicate = 0;
                    int Error = 0;


                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == CmpId).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == Createdby).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(Createdby);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Lens/Frame Excel Upload", "User name :", username, "User ID :", userid, "Mode : Add");

                    foreach (var lensframe in UpdateExceldatas.Lensarray.ToList())
                    {

                        try
                        {

                            int? Brand = WYNKContext.Brand.Where(x => x.Description.Replace(" ", string.Empty).Equals(lensframe.Brand.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.BrandType.Replace(" ", string.Empty).Equals(lensframe.Type.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.cmpID == CmpId).Select(x => (int?)x.ID).FirstOrDefault();

                            /*Brand Insertion and assign to IsBrand*/
                            if (Brand == null)
                            {
                                var Branddb = new Brand();
                                Branddb.Description = lensframe.Brand;
                                Branddb.BrandType = lensframe.Type;
                                Branddb.cmpID = CmpId;
                                Branddb.IsDeleted = false;
                                Branddb.IsActive = true;
                                Branddb.CreatedUTC = DateTime.UtcNow;
                                Branddb.CreatedBy = Createdby;
                                WYNKContext.Brand.Add(Branddb);
                                WYNKContext.SaveChanges();

                                Brand = Branddb.ID;
                            }

                            int? Index = lensframe.Index != null ? CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).Equals(lensframe.Index.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.ParentTag == "IX" && x.IsActive == true && x.IsDeleted == false).Select(x => (int?)x.OLMID).FirstOrDefault() : 0;

                            /*Index Insertion and assign to IsIndex*/
                            if (Index == null)
                            {
                                var onelinemaster = new OneLine_Masters();
                                onelinemaster.ParentDescription = lensframe.Index;
                                onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "IX").Select(x => x.OLMID).FirstOrDefault();
                                onelinemaster.ParentTag = "IX";
                                onelinemaster.IsDeleted = false;
                                onelinemaster.IsActive = true;
                                onelinemaster.CreatedUTC = DateTime.UtcNow;
                                onelinemaster.CreatedBy = Createdby;
                                CMPSContext.OneLineMaster.Add(onelinemaster);
                                CMPSContext.SaveChanges();

                                Index = onelinemaster.OLMID;
                            }



                            var taxdesc = lensframe.TaxDescription != null ? lensframe.TaxDescription.Replace(" ", string.Empty) : string.Empty;
                            var cessdesc = lensframe.CessDescription != null ? lensframe.CessDescription.Replace(" ", string.Empty) : string.Empty;
                            var addcessdesc = lensframe.AddtionalDescription != null ? lensframe.AddtionalDescription.Replace(" ", string.Empty) : string.Empty;

                            int? TaxID = CMPSContext.TaxMaster.Where(x => x.TaxDescription.Replace(" ", string.Empty).ToLower().Contains(taxdesc) && x.GSTPercentage == lensframe.GSTPercentage && x.IsActive == true).Select(x => (int?)x.ID).FirstOrDefault();
                            int? CessID = CMPSContext.TaxMaster.Where(x => x.CESSDescription.Replace(" ", string.Empty).ToLower().Contains(cessdesc) && x.CESSPercentage == lensframe.CESSPercentage && x.IsActive == true).Select(x => (int?)x.ID).FirstOrDefault();
                            int? AddID = CMPSContext.TaxMaster.Where(x => x.AdditionalCESSDescription.Replace(" ", string.Empty).ToLower().Contains(addcessdesc) && x.AdditionalCESSPercentage == lensframe.ADDCESSPercentage && x.IsActive == true).Select(x => (int?)x.ID).FirstOrDefault();

                            /*TaxDescription Insertion and assign to IsTaxDescription*/
                            if (lensframe.TaxDescription != null)
                            {
                                if (TaxID == null || CessID == null || AddID == null)
                                {
                                    var taxmaster = new TaxMaster();
                                    taxmaster.TaxDescription = lensframe.TaxDescription;
                                    taxmaster.GSTPercentage = lensframe.GSTPercentage;
                                    taxmaster.CESSDescription = lensframe.CessDescription;
                                    taxmaster.CESSPercentage = lensframe.CESSPercentage;
                                    taxmaster.AdditionalCESSDescription = lensframe.AddtionalDescription;
                                    taxmaster.AdditionalCESSPercentage = lensframe.ADDCESSPercentage;
                                    taxmaster.IsActive = true;
                                    taxmaster.CreatedUTC = DateTime.UtcNow;
                                    taxmaster.CreatedBy = Createdby;
                                    CMPSContext.TaxMaster.Add(taxmaster);
                                    CMPSContext.SaveChanges();

                                    TaxID = taxmaster.ID;
                                }
                            }



                            if (lensframe.Type.Equals("Frame", StringComparison.OrdinalIgnoreCase))
                            {
                                var FrameShape = lensframe.FrameShapee != null ? lensframe.FrameShapee.Replace(" ", string.Empty) : string.Empty;
                                var FrameType = lensframe.FrameTypee != null ? lensframe.FrameTypee.Replace(" ", string.Empty) : string.Empty;
                                var FrameStyle = lensframe.FrameStylee != null ? lensframe.FrameStylee.Replace(" ", string.Empty) : string.Empty;
                                var FrameWidth = lensframe.FrameWidthh != null ? lensframe.FrameWidthh.Replace(" ", string.Empty) : string.Empty;

                                getData.FrameShap = CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).ToLower().Contains(FrameShape) && x.ParentTag == "FrameShape" && x.IsActive == true && x.IsDeleted == false).Select(x => (int?)x.OLMID).FirstOrDefault();

                                /*FrameShape Insertion and assign to IsFrameShape*/
                                if (getData.FrameShap == null)
                                {
                                    var onelinemaster = new OneLine_Masters();
                                    onelinemaster.ParentDescription = lensframe.FrameShapee;
                                    onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameShape").Select(x => x.OLMID).FirstOrDefault();
                                    onelinemaster.ParentTag = "FrameShape";
                                    onelinemaster.IsDeleted = false;
                                    onelinemaster.IsActive = true;
                                    onelinemaster.CreatedUTC = DateTime.UtcNow;
                                    onelinemaster.CreatedBy = Createdby;
                                    CMPSContext.OneLineMaster.Add(onelinemaster);
                                    CMPSContext.SaveChanges();

                                    getData.FrameShap = onelinemaster.OLMID;
                                }

                                getData.FrameTyp = CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).ToLower().Contains(FrameType) && x.ParentTag == "FrameType" && x.IsActive == true && x.IsDeleted == false).Select(x => (int?)x.OLMID).FirstOrDefault();

                                /*FrameType Insertion and assign to IsFrameType*/
                                if (getData.FrameTyp == null)
                                {
                                    var onelinemaster = new OneLine_Masters();
                                    onelinemaster.ParentDescription = lensframe.FrameTypee;
                                    onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameType").Select(x => x.OLMID).FirstOrDefault();
                                    onelinemaster.ParentTag = "FrameType";
                                    onelinemaster.IsDeleted = false;
                                    onelinemaster.IsActive = true;
                                    onelinemaster.CreatedUTC = DateTime.UtcNow;
                                    onelinemaster.CreatedBy = Createdby;
                                    CMPSContext.OneLineMaster.Add(onelinemaster);
                                    CMPSContext.SaveChanges();

                                    getData.FrameTyp = onelinemaster.OLMID;
                                }

                                getData.FrameStyl = CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).ToLower().Contains(FrameStyle) && x.ParentTag == "FrameStyle" && x.IsActive == true && x.IsDeleted == false).Select(x => (int?)x.OLMID).FirstOrDefault();

                                /*FrameStyle Insertion and assign to IsFrameStyle*/
                                if (getData.FrameStyl == null)
                                {
                                    var onelinemaster = new OneLine_Masters();
                                    onelinemaster.ParentDescription = lensframe.FrameStylee;
                                    onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameStyle").Select(x => x.OLMID).FirstOrDefault();
                                    onelinemaster.ParentTag = "FrameStyle";
                                    onelinemaster.IsDeleted = false;
                                    onelinemaster.IsActive = true;
                                    onelinemaster.CreatedUTC = DateTime.UtcNow;
                                    onelinemaster.CreatedBy = Createdby;
                                    CMPSContext.OneLineMaster.Add(onelinemaster);
                                    CMPSContext.SaveChanges();

                                    getData.FrameStyl = onelinemaster.OLMID;
                                }

                                getData.FrameWidt = CMPSContext.OneLineMaster.Where(x => x.ParentDescription.Replace(" ", string.Empty).ToLower().Contains(FrameWidth) && x.ParentTag == "FrameWidth" && x.IsActive == true && x.IsDeleted == false).Select(x => (int?)x.OLMID).FirstOrDefault();

                                /*FrameWidth Insertion and assign to IsFrameWidth*/
                                if (getData.FrameWidt == null)
                                {
                                    var onelinemaster = new OneLine_Masters();
                                    onelinemaster.ParentDescription = lensframe.FrameWidthh;
                                    onelinemaster.ParentID = CMPSContext.OneLineMaster.Where(x => x.ParentDescription == "FrameWidth").Select(x => x.OLMID).FirstOrDefault();
                                    onelinemaster.ParentTag = "FrameWidth";
                                    onelinemaster.IsDeleted = false;
                                    onelinemaster.IsActive = true;
                                    onelinemaster.CreatedUTC = DateTime.UtcNow;
                                    onelinemaster.CreatedBy = Createdby;
                                    CMPSContext.OneLineMaster.Add(onelinemaster);
                                    CMPSContext.SaveChanges();

                                    getData.FrameWidt = onelinemaster.OLMID;

                                }

                            }

                            int? IsUOM = CMPSContext.uommaster.Where(x => x.Description.Replace(" ", string.Empty).Equals(lensframe.UOM.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false).Select(x => (int?)x.id).FirstOrDefault();

                            /*UOM Insertion and assign to IsManufacturer*/
                            if (IsUOM == null)
                            {
                                var UOms = new UOM_Master();
                                UOms.Description = lensframe.UOM;
                                UOms.CreatedUTC = DateTime.UtcNow;
                                UOms.CreatedBy = Createdby;
                                CMPSContext.uommaster.Add(UOms);
                                CMPSContext.SaveChanges();

                                IsUOM = UOms.id;
                            }


                            string Lensframemaster = WYNKContext.Lensmaster.Where(x => x.LensType.Replace(" ", string.Empty).Equals(lensframe.Type.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase) && x.CMPID == CmpId).Select(x => x.RandomUniqueID).FirstOrDefault();

                            /*Lensframemaster Insertion and assign to IsLensframemaster*/
                            if (Lensframemaster == null)
                            {
                                var lensmaster = new Lensmaster();
                                lensmaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                                lensmaster.LensType = lensframe.Type;
                                lensmaster.CMPID = CmpId;
                                lensmaster.CreatedUTC = DateTime.UtcNow;
                                lensmaster.CreatedBy = Createdby;
                                WYNKContext.Lensmaster.Add(lensmaster);
                                WYNKContext.SaveChanges();

                                Lensframemaster = lensmaster.RandomUniqueID;

                            }

                            if (lensframe.Type.Equals("Lens".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase))
                            {
                                var sph = lensframe.Sphh;
                                var Cyl = lensframe.Cyll;
                                var Axis = lensframe.Axiss;
                                var Add = lensframe.Addd;

                                int? Lensframetran = WYNKContext.Lenstrans.Where(x => x.Brand == Brand && x.Sph.Replace(" ", string.Empty).ToLower().Contains(sph) && x.Cyl.Replace(" ", string.Empty).ToLower().Contains(Cyl) && x.Axis.Replace(" ", string.Empty).ToLower().Contains(Axis) && x.Add.Replace(" ", string.Empty).ToLower().Contains(Add) && x.Prize == lensframe.Prize && x.IsActive == true).Select(x => (int?)x.ID).FirstOrDefault();

                                /*Lensframetran Insertion and assign to IsLensframetran*/
                                if (Lensframetran == null)
                                {
                                    var LensTranModel = new LensTranModel();
                                    LensTranModel.LMID = Lensframemaster;

                                    LensTranModel.Sph = sph;
                                    LensTranModel.Cyl = Cyl;
                                    LensTranModel.Axis = Axis;
                                    LensTranModel.Add = Add;
                                    LensTranModel.Index = Convert.ToString(Index);
                                    LensTranModel.Model = lensframe.Model;
                                    LensTranModel.Size = lensframe.Size;
                                    LensTranModel.Colour = lensframe.Colour;
                                    LensTranModel.Brand = Convert.ToInt32(Brand);
                                    LensTranModel.Prize = lensframe.Prize;
                                    LensTranModel.GST = lensframe.GSTPercentage;
                                    LensTranModel.CESSAmount = lensframe.CESSPercentage;
                                    LensTranModel.ADDCESSAmount = lensframe.ADDCESSPercentage;
                                    LensTranModel.UOMID = IsUOM;
                                    LensTranModel.HSNNo = lensframe.HSNNo;
                                    LensTranModel.Description = lensframe.Description;
                                    LensTranModel.TaxID = TaxID;
                                    LensTranModel.IsActive = true;
                                    LensTranModel.CreatedUTC = DateTime.UtcNow;
                                    LensTranModel.CreatedBy = Createdby;
                                    WYNKContext.Lenstrans.Add(LensTranModel);
                                    WYNKContext.SaveChanges();
                                    lensframe.Status = "Uploaded";
                                    Uploaded = Uploaded + 1;
                                }
                                /*Lensframetran Already in Lensframetran and Logging in Loggers as Duplicate*/
                                else
                                {
                                    object names = lensframe;
                                    oErrorLogs.WriteErrorLogArray("Duplicate Lensframetran Record", names);
                                    lensframe.Status = "Duplicate";
                                    Duplicate = Duplicate + 1;
                                }
                            }

                            else if (lensframe.Type.Equals("Contactlens".Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase))
                            {
                                var sph = lensframe.Sphh;
                                var Cyl = lensframe.Cyll;
                                var Axis = lensframe.Axiss;
                                var Add = lensframe.Addd;

                                int? Lensframetran = WYNKContext.Lenstrans.Where(x => x.Brand == Brand && x.Sph.Replace(" ", string.Empty).ToLower().Contains(sph) && x.Cyl.Replace(" ", string.Empty).ToLower().Contains(Cyl) && x.Axis.Replace(" ", string.Empty).ToLower().Contains(Axis) && x.Add.Replace(" ", string.Empty).ToLower().Contains(Add) && x.Prize == lensframe.Prize && x.IsActive == true).Select(x => (int?)x.ID).FirstOrDefault();

                                if (Lensframetran == null)
                                {
                                    var LensTranModel = new LensTranModel();
                                    LensTranModel.LMID = Lensframemaster;
                                    LensTranModel.Sph = sph;
                                    LensTranModel.Cyl = Cyl;
                                    LensTranModel.Axis = Axis;
                                    LensTranModel.Add = Add;
                                    LensTranModel.Model = lensframe.Model;
                                    LensTranModel.Size = lensframe.Size;
                                    LensTranModel.Colour = lensframe.Colour;
                                    LensTranModel.Brand = Convert.ToInt32(Brand);
                                    LensTranModel.Prize = lensframe.Prize;
                                    LensTranModel.GST = lensframe.GSTPercentage;
                                    LensTranModel.CESSAmount = lensframe.CESSPercentage;
                                    LensTranModel.ADDCESSAmount = lensframe.ADDCESSPercentage;
                                    LensTranModel.UOMID = IsUOM;
                                    LensTranModel.HSNNo = lensframe.HSNNo;
                                    LensTranModel.Description = lensframe.Description;
                                    LensTranModel.TaxID = TaxID;
                                    LensTranModel.IsActive = true;
                                    LensTranModel.CreatedUTC = DateTime.UtcNow;
                                    LensTranModel.CreatedBy = Createdby;
                                    WYNKContext.Lenstrans.Add(LensTranModel);
                                    WYNKContext.SaveChanges();
                                    lensframe.Status = "Uploaded";
                                    Uploaded = Uploaded + 1;

                                }

                                /*Lensframetran Already in Lensframetran and Logging in Loggers as Duplicate*/
                                else
                                {
                                    object names = lensframe;
                                    oErrorLogs.WriteErrorLogArray("Duplicate Lensframetran Record", names);
                                    lensframe.Status = "Duplicate";
                                    Duplicate = Duplicate + 1;
                                }
                            }

                            else
                            {

                                int? Lensframetran = WYNKContext.Lenstrans.Where(x => x.Brand == Brand && x.FrameShapeID == getData.FrameShap && x.FrameTypeID == getData.FrameTyp && x.FrameStyleID == getData.FrameStyl && x.FrameWidthID == getData.FrameWidt && x.Prize == lensframe.Prize && x.IsActive == true).Select(x => (int?)x.ID).FirstOrDefault();

                                if (Lensframetran == null)
                                {
                                    var LensTranModel = new LensTranModel();
                                    LensTranModel.LMID = Lensframemaster;
                                    LensTranModel.Model = lensframe.Model;
                                    LensTranModel.Size = lensframe.Size;
                                    LensTranModel.Colour = lensframe.Colour;
                                    LensTranModel.Brand = Convert.ToInt32(Brand);
                                    LensTranModel.Prize = lensframe.Prize;
                                    LensTranModel.GST = lensframe.GSTPercentage;
                                    LensTranModel.CESSAmount = lensframe.CESSPercentage;
                                    LensTranModel.ADDCESSAmount = lensframe.ADDCESSPercentage;
                                    LensTranModel.UOMID = IsUOM;
                                    LensTranModel.HSNNo = lensframe.HSNNo;
                                    LensTranModel.Description = lensframe.Description;
                                    LensTranModel.TaxID = TaxID;
                                    LensTranModel.FrameShapeID = getData.FrameShap;
                                    LensTranModel.FrameTypeID = getData.FrameTyp;
                                    LensTranModel.FrameStyleID = getData.FrameStyl;
                                    LensTranModel.FrameWidthID = getData.FrameWidt;
                                    LensTranModel.IsActive = true;
                                    LensTranModel.CreatedUTC = DateTime.UtcNow;
                                    LensTranModel.CreatedBy = Createdby;
                                    WYNKContext.Lenstrans.Add(LensTranModel);
                                    WYNKContext.SaveChanges();
                                    lensframe.Status = "Uploaded";
                                    Uploaded = Uploaded + 1;

                                }

                                /*Lensframetran Already in Lensframetran and Logging in Loggers as Duplicate*/
                                else
                                {
                                    object names = lensframe;
                                    oErrorLogs.WriteErrorLogArray("Duplicate Lensframetran Record", names);
                                    lensframe.Status = "Duplicate";
                                    Duplicate = Duplicate + 1;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            object names = lensframe;
                            oErrorLogs.ErrorLogArrayDrug("Error Lens/Frame Record", ex.InnerException.Message.ToString(), names);
                            lensframe.Status = "Error";
                            Error = Error + 1;
                            continue;
                        }


                    }
                    dbContextTransaction.Commit();
                    return new
                    {
                        Success = true,
                        Message = "Saved successfully",
                        lensframemaster = UpdateExceldatas.Lensarray.ToList(),
                        Uploaded = Uploaded,
                        Duplicate = Duplicate,
                        Error = Error,
                    };

                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "No Files To Upload",
                    };

                }
            }
        }
        public dynamic InsertstockUploadedExceldata(LensMatserDataView UpdatestockExceldatas, int cmpid, int Createdby, int StoreID, int M_Vendor, int TransactionId)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {

                    var transcation = CMPSContext.TransactionType.ToList();
                    var lensmas = WYNKContext.Lensmaster.ToList();
                    var lenstrns = WYNKContext.Lenstrans.ToList();

                    int Uploaded = 0;
                    int Error = 0;

                    string cmpname = CMPSContext.Company.Where(x => x.CmpID == cmpid).Select(x => x.CompanyName).FirstOrDefault();
                    string username = CMPSContext.DoctorMaster.Where(s => s.EmailID == CMPSContext.Users.Where(x => x.Userid == Createdby).Select(x => x.Username).FirstOrDefault()).Select(c => c.Firstname + "" + c.MiddleName + "" + c.LastName).FirstOrDefault();
                    string userid = Convert.ToString(Createdby);
                    ErrorLog oErrorLogs = new ErrorLog();
                    oErrorLogs.WriteErrorLogTitle(cmpname, "Lens/Frame Stock Upload", "User name :", username, "User ID :", userid, "Mode : Submit");

                    var opticalmaster = new OpticalStockMaster();

                    opticalmaster.RandomUniqueID = PasswordEncodeandDecode.GetRandomnumber();
                    string RandomUniqueID = opticalmaster.RandomUniqueID;
                    opticalmaster.TransactionID = TransactionId;
                    opticalmaster.CMPID = cmpid;
                    opticalmaster.DocumentNumber = UpdatestockExceldatas.ResData;
                    var Datee = DateTime.Now;
                    opticalmaster.Fyear = Convert.ToString(WYNKContext.FinancialYear.Where(x => x.ID == WYNKContext.FinancialYear.Where(b => Convert.ToDateTime(b.FYFrom) <= Datee && Convert.ToDateTime(b.FYTo) >= Datee && x.CMPID == cmpid && x.IsActive == true).Select(f => f.ID).FirstOrDefault()).Select(c => c.FYAccYear).FirstOrDefault());
                    opticalmaster.DocumentDate = DateTime.UtcNow;
                    opticalmaster.OpticalOrderID = "0";
                    opticalmaster.StoreID = StoreID;
                    opticalmaster.TransactionType = transcation.Where(X => X.TransactionID == TransactionId).Select(x => x.Rec_Issue_type).FirstOrDefault();
                    opticalmaster.VendorID = M_Vendor;
                    opticalmaster.DepartmentID = 0;
                    opticalmaster.TotalPOValue = 0;

                    foreach (var item in UpdatestockExceldatas.Lensframestock.ToList())
                    {
                        var LTID = lenstrns.Where(s => s.Brand == WYNKContext.Brand.Where(x => x.Description.Equals(Convert.ToString(item.Brand.Replace(" ", string.Empty)), StringComparison.OrdinalIgnoreCase) && x.BrandType.Equals(Convert.ToString(item.Type.Replace(" ", string.Empty)), StringComparison.OrdinalIgnoreCase) && x.cmpID == cmpid).Select(x => x.ID).FirstOrDefault()).ToList();
                        if (LTID.Count() > 0)
                        {
                            foreach (var items in LTID.ToList())
                            {
                                opticalmaster.TotalPOValue += item.Quantity * items.Prize;
                            }
                        }
                    }
                    opticalmaster.TotalDiscountValue = 0.0M;
                    opticalmaster.IsCancelled = false;
                    opticalmaster.IsDeleted = false;
                    opticalmaster.CreatedUTC = DateTime.UtcNow;
                    opticalmaster.CreatedBy = Createdby;
                    WYNKContext.OpticalStockMaster.AddRange(opticalmaster);
                    WYNKContext.SaveChanges();
                    object names = opticalmaster;
                    oErrorLogs.WriteErrorLogArray("OpticalStockMaster", names);


                    if (UpdatestockExceldatas.Lensframestock.Count() > 0)
                    {
                        foreach (var item in UpdatestockExceldatas.Lensframestock.ToList())
                        {
                            var LTID = lenstrns.Where(s => s.Brand == WYNKContext.Brand.Where(x => x.Description.Equals(Convert.ToString(item.Brand.Replace(" ", string.Empty)), StringComparison.OrdinalIgnoreCase) && x.BrandType.Equals(Convert.ToString(item.Type.Replace(" ", string.Empty)), StringComparison.OrdinalIgnoreCase) && x.cmpID == cmpid).Select(x => x.ID).FirstOrDefault()).ToList();

                            if (LTID.Count() > 0)
                            {
                                foreach (var items in LTID.ToList())
                                {
                                    var opticaltrans = new OpticalStockTran();
                                    opticaltrans.RandomUniqueID = RandomUniqueID;
                                    opticaltrans.LMIDID = items.ID;
                                    opticaltrans.ItemQty = item.Quantity;
                                    opticaltrans.UOMID = items.UOMID;
                                    opticaltrans.ItemRate = items.Prize;
                                    opticaltrans.ItemValue = item.Quantity * items.Prize;
                                    opticaltrans.DiscountPercentage = 0.0M;
                                    opticaltrans.DiscountAmount = 0.0M;
                                    opticaltrans.POID = 5;
                                    opticaltrans.IsDeleted = false;
                                    opticaltrans.CreatedUTC = DateTime.UtcNow;
                                    opticaltrans.CreatedBy = opticalmaster.CreatedBy;
                                    WYNKContext.OpticalStockTran.AddRange(opticaltrans);
                                    WYNKContext.SaveChanges();
                                    ErrorLog oErrorLogstran = new ErrorLog();
                                    object namestr = opticaltrans;
                                    oErrorLogstran.WriteErrorLogArray("OpticalStockTran", namestr);
                                    item.Status = "Uploaded";
                                    Uploaded = Uploaded + 1;
                                }
                            }
                            else
                            {
                                item.Status = "None of the brand";
                                Error = Error + 1;
                            }


                        }
                    }


                    var Date = DateTime.Now;
                    var CurrentMonth = Date.Month;
                    var FinancialYearId = WYNKContext.FinancialYear.Where(x => Convert.ToDateTime(x.FYFrom) <= Date && Convert.ToDateTime(x.FYTo) >= Date && x.CMPID == cmpid && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

                    if (FinancialYearId == 0)
                    {
                        dbContextTransaction.Rollback();
                        return new
                        {
                            Success = false,
                            Message = "Financial year doesn't exists",
                        };
                    }
                    else
                    {
                        if (UpdatestockExceldatas.Lensframestock.Count() > 0)
                        {
                            foreach (var item in UpdatestockExceldatas.Lensframestock.ToList())
                            {
                                var LTID = lenstrns.Where(s => s.Brand == WYNKContext.Brand.Where(x => x.Description.Equals(Convert.ToString(item.Brand.Replace(" ", string.Empty)), StringComparison.OrdinalIgnoreCase) && x.BrandType.Equals(Convert.ToString(item.Type.Replace(" ", string.Empty)), StringComparison.OrdinalIgnoreCase) && x.cmpID == cmpid).Select(x => x.ID).FirstOrDefault()).ToList();

                                if (LTID.Count() > 0)
                                {
                                    foreach (var items in LTID.ToList())
                                    {
                                        var ItemBalance = WYNKContext.OpticalBalance.Where(x => x.FYID == FinancialYearId && x.LTID == items.ID && x.StoreID == StoreID && x.CmpID == cmpid).FirstOrDefault();

                                        if (ItemBalance != null)
                                        {
                                            switch (CurrentMonth)
                                            {
                                                case 1:
                                                    ItemBalance.REC01 = ItemBalance.REC01 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 2:
                                                    ItemBalance.REC02 = ItemBalance.REC02 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 3:
                                                    ItemBalance.REC03 = ItemBalance.REC03 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 4:
                                                    ItemBalance.REC04 = ItemBalance.REC04 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 5:
                                                    ItemBalance.REC05 = ItemBalance.REC05 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 6:
                                                    ItemBalance.REC06 = ItemBalance.REC06 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 7:
                                                    ItemBalance.REC07 = ItemBalance.REC07 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 8:
                                                    ItemBalance.REC08 = ItemBalance.REC08 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 9:
                                                    ItemBalance.REC09 = ItemBalance.REC09 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 10:
                                                    ItemBalance.REC10 = ItemBalance.REC10 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 11:
                                                    ItemBalance.REC11 = ItemBalance.REC11 + Convert.ToInt32(item.Quantity);
                                                    break;
                                                case 12:
                                                    ItemBalance.REC12 = ItemBalance.REC12 + Convert.ToInt32(item.Quantity);
                                                    break;
                                            }

                                            ItemBalance.ClosingBalance = ItemBalance.ClosingBalance + Convert.ToInt32(item.Quantity);
                                            ItemBalance.StoreID = StoreID;
                                            ItemBalance.UpdatedBy = Createdby;
                                            ItemBalance.UpdatedUTC = DateTime.UtcNow;
                                            WYNKContext.OpticalBalance.UpdateRange(ItemBalance);
                                            WYNKContext.SaveChanges();
                                            object IB = ItemBalance;
                                            oErrorLogs.WriteErrorLogArray("OpticalBalance", names);
                                        }
                                        else
                                        {
                                            var closebal = WYNKContext.OpticalBalance.Where(x => x.LTID == items.ID && x.CmpID == cmpid).Sum(x => x.ClosingBalance);
                                            if (closebal != 0)
                                            {
                                                var storeid = WYNKContext.OpticalBalance.Where(x => x.LTID == items.ID && x.StoreID != StoreID && x.FYID == FinancialYearId).Select(x => x.StoreID).FirstOrDefault();
                                                if (storeid != 0)
                                                {
                                                    var ItemBalance1 = new OpticalBalance();

                                                    switch (CurrentMonth)
                                                    {

                                                        case 1:
                                                            ItemBalance1.REC01 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 2:
                                                            ItemBalance1.REC02 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 3:
                                                            ItemBalance1.REC03 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 4:
                                                            ItemBalance1.REC04 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 5:
                                                            ItemBalance1.REC05 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 6:
                                                            ItemBalance1.REC06 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 7:
                                                            ItemBalance1.REC07 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 8:
                                                            ItemBalance1.REC08 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 9:
                                                            ItemBalance1.REC09 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 10:
                                                            ItemBalance1.REC10 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 11:
                                                            ItemBalance1.REC11 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 12:
                                                            ItemBalance1.REC12 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                    }

                                                    ItemBalance1.ISS01 = 0;
                                                    ItemBalance1.ISS02 = 0;
                                                    ItemBalance1.ISS03 = 0;
                                                    ItemBalance1.ISS04 = 0;
                                                    ItemBalance1.ISS05 = 0;
                                                    ItemBalance1.ISS06 = 0;
                                                    ItemBalance1.ISS07 = 0;
                                                    ItemBalance1.ISS08 = 0;
                                                    ItemBalance1.ISS09 = 0;
                                                    ItemBalance1.ISS10 = 0;
                                                    ItemBalance1.ISS11 = 0;
                                                    ItemBalance1.ISS12 = 0;
                                                    ItemBalance1.LTID = items.ID;
                                                    ItemBalance1.UOMID = Convert.ToInt32(items.UOMID);
                                                    ItemBalance1.FYID = FinancialYearId;
                                                    ItemBalance1.OpeningBalance = 0;
                                                    ItemBalance1.StoreID = StoreID;
                                                    ItemBalance1.ClosingBalance = Convert.ToInt32(item.Quantity);
                                                    ItemBalance1.CreatedBy = Createdby;
                                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                                    ItemBalance1.CmpID = cmpid;
                                                    WYNKContext.OpticalBalance.AddRange(ItemBalance1);
                                                    WYNKContext.SaveChanges();
                                                    object IB1 = ItemBalance1;
                                                    oErrorLogs.WriteErrorLogArray("OpticalBalance", IB1);


                                                }
                                                else
                                                {

                                                    var ItemBalance1 = new OpticalBalance();

                                                    switch (CurrentMonth)
                                                    {
                                                        case 1:
                                                            ItemBalance1.REC01 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 2:
                                                            ItemBalance1.REC02 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 3:
                                                            ItemBalance1.REC03 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 4:
                                                            ItemBalance1.REC04 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 5:
                                                            ItemBalance1.REC05 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 6:
                                                            ItemBalance1.REC06 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 7:
                                                            ItemBalance1.REC07 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 8:
                                                            ItemBalance1.REC08 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 9:
                                                            ItemBalance1.REC09 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 10:
                                                            ItemBalance1.REC10 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 11:
                                                            ItemBalance1.REC11 = Convert.ToInt32(item.Quantity);
                                                            break;
                                                        case 12:
                                                            ItemBalance1.REC12 = Convert.ToInt32(item.Quantity);
                                                            break;

                                                    }

                                                    ItemBalance1.ISS01 = 0;
                                                    ItemBalance1.ISS02 = 0;
                                                    ItemBalance1.ISS03 = 0;
                                                    ItemBalance1.ISS04 = 0;
                                                    ItemBalance1.ISS05 = 0;
                                                    ItemBalance1.ISS06 = 0;
                                                    ItemBalance1.ISS07 = 0;
                                                    ItemBalance1.ISS08 = 0;
                                                    ItemBalance1.ISS09 = 0;
                                                    ItemBalance1.ISS10 = 0;
                                                    ItemBalance1.ISS11 = 0;
                                                    ItemBalance1.ISS12 = 0;
                                                    ItemBalance1.LTID = 0;
                                                    ItemBalance1.LTID = items.ID;
                                                    ItemBalance1.UOMID = Convert.ToInt32(items.UOMID);
                                                    ItemBalance1.FYID = FinancialYearId;
                                                    ItemBalance1.OpeningBalance = 0;
                                                    ItemBalance1.StoreID = StoreID;
                                                    ItemBalance1.ClosingBalance = Convert.ToInt32(item.Quantity);
                                                    ItemBalance1.CreatedBy = Createdby;
                                                    ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                                    ItemBalance1.CmpID = cmpid;
                                                    WYNKContext.OpticalBalance.AddRange(ItemBalance1);
                                                    WYNKContext.SaveChanges();
                                                    object IB2 = ItemBalance1;
                                                    oErrorLogs.WriteErrorLogArray("OpticalBalance", IB2);

                                                }
                                            }
                                            else
                                            {
                                                var ItemBalance1 = new OpticalBalance();
                                                switch (CurrentMonth)
                                                {
                                                    case 1:
                                                        ItemBalance1.REC01 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 2:
                                                        ItemBalance1.REC02 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 3:
                                                        ItemBalance1.REC03 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 4:
                                                        ItemBalance1.REC04 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 5:
                                                        ItemBalance1.REC05 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 6:
                                                        ItemBalance1.REC06 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 7:
                                                        ItemBalance1.REC07 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 8:
                                                        ItemBalance1.REC08 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 9:
                                                        ItemBalance1.REC09 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 10:
                                                        ItemBalance1.REC10 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 11:
                                                        ItemBalance1.REC11 = Convert.ToInt32(item.Quantity);
                                                        break;
                                                    case 12:
                                                        ItemBalance1.REC12 = Convert.ToInt32(item.Quantity);
                                                        break;

                                                }

                                                ItemBalance1.ISS01 = 0;
                                                ItemBalance1.ISS02 = 0;
                                                ItemBalance1.ISS03 = 0;
                                                ItemBalance1.ISS04 = 0;
                                                ItemBalance1.ISS05 = 0;
                                                ItemBalance1.ISS06 = 0;
                                                ItemBalance1.ISS07 = 0;
                                                ItemBalance1.ISS08 = 0;
                                                ItemBalance1.ISS09 = 0;
                                                ItemBalance1.ISS10 = 0;
                                                ItemBalance1.ISS11 = 0;
                                                ItemBalance1.ISS12 = 0;
                                                ItemBalance1.LTID = items.ID;
                                                ItemBalance1.UOMID = Convert.ToInt32(items.UOMID);
                                                ItemBalance1.FYID = FinancialYearId;
                                                ItemBalance1.OpeningBalance = 0;
                                                ItemBalance1.StoreID = StoreID;
                                                ItemBalance1.ClosingBalance = Convert.ToInt32(item.Quantity);
                                                ItemBalance1.CreatedBy = Createdby;
                                                ItemBalance1.CreatedUTC = DateTime.UtcNow;
                                                ItemBalance1.CmpID = cmpid;
                                                WYNKContext.OpticalBalance.AddRange(ItemBalance1);
                                                WYNKContext.SaveChanges();
                                                object IB3 = ItemBalance1;
                                                oErrorLogs.WriteErrorLogArray("OpticalBalance", IB3);

                                            }
                                        }

                                    }

                                }



                            }
                        }
                    }



                    dbContextTransaction.Commit();

                    var commonRepos = new CommonRepository(_Wynkcontext, _Cmpscontext);
                    var RunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "GetRunningNo");

                    if (RunningNumber == UpdatestockExceldatas.ResData)
                    {
                        commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "UpdateRunningNo");
                    }
                    else
                    {
                        var GetRunningNumber = commonRepos.GenerateRunningCtrlNoo(TransactionId, cmpid, "UpdateRunningNo");

                        var opticalno = WYNKContext.OpticalStockMaster.Where(x => x.DocumentNumber == UpdatestockExceldatas.ResData).FirstOrDefault();
                        opticalno.DocumentNumber = GetRunningNumber;
                        WYNKContext.OpticalStockMaster.UpdateRange(opticalno);
                        WYNKContext.SaveChanges();
                    }



                    return new
                    {
                        Success = true,
                        lensframestock = UpdatestockExceldatas.Lensframestock.ToList(),
                        Uploaded = Uploaded,
                        Error = Error,
                    };
                }

                catch (Exception ex)
                {

                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                    string msg = ex.InnerException.Message;
                    return new { Success = false, Message = msg, grn = UpdatestockExceldatas.ResData };
                }
            }
        }










    }
   
}
