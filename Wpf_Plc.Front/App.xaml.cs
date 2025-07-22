using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Interfaces.Services;
using Wpf_Plc.Infrastructure;
using Wpf_Plc.Infrastructure.Services;

namespace Wpf_Plc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using var context = new PlcAppContext();
            // убираем UnitOfWork — он нам больше не нужен в сервисе
            var storagePath = Path.Combine(GetSolutionDirectory(),
                "Wpf_Plc.Infrastructure", "Storage", "Programs");
            var fileStorageService = new ProgramStorageService(storagePath);
            var seeder = new DataSeeder(context, fileStorageService);

            await seeder.DataSeedAsync();
        }
        
        private string GetSolutionDirectory()
        {
            // Получаем путь к исполняемой сборке
            var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            // Поднимаемся на 3 уровня вверх к корню решения
            return Path.Combine(
                assemblyDir, 
                "..", "..", "..", "..");
        }
    }

}
