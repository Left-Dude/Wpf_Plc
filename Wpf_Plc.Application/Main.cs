using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Wpf_Plc.Application;
public class CxProgrammerAutomation
{
    // Импорт функций AutoIt
    [DllImport("AutoItX3_x64.dll", EntryPoint = "AU3_WinActivate")]
    public static extern void WinActivate(string title, string text);

    [DllImport("AutoItX3_x64.dll", EntryPoint = "AU3_ControlClick")]
    public static extern void ControlClick(string title, string text, string control, string button, int clicks);


    [DllImport("AutoItX3_x64.dll", EntryPoint = "AU3_Send", CharSet = CharSet.Unicode)]
    public static extern void Send(string keys, int nMode = 0);

    [DllImport("AutoItX3_x64.dll", EntryPoint = "AU3_WinWait")]
    public static extern int WinWait(string title, string text, int timeout);

    [DllImport("AutoItX3_x64.dll", EntryPoint = "AU3_WinWaitActive")]
    public static extern int WinWaitActive(string title, string text, int timeout);

    [DllImport("AutoItX3_x64.dll", EntryPoint = "AU3_ControlCommand")]
    public static extern IntPtr ControlCommand(string title, string text, string control, string command, string extra, StringBuilder result, int bufSize);

    int AutoItTimeout = 50; // Скорость работы (мс)

    private readonly string _projectPath = GetTestProgramPath();

    public void LoadProgram(string Ip = "1921682501", string Port = "9600", string cxPath = @"D:\cxprog\CX-Programmer\CX-P.exe")
    {
        try
        {
            // Смена раскладки
            InputLanguage russian = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
            InputLanguage.CurrentInputLanguage = russian;

            // Формируем команду для запуска CX-Programmer с указанным проектом
            string command = $"/C start \"\" \"{cxPath}\" \"{_projectPath}\"";

            // Запускаем через CMD
            Process.Start("cmd.exe", command);

            Thread.Sleep(2000);

            //Активация окна CX Programmer
            WinActivate("CX-Programmer - program.cxp", "");
            Thread.Sleep(500);

            Send("{Alt}");
            Thread.Sleep(AutoItTimeout);

            Send("{к}");
            Thread.Sleep(AutoItTimeout);

            Send("{Enter}");
            Thread.Sleep(AutoItTimeout);

            Send("{с}");
            Thread.Sleep(AutoItTimeout);

            for (int i = 0; i <= 7; i++)
            {
                Send("{Up}");
                Thread.Sleep(AutoItTimeout);
            }

            Send("{Tab}");
            Thread.Sleep(AutoItTimeout);

            Send("{Enter}");
            Thread.Sleep(AutoItTimeout);

            Send("+{Tab}");
            Thread.Sleep(AutoItTimeout);

            Send("{Right}");
            Thread.Sleep(AutoItTimeout);

            for (int i = 0; i <= 1; i++)
            {
                Send("{Tab}");
            }

            Send(Ip);
            Thread.Sleep(AutoItTimeout);

            Send("{Tab}");
            Thread.Sleep(AutoItTimeout);

            Send(Port);
            Thread.Sleep(AutoItTimeout);

            Send("{Tab}");
            Thread.Sleep(AutoItTimeout);

            Send("{Enter}");
            Thread.Sleep(AutoItTimeout);

            Send("{Tab}");
            Thread.Sleep(AutoItTimeout);

            Send("{Enter}");
            Thread.Sleep(AutoItTimeout);

            Send("^{ц}");
            Thread.Sleep(AutoItTimeout);

            Send("{Tab}");
            Thread.Sleep(AutoItTimeout);

            Send("{Enter}");
            Thread.Sleep(AutoItTimeout);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    
    private static string GetTestProgramPath()
    {
        var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        // Путь к корню решения
        var solutionDir = Path.Combine(
            assemblyDir, 
            "..", "..", "..", "..");
        var testFilesPath = Path.Combine(solutionDir, "Wpf_Plc.Tests", "TestFiles");
        return Path.Combine(testFilesPath, "test_program.cxp");
    }
}