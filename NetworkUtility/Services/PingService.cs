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
        PingReply? pingReply { get; set; }
        string data { get; set; }
        byte[] buffer { get; set; }
        int timeout { get; set; }
        string? address { get; set; }
        string[]? addresses { get; set; }

        public PingService()
        {
            pingSender = new Ping();
            pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            data = "**Network Connection Test Ping**";
            buffer = Encoding.ASCII.GetBytes(data);
            timeout = 120;
        }

        public PingReply? SendPing(string address)
        {
            if(address == String.Empty) return null;

            this.address = address;

            pingReply = pingSender.Send(address, timeout, buffer, pingOptions);

            ReadPingInfo(pingReply);

            return pingReply;
        }

        public PingReply? SendPing(params string[] addresses)
        {
            this.addresses = addresses;

            foreach (string address in addresses)
            {
                if (address != String.Empty)
                {
                    pingReply = pingSender.Send(address, timeout, buffer, pingOptions);
                    ReadPingInfo(pingReply);
                }
            }

            return pingReply == null ? null : pingReply;
        }

        void ReadPingInfo(PingReply pingReply)
        {
            Console.WriteLine(pingReply.Status);
            if (pingReply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", pingReply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", pingReply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", pingReply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", pingReply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", pingReply.Buffer.Length);
                Console.WriteLine();
            }
            // TODO : else print reason ping failed
        }
    }
}
