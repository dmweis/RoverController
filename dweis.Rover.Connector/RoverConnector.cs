using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dweis.Rover.Connector
{
   public class RoverConnector
   {
      private SerialPort _port;
      private Thread readerThread;
      private bool _keepreading = true;

      public RoverConnector(SerialPortAddress address)
      {
         _port = new SerialPort(address.Name);
         _port.Encoding = Encoding.ASCII;
         _port.Open();
         readerThread = new Thread(() =>
         {
            while (_keepreading)
            {
               try
               {
                  System.Diagnostics.Debug.WriteLine(_port.ReadLine());
               }
               catch
               {
               }
            }
         });
         readerThread.Start();
      }

      public void Close()
      {
         try
         {
            _keepreading = false;
            readerThread.Abort();
            readerThread.Join(TimeSpan.FromSeconds(1));
            _port.Close();
            _port.Dispose();
         }
         catch
         {
            
         }
      }

      public void SetServo(Motors wheel, float pulse)
      {
         _port?.Write($"{{0 {(int)wheel} {(int)pulse}}}");
      }

      public void TurnCrossLegs()
      {
         _port?.Write($"{{2 390 390 390 390}}");
      }

      public void TurnServos90()
      {
         _port?.Write($"{{2 250 490 480 250}}");
      }

      public void TurnServos180()
      {
         _port?.Write($"{{2 480 280 280 480}}");
      }

      public void TurnSlightlyLeft()
      {
         _port?.Write($"{{2 165 424 550 320}}");
      }

      public void TurnSlightlyRight()
      {
         _port?.Write($"{{2 310 555 420 200}}");
      }

      public void FullForward()
      {
         _port?.Write($"{{1 200 550 200 550}}");
      }

      public void FullBackwards()
      {
         _port?.Write($"{{1 550 200 550 200}}");
      }

      public void StopMotors()
      {
         _port?.Write($"{{1 345 365 345 360}}");
      }

      public void ParallelLeft()
      {
         _port?.Write($"{{1 550 550 200 200}}");
      }

      public void ParallelRight()
      {
         _port?.Write($"{{1 200 200 550 550}}");
      }

      public void RotateClockwise()
      {
         _port?.Write($"{{1 200 200 200 200}}");
      }

      public void RotateCounterClockwise()
      {
         _port?.Write($"{{1 550 550 550 550}}");
      }
   }
}
