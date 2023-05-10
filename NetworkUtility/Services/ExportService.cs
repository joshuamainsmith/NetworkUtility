using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class ExportService
    {
        string path { get; set; }
        StreamWriter writeText { get; set; }
        StringBuilder buffer { get; set; }

        public ExportService()
        {
            path = string.Empty;
            buffer = new StringBuilder();
        }

        public bool WriteLogsAppend(string path, StringBuilder buffer)
        {
            this.path = path;
            this.buffer = buffer;
            writeText = File.AppendText(path);
            writeText.WriteLine(buffer);
            writeText.Flush();

            return false;
        }

        public bool WriteLogsAppend(string path, string buffer)
        {
            this.path = path;
            this.buffer = new StringBuilder(buffer);
            writeText = File.AppendText(path);
            writeText.WriteLine(buffer);
            writeText.Flush();

            return false;
        }
    }
}
