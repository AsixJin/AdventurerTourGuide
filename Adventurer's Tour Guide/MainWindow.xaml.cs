using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;

using System.Threading;


namespace Adventurer_Tour_Guide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Adventurer Tour Guide";
        public static List<Entry> EntryList = new List<Entry>();

        public MainWindow()
        {
            InitializeComponent();
            GetCred();

            /* The JSON file will need to be loaded from the Drive
            and saved locally. If no internet is avaiable or something
            is wrong we will use the local copy. */
             
            if (EntryList.Count != 0)
            {
                ComBox_EntryList.ItemsSource = EntryList;
            }
            
        }

        public void GetCred()
        {
            //Get Credential
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart");

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

        private void But_Add_Click(object sender, RoutedEventArgs e)
        {
            EntryEditor main = new EntryEditor(true);
            main.ShowDialog();
        }

        private void But_Edit_Click(object sender, RoutedEventArgs e)
        {
            EntryEditor main = new EntryEditor(false);
            main.ShowDialog();
        }
    }

    public class Entry
    {
        public string Title;
        public string Descritpion;
        public string Details;

        public Entry(string mTitle, string mDescription, string mDetails)
        {
            Title = mTitle;
            Descritpion = mDescription;
            Details = mDetails;
        }
    }
}
    
            

