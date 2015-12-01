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
        private int Index = 0;
        MainWindow main;

        public EntryEditor(MainWindow mMain)
        {
            InitializeComponent();

            main = mMain;
        }

        public EntryEditor(MainWindow mMain, int mIndex)
        {
            InitializeComponent();

            main = mMain;
            Index = mIndex;

            TexBox_Title.Text = MainWindow.EntryList[Index].Title;
            TexBox_Description.Text = MainWindow.EntryList[Index].Description;
            TexBox_Details.Text = MainWindow.EntryList[Index].Details;
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
            else
            {
                MainWindow.EntryList[Index].Title = TexBox_Title.Text;
                MainWindow.EntryList[Index].Description = TexBox_Description.Text;
                MainWindow.EntryList[Index].Details = TexBox_Details.Text;
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            main.UpdateList();
        }

        private void textBlock2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                TexBox_Details.Text += "\n";
            }
        }
    }
}
