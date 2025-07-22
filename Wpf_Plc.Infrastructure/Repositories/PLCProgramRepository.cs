using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Infrastructure.Repositories;

public class PLCProgramRepository(PlcAppContext context) : BaseRepository<PLCProgram>(context)
{
    
}