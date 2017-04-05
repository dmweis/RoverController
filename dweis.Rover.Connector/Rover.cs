using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dweis.Rover.Connector
{
   public class Rover
   {
      private SerialPort _port;

      public Rover(SerialPortAddress address)
      {
         _port = new SerialPort(address.Name);
         _port.Open();
      }

      public void Close()
      {
         try
         {
            _port.Close();
            _port.Dispose();
         }
         catch
         {
            
         }
      }

      public void TurnServos90()
      {
         _port?.Write($"{{2 250 490 480 250}}");
      }

      public void TurnServos180()
      {
         _port?.Write($"{{2 480 280 250 480}}");
      }

      public void FullForward()
      {
         _port?.Write($"{{1 150 600 150 600}}");
      }

      public void FullBackwards()
      {
         _port?.Write($"{{1 600 150 600 150}}");
      }

      public void StopMotors()
      {
         _port?.Write($"{{1 345 330 375 360}}");
      }

      public void ParallelLeft()
      {
         _port?.Write($"{{1 600 600 150 150}}");
      }

      public void ParallelRight()
      {
         _port?.Write($"{{1 150 150 600 600}}");
      }
   }
}
