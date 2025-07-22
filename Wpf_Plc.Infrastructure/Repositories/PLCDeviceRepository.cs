using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Infrastructure.Repositories;

public class PLCDeviceRepository(PlcAppContext context) :BaseRepository<PLCDevice>(context)
{
    
}