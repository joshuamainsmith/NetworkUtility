using NetworkUtility.Services;

PingService pingService = new PingService();

pingService.SendPingByFile("C:\\addresses.txt");

NetworkStatisticsService statistics = new NetworkStatisticsService();

statistics.GetTcpStatistics();

statistics.GetUdpStatistics();

WhoIsService whoIs = new WhoIsService();

whoIs.WhoIs("8.8.8.8");

whoIs.GetWhoIsInfo(true, true, true);