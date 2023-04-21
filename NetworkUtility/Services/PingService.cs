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
        Ping pingSender { get; set; }
        PingOptions pingOptions { get; set; }
        PingReply pingReply { get; set; }
        string data { get; set; }
        byte[] buffer { get; set; }
        int timeout { get; set; }
        string address { get; set; }

        public PingService()
        {
            pingSender = new Ping();
            pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            data = "**Network Connection Test Ping**";
            buffer = Encoding.ASCII.GetBytes(data);
            timeout = 120;
            address = "localhost";
        }

        public PingReply SendPing(string address)
        {
            this.address = address;

            pingReply = pingSender.Send(address, timeout, buffer, pingOptions);

            return pingReply;
        }    

        public void ReadPingInfo(PingReply pingReply)
        {
            Console.WriteLine(pingReply.Status);
            if (pingReply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", pingReply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", pingReply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", pingReply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", pingReply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", pingReply.Buffer.Length);
            }
        }
    }
}
