using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_Plc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPROG_Click(object sender, RoutedEventArgs e)
        {
            if (!Program.IsVisible)
            {
                PLC.Visibility = Visibility.Collapsed;
                Program.Visibility = Visibility.Visible;
            }
        }

        private void BtnPLC_Click(object sender, RoutedEventArgs e)
        {
            if (!PLC.IsVisible)
            {
                Program.Visibility = Visibility.Collapsed;
                PLC.Visibility = Visibility.Visible;
            }
        }

        private void BtnEXIT_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}