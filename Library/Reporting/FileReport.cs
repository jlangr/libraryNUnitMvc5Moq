using System;
using System.IO;
using System.Text;
using FluentFTP;
using System.Net;

namespace Library.Reporting
{
    public class FileReport : IReport
    {
        private bool isLoaded = false;
        private string filename;
        private string name;
        private string text;
        private const string FtpServer = //"ftp.somewhere.com";
               "gatekeeper.dec.com";

        public FileReport(string filename)
        {
            this.filename = filename;
            // copy file from remote server
            var ftp = new FtpClient(FtpServer);
            try
            {
                int reply;
                ftp.Credentials = new NetworkCredential("ftp", "");
                ftp.Connect();
                ftp.DownloadFile(@"c:\\temp\{this.filename}", this.filename);
                ftp.Disconnect();
            }
            catch (Exception e)
            {
                throw new Exception("unable to do it, ya!", e);
            }
            finally
            {
                if (ftp.IsConnected)
                {
                    try
                    {
                        ftp.Disconnect();
                    }
                    catch (Exception ioe)
                    {
                        Console.WriteLine(ioe.StackTrace);
                    }
                }
            }
        }

        public string Text
        {
            get
            {
                if (!isLoaded)
                    Load();
                return text;
            }
        }

        public string Name
        {
            get
            {
                if (!isLoaded)
                    Load();
                return name;
            }
        }

        public void Load()
        {
            try
            {
                var reader = new StreamReader(@"c:\\temp\\{this.filename}");
                var list = Load(reader);
                name = list[0];
                text = list[1];
                isLoaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception("error loading file", e);
            }
        }

        public string[] Load(StreamReader reader)
        {
            try
            {
                var first = reader.ReadLine();
                var buffer = new StringBuilder();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    buffer.Append(line);
                    buffer.AppendLine();
                }
                var rest = buffer.ToString();

                return new string[] { first, rest };
            }
            catch (Exception e)
            {
                throw new Exception($"unable to load", e);
            }
        }
    }
}