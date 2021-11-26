using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Repository.Operation
{
    class ErrorLog
    {
        public bool WriteErrorLog(string EMessage, string LogMessage)
        {
            bool Status = false;
            var currentDir = Directory.GetCurrentDirectory();
            var res = Directory.CreateDirectory(currentDir + '/' + "ErrorLog");
            string LogDirectory = res.ToString();
            DateTime CurrentDateTime = DateTime.UtcNow;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);
            string logLine = BuildLogLine(CurrentDateTime, EMessage, LogMessage);
            //LogDirectory = (LogDirectory + '/' + "Log" + ".txt");
            LogDirectory = (LogDirectory + '/' + "Log_" + LogFileName(DateTime.UtcNow) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);

                    oStreamWriter.WriteLine(logLine);
                    Status = true;
                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }



        public bool WriteErrorLogTitle(string cmpname, string LogMessage, string huname, string uname, string huid, string uid, string mode)
        {
            bool Status = false;
            var currentDir = Directory.GetCurrentDirectory();
            var res = Directory.CreateDirectory(currentDir + '/' + "ErrorLog");
            string LogDirectory = res.ToString();
            DateTime CurrentDateTime = DateTime.UtcNow;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);
            string logLine = BuildLogLineTitle(CurrentDateTime, cmpname, LogMessage, huname, uname, huid, uid, mode);
            //LogDirectory = (LogDirectory + '/' + "Log" + ".txt");
            LogDirectory = (LogDirectory + '/' + "Log_" + LogFileName(DateTime.UtcNow) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);

                    oStreamWriter.WriteLine("\n" + logLine);
                    Status = true;
                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }


        public bool WriteErrorLogArray(string tname, object LogMessage)
        {
            bool Status = false;
            var currentDir = Directory.GetCurrentDirectory();
            var res = Directory.CreateDirectory(currentDir + '/' + "ErrorLog");
            string LogDirectory = res.ToString();

            DateTime CurrentDateTime = DateTime.UtcNow;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);
            object logLine = BuildLogLinearray(CurrentDateTime, tname, LogMessage);
            //LogDirectory = (LogDirectory + '/' + "Log" + ".txt");
            LogDirectory = (LogDirectory + '/' + "Log_" + LogFileName(DateTime.UtcNow) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(logLine);
                    Status = true;
                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }


        private bool CheckCreateLogDirectory(string LogPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(LogPath);
            if (oDirectoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(LogPath);
                    loggingDirectoryExists = true;
                }
                catch
                {
                    // Logging failure
                }
            }
            return loggingDirectoryExists;
        }


        private string BuildLogLine(DateTime CurrentDateTime, string EMessage, string LogMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(EMessage);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(LogMessage);
            return loglineStringBuilder.ToString();
        }


        private string BuildLogLineTitle(DateTime CurrentDateTime, string cmpname, string LogMessage, string huname, string uname, string huid, string uid, string mode)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(cmpname);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(LogMessage);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(huname);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(uname);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(huid);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(uid);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(mode);
            return loglineStringBuilder.ToString();
        }

        private object BuildLogLinearray(DateTime CurrentDateTime, string tname, object LogMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(tname);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(JsonConvert.SerializeObject(LogMessage));
            return loglineStringBuilder;
        }


        public string LogFileEntryDateTime(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }


        private string LogFileName(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MM-yyyy");
        }


        public bool ErrorLogArrayDrug(string tname, string message, object LogMessage)
        {
            bool Status = false;
            var currentDir = Directory.GetCurrentDirectory();
            var res = Directory.CreateDirectory(currentDir + '/' + "ErrorLog");
            string LogDirectory = res.ToString();

            DateTime CurrentDateTime = DateTime.UtcNow;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);
            object logLine = BuildLogLinearray1(CurrentDateTime, tname, message, LogMessage);
            LogDirectory = (LogDirectory + '/' + "Log" + ".txt");
            //LogDirectory = (LogDirectory + '/' + "Log_" + LogFileName(DateTime.Now) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(logLine);
                    Status = true;
                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }

        private object BuildLogLinearray1(DateTime CurrentDateTime, string tname, string message, object LogMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(tname);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(message);
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(JsonConvert.SerializeObject(LogMessage));
            return loglineStringBuilder;
        }
    }
}
