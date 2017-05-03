using System;
using System.Collections.Generic;
using System.Text;

namespace dweis.Rover.Controller
{
   public class RoverLeg
   {
      public RoverServo Servo { get; }
      public RoverMotor Motor { get; }

      private bool _inverse = false;

      public RoverLeg(RoverServo servo, RoverMotor motor)
      {
         Servo = servo;
         Motor = motor;
      }

      public RoverLeg()
      {
         Servo = new RoverServo();
         Motor = new RoverMotor();
      }

      public int CalculateAngle(int angle)
      {
         return Servo.CalculateAngle(angle, out _inverse);
      }

      public int CalculateSpeed(int speed)
      {
         return Motor.CalculateSpeed(speed, _inverse);
      }
   }
}
