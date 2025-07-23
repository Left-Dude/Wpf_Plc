namespace Wpf_Plc.Domain.Entities;

public class PLCProgram : BaseEntity
{
    public string Name { get; set; }
    public string OriginalFileName { get; set; }
    public long SizeBytes { get; set; }
    public string FileExtension { get; set; }

    //Связи
    public PLCModel PLCModel { get; set; }
    public Guid PLCModelId => this.PLCModel.Id;
}