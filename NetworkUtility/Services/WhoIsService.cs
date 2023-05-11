using NetworkUtility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private readonly CheckHostName _checkHostName;

        public WhoIsService()
        {
            response = null;
            host = null;
            _checkHostName = new CheckHostName();
        }

        public WhoIsService(string host)
        {
            response = null;
            
            bool isValid = _checkHostName.CheckHostNameOrAddress(host);

            if (!isValid) this.host = null;
            else this.host = host;
        }

        public WhoisResponse? WhoIs()
        {
            if (host == null) return null;

            response = WhoisClient.Query(host);

            return response;
        }

        public WhoisResponse? WhoIs(string host)
        {
            bool isValid = _checkHostName.CheckHostNameOrAddress(host);

            if(!isValid) return null;

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
                AnsiConsole.MarkupLine($"[blue]{WhoIsService.OrganizationName}[/]");

                AnsiConsole.MarkupLine($"{response.OrganizationName}");

                Console.WriteLine();
            }
            if (AddressRange && (response.AddressRange != null))
            {
                AnsiConsole.MarkupLine($"[blue]{WhoIsService.AddressRange}[/]");

                AnsiConsole.MarkupLine("{0} - {1}", response.AddressRange.Begin, response.AddressRange.End);

                Console.WriteLine();
            }
            if (RespondedServers && (response.RespondedServers != null))
            {
                AnsiConsole.MarkupLine($"[blue]{WhoIsService.RespondedServers}[/]");

                foreach (var server in response.RespondedServers)
                {
                    AnsiConsole.MarkupLine(server);
                }

                Console.WriteLine();
            }
            if (RawData && (response.Raw != null))
            {
                AnsiConsole.MarkupLine($"[blue]{WhoIsService.RawData}[/]");

                AnsiConsole.MarkupLine(response.Raw);

                Console.WriteLine();
            }

        }
    }
}
