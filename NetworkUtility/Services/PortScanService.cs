using NetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class PortScanService
    {
        int port { get; set; }
        string host { get; set; }
             
        public PortScanService(string host = "127.0.0.1", string port = "0")
        {
            this.port = Int32.Parse(port);
            this.host = host;
        }

        /// <summary>
        /// Tests a single port given a host.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// TODO: validation checking for params
        public void ScanPort (string host, string port) 
        { 
            this.host = host;
            this.port = Int32.Parse(port);

            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(this.host, this.port);
                    AnsiConsole.MarkupLine($"[green]Port {this.port} is open on {this.host}[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Port {this.port} is not open on {this.host}[/]");
                    AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                    #if DEBUG
                        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                        Console.WriteLine();
                    #endif
                }
            }
        }

        /// <summary>
        /// Tests a range of port connections given a host.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="startPort"></param>
        /// <param name="endPort"></param>
        /// TODO: validation checking for params
        public void ScanPortByRange(string host, string startPort, string endPort)
        {
            this.host = host;
            var start = Int32.Parse(startPort);
            var end = Int32.Parse(endPort);

            for (var port = start; port < end; port++)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        tcpClient.Connect(this.host, port);
                        AnsiConsole.MarkupLine($"[green]Port {port} is open on {this.host}[/]");
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Port {port} is not open on {this.host}[/]");
                        AnsiConsole.MarkupLine($"[red]{ex}[/]");
                        #if DEBUG
                        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                        Console.WriteLine();
                        #endif
                    }
                }
            }
        }
    }
}
