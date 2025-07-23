using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Wpf_Plc.Domain.Entities;
using Wpf_Plc.Domain.Enums;

namespace Wpf_Plc
{
    public class BoolToYesNoConverter : IValueConverter
    {
        public BoolToYesNoConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (s.Equals("Да", StringComparison.OrdinalIgnoreCase)) return true;
                if (s.Equals("Нет", StringComparison.OrdinalIgnoreCase)) return false;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class ConnectionTypesConverter : IValueConverter
    {
        public ConnectionTypesConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = new List<string>();
            if (value is PLCModel plc)
            {
                if (plc.SupportsEthernet) list.Add("ETHERNET");
                if (plc.SupportsRS232)   list.Add("RS-232");
                if (plc.SupportsRS485)   list.Add("RS-485");
                if (plc.SupportsUsb)     list.Add("USB");
                if (plc.SupportsCanBus)  list.Add("CAN-Bus");
            }
            if (list.Count == 0) list.Add("Нет данных");
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class PowerSupplyTypeConverter : IValueConverter
    {
        public PowerSupplyTypeConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is PowerSupplyType t
                ? t switch
                  {
                    PowerSupplyType.Ac   => "Переменный ток (AC)",
                    PowerSupplyType.Dc   => "Постоянный ток (DC)",
                    PowerSupplyType.AcDc => "Постоянный(DC) и переменный(AC) токи",
                    _                    => "Неизвестный тип"
                  }
                : "Неизвестный тип";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class FileSizeConverter : IValueConverter
    {
        public FileSizeConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long b)
            {
                if (b >= 1_000_000_000) return $"{(b / 1_000_000_000.0):0.##} GB";
                if (b >= 1_000_000)     return $"{(b / 1_000_000.0):0.##} MB";
                if (b >= 1_000)         return $"{(b / 1_000.0):0.##} KB";
                return $"{b} байт";
            }
            return "Нет данных";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class FileTypeConverter : IValueConverter
    {
        public FileTypeConverter() { }

        private static readonly Dictionary<string,string> Descs =
            new(StringComparer.OrdinalIgnoreCase)
        {
            [".ld"]  = "Программа Ladder Diagram",
            [".fbd"] = "Программа Function Block Diagram",
            [".st"]  = "Программа Structured Text",
            [".awl"] = "Программа AWL (Siemens)",
            [".scl"] = "Программа SCL (Siemens)",
            [".plc"] = "Обобщенный файл PLC программы"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ext)
            {
                var up = ext.ToUpper();
                return Descs.TryGetValue(ext, out var d)
                    ? $"{up} – {d}"
                    : $"{up} – Неизвестный тип файла";
            }
            return "Неизвестный тип файла";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
