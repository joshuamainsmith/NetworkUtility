using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Services
{
    public class NetworkStatisticsService
    {
        string protocol { get; set; }
        NetworkInterfaceComponent version { get; set; }
        TcpStatistics? tcpstat { get; set; }
        UdpStatistics? udpStat { get; set; }
        IPGlobalProperties properties { get; set; }

        public NetworkStatisticsService()
        {
            properties = IPGlobalProperties.GetIPGlobalProperties();
            tcpstat = null;
            udpStat = null;
        }

        public void GetTcpStatistics(bool IpV6 = false)
        {
            var tcp = SetTcpStatistics(IpV6);

            if (tcp != null)
            {
                Console.WriteLine("  Minimum Transmission Timeout............. : {0}",
                    tcp.MinimumTransmissionTimeout);
                Console.WriteLine("  Maximum Transmission Timeout............. : {0}",
                    tcp.MaximumTransmissionTimeout);
                Console.WriteLine("  Maximum Connections...................... : {0}",
                    tcp.MaximumConnections);

                Console.WriteLine("  Connection Data:");
                Console.WriteLine("      Current  ............................ : {0}",
                tcp.CurrentConnections);
                Console.WriteLine("      Cumulative .......................... : {0}",
                    tcp.CumulativeConnections);
                Console.WriteLine("      Initiated ........................... : {0}",
                    tcp.ConnectionsInitiated);
                Console.WriteLine("      Accepted ............................ : {0}",
                    tcp.ConnectionsAccepted);
                Console.WriteLine("      Failed Attempts ..................... : {0}",
                    tcp.FailedConnectionAttempts);
                Console.WriteLine("      Reset ............................... : {0}",
                    tcp.ResetConnections);
                Console.WriteLine("      Resets Sent.......................... : {0}",
                    tcp.ResetsSent);

                Console.WriteLine("      Errors Received...................... : {0}",
                    tcp.ErrorsReceived);

                Console.WriteLine("");
                Console.WriteLine("  Segment Data:");
                Console.WriteLine("      Received  ........................... : {0}",
                    tcp.SegmentsReceived);
                Console.WriteLine("      Sent ................................ : {0}",
                    tcp.SegmentsSent);
                Console.WriteLine("      Retransmitted ....................... : {0}",
                    tcp.SegmentsResent);

                Console.WriteLine("");
            }
        }

        public void GetUdpStatistics(bool IpV6 = false)
        {
            var udp = SetUdpStatistics(IpV6);

            if (udp != null)
            {
                Console.WriteLine("  Datagrams Received ...................... : {0}",
                    udp.DatagramsReceived);
                Console.WriteLine("  Datagrams Sent .......................... : {0}",
                    udp.DatagramsSent);
                Console.WriteLine("  Incoming Datagrams Discarded ............ : {0}",
                    udp.IncomingDatagramsDiscarded);
                Console.WriteLine("  Incoming Datagrams With Errors .......... : {0}",
                    udp.IncomingDatagramsWithErrors);
                Console.WriteLine("  UDP Listeners ........................... : {0}",
                    udp.UdpListeners);
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

        UdpStatistics SetUdpStatistics(bool isIpv6)
        {
            switch (isIpv6)
            {
                case false:
                    udpStat = properties.GetUdpIPv4Statistics();
                    Console.WriteLine("UDP/IPv4 Statistics:");
                    break;
                case true:
                    udpStat = properties.GetUdpIPv6Statistics();
                    Console.WriteLine("UDP/IPv6 Statistics:");
                    break;
                default:
                    throw new ArgumentException("version");
                    //    break;
            }

            return udpStat;
        }
    }
}
