using Robot.Common;
using System;
using System.Collections.Generic;
using static OleksandrKalenikov.RobotChallange.OleksandrKalenikovAlgorithm;

namespace OleksandrKalenikov.RobotChallange.Command
{
    public class RobotMoving
    {
        private const int MaxRound = 50;
        private const int MaxCellDistance = 5;
        private const int KeepedEnergy = 25;
        private int RobotIndex;
        private int RoundCount;
        private IList<EnergyStation> Stations;
        private IList<Robot.Common.Robot> Robots;

        public RobotMoving(
          int movingRobotIndex,
          IList<Robot.Common.Robot> robots,
          IList<EnergyStation> stations,
          int roundCount)
        {
            this.Stations = stations;
            this.RobotIndex = movingRobotIndex;
            this.Robots = robots;
            this.RoundCount = roundCount;
        }

        private bool IsProfitable() => RoundCount < MaxRound;

        public Position FindOptimal()
        {
            int energy = Robots[RobotIndex].Energy;
            Position position1 = Robots[RobotIndex].Position;
            int num1 = (int)Math.Sqrt(energy);
            int num2 = num1 < MaxCellDistance ? num1 : MaxCellDistance - 1;
            int num3 = int.MinValue;
            Position a = null;
            for (int index1 = -num2; index1 <= num2; ++index1)
            {
                for (int index2 = -num2; index2 <= num2; ++index2)
                {
                    Position position2 = new Position(position1.X + index1, position1.Y + index2);
                    int distance = Helper.FindDistance(position2, position1);
                    if (Helper.IsValid(position2) && Helper.IsCellFree(position2, Robots, RobotIndex) && energy >= distance + KeepedEnergy)
                    {
                        int num4 = new Cell(RobotIndex, position2, Stations, Robots).EnergyCanBeCollected();
                        if (num3 < num4)
                        {
                            num3 = num4;
                            a = position2;
                        }
                        else if (num3 == num4 && Helper.FindDistance(a, position1) > Helper.FindDistance(position2, position1))
                            a = position2;
                    }
                }
            }
            return (Robots[RobotIndex].Position == a) ? null : a;
        }

        public RobotCommand Try()
        {
            if (!IsProfitable())
                return null;
            Position optimal = FindOptimal();
            if (optimal == null)
                return null;
            return new MoveCommand()
            {
                NewPosition = optimal
            };
        }
    }
}