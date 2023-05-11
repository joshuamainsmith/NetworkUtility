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

        public bool WriteLogsAppend(string path, StringBuilder buffer, string fileName = @"LogInfo.csv")
        {
            var isValid = CheckDirectoryPath(path);
            if (!isValid) return false;

            path += fileName;
            this.path = path;
            this.buffer = buffer;

            writeText = File.AppendText(path);
            writeText.WriteLine(buffer);
            writeText.Flush();

            return true;
        }

        public bool WriteLogsAppend(string path, string buffer)
        {
            return WriteLogsAppend(path, new StringBuilder(buffer));
        }

        static bool CheckDirectoryPath(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    var dirInfo = Directory.CreateDirectory(path);
                    AnsiConsole.MarkupLine($"[green]Saved directory location[/]: {dirInfo.FullName}");
                }

                return true;
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine("[red]{0}[/]: {1}", e.GetType().Name, e.Message);
            }

            return false;
        }
    }
}
