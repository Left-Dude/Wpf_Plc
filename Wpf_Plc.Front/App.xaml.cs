using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using Wpf_Plc.Application.Services;
using Wpf_Plc.Infrastructure;

namespace Wpf_Plc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static PlcAppContext DbContext { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var options = new DbContextOptionsBuilder<PlcAppContext>()
                .UseSqlite("Data Source=PLC_database.db")
                .Options;

            DbContext = new PlcAppContext(options);

            DbContext.Database.Migrate();

            var plcService = new PLCModelService(DbContext);
            plcService.SeedData();

            var programService = new PLCProgramService(DbContext);

        }
    }
}
