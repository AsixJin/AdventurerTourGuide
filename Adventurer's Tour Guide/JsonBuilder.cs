using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace Adventurer_Tour_Guide
{
    class JsonBuilder
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Adventurer Tour Guide";

        public static JArray root = new JArray();
        public static readonly string Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\AdventurerTourGuide";
        public static readonly string JSONPath = Path + "\\ATGEntries.json";

        //Create New JSON
        public static void CreateJSON()
        {
            root = new JArray();
        }

        //Add entry with a description and add it to the root
        public static void AddEntry(string title, string description, string details)
        {
            JObject entry = new JObject();
            JProperty jTitle = new JProperty("Title", title);
            JProperty jDescript = new JProperty("Description", description);
            JProperty jDetails = new JProperty("Details", details);

            entry.Add(jTitle);
            entry.Add(jDescript);
            entry.Add(jDetails);

            root.Add(entry);
        }

        //Add entry without description and add it to the root
        public static void AddEntry(string title, string details)
        {
            JObject entry = new JObject();
            JProperty jTitle = new JProperty("Title", title);
            JProperty jDescript = new JProperty("Description", "N/A");
            JProperty jDetails = new JProperty("Details", details);

            entry.Add(jTitle);
            entry.Add(jDescript);
            entry.Add(jDetails);

            root.Add(entry);
        }

        public static void AddEntry(Entry mEntry)
        {
            JObject entry = new JObject();
            JProperty jTitle = new JProperty("Title", mEntry.Title);
            JProperty jDescript = new JProperty("Description", mEntry.Description);
            JProperty jDetails = new JProperty("Details", mEntry.Details);

            entry.Add(jTitle);
            entry.Add(jDescript);
            entry.Add(jDetails);

            root.Add(entry);
        }

        //Save JSON to file
        public static void SaveJSONtoFile()
        {
            string json = JsonConvert.SerializeObject(root.ToArray());

            //write string to file
            System.IO.File.WriteAllText(JSONPath, json);
        }

        //Load JSON from file
        public static List<Entry> LoadJSONfromFile()
        {
            List<Entry> entries = new List<Entry>();
            using (StreamReader r = new StreamReader(JSONPath))
            {
                string json = r.ReadToEnd();
                entries = JsonConvert.DeserializeObject<List<Entry>>(json);
            }
            return entries;
        }

        //Save JSON to GDrive
        public static void SaveJSONtoDrive()
        {
            GetCred();
        }

        //Load JSON from GDrive
        public static void LoadJSONfromDrive()
        {
            GetCred();
        }

        //GDrive Cred
        public static void GetCred()
        {
            //Get Credential
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/drive-dotnet-quickstart");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
    }
}
