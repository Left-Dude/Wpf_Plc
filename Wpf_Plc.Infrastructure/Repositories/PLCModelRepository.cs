using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Infrastructure.Repositories;

public class PLCModelRepository(PlcAppContext context) : BaseRepository<PLCModel>(context)
{
    
}