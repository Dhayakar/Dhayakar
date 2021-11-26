using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class Findings
    {
        public SpecialityMaster SpecialityMaster { get; set; }
        public ICollection<Historyvisualacuitypaediatric> Historyvisualacuitypaediatric { get; set; }
        public ICollection<Historyvisualacuitypaediatric1> Historyvisualacuitypaediatric1 { get; set; }
        public ICollection<EyelidImage> EyelidImage { get; set; }
        public ICollection<SquintTraninfo> SquintTraninfo { get; set; }
        public ICollection<SquintTranDeletefi> SquintTranDeletefi { get; set; }
        public ICollection<SchirmerTesthis> SchirmerTesthis { get; set; }
        public ICollection<SchDeletefi> SchDeletefi { get; set; }
        public ICollection<GlaDeletefi> GlaDeletefi { get; set; }
        public OrbitalDisorders OrbitalDisorders { get; set; }
        public ICollection<SchirmerTestcus> SchirmerTestcus { get; set; }
        public ICollection<Refractioninfola> Refractioninfola { get; set; }
        public ICollection<allergych> allergych { get; set; }
        public ICollection<orbitalcushis> orbitalcushis { get; set; }
        public ICollection<fddthis> fddthis { get; set; }
        public ICollection<syringehis> syringehis { get; set; }
        public ICollection<Imagedataslose> Imagedataslose { get; set; }
        public ICollection<Imagedataslode> Imagedataslode { get; set; }
        public ICollection<InvImgsslode> InvImgsslode { get; set; }
        public ICollection<elidhis> elidhis { get; set; }
        public ICollection<elidhisc> elidhisc { get; set; }
        public ICollection<Funlatesthichnn> Funlatesthichnn { get; set; }

        public ICollection<Slitlatesthichn> Slitlatesthichn { get; set; }
        public ICollection<Slitlatesthichnn> Slitlatesthichnn { get; set; }

        public ICollection<Funlatesthichn> Funlatesthichn { get; set; }

        public ICollection<Funlatesthin> Funlatesthin { get; set; }
        public ICollection<Fundushistorynew> Fundushistorynew { get; set; }

        public ICollection<Slitlamphistorynew> Slitlamphistorynew { get; set; }
        public ICollection<Refractioninfolachh> Refractioninfolachh { get; set; }
        public ICollection<PGPHchh> PGPHchh { get; set; }

        public ICollection<Dreyhis> Dreyhis { get; set; }
        public ICollection<squinthischis> squinthischis { get; set; }
        public ICollection<ocurechisch> ocurechisch { get; set; }
        public ICollection<squinthis> squinthis { get; set; }
        public ICollection<ocurechis> ocurechis { get; set; }
        public ICollection<OcularPro> OcularPro { get; set; }
        public ICollection<Surlatestch> Surlatestch { get; set; }
        public ICollection<invprehis> invprehis { get; set; }
        public ICollection<PatientHistoryDetails> PatientHistorys { get; set; }
        public ICollection<getPACExamDetails> getPACExamDetails { get; set; }
        public ICollection<getbhDetails> getbhDetails { get; set; }
        public ICollection<MedDatas> MedDatas { get; set; }
        public ICollection<Diaglatehich> Diaglatehich { get; set; }
        public ICollection<Funlatesthich> Funlatesthich { get; set; }
        public ICollection<Slitlatesthich> Slitlatesthich { get; set; }
        public ICollection<tonometrydetailsch> tonometrydetailsch { get; set; }
        public ICollection<opticprescription> opticprescription { get; set; }
        public ICollection<REFHch> REFHch { get; set; }
        public ICollection<Refractioninfolach> Refractioninfolach { get; set; }
        public ICollection<RegDet> RegDet { get; set; }
        public ICollection<ComplaintsDetails> ComplaintsDetails { get; set; }
        public ICollection<Slitlamphistory> Slitlamphistory { get; set; }
        public ICollection<Fundushistory> Fundushistory { get; set; }
        public ICollection<GElatest> GElatest { get; set; }
        public ICollection<RefractioninfoV> RefractioninfoV { get; set; }
        public ICollection<RefractioninfoO> RefractioninfoO { get; set; }
        public ICollection<Diag> Diag { get; set; }
        public ICollection<Patientinfo> Patientinfo { get; set; }
        public ICollection<Patientinfohi> Patientinfohi { get; set; }
        public ICollection<GlaucomaImage> GlaucomaImage { get; set; }
        public ICollection<Slitinfo> Slitinfo { get; set; }
        public ICDMaster ICDMaster { get; set; }
        public Glaucoma Glaucoma { get; set; }
        public SquintExt SquintExt { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public Registration_Master Registration { get; set; }
        public RegistrationTran_Master RegistrationTran { get; set; }
        public RegistrationExtension RegistrationExtension { get; set; }
        public Squint_ExtnMaster SquintExtnMaster { get; set; }
        public RefractionMas Refraction { get; set; }
        public Finding Finding { get; set; }       
        public SlitlampExtn SlitlampExtn { get; set; }
        public InvestigationTran InvestigationTran { get; set; }
        public EyelidDisorder EyelidDisorder { get; set; }
        public ICollection<SlitLamp> SLT { get; set; }
        public ICollection<Investigation> INV { get; set; }
        public ICollection<Diagnosis> DIA { get; set; }
        public ICollection<Slithis> Slithis { get; set; }
        public ICollection<Diaghis> Diaghis { get; set; }
        public ICollection<Slitlatest> Slitlatest { get; set; }
        public ICollection<InvImgressslod> InvImgressslod { get; set; }
        public ICollection<InvImgressslode> InvImgressslode { get; set; }
        public ICollection<Imagedataglod> Imagedataglod { get; set; }
        public ICollection<Imagedatavfod> Imagedatavfod { get; set; }
        public ICollection<Imagedatavfos> Imagedatavfos { get; set; }
        public ICollection<InvImgsglod> InvImgsglod { get; set; }
        public ICollection<InvImgsvfod> InvImgsvfod { get; set; }
        public ICollection<InvImgsvfos> InvImgsvfos { get; set; }
        public ICollection<InvImgressslos> InvImgressslos { get; set; }
        public ICollection<InvImgressslose> InvImgressslose { get; set; }


        public ICollection<PGPp> PGPp { get; set; }
        public ICollection<PGPH> PGPH { get; set; }
        public ICollection<PGPHch> PGPHch { get; set; }

        public ICollection<Reff> Reff { get; set; }

        public ICollection<Slitlatesthi> Slitlatesthi { get; set; }
        public ICollection<Funlatesthi> Funlatesthi { get; set; }
        public ICollection<Diaglate> Diaglate { get; set; }
        public ICollection<IOP> IOP { get; set; }
        public ICollection<IOPhi> IOPhi { get; set; }
        public ICollection<SquintImage> SQI { get; set; }
        public ICollection<billinginfo> billinginform { get; set; }
        public ICollection<PatientinfoOs> PatientinfoOs { get; set; }
        public ICollection<LastRecDataa> LastRecDataa { get; set; }
        public ICollection<Refractioninfopgp> Refractioninfopgp { get; set; }
        public ICollection<PatientinfoOshi> PatientinfoOshi { get; set; }
        public ICollection<IOPOs> IOPOs { get; set; }
        public ICollection<Surlatesthi> Surlatesthi { get; set; }
        public ICollection<orbitalhis> orbitalhis { get; set; }
        public ICollection<IOPOshi> IOPOshi { get; set; }
        public ICollection<FindingsExt> FindingsExt { get; set; }
        public ICollection<SquintTran> SquintTran { get; set; }
        public SquintMaster SquintMaster { get; set; }
        public ICollection<Adnexa> Adnexa { get; set; }
        public ICollection<Lids> Lids { get; set; }
        public ICollection<Conjuctiva> Conjuctiva { get; set; }
        public ICollection<Cornea> Cornea { get; set; }
        public ICollection<Antchmbr> Antchmbr { get; set; }
        public ICollection<Iris> Iris { get; set; }
        public ICollection<Pupil> Pupil { get; set; }
        public ICollection<Lens> Lens { get; set; }
        public ICollection<OcularMovements> OcularMovements { get; set; }
        public ICollection<MedicalPrescriptionTran> MPT { get; set; }
        public MedicalPrescription MedicalPrescription { get; set; }
        public MedicalPrescriptionTran MedicalPrescriptionTran { get; set; }
        public ICollection<DrugInfo> DrugInfo { get; set; }
        public ICollection<UploadMedicalPrescription> UploadMedicalPrescription { get; set; }
        public List<InvImg> InvImg { get; set; }
        public ICollection<subslt> subslt { get; set; }
        public ICollection<Invest> Invest { get; set; }
        public ICollection<Fundus> Fundus { get; set; }
        public ICollection<Disc> Disc { get; set; }
        public ICollection<Vessels> Vessels { get; set; }
        public ICollection<Macula> Macula { get; set; }
        public ICollection<Funlatest> Funlatest { get; set; }
        public ICollection<Slitlatest1> Slitlatest1 { get; set; }
        public ICollection<Funlatest1> Funlatest1 { get; set; }
        public ICollection<Surlatest> Surlatest { get; set; }
        public ICollection<goniohis> goniohis { get; set; }
        public ICollection<SlitDesc> SlitDesc { get; set; }
        public ICollection<FunDesc> FunDesc { get; set; }
        public ICollection<SlitDesch> SlitDesch { get; set; }
        public ICollection<FunDesch> FunDesch { get; set; }
        public ICollection<DocOptoDetails> DocOptoDetails { get; set; }
        public ICollection<Slitlatesthin> Slitlatesthin { get; set; }
        public ICollection<Imagedataslod> Imagedataslod { get; set; }
        public ICollection<Goniosco> Goniosco { get; set; }
        public ICollection<Imagedataslos> Imagedataslos { get; set; }
        public ICollection<samplelistsfnod> samplelistsfnod { get; set; }

        public ICollection<IOPOf> IOPOf { get; set; }
        public ICollection<REFH> REFH { get; set; }
        public ICollection<Refractioninforef> Refractioninforef { get; set; }
        public ICollection<Squintinfo> Squintinfo { get; set; }
        public ICollection<IOPOdBd> IOPOdBd { get; set; }
        public ICollection<IOPOsBd> IOPOsBd { get; set; }
        public ICollection<IOPOdBdh> IOPOdBdh { get; set; }
        public ICollection<IOPOsBdh> IOPOsBdh { get; set; }

        public ICollection<InvImgress> InvImgress { get; set; }
        public ICollection<InvImgressos> InvImgressos { get; set; }
        public ICollection<Squintinfo1> Squintinfo1 { get; set; }
        public ICollection<Imagedatasq> Imagedatasq { get; set; }
        public ICollection<Imagedatasqs> Imagedatasqs { get; set; }
        public ICollection<samplelists> samplelists { get; set; }
        public ICollection<samplelistos> samplelistos { get; set; }
        public ICollection<InvImgs> InvImgs { get; set; }
        public ICollection<InvImgos> InvImgos { get; set; }
        public ICollection<VAF> VAF { get; set; }
        public ICollection<LastRecData> LastRecData { get; set; }
        public ICollection<Diaglatehi> Diaglatehi { get; set; }
        public ICollection<Refractioninfoiop> Refractioninfoiop { get; set; }
        public ICollection<NCT> NCT { get; set; }
        public ICollection<AT> AT { get; set; }
        public ICollection<SlitLampImage> SlitLampImage { get; set; }
        public ICollection<FundusImage> FundusImage { get; set; }
        public ICollection<InvImgsfnod> InvImgsfnod { get; set; }
        public ICollection<Imagedataglos> Imagedataglos { get; set; }
        public ICollection<InvImgsglos> InvImgsglos { get; set; }
        public ICollection<InvImgressglos> InvImgressglos { get; set; }
        public ICollection<InvImgressglod> InvImgressglod { get; set; }
        public ICollection<InvImgressvfod> InvImgressvfod { get; set; }
        public ICollection<InvImgressvfos> InvImgressvfos { get; set; }
        public ICollection<Imagedatafnos> Imagedatafnos { get; set; }
        public ICollection<InvImgsfnos> InvImgsfnos { get; set; }
        public ICollection<InvImgressfnos> InvImgressfnos { get; set; }
        public ICollection<Imagedatafnod> Imagedatafnod { get; set; }
        public ICollection<InvImgressfnod> InvImgressfnod { get; set; }
        public ICollection<InvImgsslod> InvImgsslod { get; set; }
        public ICollection<InvImgsslos> InvImgsslos { get; set; }
        public ICollection<InvImgsslose> InvImgsslose { get; set; }
        public ICollection<VisualField> VisualField { get; set; }
        public ICollection<GlaucomaEvaluation> GlaucomaEvaluation { get; set; }
        public ICollection<UploadInvestigationPrescription> UploadInvestigationPrescription { get; set; }
        public InvestigationImages Investigation { get; set; }
        public InvestigationExtn InvestigationExtn { get; set; }
        public InvestigationPrescription InvestigationPrescription { get; set; }
        public ICollection<InvestigationPrescriptionTran> InvestigationPrescriptionTran { get; set; }

        public ICollection<TonometryTran> TonometryTran { get; set; }
        public ICollection<tonometrydetails> tonometrydetails { get; set; }

        public ICollection<tonometrydetailss> tonometrydetailss { get; set; }
        public ICollection<glvalch> glvalch { get; set; }
        public ICollection<SchirmerTest> SchirmerTest { get; set; }
        public DryEyes DryEyes { get; set; }
        public string ProductImage { get; set; }
        public string[] stringArrayslod { get; set; }
        public string[] stringArrayslode { get; set; }
        public string[] stringArrayslos { get; set; }
        public string[] stringArrayslose { get; set; }
        public string[] stringArrayfnod { get; set; }
        public string[] stringArrayfnos { get; set; }
        public string[] stringArrayglod { get; set; }
        public string[] stringArrayvfod { get; set; }
        public string[] stringArrayvfos { get; set; }
        public string[] stringArrayglos { get; set; }
        public string[] stringArray { get; set; }
        public string[] stringArrays { get; set; }
        public string ProductImagefod { get; set; }
        public string ProductImagesod { get; set; }
        public string ProductImagesos { get; set; }
        public string ProductFile { get; set; }
        public string Docnames { get; set; }
        public string Docnamesf { get; set; }
        public string Docnamesm { get; set; }
        public string OptoNames { get; set; }
        public string OptoNamesf { get; set; }
        public string OptoNamesm { get; set; }
        public string VCNames { get; set; }
        public string MastersName { get; set; }

        public string Regnos { get; set; }
        public string Dname { get; set; }
        public string Registerationnos { get; set; }
        public int NoOfImg { get; set; }
        public DateTime Fdt { get; set; }
        public decimal? Docfee { get; set; }
        public DateTime Fidt { get; set; }
        public DateTime Fidtt { get; set; }

        public string CAddress { get; set; }
        public string Cphone { get; set; }
        public string Cweb { get; set; }
        public string Cname { get; set; }
        public string Cnamegl { get; set; }
        public string odpach { get; set; }
        public string ospach { get; set; }
        public string treatment { get; set; }
        public string pfc { get; set; }

        public ICollection<FDDTDescriptionDetail> FDDTDescriptionDetails { get; set; }
        public ICollection<FDDTDescriptionDetail> SYRINGINGDescriptionDetails { get; set; }
        public ICollection<Amslermas> Amsler { get; set; }

        public string UIN { get; set; }
        public int cmpid { get; set; }
        public int Createdby { get; set; }
        public int updatedby { get; set; }
        public string Tag { get; set; }
        public string Userid { get; set; }
        public string Role { get; set; }
        public string Registrationtranid { get; set; }
        public string CMPID { get; set; }
        public string usid { get; set; }
        public string dilation { get; set; }
    }


}

