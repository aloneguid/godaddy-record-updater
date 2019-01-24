using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cpf.Widgets;
using static Cpf.PoshConsole;

namespace GoDaddyRecordUpdater
{
   class UpdateRecordCommand
   {
      private readonly IGoDaddyClient _goDaddyClient;

      public UpdateRecordCommand(string shopperId, string key, string secret)
      {
         _goDaddyClient = ClientFactory.CreateGoDaddyClient(shopperId, key, secret);
      }

      public async Task UpdateAsync(string domainName, string recordName, string newValue)
      {
         if(string.IsNullOrEmpty(newValue))
         {
            using (var progress = new ProgressMessage("detecting your external IP address..."))
            {
               newValue = await GetMyPublicIp();

               progress.Message += " " + newValue + ".";
            }
         }

         GoDaddyDnsRecord record;

         using (var progress = new ProgressMessage("fetching details..."))
         {
            record = (await _goDaddyClient.GetDomainRecordDetailsAsync(domainName, "A", recordName)).FirstOrDefault();

            PoshWriteLine($"IP: {{{record.Data}}}, TTL: {{{record.Ttl}}}", ConsoleColor.Green, ConsoleColor.Magenta);
         }

         using (var progress = new ProgressMessage($"updating IP address to {newValue}..."))
         {
            record.Data = newValue;

            await _goDaddyClient.SetDomainRecordDetailsAsync(domainName, "A", recordName, new[] { record });
         }
      }

      private async Task<string> GetMyPublicIp()
      {
         string ipHtml = await new WebClient().DownloadStringTaskAsync(new Uri("http://checkip.dyndns.org"));

         var rgx = new Regex(@"\d+\.\d+.\d+.\d+");
         Match mtch = rgx.Match(ipHtml);
         return mtch.Value;
      }
   }
}
