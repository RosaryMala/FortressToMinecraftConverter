using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            mapReader = new MapReader();
            this.DataContext = mapReader;
            zList.Items.SortDescriptions.Add(new SortDescription("Level", ListSortDirection.Descending));
        }

        private void readMapButton_Click(object sender, RoutedEventArgs e)
        {
            mapReader.ReadMap();
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
