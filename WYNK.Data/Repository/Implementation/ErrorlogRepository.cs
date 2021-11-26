
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Implementation
{
    class ErrorlogRepository : RepositoryBase<ErrorLogviewmodel>, IErrorlogRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public ErrorlogRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;
            
        }

        public ErrorLogviewmodel Geterrorlogfile(DateTime FromDate, DateTime Todate, string Time)
        {
            var errorlog = new ErrorLogviewmodel();
            errorlog.Totalerrorlog = new List<Totalerrorlog>();
            var dates = new List<DateTime>();
            TimeSpan ts = TimeSpan.Parse(Time);

            for (var dt = FromDate.Add(ts); dt <= Todate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            foreach (var item in dates.ToList())
            {

                var foldername = "/ErrorLog/";
                var currentDir = Directory.GetCurrentDirectory();
                var path = currentDir + foldername + "Log_" + item.ToString("dd-MM-yyyy") + ".txt";


                if (File.Exists(path))
                {
                    var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        string data;
                        while ((data = streamReader.ReadLine()) != null)
                        {
                            var list = new Totalerrorlog();
                            list.totallines = data;
                            errorlog.Totalerrorlog.Add(list);

                        }
                    }
                }
                else
                {

                }

            }

            return errorlog;
        }

        public dynamic gettotallines()
        {
            var Counselmaster = new Counselling_Master();
            string[] lines;
            var list = new List<string>();
            var foldername = "/Whatsnew/";
            var currentDir = Directory.GetCurrentDirectory();
            var path = currentDir + foldername + "Whats new - ver 1.1.4.5" + ".txt";
            if (File.Exists(path))
            {
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
                Counselmaster.TOtalLines = lines;
            }
            return Counselmaster;
        }
        public dynamic gettotalreleases()
        {
            var Counselmaster = new Counselling_Master();
            Counselmaster.Textdetails = new List<Textdetails> ();
            var foldername = "/Whatsnew/OLD";
            var currentDir = Directory.GetCurrentDirectory();
            var path = currentDir + foldername;
            string folderPath = path;
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] files = dir.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            if(files.Count() != 0)
            {
                foreach (var item in files)
                {
                    var liist = new Textdetails();                    
                    liist.Datetime = item.LastWriteTime;
                    liist.Textname = item.Name;
                    Counselmaster.Textdetails.Add(liist);
                }
            }
              return Counselmaster;
        }


        public dynamic gettotalreleasesbasedondate(string textname)
        {
            var Counselmaster = new Counselling_Master();
            string[] lines;
            var list = new List<string>();
            var foldername = "/Whatsnew/OLD/";
            var currentDir = Directory.GetCurrentDirectory();
            var path = currentDir + foldername + textname;
            if (File.Exists(path))
            {
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
                Counselmaster.TOtalLines = lines;
            }
            return Counselmaster;
        }
    }
}
