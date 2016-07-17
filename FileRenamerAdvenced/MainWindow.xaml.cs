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
using System.Windows.Shapes;
using FileRenamer;
using Microsoft.Win32;
using FileRenamerAdvanced;

namespace FileRenamerAdvenced
{
    public partial class MainWindow : Window
    {
        public Renamer renamer;
        internal List<FileModel> files;
        public string PassedPath;
        public string PassedPattern;
        public int StartIndex { get; internal set; }

        public MainWindow()
        {
            SingleRename single = new SingleRename(this);
            PatternRename pattern = new PatternRename(this);
            files = new List<FileModel>();
            renamer = new Renamer();
            InitializeComponent();
        }

        private void btnStartRenaming_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = files.Count();
                renamer.RenameFilesWithPattern(files, StartIndex);
                files.Clear();
                PassedPattern = string.Empty;
                PassedPath = string.Empty;
                SaveChanges();
                MessageBox.Show($"{count} Files has been completely renamed.", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog() { Multiselect = true };
            open.ShowDialog();
            if (open.FileNames != null)
            {
                foreach (string str in open.FileNames)
                {
                    FileModel model = new FileModel()
                    {
                        IsChecked = true,
                        Path = str
                    };
                    files.Add(model);
                    lblQuantityItems.Content = $"{str} has been added.";
                }
            }
            SaveChanges(UnChck: true);
        }

        private void SaveChanges(bool UnChck = true, bool Distinct = false)
        {
            if (UnChck)
                UnCheckeAll();
            if (Distinct)
                files.Distinct();
            if (files.Count == 0)
                btnRename.IsEnabled = true;
            lstFileItems.ItemsSource = files;
            lstFileItems.Items.Refresh();
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = files.Where(f => f.IsChecked == true);
            foreach (var item in isChecked)
            {
                SingleRename single = new SingleRename(this);
                single.PassedPath = item.Path;
                single.ShowDialog();
                if (!string.IsNullOrEmpty(this.PassedPath))
                {
                    item.RenameTo = this.PassedPath;
                }
            }
            SaveChanges(UnChck: true);
        }

        private void UnCheckeAll()
        {
            foreach (var item in files)
            {
                item.IsChecked = false;
            }
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRemoveFiles_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Are you sure to remove Checked item(s) ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (msg == MessageBoxResult.Yes)
            {
                SaveChanges(UnChck: false);
                foreach (var item in ((IEnumerable<FileModel>)lstFileItems.ItemsSource).ToList())
                {
                    if (item.IsChecked == true)
                    {
                        files.Remove(item);
                    }
                }
            }
            SaveChanges();
        }

        private void btnPatternSetting_Click(object sender, RoutedEventArgs e)
        {
            PatternRename pattern = new PatternRename(this);
            pattern.ShowDialog();
        }

        private void lstFileItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FileModel item = (FileModel)lstFileItems.SelectedItem;
                files.All(x => x.IsChecked = false);
                SaveChanges();
                item.IsChecked = true;
            }
            catch { }
        }
    }
}