public class FddtSyringeRemovals
{
    public FddtSyringeRemoval FddtSyringeRemoval { get; set; }
}
public class FddtSyringeRemoval
{
    public string RemovalReason { get; set; }
    public int RemovedBy { get; set; }
}




public class SquintTranDeletefi
{
    public int ID { get; set; }
    public bool IsActive { get; set; }

}

public class SchDeletefi
{
    public int ID { get; set; }
    public bool IsActive { get; set; }

}
public class GlaDeletefi
{
    public int ID { get; set; }
    public bool IsActive { get; set; }

}
public class FDDTDescriptionDetail
{

    public int? ID { get; set; }
    public string UIN { get; set; }
    public DateTime VISITDATE { get; set; }
    public int FDDTSYRINGEID { get; set; }
    public string FDDTDESCRIPTION { get; set; }
    public string REGURGITATION { get; set; }
    public string FLUID { get; set; }
    public string REMARKS { get; set; }
    public string Stop { get; set; }
    public int CREATEDBY { get; set; }
    public int CMPID { get; set; }
    public int REGTRANID { get; set; }
    public int? DoctorID { get; set; }
    public string Doctor { get; set; }
    public string Status { get; set; }
    public string Eye { get; set; }
    public string RemovalReasons { get; set; }
    public string Removedby { get; set; }
    public DateTime? RemovedDateTime { get; set; }
}



