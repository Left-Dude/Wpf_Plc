using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace Wpf_Plc.Application;

public class CxProgrammerAutomation
{
    // Импорт функций AutoIt
    [DllImport("AutoItX3.dll", EntryPoint = "AU3_WinActivate")]
    public static extern void WinActivate(string title, string text);

    [DllImport("AutoItX3.dll", EntryPoint = "AU3_ControlClick")]
    public static extern void ControlClick(string title, string text, string control, string button, int clicks);


    [DllImport("AutoItX3.dll", EntryPoint = "AU3_Send", CharSet = CharSet.Unicode)]
    public static extern void Send(string keys, int nMode = 0);

    [DllImport("AutoItX3.dll", EntryPoint = "AU3_WinWait")]
    public static extern int WinWait(string title, string text, int timeout);

    [DllImport("AutoItX3.dll", EntryPoint = "AU3_WinWaitActive")]
    public static extern int WinWaitActive(string title, string text, int timeout);

    [DllImport("AutoItX3.dll", EntryPoint = "AU3_ControlCommand")]
    public static extern IntPtr ControlCommand(string title, string text, string control, string command, string extra, StringBuilder result, int bufSize);

    const int AutoItTimeout = 300;

    const string _Ip = "1921682501";

    public void LoadProgram()
    {
        try
        {
            // Смена раскладки
            /* InputLanguage newLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
            InputLanguage.CurrentInputLanguage = newLanguage;
            */

            //Запуск CX Programmer с проектом (Сейчас локальный)
            //Process.Start("cmd.exe", "/C start \"\" \"D:\\cxprog\\CX-Programmer\\CX-P.exe\" \"C:\\Users\\NoMoneySlave\\Desktop\\program.cxp\"");
            Thread.Sleep(2000);

            //Активация окна CX Programmer
            WinActivate("CX-Programmer - program.cxp", "");
            Thread.Sleep(1000);

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

            Send(_Ip);
            Thread.Sleep(AutoItTimeout);

            for (int i = 0; i <= 1; i++)
            {
                Send("{Tab}");
                Thread.Sleep(AutoItTimeout);
            }

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
}



