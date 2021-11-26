using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface ISurgeryadmissionandassignRepository : IRepositoryBase<Surgeryadmissionandassign>
    {
        dynamic GetDoctDetails(int cmpid);
        dynamic Getpatient(int cmpid, string Time);
        dynamic Getdemography(string UIN, int RegID, string Time);
        dynamic Getroom(Surgeryadmissionandassign sur, string roomid, int cmpid);
        dynamic InsertSurgeryAdmandAssign(Surgeryadmissionandassign Addsurgery, int cmpid, int TransactionId, string M_TelNo, string Category_M);
        dynamic UpdateSurgeryAdmandAssign(Surgeryadmissionandassign Addsurgery, int cmpid, int TransactionId, string M_TelNo, string Category_M);
        dynamic Getadmissionprint(string AdmitNo, int? cmpid, string uin, int? tid, string Time, string recepno);
        dynamic Getviewpatient(int cmpid, string Time);
        dynamic Getviewselectpatient(string RandomUniqueID, int RegID, string Time, string uin, int AdmiD);
    }

}


