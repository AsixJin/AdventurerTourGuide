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
        #region Members (List)
        public static List<string> sEntryList = new List<string>(); //String list for ComboBox
        public static List<Entry> EntryList = new List<Entry>(); //Entry list
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(JsonBuilder.Path))
            {
                Directory.CreateDirectory(JsonBuilder.Path);
            }
            if (System.IO.File.Exists(JsonBuilder.JSONPath))
            {
                EntryList = JsonBuilder.LoadJSONfromFile();
                UpdateList();
            }

            /* The JSON file will need to be loaded from the Drive
            and saved locally. If no internet is avaiable or something
            is wrong we will use the local copy. */

            if (sEntryList.Count != 0)
            {
                ComBox_EntryList.SelectedIndex = 0;
            }
            
        }

        #region Button Events
        private void But_Add_Click(object sender, RoutedEventArgs e)
        {
            EntryEditor main = new EntryEditor(this);
            main.ShowDialog();
        }

        private void But_Edit_Click(object sender, RoutedEventArgs e)
        {
            int selected = 0;
            for (int i = 0; i <= MainWindow.EntryList.Count - 1; i++)
            {
                if (MainWindow.EntryList[i].Title.Equals(ComBox_EntryList.SelectedItem.ToString()))
                {
                    selected = i;
                    break;
                }
            }
            EntryEditor main = new EntryEditor(this, selected);
            main.ShowDialog();
        }

        private void But_Delete_Click(object sender, RoutedEventArgs e)
        {
            int selected = 0;
            for (int i = 0; i <= MainWindow.EntryList.Count - 1; i++)
            {
                if (MainWindow.EntryList[i].Title.Equals(ComBox_EntryList.SelectedItem.ToString()))
                {
                    selected = i;
                    break;
                }
            }
            MainWindow.EntryList.RemoveAt(selected);
            UpdateList();
        }
        #endregion

        //Takes the selected entry and displays it
        public void RefreshActiveEntry()
        {
            foreach (Entry ee in EntryList)
            {
                if (ee.Title.Equals(ComBox_EntryList.SelectedItem.ToString()))
                {
                    texBlock_EntryTitle.Text = ee.Title;
                    TexBlox_Description.Text = ee.Description;
                    texBox_Details.Text = ee.Details;
                    break;
                }
            }
        }

        //Adds all entries for the ComboBox list
        public void UpdateList()
        {
            sEntryList = new List<string>();
            foreach(Entry e in EntryList)
            {
                sEntryList.Add(e.Title);
            }
            sEntryList.Sort();
            ComBox_EntryList.ItemsSource = sEntryList;
            ComBox_EntryList.SelectedIndex = 0;
            RefreshActiveEntry();
        }
        
        //Event for when new entry is selected in ComboBox (Refreshes display)
        private void ComBox_EntryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RefreshActiveEntry();
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }      
        }
        
        //Event for when application is closed (Saves to file and drive)
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JsonBuilder.CreateJSON();
            foreach (Entry ee in EntryList)
            {
                JsonBuilder.AddEntry(ee);
            }
            JsonBuilder.SaveJSONtoFile();
        }

    }

}
    
            

