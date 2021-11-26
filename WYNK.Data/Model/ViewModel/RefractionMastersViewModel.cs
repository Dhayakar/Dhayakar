



using System;
using System.Collections.Generic;
using System.Text;

namespace WYNK.Data.Model.ViewModel
{
    public class RefractionMasterss
    {
        public ICollection<categoryhis> categoryhis { get; set; }
        public ICollection<instrutmenthis> instrutmenthis { get; set; }
        public ICollection<strabismushis> strabismushis { get; set; }

        public ICollection<charttypehis> charttypehis { get; set; }
        public ICollection<distancehis> distancehis { get; set; }
        public ICollection<distancenear> distancenear { get; set; }
        public ICollection<distancepinhole> distancepinhole { get; set; }
        public OpticalPrescriptionn opticalPrescriptionn { get; set; }
        public ICollection<DocOptoDetailsr> DocOptoDetailsr { get; set; }
        public OneLine_Masters OneLineMaster { get; set; }
        public Registerationtrans Registerationtrans { get; set; }
        public ICollection<Refraction> Refracion { get; set; }
        public ICollection<RefractionMas> RefractionMas { get; set; }
        public ICollection<VISUALACUITYMas> VISUALACUITY { get; set; }

        public ICollection<paediatricvisualacuity> paediatricvisualacuity { get; set; }
        public ICollection<Amslermas> Amsler { get; set; }
        public ICollection<PGPMas> PGP { get; set; }
        public ICollection<REFRACTIONEXTMast> REFRACTIONEXT { get; set; }
        public ICollection<ACCEPTANCEMas> ACCEPTANCE { get; set; }
        public ICollection<FINALPRESCRIPTIONMas> FINALPRESCRIPTION { get; set; }
        public ICollection<visualacuitypaediatric> visualacuitypaediatric { get; set; }
        public ICollection<ColorVisionMas> ColorVision { get; set; }
        public Registration_Master Registration { get; set; }
        public PatientHistory PatientHistory { get; set; }
        public Companymas Companymas { get; set; }
        public ICollection<SquintTraninfo> SquintTraninfo { get; set; }
        public ICollection<SquintTran> SquintTran { get; set; }
        public ICollection<SquintTranDelete> SquintTranDelete { get; set; }
        public ICollection<HistoryVisualacuity1> HistoryVisualacuity1 { get; set; }
        public ICollection<Historyvisualacuitypaediatric1> Historyvisualacuitypaediatric1 { get; set; }
        public ICollection<Historyvisualacuitypaediatric> Historyvisualacuitypaediatric { get; set; }

        public ICollection<HistoryVisualacuity> HistoryVisualacuity { get; set; }
        public ICollection<Historypgp> Historypgp { get; set; }
        public ICollection<Historycv> Historycv { get; set; }
        public ICollection<Historyrefraction> Historyrefraction { get; set; }
        public ICollection<Historypgp1> Historypgp1 { get; set; }
        public ICollection<Historycv1> Historycv1 { get; set; }
        public ICollection<Historyrefraction1> Historyrefraction1 { get; set; }
        public string UIN { get; set; }
        public int cmpid { get; set; }
        public int Createdby { get; set; }
        public int updatedby { get; set; }
        public int TransID { get; set; }
        public string docname { get; set; }
        public string docnamefirst { get; set; }
        public string docnamesecond { get; set; }
        public string docreg { get; set; }
        public string Docname { get; set; }
        public string OptoName { get; set; }
        public string VCName { get; set; }
        public string docnam { get; set; }
        public string docregg { get; set; }
        public string Regno { get; set; }
        public string Registerationno { get; set; }
        public string Arcwithtran { get; set; }
        public string White { get; set; }
        public string Tinted { get; set; }
        public string Photogray { get; set; }
        public string Photobrown { get; set; }
        public string Arc { get; set; }
        public string BlueControl { get; set; }
        public string dsph { get; set; }
        public string pinaxsis { get; set; }
        public string nearcyl { get; set; }
        public string add { get; set; }
        public string dsphos { get; set; }
        public string pinaxsisos { get; set; }
        public string nearcylos { get; set; }
        public string addos { get; set; }
        public string UINID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string DsphOD { get; set; }
        public string addOD { get; set; }
        public string DsphOS { get; set; }
        public string AddOS { get; set; }
        public string Single { get; set; }
        public string Progressive { get; set; }
        public string DShape { get; set; }
        public string Remark { get; set; }
        public string PD { get; set; }
        public string MPDOD { get; set; }
        public string MPDOS { get; set; }
        public string SeparateRX { get; set; }
        public string ReadingGlass { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string phone { get; set; }
        public string web { get; set; }
        public string Compnayname { get; set; }
        public int RECID { get; set; }
        public string Tag { get; set; }
        public string Userid { get; set; }
        public string Role { get; set; }
        public string Registrationtranid { get; set; }
        public string CMPID { get; set; }
        public string usid { get; set; }
        public string fname { get; set; }
    }


}



public class HistoryVisualacuity
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
    public string ChartType { get; set; }
    public string Remarks { get; set; }
}




