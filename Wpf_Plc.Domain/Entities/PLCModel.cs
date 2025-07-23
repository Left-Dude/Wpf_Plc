using System.ComponentModel.DataAnnotations;
using Wpf_Plc.Domain.Enums;

namespace Wpf_Plc.Domain.Entities;

public class PLCModel : BaseEntity
{
    //Базовая информация
    public string Manufacturer {get; set;}
    public string Model {get; set;}
    
    // Количество входов и выходов
    public int DigitalInputsCount { get; set; }
    public int DigitalOutputsCount { get; set; }
    public int AnalogInputsCount { get; set; }
    public int AnalogOutputsCount { get; set; }
    
    // Возможность подключения плат расширений
    public bool SupportsExpansionModules { get; set; }
    
    //Тип и значение питания
    public PowerSupplyType  PowerSupplyType { get; set; }
    public double PowerConsumption { get; set; }
    
    //Поддерживаемые типы соединения
    public bool SupportsEthernet { get; set; }
    public bool SupportsRS232 { get; set; }
    public bool SupportsRS485 { get; set; }
    public bool SupportsUsb { get; set; }
    public bool SupportsCanBus { get; set; }
    
    //Ссылки
    [Url]
    public string ManufacturerURL { get; set; }
}