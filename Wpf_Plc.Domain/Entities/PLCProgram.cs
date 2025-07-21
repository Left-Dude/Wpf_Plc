namespace Wpf_Plc.Domain.Entities;

public class PLCProgram : BaseEntity
{
    public string Name { get; set; }
    public string OriginalFileName { get; set; }
    public long SizeBytes { get; set; }

    //Связи
    public Guid PLCDeviceId { get; set; }
    public PLCDevice PLCDevice { get; set; }

    //Файл .cpx 
    public byte[] CpxFileData { get; set; }
    public string CpxFileName { get; set; }

    //Cхема на программу
    public byte[] SchemeImageData { get; set; }
    public string SchemeFileName { get; set; }
}