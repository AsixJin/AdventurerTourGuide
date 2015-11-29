using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Adventurer_Tour_Guide
{
    class JsonBuilder
    {
        public static JArray root = new JArray();

        //Create the JSON file
        public static void CreateJSON()
        {
          

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

        //Save JSON to file
        public static void SaveJSONtoFile()
        {
            string json = JsonConvert.SerializeObject(root.ToArray());

            //write string to file
            string Path = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
            System.IO.File.WriteAllText(Path + "\\AdventurerTourGuide\\ATGEntries.json", json);
        }

        //Load JSON from file
        public static void LoadJSONfromFile(List<Entry> entries)
        {
            string Path = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
            using (StreamReader r = new StreamReader(Path + "\\AdventurerTourGuide\\ATGEntries.json"))
            {
                string json = r.ReadToEnd();
                entries = JsonConvert.DeserializeObject<List<Entry>>(json);
            }
        }

        //Save JSON to GDrive
        public static void SaveJSONtoDrive()
        {

        }

        //Load JSON from GDrive
        public static void LoadJSONfromDrive()
        {

        }

    }
}
