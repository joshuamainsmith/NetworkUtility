using FluentAssertions;
using NetworkUtility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Tests.HelperTests
{
    public class CheckHostNameTests
    {
        private readonly CheckHostName _checkHostName;

        public CheckHostNameTests()
        {
            _checkHostName = new CheckHostName();
        }

        [Theory]
        [InlineData("www.clickme.edu", true)]
        [InlineData("sub.sub.sub.domain.click", true)]
        [InlineData("0.0.0.0", true)]
        [InlineData("1.3.3.7", true)]
        [InlineData("1234", false)]
        [InlineData("https://www.clickme.edu", false)]
        [InlineData("sub.sub.sub.domain.click/some/path", false)]
        [InlineData("some.domain.com:1337", false)]
        [InlineData("!@#$%.42.com", false)]
        public void PingService_CheckHostNameOrAddress_ReturnsBool(string address, bool expected)
        {
            var actual = _checkHostName.CheckHostNameOrAddress(address);

            actual.Should().Be(expected);
        }
    }
}
