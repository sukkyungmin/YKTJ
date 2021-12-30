using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WasteReport
{
    class LogWriter
    {
        public void LOG(string txt, string filename)
        {
            for (int cnt = 0; cnt < 2; cnt++)
            {
                try
                {
                    lock (this)
                    {
                        TextRW LOG = new TextRW();

                        if (LOG.WriteTxt(string.Format("./LOG/{0}/{1}/", DateTime.Now.Year, DateTime.Now.Month),
                            string.Format("{0}_{1}.TXT", DateTime.Now.Day.ToString(), filename),
                            string.Format("{0}\t\t//{1}", DateTime.Now.ToString(), txt), false) == true) return;
                    }
                }
                catch { }
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            try
            {
                TextRW LOG_ERR = new TextRW();

                LOG_ERR.WriteTxt(string.Format("./LOG/{0}/{1}/", DateTime.Now.Year, DateTime.Now.Month),
                    string.Format("{0}_{1}.TXT", DateTime.Now.Day.ToString(), "LOG_ERR"),
                    string.Format("{0}\t\t//{1}", DateTime.Now.ToString(), txt), false);
            }
            catch { }
        }

        public void HashLOG(Hashtable ht, string filename)
        {
            string txt = null;
            for (int cnt = 0; cnt < 2; cnt++)
            {
                try
                {
                    lock (this)
                    {
                        TextRW LOG = new TextRW();

                        txt += ht["date"];
                        txt += ht["time"];
                        txt += ht["bodyN"];
                        txt += ht["commitN"];
                        txt += ht["01value"];
                        txt += ht["02value"];
                        txt += ht["03value"];
                        txt += ht["04value"];
                        txt += ht["05value"];

                        txt += "\r\n";

                        if (LOG.WriteTxt(string.Format("./LOG/{0}/{1}/", DateTime.Now.Year, DateTime.Now.Month),
                            string.Format("{0}_{1}.TXT", DateTime.Now.Day.ToString(), filename),
                            string.Format("{0}\t\t//{1}", DateTime.Now.ToString(), txt), false) == true) return;
                    }
                }
                catch { }
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            try
            {
                TextRW LOG_ERR = new TextRW();

                LOG_ERR.WriteTxt(string.Format("./LOG/{0}/{1}/", DateTime.Now.Year, DateTime.Now.Month),
                    string.Format("{0}_{1}.TXT", DateTime.Now.Day.ToString(), "LOG_ERR"),
                    string.Format("{0}\t\t//{1}", DateTime.Now.ToString(), txt), false);
            }
            catch { }
        }
    }
}