public class glvalch
{

    public DateTime GDate { get; set; }
    public string Drugs { get; set; }
    public string hvf { get; set; }
    public string oct { get; set; }
    public string intervention { get; set; }
    public string status { get; set; }


}

public class tonometrydetailsch
{
    public int ID { get; set; }
    public int CmpID { get; set; }
    public int RegistrationtranID { get; set; }
    public string UIN { get; set; }
    public string VisitDatetime { get; set; }
    public int TonometryID { get; set; }
    public string Tonometryname { get; set; }
    public string BOD { get; set; }
    public string BOS { get; set; }
    public string AOD { get; set; }
    public string AOS { get; set; }
    public string Dilation { get; set; }
    public string Time { get; set; }
    public int StaffID { get; set; }
    public string Staffname { get; set; }
    public bool IsDeleted { get; set; }
    public string RemovedReasons { get; set; }
    public int RemovedBy { get; set; }
    public string Removedname { get; set; }
    public DateTime? RemovedDatetime { get; set; }
    public string drug { get; set; }

}

public class allergych
{
    public string allergy { get; set; }
    public DateTime date { get; set; }


}

public class Refractioninfola
{

    public string SubCategory { get; set; }
    public string SubCategoryos { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string Description { get; set; }
    public string N_V_DESC { get; set; }
    public string DistSphs { get; set; }
    public string NearCyls { get; set; }
    public string PinAxiss { get; set; }
    public string Descriptions { get; set; }
    public string N_V_DESCs { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Ocular { get; set; }
    public string pgod { get; set; }
    public string pgos { get; set; }



}




public class Refractioninfolach
{

    public string ChartType { get; set; }
    public string Remarks { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string SubCategory { get; set; }
    public string SubCatgry { get; set; }
    public string Ocular { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PowerGlass { get; set; }
    public string N_V_DESC { get; set; }
    public string PinAxis { get; set; }
    public string DistSphh { get; set; }
    public string NearCyll { get; set; }
    public string PowerGlasss { get; set; }
    public string N_V_DESCC { get; set; }
    public string PinAxiss { get; set; }

}


public class Refractioninfolachh
{


    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string SubCategory { get; set; }
    public string SubCatgry { get; set; }
    public string Ocular { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PowerGlass { get; set; }
    public string N_V_DESC { get; set; }
    public string PinAxis { get; set; }
    public string DistSphh { get; set; }
    public string NearCyll { get; set; }
    public string PowerGlasss { get; set; }
    public string N_V_DESCC { get; set; }
    public string PinAxiss { get; set; }

}

public class elidhis
{

    public string UpperRE { get; set; }
    public string UpperLE { get; set; }
    public string LowerRE { get; set; }
    public string LowerLE { get; set; }
    public string BothRE { get; set; }
    public string BothLE { get; set; }
    public string PosLidRE { get; set; }
    public string PosLidLE { get; set; }
    public string LashesRE { get; set; }
    public string LashesLE { get; set; }
    public Decimal? IntDstRE { get; set; }
    public Decimal? IntDstLE { get; set; }
    public Decimal? LPSRE { get; set; }
    public Decimal? LPSLE { get; set; }
    public string POSRE { get; set; }
    public string POSLE { get; set; }




}

public class elidhisc
{

    public string UpperRE { get; set; }
    public string UpperLE { get; set; }
    public string LowerRE { get; set; }
    public string LowerLE { get; set; }
    public string BothRE { get; set; }
    public string BothLE { get; set; }
    public string PosLidRE { get; set; }
    public string PosLidLE { get; set; }
    public string LashesRE { get; set; }
    public string LashesLE { get; set; }
    public Decimal? IntDstRE { get; set; }
    public Decimal? IntDstLE { get; set; }
    public Decimal? LPSRE { get; set; }
    public Decimal? LPSLE { get; set; }
    public string POSRE { get; set; }
    public string POSLE { get; set; }




}
public class goniohis
{

    public string goniood1 { get; set; }
    public string goniood2 { get; set; }
    public string goniood3 { get; set; }
    public string goniood4 { get; set; }
    public string gonioos1 { get; set; }
    public string gonioos2 { get; set; }
    public string gonioos3 { get; set; }
    public string gonioos4 { get; set; }




}


public class RefractioninfoV
{

    public int SubCategory { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string Description { get; set; }
    public string N_V_DESC { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Ocular { get; set; }

}


public class RefractioninfoO
{

    public string SubCategory { get; set; }
    public string SubCategoryOS { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public Int32 SubCategoryv { get; set; }
    public Int32 SubCategoryvOS { get; set; }
    public string DistSphv { get; set; }
    public string NearCylv { get; set; }
    public string PinAxisv { get; set; }
    public string Description { get; set; }
    public string N_V_DESC { get; set; }
    public string PGlass { get; set; }
    public string PGlassOs { get; set; }

    public string Ocular { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string DistSphs { get; set; }
    public string NearCyls { get; set; }
    public string PinAxiss { get; set; }
    public string Descriptions { get; set; }
    public string N_V_DESCs { get; set; }
    public string DistSphsv { get; set; }
    public string NearCylsv { get; set; }
    public string PinAxissv { get; set; }

    public Boolean disableva { get; set; }

}

public class Refractioninfoiop
{

    public string Ocular { get; set; }
    public string Description { get; set; }
    public string BdNCT { get; set; }
    public string AdNCT { get; set; }
    public string BdNCTs { get; set; }
    public string AdNCTs { get; set; }
    public string TimeNct { get; set; }
    public string TimeNcts { get; set; }
    public DateTime IopDate { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }

    public Boolean disableiop { get; set; }

}


public class InvImgress
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }

}

public class InvImgressslod
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}

public class InvImgressslode
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}

public class InvImgressfnod
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}

public class InvImgressfnos
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}
public class InvImgressglod
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }

}

public class InvImgressvfod
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}

