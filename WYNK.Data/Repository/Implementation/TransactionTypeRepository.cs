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
   class TransactionTypeRepository : RepositoryBase<TransactionTypeViewM>, ITransactionTypeRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       

        public TransactionTypeRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public TransactionTypeViewM getContraTranDet()
        {
            var getContraTran = new TransactionTypeViewM();

            var TranType = CMPSContext.TransactionType.ToList();

            getContraTran.getContraDet = (from details in CMPSContext.TransactionType
                                          select new getContraDet
                                          {
                                              TransactionID = details.TransactionID,
                                              Description = details.Description

                                          }).ToList();


            return getContraTran;
        }



        public dynamic insertdata(TransactionTypeViewM TransactionType)
        {
            try
            {
                var TranType = new TransactionType();
                TranType.Description = TransactionType.TransactionType.Description;
                TranType.ContraTransactionid = TransactionType.TransactionType.ContraTransactionid;
                TranType.RecPayContra = TransactionType.TransactionType.RecPayContra;
                TranType.Rec_Issue_type = TransactionType.TransactionType.Rec_Issue_type;
                TranType.CreatedUTC = DateTime.UtcNow;
                TranType.CreatedBy = TransactionType.TransactionType.CreatedBy;

                CMPSContext.TransactionType.AddRange(TranType);
                CMPSContext.SaveChanges();

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



        public dynamic updatedata(TransactionTypeViewM TransactionType, int ID)
        {
            try
            {
                var updatedata = new TransactionType();
                updatedata = CMPSContext.TransactionType.Where(x => x.TransactionID == ID).FirstOrDefault();

                updatedata.Description = TransactionType.TransactionType.Description;
                updatedata.ContraTransactionid = TransactionType.TransactionType.ContraTransactionid;
                updatedata.RecPayContra = TransactionType.TransactionType.RecPayContra;
                updatedata.Rec_Issue_type = TransactionType.TransactionType.Rec_Issue_type;
                updatedata.UpdatedUTC = DateTime.UtcNow;
                updatedata.UpdatedBy = TransactionType.TransactionType.UpdatedBy;


                CMPSContext.TransactionType.UpdateRange(updatedata);
                CMPSContext.SaveChanges();

                return new
                {
                    Success = true,
                    Message = "Updated successfully"
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
