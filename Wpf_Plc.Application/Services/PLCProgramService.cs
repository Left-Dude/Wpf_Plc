using Wpf_Plc.Infrastructure;
using Wpf_Plc.Domain.Entities;

namespace Wpf_Plc.Application.Services
{
    public class PLCProgramService
    {
        private readonly PlcAppContext _context;

        public PLCProgramService(PlcAppContext context)
        {
            _context = context;
        }
        public Guid AddProgram(string name, string originalFileName, long sizeBytes, Guid plcDeviceId)
        {
            var newProgram = new PLCProgram
            {
                Id = Guid.NewGuid(),
                Name = name,
                OriginalFileName = originalFileName,
                SizeBytes = sizeBytes,
                PLCDeviceId = plcDeviceId,
                CpxFileData = null,
                CpxFileName = null,
                SchemeImageData = null,
                SchemeFileName = null
            };

            _context.PlcPrograms.Add(newProgram);
            _context.SaveChanges();

            return newProgram.Id; // возвращаем ID для последующей привязки файлов
        }

        /// <summary>
        /// Обновить .cpx и схему у существующей программы
        /// </summary>
        public void SaveProgramFiles(Guid programId, byte[] cpxData, string cpxName, byte[] schemeData, string schemeName)
        {
            var program = _context.PlcPrograms.FirstOrDefault(p => p.Id == programId);
            if (program == null) throw new Exception("Программа не найдена");

            program.CpxFileData = cpxData;
            program.CpxFileName = cpxName;
            program.SchemeImageData = schemeData;
            program.SchemeFileName = schemeName;

            _context.SaveChanges();
        }

        /// <summary>
        /// Получить файлы программы по её Id
        /// </summary>
        public (byte[] cpxData, string cpxName, byte[] schemeData, string schemeName) GetProgramFiles(Guid programId)
        {
            var program = _context.PlcPrograms.FirstOrDefault(p => p.Id == programId);
            if (program == null) throw new Exception("Программа не найдена");

            return (program.CpxFileData, program.CpxFileName, program.SchemeImageData, program.SchemeFileName);
        }
    }
}
