using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.IO.Ports;
using Wpf_Plc.Domain.Enums;

namespace Wpf_Plc.Domain.Entities;

public class PLCDevice : BaseEntity
{
    public Guid ModelId { get; set; }
    public PLCModel Model { get; set; }

// Для алгоритма
    public string IPAddressString { get; set; }
    
    [NotMapped]
    public IPAddress IPAddress
    {
        get => System.Net.IPAddress.Parse(IPAddressString);
        set => IPAddressString = value.ToString();
    }

    public string PortName { get; set; }
    public int BaudRate { get; set; }
    public int DataBits { get; set; }
    
    [NotMapped]
    public SerialPort Port => new()
    {
        PortName = this.PortName,
        BaudRate = this.BaudRate,
        DataBits = this.DataBits
    };
}