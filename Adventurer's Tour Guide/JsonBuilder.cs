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
        static DriveService mService;
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "adventurers-tour-guide";
        static string fileID = "null";

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

            if (!fileID.Equals("null"))
            {
                UpdateJSONonDrive();
            }
            else
            {
                CreateJSONonDrive();
            }
            
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

        //Creates JSON file on GDrive if one doesn't exist or if 1st run
        public static void CreateJSONonDrive()
        {
      
            GetCred();
            // File's metadata.
            Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
            body.Title = "ATGEntries.json";
            body.Description = "Database JSON for Adventurer's Tour Guide";
            body.MimeType = "application/json";
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = "Root" } };

            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(Path + "\\ATGEntries.json");
            MemoryStream stream = new MemoryStream(byteArray);
            try
            {
                FilesResource.InsertMediaUpload request = mService.Files.Insert(body, stream, "application/json");
                request.Upload();

                Google.Apis.Drive.v2.Data.File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                Console.WriteLine("File ID: " + file.Id);
            }
             catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
              
            }
    }

        //Updates the JSON file on GDrive
        public static void UpdateJSONonDrive()
        {

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
                new FileStream(Path + "\\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/ATG_Client");

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

            mService = service;
        }
    }
}
