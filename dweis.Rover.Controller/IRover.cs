using System;
using System.Collections.Generic;
using System.Text;

namespace dweis.Rover.Controller
{
   interface IRover
   {
      void SetLegs(int angle);
      void SetSpeed(int speed);
      void SendIndexConfiguration();
      void RequestIndexConfiguration();
   }
}
