
using System;
using System.Linq;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    public class ConfigureRepository : RepositoryBase<ConfigureViewModel>, IConfigureRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
       
        public ConfigureRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }


        public dynamic InsertCon(ConfigureViewModel Con)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var ConfigureView = new Configure();
                    ConfigureView.RRDescription = Con.Cons.RRDescription;
                    ConfigureView.RRAdvDays = Con.Cons.RRAdvDays;
                    ConfigureView.FrequencyperDay = Con.Cons.FrequencyperDay;
                    ConfigureView.HostEmailID = Con.Cons.HostEmailID;
                    if (Con.Cons.HostPassword != null)
                    {
                        var Password = PasswordEncodeandDecode.EncodePasswordToBase64(Con.Cons.HostPassword);
                        ConfigureView.HostPassword = Password;
                    }

                    ConfigureView.Phonenumber = Con.Cons.Phonenumber;
                    ConfigureView.SendSMS = Con.Cons.SendSMS;
                    ConfigureView.SendEmail = Con.Cons.SendEmail;
                    ConfigureView.CMPID = Con.Cons.CMPID;
                    ConfigureView.CreatedBy = Con.Cons.CreatedBy;
                    ConfigureView.CreatedUTC = DateTime.UtcNow;


                    WYNKContext.Configuration.Add(ConfigureView);
                    WYNKContext.SaveChanges();

                    if (Con.ConfigureTranss.Count() > 0)
                    {
                        foreach (var config in Con.ConfigureTranss.ToList())
                        {
                            var configtrans = new ConfigureTrans();
                            configtrans.ConfigurationID = ConfigureView.ID;
                            configtrans.Frequencytime = config.Frequencytime;
                            configtrans.NotifyPatient_Mail = config.NotifyPatient_Mail;
                            configtrans.NotifyPatient_SMS = config.NotifyPatient_SMS;
                            configtrans.NotifyPatient_Whatsapp = config.NotifyPatient_Whatsapp;
                            configtrans.NotifyDoctor_Whatsapp = config.NotifyDoctor_Whatsapp;
                            configtrans.NotifyDoctor_Mail = config.NotifyDoctor_Mail;
                            configtrans.NotifyDoctor_SMS = config.NotifyDoctor_SMS;
                            configtrans.CreatedBy = Con.Cons.CreatedBy;
                            configtrans.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.ConfigurationTrans.AddRange(configtrans);
                        }
                    }
                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();

                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new
                            {
                                Success = true,
                                Message = "Saved successfully"
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };
            }
        }


        public dynamic ConfiguretransDet(int ID)
        {
            var configuretransdetail = new ConfigureViewModel();

            var Config = WYNKContext.Configuration.ToList();
            var ConfigTran = WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == ID).ToList();

            configuretransdetail.getConfigureationTrans = (from C in Config
                                                           join Ct in ConfigTran
                                                           on C.ID equals Ct.ConfigurationID

                                                           select new getConfigureationTrans
                                                           {
                                                               RRDescription = C.RRDescription,
                                                               Frequencytime = Ct.Frequencytime,
                                                               NotifyDoctor_Mail = Ct.NotifyDoctor_Mail,
                                                               NotifyDoctor_SMS = Ct.NotifyDoctor_SMS,
                                                               NotifyPatient_Mail = Ct.NotifyPatient_Mail,
                                                               NotifyPatient_SMS = Ct.NotifyPatient_SMS,
                                                               NotifyPatient_Whatsapp = Ct.NotifyPatient_Whatsapp,
                                                               NotifyDoctor_Whatsapp = Ct.NotifyDoctor_Whatsapp,
                                                               
                                                               

                                                           }).ToList();

            return configuretransdetail;

        }
        public dynamic Gettrans(int ID)
        {
            var configuredetail = new ConfigureViewModel();
            var config = WYNKContext.Configuration.ToList();
            var configtrans = WYNKContext.ConfigurationTrans.ToList();

            configuredetail.ConfigureDetails1 = (from C in config.Where(x => x.ID == ID)
                                                 join Ct in configtrans
                                                 on C.ID equals Ct.ConfigurationID

                                                 select new ConfigureDetails1
                                                 {
                                                     id = C.ID,
                                                     RRDescription = C.RRDescription,
                                                     RRAdvdays = C.RRAdvDays,
                                                     FrequencyperDay = C.FrequencyperDay,
                                                     HostEmailID = C.HostEmailID,
                                                     HostPassword = PasswordEncodeandDecode.DecodeFrom64(C.HostPassword),
                                                     Phonenumber = C.Phonenumber,
                                                     SendSMS = C.SendSMS,
                                                     SendEmail = C.SendEmail,
                                                     Frequencytime = Ct.Frequencytime,


                                                 }).ToList();


            return configuredetail;

        }





        public dynamic UpdateCon(ConfigureViewModel Con1, int ID)

        {
            using (var dbContextTransaction = WYNKContext.Database.BeginTransaction())
            {
                try
                {
                    var configuredetail = new Configure();
                    var configs = new ConfigureTrans();
                    configuredetail = WYNKContext.Configuration.Where(x => x.ID == ID).FirstOrDefault();
                    configuredetail.RRDescription = Con1.Cons.RRDescription;
                    configuredetail.RRAdvDays = Con1.Cons.RRAdvDays;
                    configuredetail.FrequencyperDay = Con1.Cons.FrequencyperDay;
                    configuredetail.HostEmailID = Con1.Cons.HostEmailID;
                    if (Con1.Cons.HostPassword != null)
                    {
                        var PasswordUpdate = PasswordEncodeandDecode.EncodePasswordToBase64(Con1.Cons.HostPassword);
                        configuredetail.HostPassword = PasswordUpdate;
                    }

                    configuredetail.CMPID = Con1.Cons.CMPID;
                    configuredetail.UpdatedBy = Con1.Cons.UpdatedBy;
                    configuredetail.UpdatedUTC = DateTime.UtcNow;
                    WYNKContext.Configuration.UpdateRange(configuredetail);
                    WYNKContext.SaveChanges();

                    var configss = WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == ID);
                    if (configss != null)
                    {

                        WYNKContext.ConfigurationTrans.RemoveRange(WYNKContext.ConfigurationTrans.Where(x => x.ConfigurationID == ID).ToList());
                        WYNKContext.SaveChanges();
                    }




                    if (Con1.ConfigureTranss.Count() > 0)
                    {



                        foreach (var config in Con1.ConfigureTranss.ToList())
                        {
                            var configtrans = new ConfigureTrans();
                            int getid = configuredetail.ID;
                            
                            configtrans.ConfigurationID = getid;
                            configtrans.Frequencytime = config.Frequencytime;
                            configtrans.NotifyPatient_Mail = config.NotifyPatient_Mail;
                            configtrans.NotifyPatient_SMS = config.NotifyPatient_SMS;
                            configtrans.NotifyDoctor_Mail = config.NotifyDoctor_Mail;
                            configtrans.NotifyDoctor_SMS = config.NotifyDoctor_SMS;
                            configtrans.NotifyPatient_Whatsapp = config.NotifyPatient_Whatsapp;
                            configtrans.NotifyDoctor_Whatsapp = config.NotifyDoctor_Whatsapp;

                            configtrans.CreatedBy = Convert.ToInt32(Con1.Cons.UpdatedBy);
                            configtrans.CreatedUTC = DateTime.UtcNow;
                            WYNKContext.ConfigurationTrans.AddRange(configtrans);
                            WYNKContext.SaveChanges();

                        }

                    }
                    WYNKContext.SaveChanges();
                    dbContextTransaction.Commit();


                    try
                    {
                        if (WYNKContext.SaveChanges() >= 0)
                            return new

                            {
                                Success = true,
                                Message = "Updated Sucessfully",
                                Id = configuredetail.ID,
                            };
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Console.Write(ex);
                }
                return new
                {
                    Success = false,
                    Message = "Some Data Is Missing"
                };
            }

        }

    }
}



