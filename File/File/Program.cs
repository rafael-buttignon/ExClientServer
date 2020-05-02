using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileExemples
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex LogIp = new Regex(@"(\ \d{3})\.(\d{3})\.(\d{2})\.(\d{2})", RegexOptions.IgnoreCase |  RegexOptions.Compiled);
            Regex LogDate = new Regex(@"(\d{10}.\d{3})", RegexOptions.IgnoreCase |  RegexOptions.Compiled);
            Regex LogUrl = new Regex(@"(http|ftp)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.IgnoreCase |  RegexOptions.Compiled);


            var logPath = @"C:\Exercicio\access.log";

            List<string> logs = File.ReadAllLines(logPath).ToList();


            foreach (string log in logs)
                {
                Match LogIps = LogIp.Match(log);
                Match LogDates = LogDate.Match(log);
                Match LogUrls= LogUrl.Match(log);



                Console.WriteLine(" IP:   " + LogIps + " DATE: " + LogDates + "  URL:  " + LogUrls);

                ////DateTime time = UnixTimeStampToDateTime(1586406735559);
                ////Console.WriteLine(time);
            }

        }

    private static DateTime UnixTimeStampToDateTime(long LogDates)
        {


            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(LogDates);
            return dtDateTime;
        }
    }
}
