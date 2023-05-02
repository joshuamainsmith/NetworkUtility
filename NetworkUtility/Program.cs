using NetworkUtility.ConnectionInfo;
using NetworkUtility.Services;

PingService pingService = new PingService();

pingService.SendPingByFile("C:\\addresses.txt");

NetworkStatistics statistics = new NetworkStatistics();

statistics.GetTcpStatistics();

statistics.GetUdpStatistics();

WhoIsService whoIs = new WhoIsService();

whoIs.WhoIs("google.com");