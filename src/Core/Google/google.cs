using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SheetsStart
{
    class Program
    {
        static string[] Scopes = {SheetsService.Scope.Spreadsheets};
        static string ApplicationName = "Discord Gear Bot";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json",FileMode.Open,FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to " + credPath);
                    
                    var service = new SheetsService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });
                }
        }
    }
}