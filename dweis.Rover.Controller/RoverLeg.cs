using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace dweis.Rover.Controller
{
   public class RoverLeg
   {
      public RoverServo Servo { get; }
      public RoverMotor Motor { get; }
      [JsonIgnore]
      public bool Reversed { get; set; }

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
   }
}
