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
        // https://regexper.com/#%28www%5C.%29%3F%5Cw%2B%28%5C.%5Cw%2B%29%2B
        static Regex checkValidHostOrIP = new Regex(@"(www\.)?\w+(\.\w+)+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool CheckHostNameOrAddress(string address)
        {
            MatchCollection matches = checkValidHostOrIP.Matches(address);

            return matches.Count > 0;
        }
    }
}
