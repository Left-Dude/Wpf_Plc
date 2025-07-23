using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc
{
    public partial class PlcInfoCard : UserControl
    {
        public static readonly DependencyProperty PlcModelProperty =
            DependencyProperty.Register("PlcModel", typeof(PLCModel), typeof(PlcInfoCard), 
                new PropertyMetadata(null));

        public PLCModel PlcModel
        {
            get => (PLCModel)GetValue(PlcModelProperty);
            set => SetValue(PlcModelProperty, value);
        }

        public PlcInfoCard()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Uri?.AbsoluteUri))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
        }

        private void LoadProgramButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlcModel != null)
            {
                MessageBox.Show($"Загрузка программы для контроллера: {PlcModel.Manufacturer} {PlcModel.Model}", 
                    "Загрузка программы", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                
                var automation = new Wpf_Plc.Application.CxProgrammerAutomation();
                automation.LoadProgram();
            }
        }
    }
}