using Microsoft.EntityFrameworkCore;
using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Infrastructure;

public class PlcAppContext : DbContext
{

    public DbSet<PLCDevice> PlcDevices { get; set; }
    public DbSet<PLCModel> PlcModels { get; set; }
    public DbSet<PLCProgram> PlcPrograms { get; set; }

    public PlcAppContext() { }

    public PlcAppContext(DbContextOptions<PlcAppContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=PLC_database.db");
    }
}