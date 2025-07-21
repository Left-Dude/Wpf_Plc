using System.Net;
using System.IO.Ports;
using Wpf_Plc.Domain.Enums;

namespace Wpf_Plc.Domain.Entities;

public class PLCDevice : BaseEntity
{
    public PLCModel Model { get; set; }
    
    // Для алгоритма
    public IPAddress IPAddress { get; set; }
    public SerialPort SerialPort { get; set; }
}