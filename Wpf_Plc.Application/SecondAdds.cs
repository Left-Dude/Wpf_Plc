using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Wpf_Plc.Application.slave;

public class Slave
{
    public Slave() { }

    /*
     * Определить тип устройства можно с помощью WMI-запроса к классу Win32_PnpEntity. 
     * Зная DeviceID, можно получить свойства PnpClass (текстовое имя класса устройств), 
     * Service (имя драйвера) и ClassGuid (идентификатор класса устройств), на основе которых можно 
     * судить о типе устройства. 
     * 
     * PnpClass = Modem, ClassGuid = {4d36e96d-e325-11ce-bfc1-08002be10318}
     * 
     * Пример кода на c# для вывода названия, DeviceID и PnpClass для всех USB-устройств
     * 
     */
    public string USBDescription()
    {
        string Text = "";

        ManagementObjectCollection collection;
        using (var searcher = new ManagementObjectSearcher(
            "root\\CIMV2",
            @"Select Caption,DeviceID,PnpClass From Win32_PnpEntity WHERE DeviceID like '%USB%'"))
            collection = searcher.Get();

        int i = 1;
        foreach (var device in collection)
        {
            Text += "Device " + i.ToString() + ": " + Environment.NewLine;
            foreach (var p in device.Properties)
            {
                Text += p.Name + ": ";
                if (p.Value != null)
                {
                    Text += p.Value.ToString();
                }
                else Text += "null";

                Text += Environment.NewLine;
            }
            Text += Environment.NewLine;
            i++;
        }
        return Text;
    }
    
};
