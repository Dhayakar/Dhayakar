using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{ 
    public class InsuranceViewModel
    {
        public Insurance Insurance { get; set; }
        public string ParentDescriptioncity { get; set; }
        public string ParentDescriptionstate { get; set; }
        public string ParentDescriptioncountry { get; set; }
        public int? ParentDescriptionPinCode { get; set; }
        public ICollection<patientPreAuthorizationdtls> patientPreAuthorizationdtls { get; set; }
        public ICollection<PreAuthorizationupdate> PreAuthorizationupdate { get; set; }
        public preAuthorizationInsurance preAuthorizationInsurance { get; set; }
        public string Companyname { get; set; }

        public EstimateTracking EstimateTracking { get; set; }


        public ICollection<EstimateTrackingdtls> EstimateTrackingtls { get; set; }
    }

    public class patientPreAuthorizationdtls
    {
        public int ID { get; set; }
        public decimal AmtApproval { get; set; }
        public int AdmID { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }

        public string Healthinsurancecard { get; set; }
        
        public int insuranceCardNo { get; set; }
        public DateTime CardValidity { get; set; }
        public DateTime preAuthorizationDate { get; set; }
        public int policyNo { get; set; }
        public string status { get; set; }
        public string ModeOFCommunication { get; set; }
        public DateTime? RequestSentOnDate { get; set; }

        public string Remarks { get; set; }

    }
    public class PreAuthorizationupdate
    {
        public int ID { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string Healthinsurancecard { get; set; }
        public decimal AmtApproval { get; set; }
        public int insuranceCardNo { get; set; }
        public DateTime CardValidity { get; set; }
        public DateTime preAuthorizationDate { get; set; }
        public int policyNo { get; set; }
        public string status { get; set; }
        public string ModeOFCommunication { get; set; }
        public DateTime? RequestSentOnDate { get; set; }

        public string Remarks { get; set; }

    }

    public class EstimateTrackingdtls
    {
        public int ID { get; set; }
        public int PreAID { get; set; }
        public string uin { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string gender { get; set; }
        public string ClaimNO { get; set; }
        public string status { get; set; }
        public DateTime ApprovedDate { get; set; }
        public decimal ApprovedLimit { get; set; }

    }

}
