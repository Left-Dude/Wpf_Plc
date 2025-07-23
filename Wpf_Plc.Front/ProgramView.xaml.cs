using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wpf_Plc
{
    public partial class ProgramView : UserControl
    {
        public ProgramView()
        {
            InitializeComponent();
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