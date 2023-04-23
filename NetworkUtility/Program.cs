using NetworkUtility.Services;

PingService pingService = new PingService();

pingService.SendPing("google.com");

pingService.SendPing("google.com", "bing.com");

pingService.SendPingByFile("C:\\addresses.txt");