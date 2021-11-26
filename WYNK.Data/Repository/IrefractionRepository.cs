using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IrefractionRepository : IRepositoryBase<RefractionMasterss>
    {

        dynamic GetOneeyedDetails(string UIN, int cmpid);
        dynamic Insertrefraction(RefractionMasterss RefractionMasterss);
        dynamic GetrefractionDetails(string UIN, int CMPID);
        dynamic Getrefractionprint(string UIN, int CMPID, string Time);
        dynamic docname(string uin, int ID);
        dynamic GetHistoryvisualDetails(string UIN, string Time);
        dynamic GetHistoryvisualacuitypaediatric(string UIN, string Time);
        dynamic GetHistorypgpDetails(string UIN, string Time);
        dynamic GetHistorycvDetails(string UIN, string Time);
        dynamic GetHistoryrefraDetails(string UIN, string Time);
        dynamic Insertcategory(RefractionMasterss Addcategory);
        dynamic refractioncategory();
        dynamic updatecategory(RefractionMasterss Upcategory, int ID);
        dynamic Deletecategory(int ID);
        dynamic InsertDistance(RefractionMasterss Adddistance);
        dynamic refractiondistance();
        dynamic updateDistance(RefractionMasterss Updistance, int IDD);
        dynamic DeleteDistance(int IDD);
        dynamic InsertNear(RefractionMasterss Addnear);
        dynamic refractionnear();
        dynamic updateNear(RefractionMasterss UpNear, int IDN);
        dynamic DeleteNear(int IDN);
        dynamic Insertpinhole(RefractionMasterss AddPinhole);
        dynamic refractionpinhole();
        dynamic updatepinhole(RefractionMasterss UpPinhole, int IDP);
        dynamic DeletePinhole(int IDP);
        dynamic Insertinstrutment(RefractionMasterss Addinstrutment);
        dynamic Deleteinstrutment(int IDI);
        dynamic updateinstrutment(RefractionMasterss Upinstrutment, int IDI);
        dynamic refractioninstrutment();
        dynamic CategoryDetails(string uin);
        dynamic InstrumentDetails(string uin);
        dynamic Insertstrabismus(RefractionMasterss Addstrabismus);
        dynamic Deletestrabismus(int IDstrabismus);
        dynamic updatestrabismus(RefractionMasterss Upstrabismus, int IDstrabismus);
        dynamic refractionstrabismus();
        dynamic paediatrics(int cmpid);

        dynamic Insertcharttype(RefractionMasterss Addcharttype);
        dynamic Deletecharttype(int ICT);
        dynamic updatecharttype(RefractionMasterss upcharttype, int ICT);
        dynamic refractioncharttype();
    }
}
