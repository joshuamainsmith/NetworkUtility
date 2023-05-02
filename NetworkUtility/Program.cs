using NetworkUtility.Services;

PingService pingService = new PingService();

pingService.SendPingByFile("C:\\addresses.txt");

NetworkStatisticsService statistics = new NetworkStatisticsService();

statistics.GetTcpStatistics();

statistics.GetUdpStatistics();

WhoIsService whoIs = new WhoIsService();

whoIs.WhoIs("google.com");