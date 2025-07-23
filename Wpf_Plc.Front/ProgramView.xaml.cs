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
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EthernetCheckBox.IsChecked == true)
            {
                var automation = new Wpf_Plc.Application.CxProgrammerAutomation();
                automation.LoadProgram();

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ETHERNET перед загрузкой программы.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}