public class InvImgressvfos
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}
public class InvImgressglos
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }

}

public class InvImgressslos
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}

public class InvImgressslose
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }
    public object imgdttm { get; set; }

}


public class InvImgressos
{

    public int idmre { get; set; }
    public string Desccmre { get; set; }
    public object imgdtre { get; set; }

}

public class InvImgsslod
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}
public class InvImgsslode
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}

public class InvImgsfnod
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}

public class InvImgsglod
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }

}

public class InvImgsvfod
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}

public class InvImgsvfos
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}
public class InvImgsglos
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }

}

public class InvImgsfnos
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}

public class InvImgsslos
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}

public class InvImgsslose
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }
    public DateTime dttm { get; set; }

}
public class InvImgs
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }

}

public class InvImgos
{

    public string Desccm { get; set; }
    public string imgdt { get; set; }

}

public class samplelistsslod
{

    public int Uin { get; set; }


}

public class samplelistsslode
{

    public int Uin { get; set; }


}

public class samplelistsfnod
{

    public int Uin { get; set; }


}

public class samplelistsglod
{

    public int Uin { get; set; }


}

public class samplelistsvfod
{

    public int Uin { get; set; }


}

public class samplelistsvfos
{

    public int Uin { get; set; }


}
public class samplelistsglos
{

    public int Uin { get; set; }


}

public class samplelistsfnos
{

    public int Uin { get; set; }


}
public class samplelistsslos
{

    public int Uin { get; set; }


}

public class samplelistsslose
{

    public int Uin { get; set; }


}
public class samplelists
{

    public int Uin { get; set; }


}

public class samplelistos
{

    public int Uin { get; set; }


}

public class OcularPro
{

    public Boolean AE { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public Boolean OU { get; set; }


}
public class Funlatest1
{

    public string FundDescLat1 { get; set; }

    public string DescFun1 { get; set; }
    public string AdDescFun1 { get; set; }
    public int CodeFun1 { get; set; }
    public Boolean OdFun1 { get; set; }
    public Boolean OsFun1 { get; set; }
    public Boolean OuFun1 { get; set; }
}

public class Slitlatest1
{
    public string SlitDescLat1 { get; set; }
    public string DescLat1 { get; set; }
    public string AdDescLat1 { get; set; }
    public int CodeLat1 { get; set; }
    public Boolean OdLat1 { get; set; }
    public Boolean OsLat1 { get; set; }
    public Boolean OuLat1 { get; set; }



}


public class Disc
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disabledscod { get; set; }
    public Boolean disabledscos { get; set; }
    public Boolean disabledscou { get; set; }
    public Boolean checkStatusdsc { get; set; }
    public Boolean checkStatusdsc1 { get; set; }
    public Boolean checkStatusdsc2 { get; set; }
}
public class Vessels
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disablevesod { get; set; }
    public Boolean disablevesos { get; set; }
    public Boolean disablevesou { get; set; }
    public Boolean checkStatusves { get; set; }
    public Boolean checkStatusves1 { get; set; }
    public Boolean checkStatusves2 { get; set; }
}
public class Macula
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disablemacod { get; set; }
    public Boolean disablemacos { get; set; }
    public Boolean disablemacou { get; set; }
    public Boolean checkStatusmac { get; set; }
    public Boolean checkStatusmac1 { get; set; }
    public Boolean checkStatusmac2 { get; set; }
}


public class Funlatest
{
    public string DescFun { get; set; }
    public string Sname { get; set; }
    public int CodeFun { get; set; }
    public Boolean OdFun { get; set; }
    public Boolean OsFun { get; set; }
    public Boolean OuFun { get; set; }
}


public class Funlatesthi
{
    public string FundDescLat { get; set; }
    public string DescFun { get; set; }
    public int CodeFun { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }

    public string fundusname { get; set; }
    public string funduslinename { get; set; }
    public string propertyname { get; set; }
    public string description { get; set; }



}

public class Funlatesthich
{
    public string FundDescLat { get; set; }
    public string DescFun { get; set; }
    public int CodeFun { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }

    public string fundusname { get; set; }
    public string funduslinename { get; set; }
    public string propertyname { get; set; }
    public string description { get; set; }



}

public class Surlatest
{
    public string ICDcod { get; set; }
    public int speid { get; set; }
    public string ICDSpec { get; set; }
    public string IcdDesc { get; set; }
    public Boolean OdSur { get; set; }
    public Boolean OsSur { get; set; }
    public Boolean OuSur { get; set; }
}


public class Funlatesthichn
{
    public string fundus { get; set; }
    public string redesc { get; set; }
    public string ledesc { get; set; }
    public string description { get; set; }

}

public class Funlatesthichnn
{
    public string redesc { get; set; }
    public string ledesc { get; set; }
    public string description { get; set; }

}

public class GElatest
{
    public DateTime date { get; set; }
    public string regat { get; set; }
    public string legat { get; set; }
    public string time { get; set; }
    public string reoptic { get; set; }
    public string leoptic { get; set; }
    public string gdrugs { get; set; }
    public string hvf { get; set; }
    public string oct { get; set; }
    public string interven { get; set; }
    public string stapro { get; set; }
    public int ID { get; set; }
    public Boolean IsActive { get; set; }
}

public class Surlatesthi
{
    public string ICDSpec { get; set; }
    public string IcdDesc { get; set; }
    public Boolean OdSur { get; set; }
    public Boolean OsSur { get; set; }
    public Boolean OuSur { get; set; }
    public string dname { get; set; }
    public string did { get; set; }
    public DateTime sdate { get; set; }

}


public class Surlatestch
{
    public string ICDSpec { get; set; }
    public string IcdDesc { get; set; }
    public Boolean OdSur { get; set; }
    public Boolean OsSur { get; set; }
    public Boolean OuSur { get; set; }
    public DateTime sdate { get; set; }

}


public class Funlatesthin
{
    public string fundus { get; set; }
    public string redesc { get; set; }
    public string ledesc { get; set; }
    public string remarks { get; set; }


}

public class Invest
{
    public string InvName { get; set; }
    public Decimal? Amt { get; set; }
}


public class Adnexa
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean checkStatusadndi { get; set; }
    public Boolean checkStatusadndi1 { get; set; }
    public Boolean checkStatusadndi2 { get; set; }
    public Boolean checkStatusad { get; set; }
    public Boolean checkStatusad1 { get; set; }
    public Boolean checkStatusad2 { get; set; }
    public Boolean checkStatusadn { get; set; }
    public Boolean checkStatusadn1 { get; set; }
    public Boolean checkStatusadn2 { get; set; }
}

