using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Infrastructure;
using Wpf_Plc.Infrastructure.Repositories;

namespace Wpf_Plc
{
    public partial class ProgramView : UserControl, INotifyPropertyChanged
    {
        private readonly PlcAppContext _context;
        private readonly GenericRepository<PLCProgram> _repo;
        private PLCProgram _selectedProgram;

        public ObservableCollection<PLCProgram> PLCPrograms { get; } = 
            new ObservableCollection<PLCProgram>();

        public PLCProgram SelectedProgram
        {
            get => _selectedProgram;
            set
            {
                if (_selectedProgram != value)
                {
                    _selectedProgram = value;
                    OnPropertyChanged(nameof(SelectedProgram));
                }
            }
        }

        public ProgramView()
        {
            InitializeComponent();
            DataContext = this;

            _context = new PlcAppContext();
            _repo    = new GenericRepository<PLCProgram>(_context);

            Loaded += ProgramView_Loaded;
        }

        private async void ProgramView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProgramsAsync();
        }

        private async Task LoadProgramsAsync()
        {
            var list = await _repo.GetAllEntitiesAsync();
            PLCPrograms.Clear();
            foreach (var p in list)
                PLCPrograms.Add(p);

            if (PLCPrograms.Count > 0)
                SelectedProgram = PLCPrograms[0];  // через свойство — с уведомлением
        }

        private void BtnPLC_Click(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this);
            var cc  = wnd?.FindName("MainContent") as ContentControl;
            if (cc != null) cc.Content = new PlcDetailsView();
        }

        private void BtnEXIT_Click(object sender, RoutedEventArgs e)
            => System.Windows.Application.Current.Shutdown();

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProgram == null)
            {
                MessageBox.Show("Выберите программу для загрузки", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool rs       = RsCheckBox.IsChecked  ?? false;
            bool ethernet = EthernetCheckBox.IsChecked ?? false;
            if (!rs && !ethernet)
            {
                MessageBox.Show("Выберите хотя бы один тип соединения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var type = rs ? "RS" : "ETHERNET";
            MessageBox.Show($"Загрузка программы '{SelectedProgram.Name}' с использованием {type}",
                "Загрузка программы", MessageBoxButton.OK, MessageBoxImage.Information);
            
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
