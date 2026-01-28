using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web;

namespace Web_TPMTrakDashboard
{
    public static class Logger
    {
        private static string appPath;
        private static object _locker;

        static Logger()
        {

            string ProgTime = String.Format("{0:dd_MMM_yyyy}", DateTime.Now);
            appPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")); //Environment.CurrentDirectory;
            //appPath = @"D:\\";
            _locker = new object();

            string LogsFolderPath = appPath + @"\Logs";
    //        if (Directory.Exists(LogsFolderPath) == false)
    //        {
				//try
				//{ 
				//	Directory.CreateDirectory(LogsFolderPath);
				//}
				//catch
				//{

				//}
    //        }

            appPath = Path.Combine(appPath, @"Logs\ANDONLog_" + ProgTime + ".txt");

        }

        public static void WriteDebugLog(string str)
        {
            StreamWriter writer = null;
            if (Monitor.TryEnter(_locker, 1000))
            {
                try
                {
                    writer = new StreamWriter(appPath, true, Encoding.UTF8, 8195);
                    writer.WriteLine(string.Format("{0} : DEBUG - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), str));
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
                catch { }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer.Dispose();
                    }
                    Monitor.Exit(_locker);
                }
            }
        }

        public static void WriteErrorLog(string str)
        {
            StreamWriter writer = null;
            if (Monitor.TryEnter(_locker, 1000))
            {
                try
                {
                    writer = new StreamWriter(appPath, true, Encoding.UTF8, 8195);
                    writer.WriteLine(string.Format("{0} : EXCEPTION - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), str));
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
                catch { }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer.Dispose();
                    }
                    Monitor.Exit(_locker);
                }
            }
        }

        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter writer = null;
            if (Monitor.TryEnter(_locker, 1000))
            {
                try
                {
                    writer = new StreamWriter(appPath, true, Encoding.UTF8, 8195);
                    writer.WriteLine(string.Format("{0} : Exception - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), ex));
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
                catch { }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer.Dispose();
                    }
                    Monitor.Exit(_locker);
                }
            }
        }
    }

}
