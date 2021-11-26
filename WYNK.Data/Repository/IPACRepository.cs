using System;
using System.Collections.Generic;
using System.Text;
using WYNK.Data.Model.ViewModel;
using Microsoft.AspNetCore.Http;
namespace WYNK.Data.Repository
{
    public interface IPACRepository : IRepositoryBase<PACViewM>
    {
        PACViewM getPatientDrugtherapy(string ID, int CMPID);
        PACViewM getBMIrange(decimal ID);
        PACViewM getPACHistory(string UIN, int ADMID,int ID);
        PACViewM getPACExamination(string UIN, int ADMID,int ID);
        PACViewM getPACInvestigation(string UIN, int ADMID,int ID);
        PACViewM getPACPreOperative(string UIN, int ADMID, int ID);
        dynamic InserData(PACViewM PACHistory);
        dynamic PreOperativeInsert(PACViewM PreOperativeInstruction);
        dynamic InserExamData(PACViewM PACExamination);
        dynamic InserInvData(PACViewM PACInvestigation);
        dynamic UpdateData(PACViewM PACHistory,int PACHID);
        dynamic UpdateExamData(PACViewM PACExamination, int PACHID);
        dynamic UpdateInvData(PACViewM PACInvestigation, int PACHID);
        dynamic UpdatePreOperativeInstructions(PACViewM PACPreOperative, int PACPOID);
        dynamic Beforeinductionofanaesthesia(int CMPID, int SAID);
        dynamic Insertbeforeana(PACViewM Addbefore);
        dynamic Beforeinductionofanaesthesiaprint(int OLD, int SAID, int cmpid);
        dynamic PACHISTORYPRINT(int PACID, int cmpid);
        dynamic PACPREOPINSPRINT(int PPOID, int cmpid);
        dynamic getPACExaminationprint(int PacexamID, int cmpid);
        dynamic getPACInvestigationprint(int PACIVID, int cmpid);
    }
}
