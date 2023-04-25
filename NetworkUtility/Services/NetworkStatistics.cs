using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class NetworkStatistics
    {
        string protocol { get; set; }
        NetworkInterfaceComponent version { get; set; }
        TcpStatistics? tcpstat { get; set; }
        UdpStatistics? udpStat { get; set; }
        IPGlobalProperties properties { get; set; }

        public NetworkStatistics() 
        {
            properties = IPGlobalProperties.GetIPGlobalProperties();
            tcpstat = null;
            udpStat = null;
        }

        public void GetTcpStatistics(bool IpV6 = false)
        {
            tcpstat = SetTcpStatistics(IpV6);

            if (tcpstat != null)
            {
                Console.WriteLine("  Minimum Transmission Timeout............. : {0}",
                    tcpstat.MinimumTransmissionTimeout);
                Console.WriteLine("  Maximum Transmission Timeout............. : {0}",
                    tcpstat.MaximumTransmissionTimeout);

                Console.WriteLine("  Connection Data:");
                Console.WriteLine("      Current  ............................ : {0}",
                tcpstat.CurrentConnections);
                Console.WriteLine("      Cumulative .......................... : {0}",
                    tcpstat.CumulativeConnections);
                Console.WriteLine("      Initiated ........................... : {0}",
                    tcpstat.ConnectionsInitiated);
                Console.WriteLine("      Accepted ............................ : {0}",
                    tcpstat.ConnectionsAccepted);
                Console.WriteLine("      Failed Attempts ..................... : {0}",
                    tcpstat.FailedConnectionAttempts);
                Console.WriteLine("      Reset ............................... : {0}",
                    tcpstat.ResetConnections);

                Console.WriteLine("");
                Console.WriteLine("  Segment Data:");
                Console.WriteLine("      Received  ........................... : {0}",
                    tcpstat.SegmentsReceived);
                Console.WriteLine("      Sent ................................ : {0}",
                    tcpstat.SegmentsSent);
                Console.WriteLine("      Retransmitted ....................... : {0}",
                    tcpstat.SegmentsResent);

                Console.WriteLine("");
            }
        }

        TcpStatistics SetTcpStatistics(bool isIpv6)
        {
            switch (isIpv6)
            {
                case false:
                    tcpstat = properties.GetTcpIPv4Statistics();
                    Console.WriteLine("TCP/IPv4 Statistics:");
                    break;
                case true:
                    tcpstat = properties.GetTcpIPv6Statistics();
                    Console.WriteLine("TCP/IPv6 Statistics:");
                    break;
                default:
                    throw new ArgumentException("version");
                    //    break;
            }

            return tcpstat;
        }
    }    
}
