using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model;

namespace WYNK.Data.Model.ViewModel
{
    public class Medical_Prescription
    {

        public ICollection<Patientlist> Patientlist { get; set; }
        public ICollection<Altdrg> Altdrg { get; set; }
        public ICollection<Imagedata> Imagedata { get; set; }
        public ICollection<Tapdetails> Tapdetails { get; set; }
        public ICollection<TapDrug> TapDrug { get; set; }
        public ICollection<alldet> alldet { get; set; }
        public MedicalPrescription MedicalPrescription { get; set; }
        public MedicalPrescriptionTran MedicalPrescriptionTran { get; set; }
        public  IndentViewModel Indentdata { get; set; }
        public ICollection<MedicalPrescriptionTran> MPT { get; set; }
        public ICollection<UploadMedicalPrescription> UploadMedicalPrescription { get; set; }
        public DrugMaster DrugMaster { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public Registration_Master Registration { get; set; }
        public RegistrationTran_Master RegistrationTran { get; set; }
        public ICollection<MedBill> MedBill { get; set; }
        public ICollection<MedData> MedData { get; set; }
        public ICollection<DrugssDetail> DrugssDetail { get; set; }
        public ICollection<ACtStockno> ACtStockno { get; set; }
        public ICollection<TapperData> TapperData { get; set; }
        public ICollection<UinDet> UinDet { get; set; }
        public ICollection<DrugInfo> DrugInfo { get; set; }

        public ICollection<MedBills> MedBills { get; set; }

        public ICollection<MedDatas> MedDatas { get; set; }

        public ICollection<Drugssonly> Drugssonly{ get; set;}
        public ICollection<gets> getsa { get; set; }
        public ICollection<getbill> getss { get; set; }
        public ICollection<getpres> getpres { get; set; }
        public ICollection<getpress> getpress { get; set; }
        public ICollection<billed> getdetailsbill { get; set; }
        public ICollection<ACtStock> ACtStock { get; set; }
        public int medid { get; set; }
        public Decimal Restock { get; set; }
        public int regid { get; set; }
        public int ICD_DESCRIPTION { get; set; }
        public ICollection<Medview> meddetail { get; set; }
        public int count { get; set; }
        public string ProductImage { get; set; }
        public string tdesc { get; set; }
        public int did { get; set; }
        public int medicineid { get; set; }
        public int uid { get; set; }
        public int cmpid { get; set; }

        public int rpage { get; set; }
        public string rCAddress { get; set; }
        public string rCphone { get; set; }
        public string rCweb { get; set; }
        public string rCname { get; set; }
        public string allergy { get; set; }
        public string remsrks { get; set; }
        public DateTime? reviewdates { get; set; }
        public string docno { get; set; }
        public string docnamefirst { get; set; }
        public string docnamesecond { get; set; }
        public string docnamelast { get; set; }
        public string regname { get; set; }
        public string regnamemid { get; set; }
        public string regnamelast { get; set; }
        public string[] stringArray { get; set; }
        public ICollection<InvImgres> InvImgres { get; set; }
        public List<InvImg> InvImg { get; set; }
        public Boolean status { get; set; }
        public Boolean statusf { get; set; }
        public Boolean statusfd { get; set; }
        public string drform { get; set; }
    }
}



public class Altdrg
{
    public string brand { get; set; }
    public int drugid { get; set; }
    public string genname { get; set; }
    public int genid { get; set; }
    public string drgform { get; set; }
    public string uom { get; set; }
    public string Manufacturer { get; set; }



}


public class Drugssonly
{
  
    public string Description { get; set; }
    public int IDD { get; set; }

}

public class ACtStock
{

    public Decimal quantity { get; set; }

}

public class ACtStockno
{

    public Decimal quantity { get; set; }

}

public class billed
{
    public DateTime date;
    public string uin;
    public string Patientname;
    public string gender;
    public string PrescriptionNo;
    public string prescribedby;
    public string Drugname;

}

public class getpress
{
    public DateTime date;
    public string PrescriptionNo;
    public string Patientname;
    public string prescribedby;

}

public class getpres
{
    public DateTime date;
    public string uin;
    public string Patientname;
    public string gender;
    public string PrescriptionNo;
    public string prescribedby;
    public string Drugname;

}
public class gets
{
    public DateTime date;
    public string total;
}
public class getbill
{
    public DateTime date;
    public string PrescriptionNo;
    public string Patientname;
    public string prescribedby;

}
public class Medview
{
    public DateTime date;
    public string uin;
    public string Patientname;
    public string gender;
    public string PrescriptionNo;
    public string prescribedby;
    public string Drugname;


}
public class Patientlist
{
    public string ICD_DESCRIPTION { get; set; }
    public string Brand { get; set; }
    public string Dossage { get; set; }
    public string Frequency { get; set; }
    public string Eye { get; set; }
    public string Remarks { get; set; }
    public string Food { get; set; }
    public int Dayss { get; set; }
    public int? Quant { get; set; }

    public DateTime PrescribedDate { get; set; }
    public string uom { get; set; }


}

public class MedBill
{

    public string Bill { get; set; }
    public string BillDate { get; set; }
    public string DocName { get; set; }

}

public class alldet
{

    public string dname { get; set; }


}

public class MedBills
{

    public string Bill { get; set; }
    public string Id { get; set; }
    public DateTime BillDate { get; set; }
    public string DocName { get; set; }
    public string CmpName { get; set; }

}

public class UinDet
{

    public string UIN { get; set; }
    public string name { get; set; }
    public string age { get; set; }
    public string gender { get; set; }
    public string addr1 { get; set; }
    public string addr2 { get; set; }
    public string addr3 { get; set; }
    public string phone { get; set; }

}


public class MedData
{

    public string DrugName { get; set; }
    public string Eye { get; set; }
    public string Dose { get; set; }
    public string Freq { get; set; }
    public string Foodd { get; set; }
    public int Tdays { get; set; }
    public int? Quant { get; set; }


}


public class MedDatas
{

    public string DrugName { get; set; }
    public string Eye { get; set; }

    public string Dose { get; set; }
    public string Freq { get; set; }
    public string Foodd { get; set; }
    public int Tdays { get; set; }
    public int? Quant { get; set; }
    public string uom { get; set; }

}

public class DrugInfo
{
    public string DrugName { get; set; }
    public string sideeffect { get; set; }
    public string precaution { get; set; }
    public string overdose { get; set; }
}

    public class DrugssDetail
{
    public int ID { get; set; }
    public int mediid { get; set; }
    public string Brand { get; set; }
    public string Manufacturer { get; set; }
    public string DrugGroup { get; set; }
    public string MedicineName { get; set; }

    public string uomName { get; set; }

    public string sideeffect { get; set; }
    public string precaution { get; set; }

    public string overdose { get; set; }



}

public class TapDrug
{
    public int drugid { get; set; }
    public string drugname { get; set; }
}

public class Tapdetails
{
    public string drugname { get; set; }
    public string freq { get; set; }
    public int days { get; set; }
    public string food { get; set; }
    public int quant { get; set; }
}

public class TapperData
{
    public string ICD_DESCRIPTION { get; set; }
    public string Brand { get; set; }
    public string UOM { get; set; }
    public int DrugID { get; set; }
    public int medicineid { get; set; }
    public string Frequency { get; set; }
    public int Quantity { get; set; }
    public string Food { get; set; }
    public int Days { get; set; }
    public string sideeffect { get; set; }
    public string precaution { get; set; }
    public string overdose { get; set; }
    public string manufac { get; set; }
    public string dform { get; set; }
}
