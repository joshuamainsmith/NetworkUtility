using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class PingService
    {
        Ping pingSender { get; set; }
        PingOptions pingOptions { get; set; }
        PingReply? pingReply { get; set; }
        string data { get; set; }
        byte[] buffer { get; set; }
        int timeout { get; set; }
        string? address { get; set; }
        string[]? addresses { get; set; }
        string? filePath { get; set; }
        // https://regexper.com/#%5E%28www%5C.%29%3F%5Cw%2B%28%5C.%5Cw%2B%29%2B%24
        static Regex checkValidHostOrIP = new Regex(@"^(www\.)?\w+(\.\w+)+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public PingService()
        {
            pingSender = new Ping();
            pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            data = "**Network Connection Test Ping**";
            buffer = Encoding.ASCII.GetBytes(data);
            timeout = 1000;           
        }

        public PingReply? SendPing(string address)
        {
            if(address == String.Empty) return null;

            bool isValid = CheckHostNameOrAddress(address);

            if(!isValid) return null;

            this.address = address;

            try
            {
                Console.WriteLine(address);
                pingReply = pingSender.Send(address, timeout, buffer, pingOptions);
                ReadPingInfo(pingReply);
            }
            catch (PingException ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine();
            }

            return pingReply;
        }

        public PingReply? SendPing(params string[] addresses)
        {
            this.addresses = addresses;

            foreach (string address in addresses)
            {
                if (address != String.Empty)
                {
                    pingReply = SendPing(address); 
                }
            }

            return pingReply ?? null;
        }

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
                Console.WriteLine(ex.Message);
                return null;
            }

            return pingReply ?? null;
        }

        bool CheckHostNameOrAddress(string address)
        {
            MatchCollection matches = checkValidHostOrIP.Matches(address);

            return matches.Count > 0;
        }

        void ReadPingInfo(PingReply pingReply)
        {
            Console.WriteLine(pingReply.Status);
            if (pingReply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", pingReply.Address.ToString());
                Console.WriteLine("Address Family: {0}", pingReply.Address.AddressFamily.ToString());
                Console.WriteLine("RoundTrip time: {0}", pingReply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", pingReply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", pingReply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", pingReply.Buffer.Length);
                Console.WriteLine();
            }
        }
    }
}
