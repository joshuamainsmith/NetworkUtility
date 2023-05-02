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
    }
}
