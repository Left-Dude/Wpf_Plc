using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Wpf_Plc
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPLC_Click(object sender, RoutedEventArgs e)
        {
            // Показываем информацию о PLC
            MainContent.Content = new PlcDetailsView();
        }

        private void BtnPROG_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProgramView();
        }

        private void BtnEXIT_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие приложения
            Application.Current.Shutdown();
        }
    }
}