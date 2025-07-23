using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Infrastructure;
using Wpf_Plc.Infrastructure.Repositories;

namespace Wpf_Plc
{
    public partial class PlcDetailsView : UserControl
    {
        private readonly PlcAppContext _context;
        private readonly GenericRepository<PLCModel> _modelRepository;
        public ObservableCollection<PLCModel> PLCModels { get; } = new ObservableCollection<PLCModel>();
        
        public PlcDetailsView()
        {
            InitializeComponent();
            
            _context = new PlcAppContext();
            _modelRepository = new GenericRepository<PLCModel>(_context);
            
            DataContext = this;
            Loaded += PlcDetailsView_Loaded;
        }

        private async void PlcDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPlcDataAsync();
        }

        private async Task LoadPlcDataAsync()
        {
            var models = await _modelRepository.GetAllEntitiesAsync();
            PLCModels.Clear();
            foreach (var model in models)
            {
                PLCModels.Add(model);
            }
        }

        private void BtnPROG_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window wnd
                && wnd.FindName("MainContent") is ContentControl main)
            {
                main.Content = new ProgramView();
            }
        }

        private void BtnEXIT_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}