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

namespace FileRenamerAdvanced
{
    /// <summary>
    /// Interaction logic for SingleRename.xaml
    /// </summary>
    public partial class SingleRename : Window
    {
        private MainWindow mainWindow;
        public string PassedPath;
        public SingleRename(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string newPath = this.PassedPath.Substring(0, PassedPath.LastIndexOf(@"\")) + $@"\{txtPath.Text}";
            mainWindow.PassedPath = newPath;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label.Content = $"New Name for {this.PassedPath}";
        }

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSubmit_Click(null, null);
        }
    }
}
