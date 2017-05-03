using System;

namespace dweis.Rover.Controller
{
   public class RoverMotor
   {
      public int Index { get; set; }
      public int StationaryValue { get; set; }
      public int MaxForward { get; set; }
      public int MaxBackwards { get; set; }

      private static float MapFloat(float value, float inMin, float inMax, float outMin, float outMax)
      {
         return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
      }
   }
}
