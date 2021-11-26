using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    class OfferRepository : RepositoryBase<OfferViewM>, IOfferRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public OfferRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }



        public OfferViewM GetModels(int ID)
        {
            var getData = new OfferViewM();
            
            var M_Brand = WYNKContext.Brand.ToList();
            var M_LensTran = WYNKContext.Lenstrans.ToList();

            getData.getModel = (from LensTran in M_LensTran.Where(x=> x.Brand == ID)
                                select new getModel
                                {
                                    Models = LensTran.Model,
                                    LMID = LensTran.LMID,
                                    LTID = LensTran.ID,
                                    BrandID = LensTran.Brand,
                                    BrandName = WYNKContext.Brand.Where(x=> x.ID == LensTran.Brand).Select(x=> x.Description).FirstOrDefault()

                                }
                                ).ToList();

            return getData;
        }

        public dynamic Insertdata(OfferViewM offerviewm)
        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var M_Offer = new Offer();
                    M_Offer.Description = offerviewm.Offer.Description;
                    M_Offer.From = offerviewm.Offer.From;
                    M_Offer.To = offerviewm.Offer.To;
                    M_Offer.OfferValidity = offerviewm.Offer.OfferValidity;
                    M_Offer.IsAllbrand = offerviewm.Offer.IsAllbrand;
                    M_Offer.IsAllModel = offerviewm.Offer.IsAllModel;
                    M_Offer.IsActive = true;
                    M_Offer.IsTransacted = offerviewm.Offer.IsTransacted;
                    M_Offer.DiscountAmount = offerviewm.Offer.DiscountAmount;
                    M_Offer.DiscountPercentage = offerviewm.Offer.DiscountPercentage;
                    M_Offer.IsTransacted = offerviewm.Offer.IsTransacted;
                    M_Offer.TermsAndCondition = offerviewm.Offer.TermsAndCondition;
                    M_Offer.CreatedUTC = DateTime.UtcNow;
                    M_Offer.CreatedBy = offerviewm.Offer.CreatedBy;

                    WYNKContext.Offer.Add(M_Offer);
                    WYNKContext.SaveChanges();

                    if(offerviewm.ModelItems2.Count > 0)
                    {
                        foreach (var mdl in offerviewm.ModelItems2.ToList())
                        {
                            var M_OfferTran = new OfferTran();
                            M_OfferTran.OFID = M_Offer.ID;
                            M_OfferTran.LMID = mdl.LMID;
                            M_OfferTran.LTID = mdl.LTID;
                            M_OfferTran.OfferType = mdl.OfferType;
                            M_OfferTran.CreatedUTC = DateTime.UtcNow;
                            M_OfferTran.CreatedBy = offerviewm.Offer.CreatedBy;

                            WYNKContext.OfferTran.Add(M_OfferTran);
                            WYNKContext.SaveChanges();

                        }
                    }

                    if (offerviewm.oneplusOfferItems.Count > 0)
                    {
                        foreach (var offeritems in offerviewm.oneplusOfferItems.ToList())
                        {
                            var M_OfferTran1 = new OfferTran();
                            M_OfferTran1.OFID = M_Offer.ID;
                            M_OfferTran1.LMID = offeritems.LMID;
                            M_OfferTran1.LTID = offeritems.LTID;
                            M_OfferTran1.OfferType = offeritems.OfferType;
                            M_OfferTran1.CreatedUTC = DateTime.UtcNow;
                            M_OfferTran1.CreatedBy = offerviewm.Offer.CreatedBy;

                            WYNKContext.OfferTran.Add(M_OfferTran1);
                            WYNKContext.SaveChanges();

                            var M_OfferTranExt = new OfferTranExtension();

                            M_OfferTranExt.OfferID = M_Offer.ID;
                            M_OfferTranExt.OfferTranID = M_OfferTran1.ID;
                            M_OfferTranExt.OfferLMID = offeritems.OfferLMID;
                            M_OfferTranExt.OfferLTID = offeritems.OfferLTID;
                            M_OfferTranExt.IsActive = true;
                            M_OfferTranExt.CreatedUTC = DateTime.UtcNow;
                            M_OfferTranExt.CreatedBy = offerviewm.Offer.CreatedBy;

                            WYNKContext.OfferTranExtension.Add(M_OfferTranExt);
                            WYNKContext.SaveChanges();
                        }
                    }
                    




                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();
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


        public dynamic getOneplusmodelvalue(int ID)
        {
            var itemplusdetail = new OfferViewM();
            itemplusdetail.oneplusmodels = new List<oneplusmodels>();

            var LensTran = WYNKContext.Lenstrans.Where(x => x.ID == ID).ToList();

            itemplusdetail.oneplusmodels = (from LT in LensTran
                                            select new oneplusmodels
                                            {

                                                OP_LTID = LT.ID,
                                                OP_LMID = LT.LMID,
                                                OP_Model = LT.Model
                                            }).ToList();




            return itemplusdetail;
        }

    }
}
