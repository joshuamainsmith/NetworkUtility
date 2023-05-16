using System;
using System.Collections.Generic;
using System.Linq;
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
                    #if DEBUG
                        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                        Console.WriteLine();
                    #endif
                }
            }
        }
    }
}
