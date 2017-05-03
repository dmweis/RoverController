using System;

namespace dweis.Rover.Controller
{
   public class RoverServo
   {
      public int Index { get; set; }
      public int Angle1 { get; set; }
      public int Pwm1 { get; set; }
      public int Angle2 { get; set; }
      public int Pwm2 { get; set; }

      public int CalculateAngle(int angle)
      {
         int pulse = (int)Math.Round(MapFloat(angle, Angle1, Angle2, Pwm1, Pwm2));
         return pulse;
      }

      private static float MapFloat(float value, float inMin, float inMax, float outMin, float outMax)
      {
         return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
      }

      private static int Mod(int a, int b)
      {
         return ((a % b) + b) % b;
      }
   }
}
