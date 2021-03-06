using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BatchAPI_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
        //         //.ConfigureAppConfiguration((ctx, builder) =>
        //         //{
        //         //    var keyVaultEndpoint = GetKeyVaultEndpoint();
        //         //    if (!string.IsNullOrEmpty(keyVaultEndpoint))
        //         //    {
        //         //        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        //         //        var keyVaultClient = new KeyVaultClient(
        //         //           new KeyVaultClient.AuthenticationCallback(
        //         //              azureServiceTokenProvider.KeyVaultTokenCallback));
        //         //        builder.AddAzureKeyVault(
        //         //           keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        //         //    }
        //         //}

        //         .ConfigureAppConfiguration((context, config) =>
        //         {

        //             var root = config.Build();
        //             config.AddAzureKeyVault($"https://{root["KeyVault:Vault"]}.vault.azure.net/", root["KeyVault:ClientId"], root["KeyVault:ClientSecret"]);
        //         })
        //.ConfigureWebHostDefaults(webBuilder =>
        //{
        //    webBuilder.UseStartup<Startup>();
        //});

        //   //  private static string GetKeyVaultEndpoint() => "https://<BatchWebAPIDemo>.vault.azure.net/";

        .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                // .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
            });
    }
}
