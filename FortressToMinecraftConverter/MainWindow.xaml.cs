using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FortressToMinecraftConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MapReader mapReader;

        public MainWindow()
        {
            InitializeComponent();
            zList.Items.SortDescriptions.Add(new SortDescription("Level", ListSortDirection.Descending));
        }

        private void readMapButton_Click(object sender, RoutedEventArgs e)
        {
            var reader = new MapReader();
            BackgroundWorker readMapWorker = new BackgroundWorker();
            readMapWorker.WorkerReportsProgress = true;
            readMapWorker.DoWork += reader.ReadMap;
            readMapWorker.ProgressChanged += ReadMapWorker_ProgressChanged;
            readMapWorker.RunWorkerCompleted += ReadMapWorker_RunWorkerCompleted;
            readMapWorker.RunWorkerAsync();
        }

        private void ReadMapWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mapReader = e.Result as MapReader;
            this.DataContext = mapReader;
        }

        private void ReadMapWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            progressText.Text = e.UserState.ToString();
        }

        private void levelEnabled_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in zList.SelectedItems)
            {
                ((ZLevel)item).Enabled = ((CheckBox)sender).IsChecked.Value;
            }
        }
    }
}
