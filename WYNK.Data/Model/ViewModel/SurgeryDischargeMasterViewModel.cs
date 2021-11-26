using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class SurgeryDischarge_Master
    {
        public Surgery_Discharge SurgeryDischargeMaster { get; set; }
        public ICollection<SystemicConditions> SystemicConditionDetails { get; set; }
        public ICollection<OcularConditions> OcularConditionsDetails { get; set; }
        public ICollection<PatientAllergy> PatientAllergys { get; set; }
        public ICollection<MedicalPrescriptionTran> MedicalPrescriptionTranDetails { get; set; }
        public MedPrescription MedPrescription { get; set; }
        public ICollection<PrescribedMedicalPrescriptionTran> PrescribedMedicalPrescriptionTran { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string RunningNo { get; set; }
        public object CMPID { get; set; }
        public int PrescribedBy { get; set; }
        public string FYear { get; set; }
    }
    public class PatientAllergy 
    {
        public string AllergyType { get; set; }
        public string Description { get; set; }
    }
    public class SaveTemplate
    {
        public int DoctorID { get; set; }
        public string Description { get; set; }
        public int Createdby { get; set; }
        public ICollection<MedicalPrescriptionTran> MedicalPrescriptionTranDetails { get; set; }
    }

    public class MedPrescription
    {
        public int? Prescribedby { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int Createdby { get; set; }
    }

    public class SystemicConditions
    {
        public DateTime FromDate { get; set; }
        public string Description { get; set; }
        public int ToMonths { get; set; }
    }

    public class OcularConditions
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public Boolean OD { get; set; }
        public Boolean OU { get; set; }
        public Boolean OS { get; set; }
    }

    public class PrescribedMedicalPrescriptionTran
    {
        public int MedicalPrescriptionTranID { get; set; }
        public int MedicalPrescriptionID { get; set; }
        public string DrugID { get; set; }
        public string DrugForm { get; set; }
        public string Brand { get; set; }
        public string UOM { get; set; }
        public string Frequency { get; set; }
        public int Quantity { get; set; }
        public string Eye { get; set; }
        public string Food { get; set; }
        public int Days { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Remarks { get; set; }
    }

    public class SurDisTemp
    {
        public ICollection<tempDesc> tempDesc { get; set; }
    }

    public class tempDesc
    {
        public string TempDesc { get; set; }
        public ICollection<tempDetails> tempDetails { get; set; }
    }

    public class DoctorTempDetails
    {
        public ICollection<tempDetails> tempDetails { get; set; }
    }

    public class tempDetails
    {
        public int DrugID { get; set; }
        public string DrugDesc { get; set; }
        public string GenericName { get; set; }
        public string UOM { get; set; }
        public string frequency { get; set; }
        public int Days { get; set; }
        public string Food { get; set; }
        public int Quantity { get; set; }

        public string sideeffect { get; set; }
        public string precaution { get; set; }

        public string overdose { get; set; }
    }


    public   class SurgeonDetail
    {
       public string Name { get; set; }
        public string Specialization { get; set; }
    }


}

