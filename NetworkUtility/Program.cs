using NetworkUtility.Services;

PingService pingService = new PingService();

pingService.SendPing("www.google.com");

pingService.SendPing("https://www.google.com");

pingService.SendPing("1234");

pingService.SendPing("google.com", "bing.com");

pingService.SendPingByFile("C:\\addresses.txt");