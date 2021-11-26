using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class OTConsumptionViewModel
    {
        public ICollection<DoctorDetail> DoctorDetails { get; set; }

    }

    public class OTConsumptionDetails
    {
        public ICollection<OTItemDetails> OTItemDetails { get; set; }
        public string OTNotes { get; set; }
        public string SAID { get; set; }
        public DateTime SurgeryDateTime { get; set; }
        public string UIN { get; set; }
        public int? storeID { get; set; }
        public int Tc { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int Cmpid { get; set; }
        public int billtranid { get; set; }
        public int CreatedBy { get; set; }
        public string RunningNo { get; set; }
        public ICollection<InSufficientDrug> InSufficientDrugs { get; set; }
        public ICollection<SerialDetail> InSufficientSerials { get; set; }
        public ICollection<OtherDrugs> InSufficientOtherDrugs { get; set; }
        public ICollection<BeforePatientLeaves> BeforePatientLeaves { get; set; }
        public ICollection<BeforeSkinIncision> BeforeSkinIncisions { get; set; }
        public ICollection<IntraOperativeNote> IntraOperativeNotes { get; set; }
        public MonitoringDetails Monitoring { get; set; }
        public ICollection<OtImage> OtImages { get; set; }

        public decimal SurgeryPackageCost { get; set; }
        public string LensDescription { get; set; }
        public string OTSurgeryDescription { get; set; }

        public ICollection<SurgeryPerformedDoctor> SurgeryPerformedDoctors { get; set; }

        public string BillingType { get; set; }
        public decimal OTtheatreCost { get; set; }
        public int OtServiceID { get; set; }
        public int SurgeryServiceTypeID { get; set; }

    }

    public class SurgeryPerformedDoctor
    {
        public int ID { get; set; }
    }

    public class UpdateOTConsumptionDetails
    {
        public ICollection<IntraOperativeNote> IntraOperativeNotes { get; set; }
        public string SurgeryId { get; set; }
        public string AdmID { get; set; }
        public int Cmpid { get; set; }
        public int CreatedBy { get; set; }

    }
    public class BeforePatientLeaves
    {
        public int OLMID { get; set; }
        public int Sscid { get; set; }
        public string Questions { get; set; }
        public Boolean Yes { get; set; }
        public Boolean No { get; set; }
        public Boolean NA { get; set; }
        public string Description { get; set; }
    }
    public class BeforeSkinIncision
    {
        public int OLMID { get; set; }
        public int Sscid { get; set; }
        public string Questions { get; set; }
        public Boolean Yes { get; set; }
        public Boolean No { get; set; }
        public Boolean NA { get; set; }
        public string Description { get; set; }
    }
    public class SerialDetail
    {
        public string Brand { get; set; }
        public string SerialNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? DrugID { get; set; }
    }




    public class OtherDrugs
    {
        public string Brand { get; set; }
        public int? AvailableQty { get; set; }
        public int QtyTobeTaken { get; set; }
        public int? DrugID { get; set; }
    }

    public class OTItemDetails
    {
        public string Brand { get; set; }
        public string UOM { get; set; }
        public string GenericName { get; set; }
        public int DrugID { get; set; }
        public int Quantity { get; set; }
        public ICollection<string> SelectedList { get; set; }

        public ICollection<BatchlistDetails> BatchInfo { get; set; }
    }


    public class AnaesthesiaItemDetails
    {
        public string Brand { get; set; }
        public string UOM { get; set; }
        public string GenericName { get; set; }
        public int DrugID { get; set; }
        public int Quantity { get; set; }
        public ICollection<int> SelectedList { get; set; }

        public ICollection<BatchlistDetails> BatchInfo { get; set; }

        public string timeHH { get; set; }
        public string timeMM { get; set; }
        public string ConsumedUOM { get; set; }
        public decimal ConsumedQuantity { get; set; }
    }

    public class BatchlistDetails
    {
        public string BatchNo { get; set; }
        public int BalanceQty { get; set; }
        public string ExpireInDays { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int QtyTaken { get; set; }
    }
    public class DoctorDetail
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public List<string> Specialization { get; set; }

    }
    public class des
    {
        public string desc { get; set; }
    }

    public class MonitoringDetails
    {
        public string ECG { get; set; }
        public string ECGLeadsDesc { get; set; }
        public string SpO2 { get; set; }
        public string SpO2SiteDesc { get; set; }
        public string Temp { get; set; }
        public string TempDesc { get; set; }
        public string FiO2 { get; set; }
        public string ETCO2 { get; set; }
        public string ETCO2Desc { get; set; }
        public string InspETAA { get; set; }
        public string AirwayPress { get; set; }
        public string NIBP { get; set; }
        public string IBP { get; set; }
        public string IBPSiteDesc { get; set; }
        public string CVP { get; set; }
        public string CVPSiteDesc { get; set; }
        public string PCWP { get; set; }
        public string Resp { get; set; }
        public string RespSource { get; set; }
        public string BloodLoss { get; set; }
        public string UrineOutput { get; set; }
    }



    public class AllotedAnaesBatch
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string itemBatchNo { get; set; }
        public decimal balanceQty { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? CreatedUTC { get; set; }
        public decimal GoingToIssue { get; set; }
        public string ExpireInDays { get; set; }

        public string timeHH { get; set; }
        public string timeMM { get; set; }
        public string ConsumedUOM { get; set; }
        public decimal ConsumedQuantity { get; set; }

    }


    public class AnaesSerialDetail
    {
        public string Brand { get; set; }
        public int SerialNo { get; set; }
        public int? DrugID { get; set; }
        public string timeHH { get; set; }
        public string timeMM { get; set; }
        public string ConsumedUOM { get; set; }
        public decimal ConsumedQuantity { get; set; }
    }

    public class AnaesOtherDrugs
    {
        public string Brand { get; set; }
        public int? AvailableQty { get; set; }
        public int? QtyTobeTaken { get; set; }
        public int? DrugID { get; set; }
        public string timeHH { get; set; }
        public string timeMM { get; set; }
        public string ConsumedUOM { get; set; }
        public decimal ConsumedQuantity { get; set; }
    }

    public class IntraOperativeNote
    {
        public int? IOProcedureTempID { get; set; }
        public int? IOTempTranID { get; set; }
        public string OTNotesDescription { get; set; }
        public string UserInputType { get; set; }
        public string GivenInputValue { get; set; }

    }

    public class OtImage
    {
        public string ImagePath { get; set; }
    }

}

