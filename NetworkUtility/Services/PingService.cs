using CsvHelper;
using NetTools;
using NetworkUtility.Helpers;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class PingService
    {
        private readonly CheckHostName _checkHostName;
        private readonly ExportCSV _exportCSV;

        Ping pingSender { get; set; }
        PingOptions pingOptions { get; set; }
        PingReply? pingReply { get; set; }
        List<PingReply> pingReplyList { get; set; }
        string data { get; set; }
        byte[] buffer { get; set; }
        int timeout { get; set; }
        string? address { get; set; }
        string[]? addresses { get; set; }
        string? filePath { get; set; }
        

        public PingService()
        {
            pingSender = new Ping();
            pingOptions = new PingOptions
            {
                DontFragment = true,
                Ttl = 64
            };
            pingReplyList = new List<PingReply>();
            data = "**Network Connection Test Ping**";
            buffer = Encoding.ASCII.GetBytes(data);
            timeout = 1000;
            _checkHostName = new CheckHostName();
            _exportCSV = new ExportCSV();
        }

        /// <summary>
        /// Sends an ICMP to the specified IP address or host name.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public PingReply? SendPing(string address)
        {
            if(address == String.Empty) return null;

            bool isValid = _checkHostName.CheckHostNameOrAddress(address);

            if(!isValid)
            {
                AnsiConsole.MarkupLine($"[red]{address} is an invalid address[/]\n");
                return null;
            }

            this.address = address;

            try
            {
                pingReply = pingSender.Send(address, timeout, buffer, pingOptions);
                var listUpdate = UpdatePingReplyList(pingReply);
                if (!listUpdate) AnsiConsole.MarkupLine($"[red]Failed to add {address} to the list[/]");
                ReadPingInfo(pingReply);
            }
            catch (PingException ex)
            {
                AnsiConsole.Markup($"[red]{address}[/] - ");
                AnsiConsole.MarkupLine($"[red]{ex.InnerException.Message}[/]");
                #if DEBUG
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                    Console.WriteLine();
                #endif
                return null;
            }

            return pingReply;
        }

        /// <summary>
        /// A wrapper for SendPing(string) that can take multiple IP addresses or host names as input.
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        public PingReply? SendPing(params string[] addresses)
        {
            foreach (string address in addresses)
            {
                if (address != String.Empty)
                {
                    pingReply = SendPing(address); 
                }
            }

            return pingReply ?? null;
        }

        /// <summary>
        /// Sends pings asynchronously
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        async Task<PingReply> SendPingByAsync(IPAddress ip)
        {
            this.address = ip.ToString();
            AutoResetEvent waiter = new AutoResetEvent(false);

            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            pingSender.SendAsync(ip, timeout, buffer, pingOptions, waiter);

            waiter.WaitOne();
            
            return pingReply ?? null;
        }

        /// <summary>
        /// Sends a ping asynchronously
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        PingReply SendPingByAsync(string ip)
        {
            this.address = ip.ToString();
            AutoResetEvent waiter = new AutoResetEvent(false);

            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            pingSender.SendAsync(ip, timeout, buffer, pingOptions, waiter);

            waiter.WaitOne();

            return pingReply ?? null;
        }

        /// <summary>
        /// Callback for SendPingByAsyc() methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ((AutoResetEvent)e.UserState).Set();
            }

            if (e.Error != null)
            {
                ((AutoResetEvent)e.UserState).Set();
            }

            pingReply = e.Reply;

            if (pingReply != null)
            {
                var listUpdate = UpdatePingReplyList(pingReply);
                if (!listUpdate) AnsiConsole.MarkupLine($"[red]Failed to add {pingReply.Address} to the list[/]");
            }

            ((AutoResetEvent)e.UserState).Set();
        }

        /// <summary>
        /// Sends a range of pings in parallel
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        /// for later reference: 
        /// https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.parallel?view=net-7.0
        /// https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-parallel-for-loop-with-thread-local-variables
        public async Task<PingReply>? SendPingByRangeAsync(string startAddress, string endAddress)
        {
            if (startAddress == String.Empty || endAddress == String.Empty) return null;

            bool isValid = CheckIPByRange(startAddress, endAddress);

            if (!isValid) return null;

            var start = IPAddress.Parse(startAddress);
            var end = IPAddress.Parse(endAddress);

            var first = (long)(uint) IPAddress.NetworkToHostOrder((int) IPAddress.Parse(start.ToString()).Address);
            var last = (long)(uint) IPAddress.NetworkToHostOrder((int) IPAddress.Parse(end.ToString()).Address);

            var range = new IPAddressRange(start, end);

            /*foreach (var ip in range)
            {
                SendPingByAsync(ip);
                ReadPingInfo(pingReply);
            }*/

            var result = Parallel.For(
                first, last, (i, state) =>
            {
                if (state.ShouldExitCurrentIteration)
                {
                    if (state.LowestBreakIteration < i)
                        return;
                }

                var pingReply = SendPingByAsync(IPAddress.Parse(i.ToString()).ToString());

                if (pingReply is not null) ReadPingInfo(pingReply);
                else Console.WriteLine($"{IPAddress.Parse(i.ToString()).ToString()} not found");

            });

            return pingReply ?? null;
        }        

        /// <summary>
        /// Checks for valid start address and end address input
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        bool CheckIPByRange(string startAddress, string endAddress)
        {
            bool isValid = _checkHostName.CheckHostNameOrAddress(startAddress);
            bool startLength = IPAddress.Parse(startAddress).GetAddressBytes().Length is 4;
            bool endLength = IPAddress.Parse(endAddress).GetAddressBytes().Length is 4;

            if (!isValid && !startLength)
            {
                AnsiConsole.MarkupLine($"[red]{startAddress} is an invalid address[/]\n");
                return false;
            }

            isValid = _checkHostName.CheckHostNameOrAddress(endAddress);

            if (!isValid && !endLength)
            {
                AnsiConsole.MarkupLine($"[red]{endAddress} is an invalid address[/]\n");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sends each line from an input file to SendPing(string), each line being an address
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public PingReply? SendPingByFile(string filePath)
        {
            this.filePath = filePath;
            try 
            {
                StreamReader addresses = new StreamReader(filePath);
                var address = addresses.ReadLine();

                while (address != null)
                {
                    if (address != String.Empty)
                    {
                        pingReply = SendPing(address);
                    }

                    address = addresses.ReadLine();
                }

                addresses.Close();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return null;
            }

            return pingReply ?? null;
        }

        /// <summary>
        /// Writes PingReply info to the console
        /// </summary>
        /// <param name="pingReply"></param>
        /// <returns></returns>
        async Task ReadPingInfo(PingReply pingReply)
        {
            Console.Write(address + " | ");
            if (pingReply.Status == IPStatus.Success)
            {
                AnsiConsole.MarkupLine($"[green]{pingReply.Status}[/]");
                var table = new Table();

                table.Border(TableBorder.MinimalDoubleHead);

                table.AddColumn("[blue]Address[/]");
                table.AddColumn(new TableColumn("[blue]Address Family[/]"));
                table.AddColumn(new TableColumn("[blue]RoundTrip time[/]"));    
                table.AddColumn(new TableColumn("[blue]Time to live[/]"));
                table.AddColumn(new TableColumn("[blue]Don't fragment[/]"));
                table.AddColumn(new TableColumn("[blue]Buffer size[/]"));

                table.AddRow($"{pingReply.Address.ToString()}", $"{pingReply.Address.AddressFamily.ToString()}", $"{pingReply.RoundtripTime}",
                    $"{pingReply.Options.Ttl}", $"{pingReply.Options.DontFragment}", $"{pingReply.Buffer.Length}");

                AnsiConsole.Write(table);

                Console.WriteLine();
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]{pingReply.Status}[/]");
            }
        }

        /// <summary>
        /// Updates the pingReplyList property from each successful ping
        /// </summary>
        /// <param name="pingReply"></param>
        /// <returns></returns>
        bool UpdatePingReplyList(PingReply pingReply)
        {
            if (pingReply == null) return false;

            if (pingReply.Status == IPStatus.Success)
                pingReplyList.Add(pingReply);

            return true;
        }

        /// <summary>
        /// Utilizes exportCSV dependency by writing ping logs to a file location
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void ExportPingLogs(string? path = null, string fileName = @"PingInfo.csv")
        {
            if (path is null) path = Path.GetTempPath();

            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("Address,Address Family,Round Trip Time,TTL,Don't Fragment,Length");
            foreach (var pingReply in pingReplyList)
            {
                buffer.AppendLine(
                    pingReply.Address.ToString() + "," +
                    pingReply.Address.AddressFamily.ToString() + "," +
                    pingReply.RoundtripTime.ToString() + "," +
                    pingReply.Options.Ttl.ToString() + "," +
                    pingReply.Options.DontFragment.ToString() + "," +
                    pingReply.Buffer.Length.ToString()
                    );
            }

            var isExported = _exportCSV.WriteLogsAppend(path, buffer, fileName);

            if (isExported) AnsiConsole.MarkupLine($"[green]File exported to[/] {path + fileName}");
            else AnsiConsole.MarkupLine($"[red]Failed to export to[/] {path + fileName}");
        }        
    }
}
