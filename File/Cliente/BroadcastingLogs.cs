using System.Text.RegularExpressions;
using System.Diagnostics;
using Cliente.Models;
using Data.DbAccess;
using System.Linq;
using Client.Udp;
using System.IO;
using System;

namespace BroadcastringLogs
{
    public class BroadcastringLogs
    {
        static void Main(string[] args)
        {
            RabbitReceive.RabbitReceivedStart();

            RabbitSend.RabbitSendStart();
            Stopwatch ProcessParseLog = new Stopwatch();

            ProcessParseLog.Start();
            Regex LogIp = new Regex(@"(\ \d{3})\.(\d{3})\.(\d{2})\.(\d{2})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex LogDate = new Regex(@"(\d{10}\.\d{3})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex LogUrl = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            ProcessParseLog.Stop();

            Stopwatch ProcessTime = new Stopwatch();
            string ProcessDate = DateTime.Now.ToString();
            int ProcessLines = 0;
            ProcessTime.Start();

            var LinesTxt = File.ReadAllLines(@"C:\Exercicio\access.log");

            Stopwatch ProcessDB = new Stopwatch();
            foreach (var log in LinesTxt)
            {
                var LogIps = LogIp.Match(log);
                var LogUrls = LogUrl.Match(log);
                var LogDates = UnixTimeStampToDateTime(LogDate.Match(log).Value.Replace(".", string.Empty));
                var logs = ("IP: " + LogIps + "\nDate: " + LogDates + "\nUrl: " + LogUrls + "\n" + "\n");
                Console.WriteLine("Dados Enviados : \n\n\n\n" + logs);
                UdpClient.SocketUdp(logs);
                ProcessLines = LinesTxt.Count();


                using (var ctx = new ApplicationContext()){
                    ProcessDB.Start();
                    ctx.Dados.AddRange(
                        new Dados{
                        IpAddress = LogIps.ToString(),
                        UrlAddress = LogUrls.ToString(),
                        DateAddress= LogDates.ToString()
                   });
                   ctx.SaveChanges();
                }
            }

            ProcessDB.Stop();
            ProcessTime.Stop();

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-Y=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine("Linhas Processadas: " + ProcessLines);
            Console.WriteLine("Data: " + ProcessDate);
            Console.WriteLine("Tempo de Leitura: {0:hh\\:mm\\:ss}", ProcessTime.Elapsed);
            Console.WriteLine("Tempo de Leitura do Parse: {0}", ProcessParseLog.Elapsed.TotalSeconds);
            Console.WriteLine("Tempo Total de Gravação no Banco: {0:hh\\:mm\\:ss}", ProcessDB.Elapsed);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-Y=-=-=-=-=-=-=-=-=-=-=");

            Console.WriteLine("\n");
            RabbitSend.RabbitSendStart();
            Console.Read();
        }
        private static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long DateUnix = long.Parse(unixTimeStamp);
            dtDateTime = dtDateTime.AddMilliseconds(DateUnix);
            return dtDateTime;
        }
    }
}