public class Lids
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean checkStatusliddi { get; set; }
    public Boolean checkStatusliddi1 { get; set; }
    public Boolean checkStatusliddi2 { get; set; }
    public Boolean checkStatusli { get; set; }
    public Boolean checkStatusli1 { get; set; }
    public Boolean checkStatusli2 { get; set; }
    public Boolean checkStatuslid { get; set; }
    public Boolean checkStatuslid1 { get; set; }
    public Boolean checkStatuslid2 { get; set; }
}

public class Conjuctiva
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disableconod { get; set; }
    public Boolean disableconos { get; set; }
    public Boolean disableconou { get; set; }
    public Boolean checkStatuscon { get; set; }
    public Boolean checkStatuscon1 { get; set; }
    public Boolean checkStatuscon2 { get; set; }
}

public class Cornea
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disableconjod { get; set; }
    public Boolean disableconjos { get; set; }
    public Boolean disableconjou { get; set; }
    public Boolean checkStatuscor { get; set; }
    public Boolean checkStatuscor1 { get; set; }
    public Boolean checkStatuscor2 { get; set; }
}

public class Antchmbr
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disableantod { get; set; }
    public Boolean disableantos { get; set; }
    public Boolean disableantou { get; set; }
    public Boolean checkStatusant { get; set; }
    public Boolean checkStatusant1 { get; set; }
    public Boolean checkStatusant2 { get; set; }
}

public class Iris
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disableirisod { get; set; }
    public Boolean disableirisos { get; set; }
    public Boolean disableirisou { get; set; }
    public Boolean checkStatusiris { get; set; }
    public Boolean checkStatusiris1 { get; set; }
    public Boolean checkStatusiris2 { get; set; }
}

public class Pupil
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disablepupod { get; set; }
    public Boolean disablepupos { get; set; }
    public Boolean disablepupou { get; set; }
    public Boolean checkStatuspup { get; set; }
    public Boolean checkStatuspup1 { get; set; }
    public Boolean checkStatuspup2 { get; set; }
}


public class Lens
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disablelensod { get; set; }
    public Boolean disablelensos { get; set; }
    public Boolean disablelensou { get; set; }
    public Boolean checkStatuslens { get; set; }
    public Boolean checkStatuslens1 { get; set; }
    public Boolean checkStatuslens2 { get; set; }
}


public class OcularMovements
{
    public string Desc { get; set; }
    public string Descp { get; set; }
    public int Code { get; set; }
    public Boolean disableocuod { get; set; }
    public Boolean disableocuos { get; set; }
    public Boolean disableocuou { get; set; }
    public Boolean checkStatusocu { get; set; }
    public Boolean checkStatusocu1 { get; set; }
    public Boolean checkStatusocu2 { get; set; }
}



public class subslt
{
    public string Descst { get; set; }
    public int Codest { get; set; }

}



public class PatientinfoOs
{
    public string CategoryOs { get; set; }
    public string DistSpOs { get; set; }
    public string NearCyOs { get; set; }
    public string PinAxiOs { get; set; }
    public string Nearos { get; set; }

}

public class PatientinfoOshi
{
    public string CategoryOs { get; set; }
    public string DistSpOs { get; set; }
    public string NearCyOs { get; set; }
    public string PinAxiOs { get; set; }
    public string Nearos { get; set; }

}


public class IOPOs
{
    public string IolnctOs { get; set; }

}
public class IOPOshi
{
    public string IolnctOs { get; set; }

}

public class billinginfo
{
    public string UIN { get; set; }
    public string Name { get; set; }
    public string TreatmentAdvice { get; set; }
    public decimal consultationamount { get; set; }
    public int Age { get; set; }
    public DateTime DOB { get; set; }
    public string Gender { get; set; }


}


public class Patientinfo
{
    public string Category { get; set; }
    public string DistSp { get; set; }
    public string NearCy { get; set; }
    public string PinAxi { get; set; }
    public string Nearod { get; set; }



}

public class Patientinfohi
{
    public string Category { get; set; }
    public string DistSp { get; set; }
    public string NearCy { get; set; }
    public string PinAxi { get; set; }
    public string Nearod { get; set; }



}


public class IOP
{

    public string Iolnct { get; set; }


}

public class IOPOdBd
{

    public string IolnctOdBd { get; set; }


}

public class IOPOsBd
{

    public string IolnctOsBd { get; set; }


}

public class IOPOdBdh
{

    public string IolnctOdBd { get; set; }


}

public class IOPOsBdh
{

    public string IolnctOsBd { get; set; }


}

public class IOPOf
{

    public string Iolnctodbd { get; set; }
    public string Iolnctosbd { get; set; }
    public string Iolnctodad { get; set; }
    public string Iolnctosad { get; set; }
    public string Iolatodbd { get; set; }
    public string Iolatosbd { get; set; }
    public string Iolatodad { get; set; }
    public string Iolatosad { get; set; }
    public string timenct { get; set; }
    public string timead { get; set; }



}

public class VAF
{

    public int? categ { get; set; }
    public int? categos { get; set; }
    public string dstod { get; set; }
    public string nrod { get; set; }
    public string pinod { get; set; }
    public string nrdesod { get; set; }
    public string dstos { get; set; }
    public string nros { get; set; }
    public string pinos { get; set; }
    public string nrdesos { get; set; }
    public string pgod { get; set; }
    public string pgos { get; set; }






}

public class Goniosco
{

    public int? goniood1 { get; set; }
    public int? goniood2 { get; set; }
    public int? goniood3 { get; set; }
    public int? goniood4 { get; set; }
    public int? gonioos1 { get; set; }
    public int? gonioos2 { get; set; }
    public int? gonioos3 { get; set; }
    public int? gonioos4 { get; set; }

    public string goniood11 { get; set; }
    public string goniood21 { get; set; }
    public string goniood31 { get; set; }
    public string goniood41 { get; set; }
    public string gonioos11 { get; set; }
    public string gonioos21 { get; set; }
    public string gonioos31 { get; set; }
    public string gonioos41 { get; set; }


}

public class IOPhi
{

    public string Iolnct { get; set; }


}

public class PGPH
{

    public string Description { get; set; }
    public string sph { get; set; }
    public string cyl { get; set; }
    public string pin { get; set; }
    public string add { get; set; }
    public string sphs { get; set; }
    public string cyls { get; set; }
    public string pins { get; set; }
    public string adds { get; set; }


}

public class PGPHchh
{

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string Add { get; set; }
    public string DistSph1 { get; set; }
    public string NearCyl1 { get; set; }
    public string PinAxis1 { get; set; }
    public string Add1 { get; set; }
    public string Details { get; set; }


}
public class PGPHch
{

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string Add { get; set; }
    public string DistSph1 { get; set; }
    public string NearCyl1 { get; set; }
    public string PinAxis1 { get; set; }
    public string Add1 { get; set; }
    public string Details { get; set; }


}


public class NCT
{

    public string Iolnctbdod { get; set; }
    public string Iolnctbdos { get; set; }
    public string Iolnctadod { get; set; }
    public string Iolnctados { get; set; }


}
public class AT
{

    public string Iolatbdod { get; set; }
    public string Iolatbdos { get; set; }
    public string Iolatadod { get; set; }
    public string Iolatados { get; set; }


}

public class Slitinfo
{
    public string Desc { get; set; }
    public int Code { get; set; }

    public string ODDescription { get; set; }

    public string OSDescription { get; set; }


}

