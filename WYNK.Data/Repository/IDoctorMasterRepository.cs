using Microsoft.AspNetCore.Http;

using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IDoctorMasterRepository : IRepositoryBase<DoctorMaster>
    {

        dynamic UpdateDoctorMaster(DoctorMaster AddDoctor);
        dynamic UpdateDoctorVisitingHours(DoctorMaster AddDoctor);
        dynamic Getdoctorhours(int CMPID, int Doctorid);
        dynamic AddSpeciality(DoctorMaster AddSpecial);
        dynamic DeleteDoctor1(int? DoctorID);
        dynamic DoctorAssignDetails(int? DoctorID);
        dynamic DeleteDoctorpastdata(int CMPID, int DOCID, int EXTENSIONID);
        dynamic DeleteDoctor(int? DoctorID, int? DoctorSpecialityID);
        dynamic UpDateDocMas(DoctorMaster DoctorMaster, int? DoctorID);
      //  DoctorMaster Getdocmaster();
        DoctorMaster GetlocationDetails(int id);
        dynamic UploadImage(IFormFile file, string docid);
        dynamic DeleteDocImage(string docid);
        dynamic Getalldoctorssearch(string docid);
        dynamic Getpatientimage(string regNo);

        dynamic AddEngage(DoctorMaster EngagementType);

    }
}
