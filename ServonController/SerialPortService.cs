﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace dweis.Rover
{
   class SerialPortService
   {
      private const string ClassGuidSerialDevices = "{4d36e978-e325-11ce-bfc1-08002be10318}";

      public static SerialPortAddress[] GetSerialPorts()
      {
         string[] portNames = SerialPort.GetPortNames();
         SerialPortAddress[] ports = new SerialPortAddress[portNames.Length];
         for (int i = 0; i < portNames.Length; i++)
         {
            ports[i] = new SerialPortAddress(portNames[i]);
         }
         Task.Factory.StartNew(() => UpdateSerialPortsWithDescriptions(ports));
         return ports;
      }

      private static void UpdateSerialPortsWithDescriptions(SerialPortAddress[] serialPortAddresses)
      {
         var connectionScope = new ManagementScope("root\\CIMV2");
         var serialQuery =
            new SelectQuery($"SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{ClassGuidSerialDevices}\"");
         var searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
         foreach (var item in searcher.Get())
         {
            var deviceName = item["Name"]?.ToString() ?? string.Empty;
            var deviceDescription = item["Description"]?.ToString() ?? string.Empty;
            foreach (var serialPortAddress in serialPortAddresses)
            {
               if (deviceName.Contains(serialPortAddress.Name))
               {
                  serialPortAddress.Description = deviceDescription;
                  break;
               }
            }
         }
      }
   }
}
