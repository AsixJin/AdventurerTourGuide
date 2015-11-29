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
using System.Windows.Shapes;

namespace Adventurer_Tour_Guide
{
    /// <summary>
    /// Interaction logic for EntryEditor.xaml
    /// </summary>
    public partial class EntryEditor : Window
    {
        private bool NewEntry = true;

        public EntryEditor()
        {
            InitializeComponent();
        }

        public EntryEditor(Entry mEntry)
        {
            InitializeComponent();

            TexBox_Title.Text = mEntry.Title;
            TexBox_Description.Text = mEntry.Descritpion;
            TexBox_Details.Text = mEntry.Details;
            NewEntry = false;
        }

        private void But_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void But_Save_Click(object sender, RoutedEventArgs e)
        {
            if (NewEntry)
            {
                Entry mEntry = new Entry(TexBox_Title.Text, TexBox_Description.Text, TexBox_Details.Text);
                MainWindow.EntryList.Add(mEntry);
                this.Close();
            }
        }
    }
}
