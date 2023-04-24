using FluentAssertions;
using NetworkUtility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Tests.PingTests
{
    public class PingServiceTests
    {
        // ClassName_MethodName_ReturnType()
        [Fact]
        public void GivenSingleHost_SendPing_ReturnsPingReply()
        {
            // Arrange - variables, classes, mocks
            var pingService = new PingService();

            // Act - Call action methods
            var result = pingService.SendPing("google.com");

            // Assert - compare expected return vals with actual
            pingService.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Address.ToString().Should().NotBeNullOrWhiteSpace();
            result.Options.Ttl.Should().BeGreaterThanOrEqualTo(0);
            result.RoundtripTime.Should().BeGreaterThan(0);
            result.Options.DontFragment.Should().Be(false);
            result.Should().BeOfType<PingReply>().Which.Buffer.Length.Should().BeLessThanOrEqualTo(65507);   // https://stackoverflow.com/q/9449837
        }

        [Fact]
        public void GivenMultipleAddresses_SendPing_ReturnsPingReply()
        {
            var pingService = new PingService();

            var result = pingService.SendPing("google.com", "bing.com", "127.0.0.1", "142.251.211.238");

            pingService.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Address.ToString().Should().NotBeNullOrWhiteSpace();
            result.Options.Ttl.Should().BeGreaterThanOrEqualTo(0);
            result.RoundtripTime.Should().BeGreaterThan(0);
            result.Options.DontFragment.Should().Be(false);
            result.Should().BeOfType<PingReply>().Which.Buffer.Length.Should().BeLessThanOrEqualTo(65507);   // https://stackoverflow.com/q/9449837
        }
    }
}
