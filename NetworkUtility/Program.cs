using NetworkUtility.Services;

PingService pingService = new PingService();
pingService.ReadPingInfo(
    pingService.SendPing("google.com")
    );