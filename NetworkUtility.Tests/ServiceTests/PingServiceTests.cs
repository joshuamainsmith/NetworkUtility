using FluentAssertions;
using NetworkUtility.Helpers;
using NetworkUtility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace NetworkUtility.Tests.PingTests
{
    public class PingServiceTests
    {
        private readonly PingService _pingService;

        public PingServiceTests()
        {
            // SUT
            _pingService = new PingService();
        }

        // ClassName_MethodName_ReturnType()
        [Theory]
        [InlineData("google.com")]
        [InlineData("127.0.0.1")]
        [InlineData("google.com", "bing.com", "127.0.0.1", "142.251.211.238")]
        [InlineData("learn.microsoft.com", "about.google.com", "www.google.com", "console.cloud.google.com")]
        public void PingService_SendPing_ReturnsPingReply(params string[] addresses)
        {
            // Arrange - variables, classes, mocks

            // Act - Call action methods
            var result = _pingService.SendPing(addresses);

            // Assert - compare expected return vals with actual
            _pingService.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Address.ToString().Should().NotBeNullOrWhiteSpace();
            result.Options.Ttl.Should().BeGreaterThanOrEqualTo(0);
            result.RoundtripTime.Should().BeGreaterThanOrEqualTo(0);
            result.Options.DontFragment.Should().Be(false);
            result.Should().BeOfType<PingReply>().Which.Buffer.Length.Should().BeLessThanOrEqualTo(65507);   // https://stackoverflow.com/q/9449837
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("1234.doesnotexist")]
        [InlineData("!@#$%^&.url.com")]
        [InlineData("console.cloud.google.com/getting-started")]
        [InlineData("https://www.google.com", "!@#$%^&.url.com", "console.cloud.google.com/getting-started")]
        public void PingService_SendPing_ReturnsNull(params string[] addresses)
        {
            var result = _pingService.SendPing(addresses);

            _pingService.Should().NotBeNull();
            result.Should().BeNull();
        }        
    }
}
