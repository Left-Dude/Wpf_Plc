using System.Windows;
using System.Windows.Controls;

namespace Wpf_Plc
{
    public partial class PlcInfoCard : UserControl
    {
        public PlcInfoCard()
        {
            InitializeComponent();
        }

        // Свойство для заголовка
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PlcInfoCard), new PropertyMetadata("PLC", OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PlcInfoCard;
            if (control != null && control.TitleTextBlock != null)
            {
                control.TitleTextBlock.Text = e.NewValue as string;
            }
        }
    }
}