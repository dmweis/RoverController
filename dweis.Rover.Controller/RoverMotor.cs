using System;

namespace dweis.Rover.Controller
{
   public class RoverMotor
   {
      public int Index { get; set; }
      public int StationaryValue { get; set; }
      public int MaxForward { get; set; }
      public int MaxBackwards { get; set; }

      public int CalculateSpeed(int speed, bool inverted = false)
      {
         if (inverted)
         {
            speed = -speed;
         }
         if (speed > 100 || speed < -100)
         {
            throw new ArgumentOutOfRangeException(nameof(speed));
         }
         if (speed == 0)
         {
            return StationaryValue;
         }
         else if (speed > 0)
         {
            return (int)Math.Round(MapFloat(speed, 0, 100, StationaryValue, MaxForward));
         }
         else
         {
            return (int)Math.Round(MapFloat(speed, 0, -100, StationaryValue, MaxBackwards));
         }
      }

      private static float MapFloat(float value, float inMin, float inMax, float outMin, float outMax)
      {
         return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
      }
   }
}
