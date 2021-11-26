using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Helpers
{
    public static class CommonMessage
    {
        public const string saved = "Saved Successfully";

        public const string Missing = "Some data are Missing";

        public const string ResetMessage1 = "Sorry ! Email-Id Doesn't Exist";
        public const string ResetMessage2 = "Yes";

        public const Boolean Active = true;
    }

    public enum Answers
    {
        No = 0,
        Yes = 1,
        NA = 2
    }
    public enum Gender
    {
        Male = 0,
        Female = 1
    }
    public enum purchasestatus
    {
        Open = 0,
        Cancel = 1
    }

    public enum VacantType
    {
        Discharged = 0,
        Transfer = 1
    }
    public enum Ocular
    {
        OU = 0,
        OS = 1,
        OD = 2
    }
    public enum Anaesthesia
    {
        Local = 0,
        Topical = 1,
        General = 2
    }
    public enum cancel
    {
        Open = 0,
        Cancelled = 1,
    }
    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12,
    }
    public enum Genderr
    {
        Male = 0,
        Female = 1,
        Transgender = 2,
    }

    public enum TaxGroupId
    {

        withinState = 1,
        interstate = 2
    }

    public enum Typess
    {
        General = 1,
        Others = 2,

    }
    public enum Frequency
    {
        Month = 1,
        OnceinSixMonth = 2,


    }
    public enum accounttype
    {
        SavingsAccount = 1,
        CurrentAccount = 2,
        JointAccountt = 3,
        EOrSAccount = 4
    }
    public enum Vendor
    {
        Drug = 1,
        Optical = 2,
        Both = 3,
    }
    public enum OpticalOrderStatus
    {
        Open = 0,
        PartiallyOpen = 1,
        Closed = 2,
    }

    public enum RestRoomType
    {
        IndianToilet = 1,
        WesternToilet = 2,
    }
    public enum Status
    {
        Open = 1,
        Closed = 2,
        Inprocess = 3,
        Cancelled = 4,
    }
    public enum InsuranceCategory
    {
        InsuranceCompany = 0,
        MiddleMan = 1,
    }
    public enum TrackingType
    {
        SerialNumberBased = 0,
        BatchNumberBased = 1,
        None = 2
    }
    public enum DrugCategory
    {
        ImplantDrug = 1,
        NonImplantDrug = 2,
        Others = 3
    }

    public enum PACGoodFairPoor
    {

        Good = 0,
        Fair = 1,
        Poor = 2,
    }
    public enum NStatus
    {
        Obsese = 0,
        Normal = 1,
        Malnourished = 2,
    }
    public enum ISactive
    {
        Yes = 1,
        No = 0,
    
    }
    public enum VisitType
    {
        Both = 1,
        OP = 2,
        IP = 3,

    }
}
