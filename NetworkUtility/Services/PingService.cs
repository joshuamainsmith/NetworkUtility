using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    internal class PingService
    {
        public Ping pingSender { get; set; }
        public PingOptions pingOptions { get; set; }
        PingReply pingReply { get; set; }
        public string data { get; set; }
        public byte[] buffer { get; set; }
        public int timeout { get; set; }
        public string address { get; set; }

        public PingService()
        {
            pingSender = new Ping();
            pingOptions = new PingOptions();
        }
    }
}