public class SquintTranDelete
{
    public int ID { get; set; }
    public bool IsActive { get; set; }

}

public class categoryhis
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}

public class instrutmenthis
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}

public class strabismushis
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}

public class charttypehis
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}


public class distancehis
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}




public class distancenear
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}

public class distancepinhole
{
    public int ID { get; set; }
    public string Description { get; set; }
    public string IsActive { get; set; }
    public bool Active { get; set; }

}

public class DocOptoDetailsr
{
    public string Docname { get; set; }
    public string OptoName { get; set; }
    public string Regno { get; set; }


}

public class SquintTraninfo
{
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public Boolean OU { get; set; }
    public Boolean chkOD { get; set; }
    public Boolean chkOS { get; set; }
    public Boolean chkOU { get; set; }
    public Boolean checkStatusOD { get; set; }
    public Boolean checkStatusOS { get; set; }
    public Boolean checkStatusOU { get; set; }
    public Boolean IsDVOD { get; set; }
    public Boolean IsDVOS { get; set; }
    public Boolean IsDVOU { get; set; }
    public DateTime Date { get; set; }
    public int SquintType { get; set; }
    public string SquintDiagnosisDescription { get; set; }
    public int ID { get; set; }
    public Boolean IsActive { get; set; }

}

public class Refraction
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public int SubCategory { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string DistSph { get; set; }
    public string DistSphOS { get; set; }
    public string NearCyl { get; set; }
    public string NearCylOS { get; set; }
    public string PinAxis { get; set; }
    public string PinAxisOS { get; set; }
    public string PowerGlass { get; set; }
    public string PowerGlassOS { get; set; }
    public string N_V_DESC { get; set; }
    public string N_V_DESCOS { get; set; }
    public string Add { get; set; }
    public string AddOS { get; set; }
    public string Details { get; set; }
    public Boolean A_n_OD { get; set; }
    public Boolean A_abn_OD { get; set; }
    public Boolean A_n_OS { get; set; }
    public Boolean A_abn_OS { get; set; }
    public string Desc_Text { get; set; }
    public string Desc_TextOS { get; set; }
    public string Sphdry { get; set; }
    public string Cyldry { get; set; }
    public string Axisdry { get; set; }
    public string Sphwet { get; set; }
    public string Cylwet { get; set; }
    public string Axiswet { get; set; }
    public int TypeDry { get; set; }
    public string Sphdryos { get; set; }
    public string Cyldryos { get; set; }
    public string Axisdryos { get; set; }
    public string Sphwetos { get; set; }
    public string Cylwetos { get; set; }
    public string Axiswetos { get; set; }
    public int TypeWet { get; set; }
    public string CV_normal { get; set; }
    public string CV_defective { get; set; }
    public string CV_normalOS { get; set; }
    public string CV_defectiveOS { get; set; }
    public string DistSphNVOD { get; set; }
    public string AddNVOD { get; set; }
    public string DistSphNVOS { get; set; }
    public string AddNVOS { get; set; }
    public string Remarks { get; set; }
    public string PD { get; set; }
    public string MPDOD { get; set; }
    public string MPDOS { get; set; }
    public int DV { get; set; }
    public int NV { get; set; }
    public string DVName { get; set; }
    public string NVName { get; set; }
    public int CreatedBy { get; set; }
    public Boolean Central { get; set; }
    public Boolean Steady { get; set; }
    public Boolean Maintained { get; set; }
    public Boolean Uncentral { get; set; }
    public Boolean Unsteady { get; set; }
    public Boolean Unmaintained { get; set; }
    public Boolean CentralOS { get; set; }
    public Boolean SteadyOS { get; set; }
    public Boolean MaintainedOS { get; set; }
    public Boolean UncentralOS { get; set; }
    public Boolean UnsteadyOS { get; set; }
    public Boolean UnmaintainedOS { get; set; }
    public string ChartType { get; set; }
    public int RegistrationTranID { get; set; }


}

