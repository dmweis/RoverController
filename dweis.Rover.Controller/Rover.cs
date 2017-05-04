using System;
using System.Reflection;
using Newtonsoft.Json;

namespace dweis.Rover.Controller
{
   public class Rover
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

      public void SetLegs(int leftFront, int rightFront, int leftRear, int rightRear)
      {
         int frontLeftPulse = MapAngle(leftFront, FrontLeft.Servo.Angle1, FrontLeft.Servo.Angle2, FrontLeft.Servo.Pwm1, FrontLeft.Servo.Pwm2);
         int frontRightPulse = MapAngle(rightFront, FrontRight.Servo.Angle1, FrontRight.Servo.Angle2, FrontRight.Servo.Pwm1, FrontRight.Servo.Pwm2);
         int rearLeftPulse = MapAngle(leftRear, RearLeft.Servo.Angle1, RearLeft.Servo.Angle2, RearLeft.Servo.Pwm1, RearLeft.Servo.Pwm2);
         int rearRightPulse = MapAngle(rightRear, RearRight.Servo.Angle1, RearRight.Servo.Angle2, RearRight.Servo.Pwm1, RearRight.Servo.Pwm2);
         NewMessage?.Invoke(this, $"{{2 {frontLeftPulse} {frontRightPulse} {rearLeftPulse} {rearRightPulse}}}");
      }

      public void SetSpeed(int leftFront, int rightFront, int leftRear, int rightRear)
      {
         const int max = 100;
         const int min = -max;
         if (leftFront < min || leftFront > max)
         {
            throw new ArgumentOutOfRangeException(nameof(leftFront));
         }
         if (rightFront < min || rightFront > max)
         {
            throw new ArgumentOutOfRangeException(nameof(rightFront));
         }
         if (leftRear < min || leftRear > max)
         {
            throw new ArgumentOutOfRangeException(nameof(leftRear));
         }
         if (rightRear < min || rightRear > max)
         {
            throw new ArgumentOutOfRangeException(nameof(rightRear));
         }
         int frontLeftPulse = leftFront > 0 ? MapAngle(leftFront, 0, 100, FrontLeft.Motor.StationaryValue, FrontLeft.Motor.MaxForward)
            : MapAngle(leftFront, 0, -100, FrontLeft.Motor.StationaryValue, FrontLeft.Motor.MaxBackwards);
         int frontRightPulse = rightFront > 0 ? MapAngle(rightFront, 0, 100, FrontRight.Motor.StationaryValue, FrontRight.Motor.MaxForward)
            : MapAngle(rightFront, 0, -100, FrontRight.Motor.StationaryValue, FrontRight.Motor.MaxBackwards);
         int rearLeftPulse = leftRear > 0 ? MapAngle(leftRear, 0, 100, RearLeft.Motor.StationaryValue, RearLeft.Motor.MaxForward)
            : MapAngle(leftRear, 0, -100, RearLeft.Motor.StationaryValue, RearLeft.Motor.MaxBackwards);
         int rearRightPulse = rightRear > 0 ? MapAngle(rightRear, 0, 100, RearRight.Motor.StationaryValue, RearRight.Motor.MaxForward)
            : MapAngle(rightRear, 0, -100, RearRight.Motor.StationaryValue, RearRight.Motor.MaxBackwards);
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

      private static int MapAngle(float value, float inMin, float inMax, float outMin, float outMax)
      {
         return (int)Math.Round((value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin);
      }

      //private static int Mod(int a, int b)
      //{
      //   return ((a % b) + b) % b;
      //}

      #region simple commands
      public void TurnCrossLegs(int angle = 50)
      {
         SetLegs(angle, angle, angle, angle);
      }

      public void TurnServosStraight()
      {
         const int value = 0;
         SetLegs(value, value, value, value);
      }

      public void TurnServosParallel()
      {
         const int value = 90;
         SetLegs(value, value, value, value);
      }

      public void TurnSlightlyLeft(int angle = 20)
      {
         SetLegs(-angle, angle, -angle, angle);
      }

      public void TurnSlightlyRight(int angle = 20)
      {
         SetLegs(angle, -angle, angle, -angle);
      }

      public void FullForward(int speed = 100)
      {
         SetSpeed(speed, speed, speed, speed);
      }

      public void FullBackwards(int speed = 100)
      {
         SetSpeed(-speed, -speed, -speed, -speed);
      }

      public void StopMotors()
      {
         const int value = 0;
         SetSpeed(value, value, value, value);
      }

      public void ParallelLeft(int speed = 100)
      {
         TurnServosParallel();
         SetSpeed(-speed, speed, speed, -speed);
      }

      public void ParallelRight(int speed = 100)
      {
         TurnServosParallel();
         SetSpeed(speed, -speed, -speed, speed);
      }

      public void RotateClockwise(int speed = 100)
      {
         TurnCrossLegs();
         SetSpeed(speed, -speed, speed, -speed);
      }

      public void RotateCounterClockwise(int speed = 100)
      {
         TurnCrossLegs();
         SetSpeed(-speed, speed, -speed, speed);
      }
      #endregion
   }
}