public class Diag
{
    public string Desc1 { get; set; }
    public string AdDesc { get; set; }
    public string Descp { get; set; }
    public int Code1 { get; set; }
    public string IcdCode1 { get; set; }
    public Boolean disablediaod { get; set; }
    public Boolean disablediaos { get; set; }
    public Boolean disablediaou { get; set; }
    public Boolean checkStatus { get; set; }
    public Boolean checkStatus1 { get; set; }
    public Boolean checkStatus2 { get; set; }

}

public class Slithis
{

    public string PDescription { get; set; }
    public string Description { get; set; }
    public Boolean Od { get; set; }
    public Boolean Os { get; set; }
    public Boolean Ou { get; set; }
}

public class Diaghis
{
    public string IcdCode { get; set; }
    public string IcdDesc { get; set; }
    public string DigOth { get; set; }
    public Boolean Odd { get; set; }
    public Boolean Oss { get; set; }
    public Boolean Ouu { get; set; }

}


public class Diaglate
{
    public int IcdCode2 { get; set; }
    public string IcdCodess2 { get; set; }
    public string IcdDesc2 { get; set; }
    public string Desc2 { get; set; }
    public Boolean Odd2 { get; set; }
    public Boolean Oss2 { get; set; }
    public Boolean Ouu2 { get; set; }

}

public class Diaglatehi
{
    public int IcdCode2 { get; set; }
    public string IcdCodess2 { get; set; }
    public string IcdDesc2 { get; set; }
    public string Desc { get; set; }
    public Boolean Odd2 { get; set; }
    public Boolean Oss2 { get; set; }
    public Boolean Ouu2 { get; set; }

}

public class Diaglatehich
{
    public int IcdCode2 { get; set; }
    public string IcdCodess2 { get; set; }
    public string IcdDesc2 { get; set; }
    public string Desc { get; set; }
    public Boolean Odd2 { get; set; }
    public Boolean Oss2 { get; set; }
    public Boolean Ouu2 { get; set; }

}

public class invprehis
{
    public string speciality { get; set; }
    public string testdesc { get; set; }
    public string remarks { get; set; }
    public decimal? amount { get; set; }


}


public class Slitlatest
{
    public string DescLat { get; set; }
    public string Sname { get; set; }
    public int CodeLat { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }



}

public class Slitlamphistorynew
{
    public string slitlampname { get; set; }
    public string description { get; set; }
    public string redescription { get; set; }
    public string ledescription { get; set; }
    public int slitlampid { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }



}

public class Fundushistorynew
{
    public string fundusname { get; set; }
    public string description { get; set; }
    public string redescription { get; set; }
    public string ledescription { get; set; }
    public int fundusid { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }



}
public class Slitlamphistory
{
    public string slitlampname { get; set; }
    public string slitlamplinename { get; set; }
    public string propertyname { get; set; }
    public string description { get; set; }
    public int slitlampid { get; set; }
    public int slitlamplineid { get; set; }
    public int? propertyid { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }



}

public class Fundushistory
{
    public string fundusname { get; set; }
    public string funduslinename { get; set; }
    public string propertyname { get; set; }
    public string description { get; set; }
    public int fundusid { get; set; }
    public int funduslineid { get; set; }
    public int? propertyid { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }



}



public class Slitlatesthin
{
    public string slitname { get; set; }
    public string redesc { get; set; }
    public string ledesc { get; set; }
    public string remarks { get; set; }



}


public class Slitlatesthi
{
    public string SlitDescLat { get; set; }
    public string DescLat { get; set; }
    public int CodeLat { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }

    public string slitlampname { get; set; }
    public string slitlamplinename { get; set; }
    public string propertyname { get; set; }
    public string description { get; set; }



}

public class Slitlatesthich
{
    public string SlitDescLat { get; set; }
    public string DescLat { get; set; }
    public int CodeLat { get; set; }
    public Boolean OdLat { get; set; }
    public Boolean OsLat { get; set; }
    public Boolean OuLat { get; set; }

    public string slitlampname { get; set; }
    public string slitlamplinename { get; set; }
    public string propertyname { get; set; }
    public string description { get; set; }



}

public class Slitlatesthichn
{

    public string slit { get; set; }
    public string redesc { get; set; }
    public string ledesc { get; set; }
    public string description { get; set; }

}

public class Slitlatesthichnn
{

    public string redesc { get; set; }
    public string ledesc { get; set; }
    public string description { get; set; }



}

public class SlitDesc
{
    public string Descc { get; set; }
    public int slitid { get; set; }


}
public class FunDesc
{
    public string Descf { get; set; }
    public int fnid { get; set; }


}

public class SlitDesch
{
    public string Descc { get; set; }
    public int slitid { get; set; }


}
public class FunDesch
{
    public string Descf { get; set; }
    public int fnid { get; set; }


}

public class DocOptoDetails
{
    public string OptoName { get; set; }
    public string Docname { get; set; }
    public string Regno { get; set; }
}


public class Squintinfo
{
    public string sqtype { get; set; }
    public int sqid { get; set; }
    public Boolean dvod { get; set; }
    public Boolean dvos { get; set; }
    public Boolean dvou { get; set; }
    public Boolean nvod { get; set; }
    public Boolean nvos { get; set; }
    public Boolean nvou { get; set; }
    public string sqdesc { get; set; }
}

public class Squintinfo1
{
    public string sqtype1 { get; set; }
    public Boolean dvod1 { get; set; }
    public Boolean dvos1 { get; set; }
    public Boolean dvou1 { get; set; }
    public Boolean nvod1 { get; set; }
    public Boolean nvos1 { get; set; }
    public Boolean nvou1 { get; set; }
    public string sqdesc1 { get; set; }
}


public class Imagedataslod
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}

public class Imagedataslode
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}

public class Imagedatafnod
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}
public class Imagedataglod
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
}

public class Imagedatavfod
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}

public class Imagedatavfos
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}
public class Imagedataglos
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
}

public class Imagedatafnos
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}


public class Imagedataslos
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}

public class Imagedataslose
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
    public DateTime dttms { get; set; }
}

public class Imagedatasq
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
}

public class Imagedatasqs
{
    public string idd { get; set; }
    public int cont { get; set; }
    public string Descs { get; set; }
    public string ImDescs { get; set; }
}

public class LastRecData
{
    public string fiid { get; set; }
    public int rid { get; set; }
    public DateTime FiDate { get; set; }
    public string Treatment { get; set; }
    public DateTime? Rdate { get; set; }
    public string DocName { get; set; }
    public string CmpName { get; set; }
}

public class RegDet
{
    public int rid { get; set; }
    public DateTime visitdt { get; set; }
    public string CmpName { get; set; }
}



public class LastRecDataa
{
    public int fiid { get; set; }
    public int rid { get; set; }
    public DateTime FiDate { get; set; }
    public string Treatment { get; set; }
    public DateTime? Rdate { get; set; }
    public string DocName { get; set; }
    public string CmpName { get; set; }
}

public class Refractioninfopgp
{

    public string Ocular { get; set; }
    public string Description { get; set; }
    public string sph { get; set; }
    public string cyl { get; set; }
    public string pin { get; set; }
    public string add { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string sphs { get; set; }
    public string cyls { get; set; }
    public string pins { get; set; }
    public string adds { get; set; }
    public Boolean disablepgp { get; set; }



}




public class Refractioninforef
{

    public string Ocular { get; set; }
    public string Category { get; set; }
    public int SubCategoryv { get; set; }
    public string Type { get; set; }
    public string sph { get; set; }
    public string cyl { get; set; }
    public string axis { get; set; }
    public string sphwt { get; set; }
    public string cylwt { get; set; }
    public string axiswt { get; set; }
    public string sphs { get; set; }
    public string cyls { get; set; }
    public string axiss { get; set; }
    public string sphswt { get; set; }
    public string cylswt { get; set; }
    public string axisswt { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public Boolean disableref { get; set; }

}


public class REFH
{

