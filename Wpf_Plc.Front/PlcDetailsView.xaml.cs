using System.Windows;
using System.Windows.Controls;

namespace Wpf_Plc
{
    public partial class PlcDetailsView : UserControl
    {
        public PlcDetailsView()
        {
            InitializeComponent();
        }

        //вот эта залупа криво работает:
        //private void BtnPROG_Click(object sender, RoutedEventArgs e)
        //{
          //  MainContent.Content = new ProgramView();
        //}

        private void BtnEXIT_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие приложения
            Application.Current.Shutdown();
        }
    }
}