public class PGPMas
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string DistSph { get; set; }
    public string DistSphOS { get; set; }
    public string NearCyl { get; set; }
    public string NearCylOS { get; set; }
    public string PinAxis { get; set; }
    public string PinAxisOS { get; set; }
    public string Add { get; set; }
    public string AddOS { get; set; }
    public string Details { get; set; }
    public DateTime CreatedUTc { get; set; }
    public int? Subcategory { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }
}
public class VISUALACUITYMas
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public int? SubCategory { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string DistSph { get; set; }
    public string DistSphOS { get; set; }
    public string NearCyl { get; set; }
    public string NearCylOS { get; set; }
    public string PinAxis { get; set; }
    public string PinAxisOS { get; set; }
    public string PowerGlass { get; set; }
    public string PowerGlassOS { get; set; }
    public string N_V_DESC { get; set; }
    public string N_V_DESCOS { get; set; }
    public DateTime CreatedUTc { get; set; }
    public string ChartType { get; set; }
    public string Remarks { get; set; }
    public int RegistrationTranID { get; set; }

}

public class paediatricvisualacuity
{
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public Boolean Central { get; set; }
    public Boolean Steady { get; set; }
    public Boolean Maintained { get; set; }
    public Boolean Uncentral { get; set; }
    public Boolean Unsteady { get; set; }
    public Boolean Unmaintained { get; set; }
    public Boolean CentralOS { get; set; }
    public Boolean SteadyOS { get; set; }
    public Boolean MaintainedOS { get; set; }
    public Boolean UncentralOS { get; set; }
    public Boolean UnsteadyOS { get; set; }
    public Boolean UnmaintainedOS { get; set; }
    public int CreatedBy { get; set; }
    public string ChartType { get; set; }
    public string Remarks { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int SubCategory { get; set; }
}
public class visualacuitypaediatric
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public Boolean Central { get; set; }
    public Boolean Steady { get; set; }
    public Boolean Maintained { get; set; }
    public Boolean Uncentral { get; set; }
    public Boolean Unsteady { get; set; }
    public Boolean Unmaintained { get; set; }
    public Boolean CentralOS { get; set; }
    public Boolean SteadyOS { get; set; }
    public Boolean MaintainedOS { get; set; }
    public Boolean UncentralOS { get; set; }
    public Boolean UnsteadyOS { get; set; }
    public Boolean UnmaintainedOS { get; set; }
    public DateTime CreatedUTc { get; set; }
    public string ChartType { get; set; }
    public string Remarks { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }
}
public class Amslermas
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public Boolean A_n_OD { get; set; }
    public Boolean A_abn_OD { get; set; }
    public string Desc_Text { get; set; }
    public string OcularOS { get; set; }
    public Boolean A_n_OS { get; set; }
    public Boolean A_abn_OS { get; set; }
    public string Desc_TextOS { get; set; }
    public int? Subcategory { get; set; }
    public DateTime CreatedUTc { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }
}

public class REFRACTIONEXTMast
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public int? SubCategory { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string Sphdry { get; set; }
    public string Cyldry { get; set; }
    public string Axisdry { get; set; }
    public string Sphwet { get; set; }
    public string Cylwet { get; set; }
    public string Axiswet { get; set; }
    public int TypeDry { get; set; }
    public string Sphdryos { get; set; }
    public string Cyldryos { get; set; }
    public string Axisdryos { get; set; }
    public string Sphwetos { get; set; }
    public string Cylwetos { get; set; }
    public string Axiswetos { get; set; }
    public int TypeWet { get; set; }
    public DateTime CreatedUTc { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }

}