    public string Category { get; set; }
    public string sph { get; set; }
    public string cyl { get; set; }
    public string axis { get; set; }
    public string sphwt { get; set; }
    public string cylwt { get; set; }
    public string axiswt { get; set; }
    public string sphs { get; set; }
    public string cyls { get; set; }
    public string axiss { get; set; }
    public string sphswt { get; set; }
    public string cylswt { get; set; }
    public string axisswt { get; set; }





}

public class REFHch
{

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string SubCategory { get; set; }
    public string SubCatgry { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string DistSph1 { get; set; }
    public string NearCyl1 { get; set; }
    public string PinAxis1 { get; set; }
    public string DistSph2 { get; set; }
    public string NearCyl2 { get; set; }
    public string PinAxis2 { get; set; }
    public string DistSph3 { get; set; }
    public string NearCyl3 { get; set; }
    public string PinAxis3 { get; set; }


}
public class PGPp
{

    public string PgDtls { get; set; }
    public string PgSphOD { get; set; }
    public string PgCylOD { get; set; }
    public string PgAxisOD { get; set; }
    public string PgAddOD { get; set; }
    public string PgSphOS { get; set; }
    public string PgCylOS { get; set; }
    public string PgAxisOS { get; set; }
    public string PgAddOS { get; set; }




}


public class Reff
{

    public int? Instrument { get; set; }
    public string RDySphOD { get; set; }
    public string RDyCylOD { get; set; }
    public string RDyAxisOD { get; set; }
    public string RDySphOS { get; set; }
    public string RDyCylOS { get; set; }
    public string RDyAxisOS { get; set; }
    public string RWtSphOD { get; set; }
    public string RWtCylOD { get; set; }
    public string RWtAxisOD { get; set; }
    public string RWtSphOS { get; set; }
    public string RWtCylOS { get; set; }
    public string RWtAxisOS { get; set; }




}


public class ocurechis
{

    public string HorMeasurementod1 { get; set; }
    public string VerMeasurementod1 { get; set; }
    public int? OcularMovementod1 { get; set; }
    public string HorMeasurementod2 { get; set; }
    public string VerMeasurementod2 { get; set; }
    public int? OcularMovementod2 { get; set; }
    public string HorMeasurementod3 { get; set; }
    public string VerMeasurementod3 { get; set; }
    public int? OcularMovementod3 { get; set; }
    public string HorMeasurementod4 { get; set; }
    public string VerMeasurementod4 { get; set; }
    public int? OcularMovementod4 { get; set; }
    public string HorMeasurementod5 { get; set; }
    public string VerMeasurementod5 { get; set; }
    public int? OcularMovementod5 { get; set; }
    public string HorMeasurementod6 { get; set; }
    public string VerMeasurementod6 { get; set; }
    public int? OcularMovementod6 { get; set; }
    public int? OcularMovementod7 { get; set; }
    public int? OcularMovementod8 { get; set; }
    public string HorMeasurementos1 { get; set; }
    public string VerMeasurementos1 { get; set; }
    public int? OcularMovementos1 { get; set; }
    public string HorMeasurementos2 { get; set; }
    public string VerMeasurementos2 { get; set; }
    public int? OcularMovementos2 { get; set; }
    public string HorMeasurementos3 { get; set; }
    public string VerMeasurementos3 { get; set; }
    public int? OcularMovementos3 { get; set; }
    public string HorMeasurementos4 { get; set; }
    public string VerMeasurementos4 { get; set; }
    public int? OcularMovementos4 { get; set; }
    public string HorMeasurementos5 { get; set; }
    public string VerMeasurementos5 { get; set; }
    public int? OcularMovementos5 { get; set; }
    public string HorMeasurementos6 { get; set; }
    public string VerMeasurementos6 { get; set; }
    public int? OcularMovementos6 { get; set; }
    public int? OcularMovementos7 { get; set; }
    public int? OcularMovementos8 { get; set; }

}



public class squinthis
{

    public int? AngleKappa { get; set; }
    public int? Patterns { get; set; }
    public int? ACAMethod { get; set; }
    public int? ACAValue { get; set; }
    public int? WFDTDistance { get; set; }
    public int? WFDTNear { get; set; }
    public int? StreopsisMethod { get; set; }
    public int? StreopsisValue { get; set; }
    public int? ARC { get; set; }
    public int? PBCTDisHor { get; set; }
    public string PBCTDisHorValue { get; set; }
    public int? PBCTDisVer { get; set; }
    public string PBCTDisVerValue { get; set; }
    public int? PBCTNearHor { get; set; }
    public string PBCTNearHorValue { get; set; }
    public int? PBCTNearVer { get; set; }
    public string PBCTNearVerValue { get; set; }
    public int? ModKrimHor { get; set; }
    public string ModKrimHorValue { get; set; }
    public int? ModKrimVer { get; set; }
    public string ModKrimVerValue { get; set; }
    public int? PriDevHor { get; set; }
    public string PriDevHorValue { get; set; }
    public int? PriDevVer { get; set; }
    public string PriDevVerValue { get; set; }
    public int? SecDevHor { get; set; }
    public string SecDevHorValue { get; set; }
    public int? SecDevVer { get; set; }
    public string SecDevVerValue { get; set; }
    public int? Amplitude { get; set; }
    public int? Frequency { get; set; }
    public int? Type { get; set; }
    public string Pursuit { get; set; }
    public string Saccade { get; set; }
    public string ConjugateDissociated { get; set; }
    public int? ABHeadPos { get; set; }
    public int? FreqOnConvergence { get; set; }
    public int? Occlusion { get; set; }
    public int? Oscillopsia { get; set; }
    public string AssHeadPosture { get; set; }
    public string SquintBasicExam { get; set; }
    public string SpecialTest { get; set; }

    public int? VF1 { get; set; }
    public string VF1Value { get; set; }
    public int? VF2 { get; set; }
    public string VF2Value { get; set; }
    public int? VF3 { get; set; }
    public string VF3Value { get; set; }
    public int? VF4 { get; set; }
    public string VF4Value { get; set; }


}

public class orbitalhis
{

    public int? PositionofGlobeRE { get; set; }
    public int? PositionofGlobeLE { get; set; }
    public string BaseReadingRE { get; set; }
    public string BaseReadingLE { get; set; }
    public string ReadingsfortheeyeRE { get; set; }
    public string ReadingsfortheeyeLE { get; set; }
    public string FingerInsinuationTestRE { get; set; }
    public string FingerInsinuationTestLE { get; set; }
    public int? LocationRE { get; set; }
    public int? LocationLE { get; set; }
    public int? ShapeRE { get; set; }
    public int? ShapeLE { get; set; }
    public int? TextureRE { get; set; }
    public int? TextureLE { get; set; }
    public int? SizeRE { get; set; }
    public int? SizeLE { get; set; }
    public string Remarks { get; set; }
    public int? GradeofThyroidEyeDiseaseLE { get; set; }
    public int? GradeofThyroidEyeDiseaseRE { get; set; }
    public string LocationTextRE { get; set; }
    public string LocationTextLE { get; set; }
    public string ShapeTextRE { get; set; }
    public string ShapeTextLE { get; set; }
    public string TextureTextRE { get; set; }
    public string TextureTextLE { get; set; }
    public string SizeTextRE { get; set; }
    public string SizeTextLE { get; set; }
    public string SizeTextREVer { get; set; }
    public string SizeTextLEVer { get; set; }
    public int? FingerInsinuationRE { get; set; }
    public int? FingerInsinuationLE { get; set; }


}

public class orbitalcushis
{

