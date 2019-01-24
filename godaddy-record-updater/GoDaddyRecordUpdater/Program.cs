using System;
using System.Threading.Tasks;
using Cpf.App;

namespace GoDaddyRecordUpdater
{
   class Program
   {
      static async Task<int> Main(string[] args)
      {
         var app = new Application("GoDaddy DNS record updater");

         LinePrimitive<string> shopperId = app.SharedOption<string>("-c|--customer-id", "customer id").Required();
         LinePrimitive<string> key = app.SharedOption<string>("-k|--key", "your key (go to https://developer.godaddy.com/keys to obtain one)").Required();
         LinePrimitive<string> secret = app.SharedOption<string>("-s|--secret", "your secret").Required();

         app.Command("a", cmd =>
         {
            cmd.Description = "updates A record";

            LinePrimitive<string> domainName = cmd.Argument<string>("domain", "name of the domain to update").Required();
            LinePrimitive<string> recordName = cmd.Argument<string>("record", "name of the record to update").Required();
            LinePrimitive<string> ip = cmd.Option<string>("-v|--value", "value of the new IP address, optional, when not specified will set to your current external IP address");

            cmd.OnExecute(async () =>
            {
               await new UpdateRecordCommand(shopperId, key, secret).UpdateAsync(domainName, recordName, ip);
            });
            
         });

         return app.Execute();
      }
   }
}
