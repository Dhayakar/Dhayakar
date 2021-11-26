using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Common;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IMedicalPrescriptionRepository : IRepositoryBase<Medical_Prescription>
    {

        Medical_Prescription GetPatientDetails(string UIN,int rid);
        Medical_Prescription TapperedDetails(int DoctorId, int cmpid);
        Medical_Prescription TapperedDetailsall(int drugid, int DoctorId, int cmpid);
        Medical_Prescription TapperedDetailsdelete(int drugid, int DoctorId, int cmpid);
        Medical_Prescription ViewTempDetailsdel(string drugname, int DoctorId, int cmpid);
        Medical_Prescription Getallergyinfo(int Drugid, string UIN, int cmpid);
        Medical_Prescription GetUINDetails(int cid);
        Medical_Prescription GetHistoryDetails(string UIN, int rid, string gmt);
        Medical_Prescription GetHistoryDetailsm(string UIN, int rid, string gmt);
        Medical_Prescription GetStockDetails(int quantity, int drugid, int cmpid);
        Medical_Prescription GetStockNo(int drugid, int cmpid);
        Medical_Prescription Checkmedicine(string drugname, int cmpid);
        Medical_Prescription getdform(string ptag, int cmpid);
        Medical_Prescription Checkdform(string drugname, string dform, int cmpid);
        Medical_Prescription Checkfqy(string fyname);
        Medical_Prescription Checkfod(string fdname);
        Medical_Prescription GetAllMedicineDetails(string rid);
        Medical_Prescription GetAllMedicineDetailsm(string rid);
        Medical_Prescription GetMedicineDetails(string ID, DateTime presdate,int rid);
        Medical_Prescription GetTapperingDetails(int medid, int docid, int cmpid);
        dynamic UpdateMedicalPrescription(Medical_Prescription MedicalPrescription, int cmpPid, int TransactionTypeid, string cmpname, string dcname);//UpdateMedicalPrescriptionnew
        dynamic UpdateMedicalPrescriptionnew(Medical_Prescription MedicalPrescription, int cmpPid, int TransactionTypeid, string cmpname, string dcname);
        dynamic UpdateFreq(Medical_Prescription MedicalPrescription);
        dynamic UpdateFood(Medical_Prescription MedicalPrescription);
        dynamic Updatepres(Medical_Prescription MedicalPrescription, string UIN);
        bool uploadImag(IFormFile file, string desc, string uin, string id);
        IEnumerable<Dropdown> GetBrandDetails(int surgerytype);
        Medical_Prescription getDrug(int cmpid);
        Medical_Prescription GetAltdtls(int drugid, int cmpid);
        dynamic Getimage(string uin);
        bool uploadImag(IFormFile file, string uin);
        dynamic SaveTemplate(Medical_Prescription MedicalPrescription);
        dynamic SaveTemplatem(Medical_Prescription MedicalPrescription);
        dynamic SaveTappering(Medical_Prescription MedicalPrescription, int medid, int docid);
        dynamic OverrideTemplate(Medical_Prescription MedicalPrescription);

    }
}
