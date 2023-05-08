using NetworkUtility.Services;

PingService pingService = new PingService();

//pingService.SendPingByFile("C:\\addresses.txt");

//pingService.SendPing("google.com");

pingService.SendPingByRange("10.32.64.250", "10.32.64.254");

/*NetworkStatisticsService statistics = new NetworkStatisticsService();

statistics.GetTcpStatistics();

statistics.GetUdpStatistics();*/

/*WhoIsService whoIs = new WhoIsService();

whoIs.WhoIs("8.8.8.8");

whoIs.GetWhoIsInfo(true, true, true);*/