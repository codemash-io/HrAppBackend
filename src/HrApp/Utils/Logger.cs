using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp
{
    public class Logger
    {
        private static readonly Logger _instance = new Logger();
        private static MemoryStream memoryStream = new MemoryStream();
        private string LastLog;

        private Logger()
        {
            memoryStream = new MemoryStream();
        }

        public static Logger GetLogger()
        {
            return _instance;
        }

        public static void ClearData()
        {
            memoryStream = new MemoryStream();
        }

        public void Log(String text)
        {
            UTF8Encoding uniEncoding = new UTF8Encoding();

            var message = uniEncoding.GetBytes(text);
            byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);

            memoryStream.Write(message, 0, message.Length);
            memoryStream.Write(newline, 0, newline.Length); //new line

            this.LastLog = text;
        }
        public static MemoryStream GetStream()
        {
            return memoryStream;
        }

        public string GetLastLog()
        {
            return LastLog;
        }

    }
}
