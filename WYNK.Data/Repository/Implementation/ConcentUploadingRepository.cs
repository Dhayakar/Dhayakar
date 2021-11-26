
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;


namespace WYNK.Data.Repository.Implementation
{
    public class ConcentUploadingRepository : RepositoryBase<ConcentUploadingViewModel>, IConcentUploadingRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        
        public ConcentUploadingRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic InsertConcent (ConcentUploadingViewModel con)
        {

            var Data = con.Description;
            var Modname = con.Modulename;
            var CMPID = con.CMPID;
            var currentDir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(currentDir + "/ConcernPages/" + CMPID + '/'))
                Directory.CreateDirectory(currentDir + "/ConcernPages/" + CMPID + '/');
            var fileName = $"{Modname}";
            var path = $"{currentDir}/ConcernPages/{CMPID}/{fileName}";

            if (File.Exists(path))
            {                
                File.Delete(path);
                TextWriter txt = new StreamWriter(path + ".txt");
                txt.Write(Data);
                txt.Close();

                
                if (!Directory.Exists(currentDir + "/ConcernPages/History/" + CMPID + '/'))
                    Directory.CreateDirectory(currentDir + "/ConcernPages/History/" + CMPID + '/');
                var histfileName = $"{Modname}";
                var idvalue = CMPSContext.ConsentExtension.OrderByDescending(x =>x.ID).Select(x => x.ID).FirstOrDefault();
                var incrementid = idvalue + 1;

                var histpath = $"{currentDir}/ConcernPages/History/{CMPID}/{fileName}-{incrementid}";
                TextWriter histtxt = new StreamWriter(histpath + ".txt");
                histtxt.Write(Data);
                histtxt.Close();

                var conset = new Consentextension();
                conset.CMPID = Convert.ToInt32(CMPID);
                conset.ConsentDescription = fileName;
                conset.CreatedUTC = DateTime.UtcNow;
                conset.Createdby = 1;
                conset.IsActive = true;
                conset.Consentpath = histpath + ".txt";
                CMPSContext.ConsentExtension.Add(conset);
                CMPSContext.SaveChanges();
            }
            else
            {
                if (!Directory.Exists(currentDir + "/ConcernPages/History/" + CMPID + '/'))
                    Directory.CreateDirectory(currentDir + "/ConcernPages/History/" + CMPID + '/');
                var histfileName = $"{Modname}";
                var idvalue = CMPSContext.ConsentExtension.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault();
                var incrementid = idvalue + 1;
                var histpath = $"{currentDir}/ConcernPages/History/{CMPID}/{fileName}-{incrementid}";
                TextWriter histtxt = new StreamWriter(histpath + ".txt");
                histtxt.Write(Data);
                histtxt.Close();

                TextWriter txt = new StreamWriter(path + ".txt");
                txt.Write(Data);
                txt.Close();

                var conset = new Consentextension();
                conset.CMPID = Convert.ToInt32(CMPID);
                conset.ConsentDescription = fileName;
                conset.CreatedUTC = DateTime.UtcNow;
                conset.Createdby = 1;
                conset.IsActive = true;
                conset.Consentpath = histpath + ".txt";
                CMPSContext.ConsentExtension.Add(conset);
                CMPSContext.SaveChanges();
            }

            try
            {
             
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

            return new
            {
                Success = false,
                Message = "Some Data Missing"

            };
        }



        public static void Log(string logInfo,string Modname)
        {
            File.AppendAllText(Modname+".txt", logInfo);

        }

        public dynamic Getallconnectionstring(int CMPID, string Module)
        {
            var data = new ConcentUploadingViewModel();
            var utctime = CMPSContext.Setup.Where(x => x.CMPID == CMPID).Select(x => x.UTCTime).FirstOrDefault();
            TimeSpan ts = TimeSpan.Parse(utctime);
            data.Concentdata = (from c in CMPSContext.ConsentExtension.Where(x => x.CMPID == CMPID && x.ConsentDescription == Module)
                                select new Concentdata
                                {
                                    date = c.CreatedUTC + ts,
                                    Description = c.ConsentDescription,
                                    TOtalLines = getallfiles(c.Consentpath),
                                }).ToList();

            return data.Concentdata;
        }


        public string[] getallfiles(string path)
        {
            var data = new ConcentUploadingViewModel();
            string[] lines;
            var list = new List<string>();
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            lines = list.ToArray();
            data.TOtalLines = lines;
            return data.TOtalLines;
        }

    }
}



