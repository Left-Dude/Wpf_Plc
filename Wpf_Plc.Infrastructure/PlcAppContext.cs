using Microsoft.EntityFrameworkCore;
using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Infrastructure;

public class PlcAppContext : DbContext
{
    public DbSet<PLCDevice> PlcDevices { get; set; }
    public DbSet<PLCModel> PlcModels { get; set; }
    public DbSet<PLCProgram> PlcPrograms { get; set; }    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbFolder = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Wpf_Plc.Infrastructure");
        var dbFile = Path.Combine(dbFolder, "PLC_database.db");
        var fullPath = Path.GetFullPath(dbFile);

        optionsBuilder.UseSqlite($"Data Source={fullPath}");
    }
}