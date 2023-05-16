﻿using NetTools;
using System;
using System.Collections.Generic;
using System.Globalization;
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
             
        /************************************
         * TODO:                            *
         * scan port by file                *
         * scan port by endpoint            *
         * scan array of ports              *
         * scan array of ips                *
         * continuous port scan             *
         * param validation checking        *
         * save ports scanned into a list?  *
         ***********************************/

        public PortScanService(string host = "127.0.0.1", string port = "0")
        {
            this.port = Int32.Parse(port);
            this.host = host;
        }

        /// <summary>
        /// Scans a single port given a host. Wrapper for ScanPort(string, int).
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// TODO: validation checking for params
        public void ScanPort (string host, string port) 
        { 
            ScanPort(host, Int32.Parse(port));            
        }

        /// <summary>
        /// Scans a single port given a host. Wrapper for ScanPort(string, int).
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void ScanPort(IPAddress host, int port)
        {
            ScanPort(host.ToString(), port);
        }

        /// <summary>
        /// Scans a single endpoint. Wrapper for ScanPort(string, int).
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public void ScanEndPoint(string ipEndPoint)
        {
            IPEndPoint endPoint = CreateIPEndPoint(ipEndPoint);

            ScanPort(endPoint.Address, endPoint.Port);
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
        /// Scans a range of port connections given a host.
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
    }
}
