using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Enums;

namespace Wpf_Plc.Infrastructure;

public class DataSeeder
{
    private readonly PlcAppContext _context;

    public DataSeeder(PlcAppContext context)
    {
        _context = context;
    }

    public void DataSeed()
    {
        if (!_context.PlcModels.Any())
        {
            _context.PlcModels.AddRange(new List<PLCModel>
            {
                new PLCModel
                {
                    Manufacturer = "Omron",
                    Model = "CP1L",
                    DigitalInputsCount = 12,
                    DigitalOutputsCount = 8,
                    AnalogInputsCount = 1,
                    AnalogOutputsCount = 1,
                    SupportsExpansionModules = true,
                    PowerSupplyType = PowerSupplyType.Dc,
                    PowerConsumption = 2.5,
                    SupportsEthernet = true,
                    SupportsRS232 = true,
                    SupportsRS485 = false,
                    SupportsUsb = false,
                    SupportsCanBus = false,
                    ManufacturerURL = "https://www.omron.com/"
                }
            });

            _context.SaveChanges();
        }

        if (!_context.PlcDevices.Any())
        {
            var modelId = _context.PlcModels.First().Id;
            _context.PlcDevices.Add(new PLCDevice
            {
                ModelId = modelId,
                IPAddressString = "192.168.250.1",
                PortName = "COM3",
                BaudRate = 9600,
                DataBits = 8
            });

            _context.SaveChanges();
        }

        if (!_context.PlcPrograms.Any())
        {
            var deviceId = _context.PlcDevices.First().Id;
            _context.PlcPrograms.AddRange(new List<PLCProgram>
            {
                new PLCProgram
                {
                    Id = Guid.NewGuid(),
                    Name = "TraficSignal",
                    OriginalFileName = "program_trafic_signal.cpx",
                    SizeBytes = 12345,
                    PLCDeviceId = deviceId
                },
                new PLCProgram
                {
                    Id = Guid.NewGuid(),
                    Name = "Blinking",
                    OriginalFileName = "program_blinking.cpx",
                    SizeBytes = 15678,
                    PLCDeviceId = deviceId
                }
            });

            _context.SaveChanges();
        }
    }
}
