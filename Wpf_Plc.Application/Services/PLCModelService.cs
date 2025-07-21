using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Enums;
using Wpf_Plc.Infrastructure;


namespace Wpf_Plc.Application.Services
{
    public class PLCModelService
    {
        private readonly PlcAppContext _context;

        public PLCModelService(PlcAppContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            if (_context.PlcModels.Any())
                return; 

            var models = new List<PLCModel>
        {
            new PLCModel
            {
                Manufacturer = "Omron",
                Model = "CP1L",
                DigitalInputsCount = 12,
                DigitalOutputsCount = 8,
                AnalogInputsCount = 1,
                AnalogOutputsCount = 1,
                SupportsExpansionModules = false,
                PowerSupplyType = PowerSupplyType.Ac,
                PowerConsumption = 3.3,
                SupportsEthernet = false,
                SupportsRS232 = true,
                SupportsRS485 = false,
                SupportsUsb = true,
                SupportsCanBus = false,
                ManufacturerURL = "https://www.omron.com/"
            }
        };

            _context.PlcModels.AddRange(models);
            _context.SaveChanges();
        }

        public List<PLCModel> GetAllModels()
        {
            return _context.PlcModels.ToList();
        }

    }
}
