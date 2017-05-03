using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dweis.Rover.Connector
{
   public class NewRoverConnector : IRoverConnector
   {
      private SerialPort _port;
      private Thread readerThread;
      private bool _keepreading = true;
      private Controller.Rover _rover;

      public NewRoverConnector(SerialPortAddress address, string json)
      {
         _rover = Controller.Rover.FromJson(json);
         _rover.NewMessage += _rover_NewMessage;
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

      private void _rover_NewMessage(object sender, string e)
      {
         _port?.Write(e);
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

      public void TurnCrossLegs()
      {
         int angle = 50;
         _rover.SetLegs(angle, angle, angle, angle);
      }

      public void TurnServos90()
      {
         _rover.SetLegs(0, 0, 0, 0);
      }

      public void TurnServos180()
      {
         _rover.SetLegs(90, 90, 90, 90);
      }

      public void TurnSlightlyLeft()
      {
         int angle = 20;
         _rover.SetLegs(-angle, angle, -angle, angle);
      }

      public void TurnSlightlyRight()
      {
         int angle = 20;
         _rover.SetLegs(-angle, angle, -angle, angle);
      }

      public void FullForward()
      {
         int speed = 100;
         _rover.SetSpeed(speed, speed, speed, speed);
      }

      public void FullBackwards()
      {
         int speed = 100;
         _rover.SetSpeed(-speed, -speed, -speed, -speed);
      }

      public void StopMotors()
      {
         _rover.SetSpeed(0, 0, 0, 0);
      }

      public void ParallelLeft()
      {
         TurnServos180();
         int speed = 100;
         _rover.SetSpeed(-speed, speed, speed, -speed);
      }

      public void ParallelRight()
      {
         TurnServos180();
         int speed = 100;
         _rover.SetSpeed(speed, -speed, -speed, speed);
      }

      public void RotateClockwise()
      {
         TurnCrossLegs();
         int speed = 100;
         _rover.SetSpeed(speed, -speed, speed, -speed);
      }

      public void RotateCounterClockwise()
      {
         TurnCrossLegs();
         int speed = 100;
         _rover.SetSpeed(-speed, speed, -speed, speed);
      }
   }
}
