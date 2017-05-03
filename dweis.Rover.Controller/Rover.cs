using System;
using Newtonsoft.Json;

namespace dweis.Rover.Controller
{
   public class Rover : IRover
   {
      public event EventHandler<string> NewMessage;

      public RoverLeg FrontLeft { get; }
      public RoverLeg FrontRight { get; }
      public RoverLeg RearLeft { get; }
      public RoverLeg RearRight { get; }

      public Rover(RoverLeg frontLeft, RoverLeg frontRight, RoverLeg rearLeft, RoverLeg rearRight)
      {
         FrontLeft = frontLeft;
         FrontRight = frontRight;
         RearLeft = rearLeft;
         RearRight = rearRight;
      }

      public Rover()
      {
         FrontLeft = new RoverLeg();
         FrontRight = new RoverLeg();
         RearLeft = new RoverLeg();
         RearRight = new RoverLeg();
      }

      public static Rover FromJson(string json)
      {
         return JsonConvert.DeserializeObject<Rover>(json);
      }

      public string ToJson()
      {
         return JsonConvert.SerializeObject(this);
      }

      public void SetLegs(int angle)
      {
         int frontLeftPulse;
         int frontRightPulse;
         int rearLeftPulse;
         int rearRightPulse;
         NewMessage?.Invoke(this, $"{{2 {frontLeftPulse} {frontRightPulse} {rearLeftPulse} {rearRightPulse}}}");
      }

      public void SetSpeed(int speed)
      {
         int frontLeftPulse;
         int frontRightPulse;
         int rearLeftPulse;
         int rearRightPulse;
         NewMessage?.Invoke(this, $"{{1 {frontLeftPulse} {frontRightPulse} {rearLeftPulse} {rearRightPulse}}}");
      }

      public void SendIndexConfiguration()
      {
         string data =
            $"{{4 {FrontLeft.Motor.Index} {FrontRight.Motor.Index} {RearLeft.Motor.Index} {RearRight.Motor.Index}" +
            $"{FrontLeft.Servo.Index} {FrontRight.Servo.Index} {RearLeft.Servo.Index} {RearRight.Servo.Index}}}";
         NewMessage?.Invoke(this, data);
      }

      public void RequestIndexConfiguration()
      {
         string data = "{4}";
         NewMessage?.Invoke(this, data);
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
