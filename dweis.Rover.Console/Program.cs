using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace dweis.Rover.Console
{
   class Program
   {
      static void Main(string[] args)
      {
         System.Console.WriteLine("Starting");
         string json = File.ReadAllText("config.json");
         Controller.Rover rover = Controller.Rover.FromJson(json);
         rover.NewMessage += (sender, text) =>
         {
            System.Console.WriteLine(text);
         };
         while (true)
         {
            System.Console.WriteLine("Enter angle");
            int angle = int.Parse(System.Console.ReadLine() ?? "");
            rover.SetLegs(angle);
         }
         SerialPort port = new SerialPort("COM9");
         port.Encoding = Encoding.ASCII;
         port.Open();
         port.WriteLine($"{{0 {0} {400}}}");
         port.WriteLine($"{{0 {1} {400}}}");
         port.WriteLine($"{{0 {2} {400}}}");
         port.WriteLine($"{{0 {3} {400}}}");
         while (true)
         {
            System.Console.WriteLine("0. Exit");
            System.Console.WriteLine("1. Set single servo");
            System.Console.WriteLine("2. Set Angle");
            int command = int.Parse(System.Console.ReadLine()??"");
            if (command == 0)
            {
               break;
            }
            else if (command == 1)
            {
               System.Console.WriteLine("Enter index: ");
               int index = int.Parse(System.Console.ReadLine()??"");
               while (true)
               {
                  System.Console.WriteLine("Enter pulse: ");
                  string input = System.Console.ReadLine() ?? "";
                  if (string.IsNullOrWhiteSpace(input))
                  {
                     break;
                  }
                  int pulse = int.Parse(input);
                  System.Console.WriteLine($"Servo: {index} set to {pulse}");
                  port.WriteLine($"{{0 {index} {pulse}}}");
                  
               }
            }
            else if (command == 2)
            {
               while (true)
               {
                  
               }
            }
            else if (command == 3)
            {
               //System.Console.WriteLine("Enter index: ");
               //int index = int.Parse(System.Console.ReadLine() ?? "");
               //System.Console.WriteLine("Enter pulse: ");
               //int pulse = int.Parse(System.Console.ReadLine() ?? "");
               //System.Console.WriteLine($"Servo: {index} set to {pulse}");
               //port.WriteLine($"{{0 {index} {pulse}}}");
            }
            else
            {
               System.Console.WriteLine("Command unknown");
            }
         }
         System.Console.WriteLine("Press enter to exit");
         System.Console.ReadLine();
      }

      private static void CreateNewConfig(string path)
      {
         Controller.Rover rover = new Controller.Rover();
         string json = rover.ToJson();
         File.WriteAllText(path, json);
      }
   }
}
