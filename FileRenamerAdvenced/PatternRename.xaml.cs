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
using FileRenamerAdvenced;
using System.IO;
using System.Threading;

namespace FileRenamerAdvanced
{ 
    public partial class PatternRename : Window
    {
        private MainWindow mainWindow;
        private int startIndex = 0;
        public PatternRename(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.txtStartIndex.Text = startIndex.ToString();
            label.Content = "The pattern will be execute on all the files\nSo use the correct pattern to rename your files." +
                           "\nPattern help you to set one name with variable content like INDEX , RANDOM and etc to all files." +
                           "\nINDEX : will generate a number in the filename." +
                           "\nRANDOM5 : will generate randomly some character in the filename, this statement get a number with \nitself as its length which in this example is 5" +
                           "RANDOM is more sensitive than other statement because it can get a length parameter"+
                           "\nSo RANDOM5 must not attached to any statements and must end with a space after its parameter."+
                           "\nDATE : will generate current Date in the filename like this one : 07162016" +
                           "\nTIME : will gerate current Time in the filename like this : 06-13-33" +
                           "\nFILENAME : will use the current filename in the new filename." +
                           "\nEXTENSION : will use the current file extension in the new filename" +
                           "\nAs you saw these statements are valid when we use them in the UPPERCASE word and in other way\ncan't make any change in the filename.";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            txtStartIndex_TextChanged(null, null);
            if (!string.IsNullOrEmpty(txtPattern.Text) && !string.IsNullOrWhiteSpace(txtPattern.Text))
            {
                string rName = txtPattern.Text;
                if (checkExtension.IsChecked == true && txtPattern.Text.IndexOf("EXTENSION") == -1)
                    rName = txtPattern.Text.Insert(this.txtPattern.Text.Length, "EXTENSION");
                mainWindow.lblRenamingProgress.Content = rName + " added as a pattern to files.";
                mainWindow.StartIndex = startIndex;
                mainWindow.PassedPattern = rName;
                Close();
                foreach (var item in mainWindow.files.ToList())
                {
                    Thread.Sleep(10);
                    item.RenameTo = $"{item.Path.Substring(0, item.Path.LastIndexOf(@"\"))}\\{rName}";
                }
            }
            else
            {
                MessageBox.Show("Something wrong happened!\nPlease check the input and try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtPattern_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSubmit_Click(null, null);
        }
        private void txtStartIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool checker = int.TryParse(txtStartIndex.Text, out startIndex);
            if (checker)
            {
                btnSubmit.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Please enter a number as Start Index!", "Start Index Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btnSubmit.IsEnabled = false;
            }
        }
    }
}
