using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IFindingsRepository : IRepositoryBase<Findings>
    {

        Findings GetPatientDetails(string UIN, int CompanyID);
        Findings GetallPatientDetails(string UIN, int CompanyID, string gmt);
        Findings GetregPatientDetails(string UIN, int CompanyID, string GMT);
        Findings Getcustvalues(int RID, string GMT);       
        IEnumerable<Diag> GetdiaDetails(int CmpID);
        Findings GetFundnewDetails(string UIN);
        Findings GetOneeyedDetails(string UIN, int cmpid);
        Findings Getpastvalues(string id);
        Findings GetcmpDetails(int id);
        Findings GetSlitnewDetails(string UIN);
        dynamic UpdateFindings(Findings Findings, string UIN, int DOCID, string cpname, string dcname);
        dynamic UpdateTono(Findings Findings, string UIN, int DOCID, string cpname, string dcname);
        bool uploadImagsqd(IFormFile file, string desc, string uin, string id);
        bool uploadImagsqs(IFormFile file, string desc, string uin, string id);
        bool uploadImagslod(IFormFile file, string desc, string uin, string id);
        bool uploadImagslos(IFormFile file, string desc, string uin, string id);
        bool uploadImagelod(IFormFile file, string desc, string uin, string id);
        bool uploadImagelos(IFormFile file, string desc, string uin, string id);
        bool uploadImagfnod(IFormFile file, string desc, string uin, string id);

        bool uploadImagfnos(IFormFile file, string desc, string uin, string id);
        bool uploadImagglod(IFormFile file, string desc, string uin, string id);
        bool uploadImagglos(IFormFile file, string desc, string uin, string id);
        bool uploadImagvfod(IFormFile file, string desc, string uin, string id);
        bool uploadImagvfos(IFormFile file, string desc, string uin, string id);
        dynamic docnames(string uin);
        dynamic UpdateDiagnosis(Findings Findings);
       
        bool UploadImage(IFormFile file, string uin);

        bool UploadImage1(IFormFile file, string uin);
        bool uploadFile(IFormFile file, string uin);
        bool UploadImage2(IFormFile file, string uin);

        bool UploadImage3(IFormFile file, string uin);
        
        dynamic Remove(Findings Findings, int ID);
        dynamic Getpatientimage(string uin);
        dynamic Getpatientimagefnod(string uin);
        dynamic Getpatientimageslod(string uin);

        dynamic Getpatientimageslos(string uin);
        dynamic Getpatientfile(string uin);

        dynamic GetFDDtSyrDetails(string uin, string GMT,int Cmpid);
        dynamic GetRemovedFDDtSyrDetails(string uin, string GMT, int Cmpid);
        dynamic DeleteFDDTSyringe(FddtSyringeRemovals FddtSyringeRemovalss, string uin, int ID, int Cmpid);
    }
}
