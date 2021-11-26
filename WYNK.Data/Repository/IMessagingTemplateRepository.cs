
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository
{
    public interface IMessagingTemplateRepository : IRepositoryBase<MessagingTemplate>
    {

        dynamic InsertMessagingTemplate(MessagingTemplate MT);

        dynamic InsertBulsmstemplateTemplate(MessagingTemplate MT);
        

        MessagingTemplate GetDBColumns(string Statuscolumn);

        MessagingTemplate Gettemplates(int cmpid);
        MessagingTemplate Getformattemplates(int cmpid, string format);
        MessagingTemplate Getperioddetails(int cmpid, string periodtype,string visisttype);
        MessagingTemplate Getspecificperioddetails(int cmpid, string fromdate, string todate,string visisttype);
        MessagingTemplate Updatesmstemplate(int cmpid, string Desc, string id);
        MessagingTemplate Updatemailtemplate(int cmpid, string Desc, string id);
        MessagingTemplate Updatewhatsapptemplate(int cmpid, string Desc, string id);
        dynamic GetAadhaarOTP(string Aadhaatrnumber);
        dynamic Panverification(string pan, int amount);
        dynamic SubmitAadhaarOTP(string OTP, string ClientID);
        
    }
}
