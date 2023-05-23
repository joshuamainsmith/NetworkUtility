using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetworkUtility.Helpers
{
    public class CheckHostName
    {
        // https://regexper.com/#%5E%28www%5C.%29%3F%28%5Cw%28%5C-%3F%5Cw%29%3F%29%2B%28%28%5C.%5Cw%2B%29%3F%29%2B%24
        static Regex checkValidHostOrIP = new Regex(@"^(www\.)?(\w(\-?\w)?)+((\.\w+)?)+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool CheckHostNameOrAddress(string address)
        {
            MatchCollection matches = checkValidHostOrIP.Matches(address);

            return matches.Count > 0;
        }
    }
}
