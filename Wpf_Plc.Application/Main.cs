using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Automation;
using System.Diagnostics;

namespace Wpf_Plc.Application;

    public class OmronFinsService
    {
        public void LoadTest()
        {
            // Убедись, что CX-Programmer запущен
            Process.Start("cmd.exe", "/C start \"\" \"D:\\cxprog\\CX-Programmer\\CX-P.exe\" \"C:\\Users\\NoMoneySlave\\Desktop\\program.cxp\"");
            Thread.Sleep(8000); // Ждем запуска

            // Получаем главное окно
            var root = AutomationElement.RootElement;
            var cxWindow = root.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, "program - CX-Programmer"));

            if (cxWindow == null)
            {
                Console.WriteLine("Окно CX-Programmer не найдено.");
                return;
            }

            Console.WriteLine("Окно CX-Programmer найдено. Ищем меню 'ПЛК'...");

            // Ищем строку меню
            var menuBar = cxWindow.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuBar));

            if (menuBar == null)
            {
                Console.WriteLine("Строка меню не найдена.");
                return;
            }

            // Находим пункт меню "Система" (или "ПЛК", если у тебя он называется так)
            var systemMenu = menuBar.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, "Система")); // замените на "ПЛК", если это нужный пункт

            if (systemMenu == null)
            {
                Console.WriteLine("Меню 'Система' не найдено.");
                return;
            }

            // Пытаемся раскрыть меню (если поддерживает ExpandCollapsePattern)
            var expandPattern = systemMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            expandPattern?.Expand();

            Thread.Sleep(1000); // небольшая пауза, чтобы UI успел отобразить

            // Получаем все элементы внутри раскрытого меню
            var submenuItems = systemMenu.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));

            Console.WriteLine("Элементы в подменю 'Система':");

            int i = 0;
            foreach (AutomationElement item in submenuItems)
            {
                i++;
                Console.WriteLine($"[{i}] {item.Current.ControlType.ProgrammaticName}");
                Console.WriteLine($"     Name: {item.Current.Name}");
                Console.WriteLine($"     AutomationId: {item.Current.AutomationId}");
                Console.WriteLine(new string('-', 40));
            }

            if (i == 0)
            {
                Console.WriteLine("Нет вложенных элементов. Возможно, меню не раскрыто.");
            }

            // Ищем пункт меню "ПЛК"
            var plcMenu = menuBar.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, "ПЛК"));

            if (plcMenu == null)
            {
                Console.WriteLine("Пункт меню 'ПЛК' не найден.");
                return;
            }

            var expand = plcMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            expand?.Expand(); // раскрываем меню "ПЛК"
            Thread.Sleep(500); // небольшая задержка

            // Ищем пункт "Работать в сети" внутри раскрытого меню "ПЛК"
            var workOnlineItem = plcMenu.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, "Работать в сети")); // или "Work Online" если англ. локализация

            if (workOnlineItem == null)
            {
                Console.WriteLine("Пункт меню 'Работать в сети' не найден.");
                return;
            }

            var invoke = workOnlineItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            invoke?.Invoke();

            Console.WriteLine("Пункт 'Работать в сети' успешно вызван!");
        }
    }