public class ACCEPTANCEMas
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string Add { get; set; }
    public string DistSphOS { get; set; }
    public string NearCylOS { get; set; }
    public string PinAxisOS { get; set; }
    public string AddOS { get; set; }
    public string DistSphNVOD { get; set; }
    public string AddNVOD { get; set; }
    public string DistSphNVOS { get; set; }
    public string AddNVOS { get; set; }
    public string Remarks { get; set; }
    public string PD { get; set; }
    public string MPDOD { get; set; }
    public string MPDOS { get; set; }
    public int DV { get; set; }
    public int NV { get; set; }
    public string DVName { get; set; }
    public string NVName { get; set; }
    public DateTime CreatedUTc { get; set; }
    public int? Subcategory { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }

}

public class FINALPRESCRIPTIONMas
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string DistSph { get; set; }
    public string NearCyl { get; set; }
    public string PinAxis { get; set; }
    public string Add { get; set; }
    public string DistSphOS { get; set; }
    public string NearCylOS { get; set; }
    public string PinAxisOS { get; set; }
    public string AddOS { get; set; }
    public string DistSphNVOD { get; set; }
    public string AddNVOD { get; set; }
    public string DistSphNVOS { get; set; }
    public string AddNVOS { get; set; }
    public string Remarks { get; set; }
    public string PD { get; set; }
    public string MPDOD { get; set; }
    public string MPDOS { get; set; }
    public int DV { get; set; }
    public int NV { get; set; }
    public string DVName { get; set; }
    public string NVName { get; set; }
    public DateTime CreatedUTc { get; set; }
    public int? Subcategory { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }
}

public class ColorVisionMas
{
    public int ID { get; set; }
    public int IDOS { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public string OcularOS { get; set; }
    public string CV_normal { get; set; }
    public string CV_defective { get; set; }
    public string Desc_Text { get; set; }
    public string CV_normalOS { get; set; }
    public string CV_defectiveOS { get; set; }
    public string Desc_TextOS { get; set; }
    public DateTime CreatedUTc { get; set; }
    public int? Subcategory { get; set; }
    public Boolean OD { get; set; }
    public Boolean OS { get; set; }
    public int RegistrationTranID { get; set; }
}
public class Historyvisualacuitypaediatric
{
    public string ChartType { get; set; }
    public string Remarks { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public Boolean Central { get; set; }
    public Boolean Steady { get; set; }
    public Boolean Maintained { get; set; }
    public Boolean Uncentral { get; set; }
    public Boolean Unsteady { get; set; }
    public Boolean Unmaintained { get; set; }
    public Boolean CentralOS { get; set; }
    public Boolean SteadyOS { get; set; }
    public Boolean MaintainedOS { get; set; }
    public Boolean UncentralOS { get; set; }
    public Boolean UnsteadyOS { get; set; }
    public Boolean UnmaintainedOS { get; set; }
    public DateTime createby { get; set; }
}
public class Historyvisualacuitypaediatric1
{
    public string ChartType { get; set; }
    public string Remarks { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string Ocular { get; set; }
    public Boolean Central { get; set; }
    public Boolean Steady { get; set; }
    public Boolean Maintained { get; set; }
    public Boolean Uncentral { get; set; }
    public Boolean Unsteady { get; set; }
    public Boolean Unmaintained { get; set; }
    public Boolean CentralOS { get; set; }
    public Boolean SteadyOS { get; set; }
    public Boolean MaintainedOS { get; set; }
    public Boolean UncentralOS { get; set; }
    public Boolean UnsteadyOS { get; set; }
    public Boolean UnmaintainedOS { get; set; }
    public DateTime createby { get; set; }
}

public class HistoryVisualacuity1
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
    public DateTime createby { get; set; }
}
public class Historypgp
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

public class Historycv
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string CV_normal { get; set; }
    public string CV_defective { get; set; }
    public string Desc_Text { get; set; }
    public string CV_normal1 { get; set; }
    public string CV_defective1 { get; set; }
    public string Desc_Text1 { get; set; }
}

public class Historyrefraction
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

public class Historypgp1
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

public class Historycv1
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string CV_normal { get; set; }
    public string CV_defective { get; set; }
    public string Desc_Text { get; set; }
    public string CV_normal1 { get; set; }
    public string CV_defective1 { get; set; }
    public string Desc_Text1 { get; set; }
}

public class Historyrefraction1
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