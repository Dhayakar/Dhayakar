using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using WYNK.Data.Repository;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Services.Controllers
{
    [Route("[controller]")]
    public class RefractionController : Controller
    {
        private IRepositoryWrapper _repoWrapper;
        public RefractionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }


        [HttpPost("Insertrefraction")]
        public dynamic Insertrefraction([FromBody] RefractionMasterss RefractionMasterss)
        {
            return _repoWrapper.refraction.Insertrefraction(RefractionMasterss);
        }



        [HttpGet("docname/{uin}/{ID}")]
        public dynamic docname(string uin, int ID)
        {
            return _repoWrapper.refraction.docname(uin, ID);
        }

        [HttpGet("GetOneeyedDetails/{UIN}/{cmpid}")]
        public dynamic GetOneeyedDetails(string UIN, int cmpid)
        {
            return _repoWrapper.refraction.GetOneeyedDetails(UIN, cmpid);
        }

        [HttpGet("GetrefractionDetails/{UIN}/{CMPID}")]
        public RefractionMasterss GetrefractionDetails(string UIN, int CMPID)

        {
            return _repoWrapper.refraction.GetrefractionDetails(UIN, CMPID);
        }

        [HttpGet("Getrefractionprint/{UIN}/{CMPID}/{Time}")]
        public dynamic Getrefractionprint(string UIN, int CMPID, string Time)
        {
            return _repoWrapper.refraction.Getrefractionprint(UIN, CMPID, Time);
        }

        [HttpGet("GetHistoryvisualDetails/{UIN}/{Time}")]
        public RefractionMasterss GetHistoryvisualDetails(string UIN, string Time)

        {
            return _repoWrapper.refraction.GetHistoryvisualDetails(UIN, Time);
        }

        [HttpGet("GetHistoryvisualacuitypaediatric/{UIN}/{Time}")]
        public RefractionMasterss GetHistoryvisualacuitypaediatric(string UIN, string Time)

        {
            return _repoWrapper.refraction.GetHistoryvisualacuitypaediatric(UIN, Time);
        }

        [HttpGet("GetHistorypgpDetails/{UIN}/{Time}")]
        public RefractionMasterss GetHistorypgpDetails(string UIN, string Time)

        {
            return _repoWrapper.refraction.GetHistorypgpDetails(UIN, Time);
        }


        [HttpGet("GetHistorycvDetails/{UIN}/{Time}")]
        public RefractionMasterss GetHistorycvDetails(string UIN, string Time)

        {
            return _repoWrapper.refraction.GetHistorycvDetails(UIN, Time);
        }

        [HttpGet("GetHistoryrefraDetails/{UIN}/{Time}")]
        public RefractionMasterss GetHistoryrefraDetails(string UIN, string Time)

        {
            return _repoWrapper.refraction.GetHistoryrefraDetails(UIN, Time);
        }


        [HttpPost("Insertcategory")]
        public dynamic Insertcategory([FromBody] RefractionMasterss Addcategory)
        {
            return _repoWrapper.refraction.Insertcategory(Addcategory);
        }

        [HttpGet("refractioncategory")]
        public dynamic refractioncategory()
        {
            return _repoWrapper.refraction.refractioncategory();
        }

        [HttpPost("updatecategory/{ID}")]
        public dynamic updatecategory([FromBody] RefractionMasterss Upcategory, int ID)
        {
            return _repoWrapper.refraction.updatecategory(Upcategory, ID);
        }

        [HttpPost("Deletecategory/{ID}")]
        public dynamic Deletecategory(int ID)
        {
            return _repoWrapper.refraction.Deletecategory(ID);
        }

        [HttpPost("InsertDistance")]
        public dynamic InsertDistance([FromBody] RefractionMasterss Adddistance)
        {
            return _repoWrapper.refraction.InsertDistance(Adddistance);
        }

        [HttpGet("refractiondistance")]
        public dynamic refractiondistance()
        {
            return _repoWrapper.refraction.refractiondistance();
        }

        [HttpPost("updateDistance/{IDD}")]
        public dynamic updateDistance([FromBody] RefractionMasterss Updistance, int IDD)
        {
            return _repoWrapper.refraction.updateDistance(Updistance, IDD);
        }

        [HttpPost("DeleteDistance/{IDD}")]
        public dynamic DeleteDistance(int IDD)
        {
            return _repoWrapper.refraction.DeleteDistance(IDD);
        }

        [HttpPost("InsertNear")]
        public dynamic InsertNear([FromBody] RefractionMasterss Addnear)
        {
            return _repoWrapper.refraction.InsertNear(Addnear);
        }

        [HttpGet("refractionnear")]
        public dynamic refractionnear()
        {
            return _repoWrapper.refraction.refractionnear();
        }

        [HttpPost("updateNear/{IDN}")]
        public dynamic updateNear([FromBody] RefractionMasterss UpNear, int IDN)
        {
            return _repoWrapper.refraction.updateNear(UpNear, IDN);
        }

        [HttpPost("DeleteNear/{IDN}")]
        public dynamic DeleteNear(int IDN)
        {
            return _repoWrapper.refraction.DeleteNear(IDN);
        }

        [HttpPost("Insertpinhole")]
        public dynamic Insertpinhole([FromBody] RefractionMasterss AddPinhole)
        {
            return _repoWrapper.refraction.Insertpinhole(AddPinhole);
        }

        [HttpGet("refractionpinhole")]
        public dynamic refractionpinhole()
        {
            return _repoWrapper.refraction.refractionpinhole();
        }

        [HttpPost("updatepinhole/{IDP}")]
        public dynamic updatepinhole([FromBody] RefractionMasterss UpPinhole, int IDP)
        {
            return _repoWrapper.refraction.updatepinhole(UpPinhole, IDP);
        }

        [HttpPost("DeletePinhole/{IDP}")]
        public dynamic DeletePinhole(int IDP)
        {
            return _repoWrapper.refraction.DeletePinhole(IDP);
        }

        [HttpPost("Insertinstrutment")]
        public dynamic Insertinstrutment([FromBody] RefractionMasterss Addinstrutment)
        {
            return _repoWrapper.refraction.Insertinstrutment(Addinstrutment);
        }

        [HttpGet("refractioninstrutment")]
        public dynamic refractioninstrutment()
        {
            return _repoWrapper.refraction.refractioninstrutment();
        }

        [HttpPost("updateinstrutment/{IDI}")]
        public dynamic updateinstrutment([FromBody] RefractionMasterss Upinstrutment, int IDI)
        {
            return _repoWrapper.refraction.updateinstrutment(Upinstrutment, IDI);
        }

        [HttpPost("Deleteinstrutment/{IDI}")]
        public dynamic Deleteinstrutment(int IDI)
        {
            return _repoWrapper.refraction.Deleteinstrutment(IDI);
        }

        [HttpGet("CategoryDetails/{uin}")]
        public dynamic CategoryDetails(string uin)
        {
            return _repoWrapper.refraction.CategoryDetails(uin);
        }


        [HttpGet("InstrumentDetails/{uin}")]
        public dynamic InstrumentDetails(string uin)
        {
            return _repoWrapper.refraction.InstrumentDetails(uin);
        }

        [HttpPost("Insertstrabismus")]
        public dynamic Insertstrabismus([FromBody] RefractionMasterss Addstrabismus)
        {
            return _repoWrapper.refraction.Insertstrabismus(Addstrabismus);
        }

        [HttpGet("refractionstrabismus")]
        public dynamic refractionstrabismus()
        {
            return _repoWrapper.refraction.refractionstrabismus();
        }

        [HttpPost("updatestrabismus/{IDstrabismus}")]
        public dynamic updatestrabismus([FromBody] RefractionMasterss Upstrabismus, int IDstrabismus)
        {
            return _repoWrapper.refraction.updatestrabismus(Upstrabismus, IDstrabismus);
        }

        [HttpPost("Deletestrabismus/{IDstrabismus}")]
        public dynamic Deletestrabismus(int IDstrabismus)
        {
            return _repoWrapper.refraction.Deletestrabismus(IDstrabismus);
        }

        [HttpGet("paediatrics/{cmpid}")]
        public dynamic paediatrics(int cmpid)
        {
            return _repoWrapper.refraction.paediatrics(cmpid);
        }


        [HttpPost("Insertcharttype")]
        public dynamic Insertcharttype([FromBody] RefractionMasterss Addcharttype)
        {
            return _repoWrapper.refraction.Insertcharttype(Addcharttype);
        }

        [HttpGet("refractioncharttype")]
        public dynamic refractioncharttype()
        {
            return _repoWrapper.refraction.refractioncharttype();
        }

        [HttpPost("updatecharttype/{ICT}")]
        public dynamic updatecharttype([FromBody] RefractionMasterss upcharttype, int ICT)
        {
            return _repoWrapper.refraction.updatecharttype(upcharttype, ICT);
        }

        [HttpPost("Deletecharttype/{ICT}")]
        public dynamic Deletecharttype(int ICT)
        {
            return _repoWrapper.refraction.Deletecharttype(ICT);
        }

    }
}
