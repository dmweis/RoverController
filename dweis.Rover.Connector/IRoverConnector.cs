using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dweis.Rover.Connector
{
   public interface IRoverConnector
   {
      void Close();
      void TurnCrossLegs();
      void TurnServos90();
      void TurnServos180();
      void TurnSlightlyLeft();
      void TurnSlightlyRight();
      void FullForward();
      void FullBackwards();
      void StopMotors();
      void ParallelLeft();
      void ParallelRight();
      void RotateClockwise();
      void RotateCounterClockwise();
   }
}
