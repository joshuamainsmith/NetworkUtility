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
                // Create the tree
                var root = new Tree($"[blue]{protocol}[/]");

                // Add some nodes
                var transmissions = root.AddNode("[yellow]Transmission Data[/]");
                var table = transmissions.AddNode(new Table()
                    .RoundedBorder()
                    .AddColumn("[aquamarine1_1]Minimum Transmission Timeout[/]")
                    .AddColumn("[aquamarine1_1]Maximum Transmission Timeout[/]")
                    .AddColumn("[aquamarine1_1]Maximum Connections[/]")
                    .AddRow($"{tcp.MinimumTransmissionTimeout}", $"{tcp.MaximumTransmissionTimeout}", $"{tcp.MaximumConnections}"));

                var connections = root.AddNode("[yellow]Connection Data[/]");
                connections.AddNode(new Table()
                    .RoundedBorder()
                    .AddColumn("[aquamarine1_1]Current[/]")
                    .AddColumn("[aquamarine1_1]Cumulative[/]")
                    .AddColumn("[aquamarine1_1]Initiated[/]")
                    .AddColumn("[aquamarine1_1]Accepted[/]")
                    .AddColumn("[aquamarine1_1]Failed Attempts[/]")
                    .AddColumn("[aquamarine1_1]Reset[/]")
                    .AddColumn("[aquamarine1_1]Resets Sent[/]")
                    .AddColumn("[aquamarine1_1]Errors Received[/]")
                    .AddRow($"{tcp.CurrentConnections}", 
                    $"{tcp.CumulativeConnections}", 
                    $"{tcp.ConnectionsInitiated}",
                    $"{tcp.ConnectionsAccepted}",
                    $"{tcp.FailedConnectionAttempts}",
                    $"{tcp.ResetConnections}",
                    $"{tcp.ResetsSent}",
                    $"{tcp.ErrorsReceived}"
                    ));

                var segments = root.AddNode("[yellow]Segment Data[/]");
                segments.AddNode(new Table()
                    .RoundedBorder()
                    .AddColumn("[aquamarine1_1]Received[/]")
                    .AddColumn("[aquamarine1_1]Sent[/]")
                    .AddColumn("[aquamarine1_1]Retransmitted[/]")
                    .AddRow($"{tcp.SegmentsReceived}",
                    $"{tcp.SegmentsSent}",
                    $"{tcp.SegmentsResent}"
                    ));

                // Render the tree
                AnsiConsole.Write(root);

                Console.WriteLine("");
            }
        }

        public void GetUdpStatistics(bool IpV6 = false)
        {
            var udp = SetUdpStatistics(IpV6);

            if (udp != null)
            {
                // Create the tree
                var root = new Tree($"[blue]{protocol}[/]");

                // Add some nodes
                var transmissions = root.AddNode("[yellow]Transmission Data[/]");
                var table = transmissions.AddNode(new Table()
                    .RoundedBorder()
                    .AddColumn("[aquamarine1_1]Datagrams Received[/]")
                    .AddColumn("[aquamarine1_1]Datagrams Sent[/]")
                    .AddColumn("[aquamarine1_1]Incoming Datagrams Discarded[/]")
                    .AddColumn("[aquamarine1_1]Incoming Datagrams With Errors[/]")
                    .AddColumn("[aquamarine1_1]UDP Listeners[/]")
                    .AddRow($"{udp.DatagramsReceived}", 
                    $"{udp.DatagramsSent}", 
                    $"{udp.IncomingDatagramsDiscarded}",
                    $"{udp.IncomingDatagramsWithErrors}",
                    $"{udp.UdpListeners}"
                    ));

                // Render the tree
                AnsiConsole.Write(root);

                Console.WriteLine("");
            }
        }

        TcpStatistics SetTcpStatistics(bool isIpv6)
        {
            switch (isIpv6)
            {
                case false:
                    tcpstat = properties.GetTcpIPv4Statistics();
                    protocol = "TCP/IPv4 Statistics";
                    break;
                case true:
                    tcpstat = properties.GetTcpIPv6Statistics();
                    protocol = "TCP/IPv6 Statistics";
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
                    protocol = "UDP/IPv4 Statistics";
                    break;
                case true:
                    udpStat = properties.GetUdpIPv6Statistics();
                    protocol = "UDP/IPv6 Statistics";
                    break;
                default:
                    throw new ArgumentException("version");
                    //    break;
            }

            return udpStat;
        }
    }
}
