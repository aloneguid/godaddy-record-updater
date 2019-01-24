using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace GoDaddyRecordUpdater
{
   static class ClientFactory
   {
      public static IGoDaddyClient CreateGoDaddyClient(string shopperId, string key, string secret)
      {
         var http = new HttpClient(new GoDaddyAuthenticationHandler(shopperId, key, secret))
         {
            BaseAddress = new Uri("https://api.godaddy.com")
         };

         return RestService.For<IGoDaddyClient>(http);
      }

      private class GoDaddyAuthenticationHandler : HttpClientHandler
      {
         private readonly string _shopperId;
         private readonly string _key;
         private readonly string _secret;

         public GoDaddyAuthenticationHandler(string shopperId, string key, string secret)
         {
            _shopperId = shopperId;
            _key = key;
            _secret = secret;
         }

         protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
         {
            //add auth info
            request.Headers.Add("X-Shopper-Id", _shopperId);
            request.Headers.Add("Authorization", $"sso-key {_key}:{_secret}");

            return await base.SendAsync(request, cancellationToken);
         }
      }
   }
}
