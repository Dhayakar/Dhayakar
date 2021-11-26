
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WYNK.Data.Repository
{
    public interface IInvestigationRepository : IRepositoryBase<InvestigationImage>
    {

        dynamic UpdateInvestigation(InvestigationImage Investigation, string UIN, string ipid);
        dynamic UpdateInv(InvestigationImage Investigation, string UIN, int IID, int cmpid);
        bool uploadImag(IFormFile file, string desc, string uin, string id);
        dynamic Getimage(string uin);
        dynamic Getpahistory(int cmpid);
        InvestigationImage GetInvpresDetails(string ID);
        InvestigationImage GetInvpastDetails(int cmpid, string UIN);
        InvestigationImage GetPatDetails(string UIN, int cmpid, string GMT);
        InvestigationImage GetInvpresTranDetails(string ID, int NO);
        InvestigationImage GetUINDetails(int cid);
        dynamic Getnotificationalerts(int Docid, int cmpid);//GetamtDetails
        dynamic GetamtDetails(int cmpid, string id);
        IEnumerable<Test> GettestDetails(int cmpid, string tag);
    }
}
