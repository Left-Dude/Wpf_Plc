namespace Wpf_Plc.Domain.Entities;

public class PLCProgram : BaseEntity
{
    public string Name { get; set; }
    public string OriginalFileName { get; set; }
    public long SizeBytes { get; set; }

    //Связи
    public Guid PLCDeviceId { get; set; }
    public PLCDevice PLCDevice { get; set; }
}