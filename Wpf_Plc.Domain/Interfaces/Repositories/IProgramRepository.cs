using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Domain.Interfaces.Repositories;

public interface IProgramRepository
{
    Task Add(PLCProgram program);
}