using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Enums;
using Wpf_Plc.Domain.Interfaces.Services;
using Wpf_Plc.Infrastructure.Services;

namespace Wpf_Plc.Infrastructure
{
    public class DataSeeder
{
    private readonly PlcAppContext _context;
    private readonly ProgramStorageService _fileStorageService;

    public DataSeeder(
        PlcAppContext context,
        ProgramStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task DataSeedAsync()
    {
        SeedPlcModels();
        SeedPlcDevices();
        await SeedPlcProgramsAsync().ConfigureAwait(false);
    }

    private void SeedPlcModels()
    {
        if (!_context.PlcModels.Any())
        {
            _context.PlcModels.Add(new PLCModel
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
            });
            _context.SaveChanges();
        }
    }

    private void SeedPlcDevices()
    {
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
    }

    private async Task SeedPlcProgramsAsync()
    {
        if (_context.PlcPrograms.Any()) 
            return;

        var model = _context.PlcModels.First();
        var list = new List<PLCProgram>();
        var testPath = GetTestProgramPath();

        if (File.Exists(testPath))
        {
            try
            {
                await using var fs = File.OpenRead(testPath);
                var program = await _fileStorageService
                    .SaveFileAsync(
                        fs,
                        Path.GetFileNameWithoutExtension(testPath),
                        Path.GetExtension(testPath).TrimStart('.'),
                        model)
                    .ConfigureAwait(false);

                list.Add(program);
            }
            catch
            {
                list.AddRange(CreateFallbackPrograms(model));
            }
        }
        else
        {
            list.AddRange(CreateFallbackPrograms(model));
        }

        // Добавляем **все** в БД одной пачкой
        _context.PlcPrograms.AddRange(list);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
        private string GetTestProgramPath()
        {
            // Путь к корню решения
            var solutionDir = GetSolutionDirectory();
            var testFilesPath = Path.Combine(solutionDir, "Wpf_Plc.Tests", "TestFiles");
            return Path.Combine(testFilesPath, "test_program.cxp");
        }

        private string GetSolutionDirectory()
        {
            // Получаем путь к исполняемой сборке
            var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            // Поднимаемся на 3 уровня вверх к корню решения
            return Path.Combine(
                assemblyDir, 
                "..", "..", "..", "..");
        }

        private IEnumerable<PLCProgram> CreateFallbackPrograms(PLCModel model)
        {
            return new List<PLCProgram>
            {
                new PLCProgram
                {
                    Id = Guid.NewGuid(),
                    Name = "TraficSignal",
                    OriginalFileName = "program_trafic_signal.cpx",
                    SizeBytes = 12345,
                    FileExtension = "cpx",
                    PLCModel = model
                },
                new PLCProgram
                {
                    Id = Guid.NewGuid(),
                    Name = "Blinking",
                    OriginalFileName = "program_blinking.cpx",
                    SizeBytes = 15678,
                    FileExtension = "cpx",
                    PLCModel = model
                }
            };
        }
    }
}