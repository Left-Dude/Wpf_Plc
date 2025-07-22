using System.Configuration;
using System.Data;
using System.Windows;
using Wpf_Plc.Infrastructure;

namespace Wpf_Plc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создание и заполнение базы данных начальными данными
            using var context = new PlcAppContext();
            var seeder = new DataSeeder(context);
            seeder.DataSeed();

        }
    }

}
