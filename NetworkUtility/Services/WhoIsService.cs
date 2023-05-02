using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whois.NET;

namespace NetworkUtility.Services
{
    public class WhoIsService
    {
        private const string OrganizationName = "Organization Name";
        private const string AddressRange = "Address Range";
        private const string RespondedServers = "Responded Servers";
        private const string RawData = "Raw Data";

        WhoisResponse? response { get; set; }
        string? host { get; set; }

        public WhoIsService()
        {
            response = null;
            host = null;
        }

        public WhoIsService(string host)
        {
            response = null;
            this.host = host;
        }

        public WhoisResponse? WhoIs()
        {
            if (host == null) return null;

            response = WhoisClient.Query(host);

            return response;
        }

        public WhoisResponse? WhoIs(string host)
        {
            this.host = host;

            response = WhoIs();

            return response;
        }

        public void GetWhoIsInfo(bool OrganizationName = false, bool AddressRange = false, 
            bool RespondedServers = false, bool RawData = true)
        {
            if (response== null) return;
            
            if (OrganizationName && (response.OrganizationName != null))
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write(WhoIsService.OrganizationName);

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine(response.OrganizationName);

                Console.WriteLine();
            }
            if (AddressRange && (response.AddressRange != null))
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write(WhoIsService.AddressRange);

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine("{0} - {1}", response.AddressRange.Begin, response.AddressRange.End);

                Console.WriteLine();
            }
            if (RespondedServers && (response.RespondedServers != null))
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write(WhoIsService.RespondedServers);

                Console.ResetColor();

                Console.WriteLine();

                foreach (var server in response.RespondedServers)
                {
                    Console.WriteLine(server);
                }

                Console.WriteLine();
            }
            if (RawData && (response.Raw != null))
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write(WhoIsService.RawData);

                Console.ResetColor();

                Console.WriteLine();

                Console.WriteLine(response.Raw);

                Console.WriteLine();
            }

        }
    }
}
