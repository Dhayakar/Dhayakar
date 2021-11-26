using Microsoft.AspNetCore.Http;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IInvestigationPrescriptionRepository : IRepositoryBase<InvestigationPrescriptionk>

    {
        dynamic InsertInvPrescription(InvestigationPrescriptionk InvestigationPrescription,int CMPID, int TransactionTypeid);

        InvestigationPrescriptionk Getdescvalues(int id);//GetUinDetails
        InvestigationPrescriptionk GetUinDetails(int id);
        dynamic Updatepres(InvestigationPrescriptionk InvestigationPrescription, string UIN);
        bool uploadImag(IFormFile file, string desc, string uin, string id);

    }

}