    public string PositionofGlobeRE { get; set; }
    public string PositionofGlobeLE { get; set; }
    public string BaseReadingRE { get; set; }
    public string BaseReadingLE { get; set; }
    public string ReadingsfortheeyeRE { get; set; }
    public string ReadingsfortheeyeLE { get; set; }
    public string FingerInsinuationTestRE { get; set; }
    public string FingerInsinuationTestLE { get; set; }
    public string LocationRE { get; set; }
    public string LocationLE { get; set; }
    public string ShapeRE { get; set; }
    public string ShapeLE { get; set; }
    public string TextureRE { get; set; }
    public string TextureLE { get; set; }
    public string SizeRE { get; set; }
    public string SizeLE { get; set; }
    public string Remarks { get; set; }
    public string GradeofThyroidEyeDiseaseLE { get; set; }
    public string GradeofThyroidEyeDiseaseRE { get; set; }
    public string LocationTextRE { get; set; }
    public string LocationTextLE { get; set; }
    public string ShapeTextRE { get; set; }
    public string ShapeTextLE { get; set; }
    public string TextureTextRE { get; set; }
    public string TextureTextLE { get; set; }
    public string SizeTextRE { get; set; }
    public string SizeTextLE { get; set; }
    public string SizeTextREVer { get; set; }
    public string SizeTextLEVer { get; set; }
    public string FingerInsinuationRE { get; set; }
    public string FingerInsinuationLE { get; set; }


}


public class ocurechisch
{

    public string HorMeasurementod1 { get; set; }
    public string VerMeasurementod1 { get; set; }
    public string OcularMovementod1 { get; set; }
    public string HorMeasurementod2 { get; set; }
    public string VerMeasurementod2 { get; set; }
    public string OcularMovementod2 { get; set; }
    public string HorMeasurementod3 { get; set; }
    public string VerMeasurementod3 { get; set; }
    public string OcularMovementod3 { get; set; }
    public string HorMeasurementod4 { get; set; }
    public string VerMeasurementod4 { get; set; }
    public string OcularMovementod4 { get; set; }
    public string HorMeasurementod5 { get; set; }
    public string VerMeasurementod5 { get; set; }
    public string OcularMovementod5 { get; set; }
    public string HorMeasurementod6 { get; set; }
    public string VerMeasurementod6 { get; set; }
    public string OcularMovementod6 { get; set; }
    public string OcularMovementod7 { get; set; }
    public string OcularMovementod8 { get; set; }
    public string HorMeasurementos1 { get; set; }
    public string VerMeasurementos1 { get; set; }
    public string OcularMovementos1 { get; set; }
    public string HorMeasurementos2 { get; set; }
    public string VerMeasurementos2 { get; set; }
    public string OcularMovementos2 { get; set; }
    public string HorMeasurementos3 { get; set; }
    public string VerMeasurementos3 { get; set; }
    public string OcularMovementos3 { get; set; }
    public string HorMeasurementos4 { get; set; }
    public string VerMeasurementos4 { get; set; }
    public string OcularMovementos4 { get; set; }
    public string HorMeasurementos5 { get; set; }
    public string VerMeasurementos5 { get; set; }
    public string OcularMovementos5 { get; set; }
    public string HorMeasurementos6 { get; set; }
    public string VerMeasurementos6 { get; set; }
    public string OcularMovementos6 { get; set; }
    public string OcularMovementos7 { get; set; }
    public string OcularMovementos8 { get; set; }

}



public class squinthischis
{

    public string AngleKappa { get; set; }
    public string Patterns { get; set; }
    public string ACAMethod { get; set; }
    public string ACAValue { get; set; }
    public string WFDTDistance { get; set; }
    public string WFDTNear { get; set; }
    public string StreopsisMethod { get; set; }
    public string StreopsisValue { get; set; }
    public string ARC { get; set; }
    public string PBCTDisHor { get; set; }
    public string PBCTDisHorValue { get; set; }
    public string PBCTDisVer { get; set; }
    public string PBCTDisVerValue { get; set; }
    public string PBCTNearHor { get; set; }
    public string PBCTNearHorValue { get; set; }
    public string PBCTNearVer { get; set; }
    public string PBCTNearVerValue { get; set; }
    public string ModKrimHor { get; set; }
    public string ModKrimHorValue { get; set; }
    public string ModKrimVer { get; set; }
    public string ModKrimVerValue { get; set; }
    public string PriDevHor { get; set; }
    public string PriDevHorValue { get; set; }
    public string PriDevVer { get; set; }
    public string PriDevVerValue { get; set; }
    public string SecDevHor { get; set; }
    public string SecDevHorValue { get; set; }
    public string SecDevVer { get; set; }
    public string SecDevVerValue { get; set; }
    public string Amplitude { get; set; }
    public string Frequency { get; set; }
    public string Type { get; set; }
    public string Pursuit { get; set; }
    public string Saccade { get; set; }
    public string ConjugateDissociated { get; set; }
    public string ABHeadPos { get; set; }
    public string FreqOnConvergence { get; set; }
    public string Occlusion { get; set; }
    public string Oscillopsia { get; set; }
    public string AssHeadPosture { get; set; }
    public string SquintBasicExam { get; set; }
    public string SpecialTest { get; set; }

    public string VF1 { get; set; }
    public string VF1Value { get; set; }
    public string VF2 { get; set; }
    public string VF2Value { get; set; }
    public string VF3 { get; set; }
    public string VF3Value { get; set; }
    public string VF4 { get; set; }
    public string VF4Value { get; set; }


}

public class SchirmerTesthis
{

    public DateTime VisitDatetime { get; set; }
    public string Ocular { get; set; }
    public string Time { get; set; }
    public string Tearsecretion { get; set; }
    public int Examinedby { get; set; }
    public string Examinedbyname { get; set; }
    public string Remarks { get; set; }
    public int ID { get; set; }
    public Boolean IsActive { get; set; }

}

public class SchirmerTestcus
{

    public DateTime VisitDatetime { get; set; }
    public string Ocular { get; set; }
    public string Time { get; set; }
    public string Tearsecretion { get; set; }
    public int Examinedby { get; set; }
    public string Examinedbyname { get; set; }
    public string Remarks { get; set; }
    public int ID { get; set; }
    public Boolean IsActive { get; set; }

}

public class syringehis
{

    public string desc { get; set; }
    public string eye { get; set; }
    public string regurgitation { get; set; }
    public string fluid { get; set; }
    public string stop { get; set; }
    public string docname { get; set; }
    public string remarks { get; set; }
    public DateTime vdate { get; set; }

}

public class fddthis
{

    public string desc { get; set; }
    public string eye { get; set; }
    public string docname { get; set; }
    public string remarks { get; set; }
    public DateTime vdate { get; set; }

}

public class Dreyhis
{

    public int? NorTMH { get; set; }
    public string NorTMHtxt { get; set; }
    public int? DryTMH { get; set; }
    public string DryTMHtxt { get; set; }
    public int? TBUT { get; set; }
    public string TBUTtxt { get; set; }
    public int? NiTBUT { get; set; }
    public string NiTBUTtxt { get; set; }


}