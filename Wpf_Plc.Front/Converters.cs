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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Да" : "Нет";
            }
            return "Нет данных";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class ConnectionTypesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var connections = new List<string>();
            
            if (value is PLCModel plc)
            {
                if (plc.SupportsEthernet) connections.Add("ETHERNET");
                if (plc.SupportsRS232) connections.Add("RS-232");
                if (plc.SupportsRS485) connections.Add("RS-485");
                if (plc.SupportsUsb) connections.Add("USB");
                if (plc.SupportsCanBus) connections.Add("CAN-Bus");
            }
            
            if (connections.Count == 0) 
                connections.Add("Нет данных");
            
            return connections;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class PowerSupplyTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PowerSupplyType type)
            {
                switch (type)
                {
                    case PowerSupplyType.Ac: return "Переменный ток (AC)";
                    case PowerSupplyType.Dc: return "Постоянный ток (DC)";
                    case PowerSupplyType.AcDc: return "Постоянный(DC) и переменный(AC) токи";
                    default: return "Неизвестный тип";
                }
            }
            return "Неизвестный тип";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}