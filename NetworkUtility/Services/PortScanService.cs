﻿using NetTools;
using NetworkUtility.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class PortScanService
    {
        private readonly ExportCSV _exportCSV;

        int port { get; set; }
        string host { get; set; }
        List<IPEndPoint> endPointList { get; set; }

        /********************************************
         * TODO:                                    *
         * continuous port scan                     *
         * Match open port to common service        *
         * param validation checking                *
         * scan ports or ips by file                *
         * save ports scanned into a list?          *
         ********************************************/

        public PortScanService(string host = "127.0.0.1", string port = "0")
        {
            this.port = Int32.Parse(port);
            this.host = host;
            endPointList = new List<IPEndPoint>();
            _exportCSV = new ExportCSV();
        }

        /// <summary>
        /// Scans a single port given a host.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// TODO: validation checking for params
        public void ScanPort(string host, string port) 
        { 
            ScanPort(host, Int32.Parse(port));            
        }

        /// <summary>
        /// Scans a single port given a host.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void ScanPort(IPAddress host, int port)
        {
            ScanPort(host.ToString(), port);
        }

        /// <summary>
        /// Scans a single endpoint.
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public void ScanEndPoint(IPEndPoint endPoint)
        {
            ScanPort(endPoint.Address, endPoint.Port);
        }

        /// <summary>
        /// Scans a single endpoint.
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public void ScanEndPoint(string ipEndPoint)
        {
            IPEndPoint endPoint = CreateIPEndPoint(ipEndPoint);

            ScanEndPoint(endPoint);
        }

        /// <summary>
        /// Scans an array of endpoints.
        /// </summary>
        /// <param name="ipEndPoints"></param>
        /// TODO: validate return value from CreateIPEndPoint
        public void ScanEndPoint(params string[] ipEndPoints)
        {
            foreach (var ipEndPoint in ipEndPoints)
            {
                IPEndPoint endPoint = CreateIPEndPoint(ipEndPoint);

                ScanEndPoint(endPoint);
            }
        }

        /// <summary>
        /// Scans a list of endpoints given a filepath.
        /// </summary>
        /// <param name="filePath"></param>
        public void ScanEndPointsByFile(string filePath)
        {
            bool isValid = ExportCSV.CheckFilePath(filePath);
            if (!isValid) return;

            try
            {
                StreamReader endPoints = new StreamReader(filePath);
                var endPoint = endPoints.ReadLine();

                while (endPoint != null)
                {
                    if (endPoint != String.Empty)
                    {
                        ScanEndPoint(endPoint);
                    }

                    endPoint = endPoints.ReadLine();
                }

                endPoints.Close();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return;
            }
        }

        /// <summary>
        /// Scans an array of ports given a host.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="ports"></param>
        public void ScanPorts(string host, params int[] ports)
        {
            foreach (var port in ports)
            {
                ScanPort(host, port);
            }
        }

        /// <summary>
        /// Scans an array of hosts by an array of ports. Each host has a one to many relation with the ports.
        /// </summary>
        /// <param name="hosts"></param>
        /// <param name="ports"></param>
        /// <example>
        /// Usage:
        /// <c>ScanPorts(new[] { "google.com", "bing.com" }, 80, 8080)</c>
        /// </example>
        public void ScanPorts(string[] hosts, params int[] ports)
        {
            foreach (var host in hosts)
            {
                foreach (var port in ports)
                {
                    ScanPort(host, port);
                }
            }
        }

        /// <summary>
        /// Scans a range of port connections given aan array of hosts.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="startPort"></param>
        /// <param name="endPort"></param>
        /// /// <example>
        /// Usage:
        /// <c>ScanPortByRange(new[] { "google.com", "bing.com" }, 1, 1000)</c>
        /// </example>
        /// TODO: validation checking for params
        public void ScanPortByRange(string[] hosts, string startPort, string endPort)
        {
            var start = Int32.Parse(startPort);
            var end = Int32.Parse(endPort);

            foreach (var host in hosts)
            {
                for (var port = start; port < end; port++)
                {
                    ScanPort(host, port);
                }
            }
        }

        /// <summary>
        /// Scans a single port given a host.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void ScanPort(string host, int port)
        {
            this.host = host;
            this.port = port;

            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(this.host, this.port);
                    AnsiConsole.MarkupLine($"[green]Port {this.port} is open on {this.host}[/]");
                    UpdatePortList(this.host, this.port);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Port {this.port} is not open on {this.host}[/]");
                    #if DEBUG
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                    Console.WriteLine();
                    #endif
                }
            }
        }              

        /// <summary>
        /// Parses an endpoint string. Throws an exception if the wrong format is detected.
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length != 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
            int port;
            if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }

        /// <summary>
        /// Updates the endpoint list with every successful port connection.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool UpdatePortList(string host, int port)
        {
            if (host.Equals(String.Empty) || port.Equals(String.Empty)) return false;

            IPEndPoint endPoint = CreateIPEndPoint(host + ":" + port);

            endPointList.Add(endPoint);

            return true;
        }

        /// <summary>
        /// Exports open port endpoints to a CSV file. The default file location and filename is %temp% and PortInfo.csv respectively.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void ExportPortLogs(string? path = null, string fileName = @"PortInfo.csv")
        {
            if (endPointList is null)
            {
                AnsiConsole.MarkupLine("[red]Nothing to export[/]");
                return;
            }

            if (path is null) path = Path.GetTempPath();

            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("Host,Port,Endpoint");
            foreach (var endPoint in endPointList)
            {
                buffer.AppendLine(
                    endPoint.Address.ToString() + "," +
                    endPoint.Port.ToString() + "," +
                    endPoint.ToString()
                    );
            }

            var isExported = _exportCSV.WriteLogsAppend(path, buffer, fileName);

            if (isExported) AnsiConsole.MarkupLine($"[green]File exported to[/] {path + fileName}");
            else AnsiConsole.MarkupLine($"[red]Failed to export to[/] {path + fileName}");
        }
    }
}
