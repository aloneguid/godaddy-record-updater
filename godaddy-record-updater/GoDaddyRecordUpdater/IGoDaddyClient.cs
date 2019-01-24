using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace GoDaddyRecordUpdater
{
   public interface IGoDaddyClient
   {
      [Get("/v1/domains/{domainName}/records/{recordType}/{recordName}")]
      Task<GoDaddyDnsRecord[]> GetDomainRecordDetailsAsync(string domainName, string recordType, string recordName);

      [Put("/v1/domains/{domainName}/records/{recordType}/{recordName}")]
      [Headers("Content-Type: application/json")]
      Task SetDomainRecordDetailsAsync(string domainName, string recordType, string recordName, [Body] GoDaddyDnsRecord[] records);
   }

   public class GoDaddyDnsRecord
   {
      /// <summary>
      /// Usually the value of IP address (for A record)
      /// </summary>
      [JsonProperty("data")]
      public string Data { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("ttl")]
      public int Ttl { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }
   }
}
