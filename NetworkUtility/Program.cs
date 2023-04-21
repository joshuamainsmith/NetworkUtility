using NetworkUtility.Services;

PingService pingService = new PingService();

pingService.SendPing("");

pingService.SendPing("", "");