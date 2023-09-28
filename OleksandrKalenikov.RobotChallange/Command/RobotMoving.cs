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
            Stations = stations;
            RobotIndex = movingRobotIndex;
            Robots = robots;
            RoundCount = roundCount;
        }

        private bool IsProfitable() => RoundCount < MaxRound;

        public Position FindOptimal()
        {
            int energy = Robots[RobotIndex].Energy;
            Position position1 = Robots[RobotIndex].Position;
            int maxWay = (int)Math.Sqrt(energy);
            int radius = maxWay < MaxCellDistance ? maxWay : MaxCellDistance - 1;
            int maximumEnergyCanCollect = int.MinValue;
            Position a = null;

            for (int i = -radius; i <= radius; ++i)
            {
                for (int j = -radius; j <= radius; ++j)
                {
                    Position position2 = new Position(position1.X + i, position1.Y + j);
                    int distance = Helper.FindDistance(position2, position1);
                    if (Helper.IsValid(position2) && Helper.IsCellFree(position2, Robots, RobotIndex) && energy >= distance + KeepedEnergy)
                    {
                        int energyCollectInCell = new Cell(RobotIndex, position2, Stations, Robots).EnergyCanBeCollected();
                        if (maximumEnergyCanCollect < energyCollectInCell)
                        {
                            maximumEnergyCanCollect = energyCollectInCell;
                            a = position2;
                        }
                        else if (maximumEnergyCanCollect == energyCollectInCell && Helper.FindDistance(a, position1) > Helper.FindDistance(position2, position1))
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