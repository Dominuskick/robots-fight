using Robot.Common;
using System.Collections.Generic;

namespace OleksandrKalenikov.RobotChallange.Command
{
    public class Cell
    {
        private const int CollectLength = 2;
        private const int EnergyPerStation = 40;
        private IList<EnergyStation> Stations;
        private int RobotCount;

        public Cell(
          int movingRobotIndex,
          Position center,
          IList<EnergyStation> stations,
          IList<Robot.Common.Robot> robots)
        {
            Stations = new List<EnergyStation>();
            RobotCount = 0;
            for (int index1 = -CollectLength; index1 <= CollectLength; ++index1)
            {
                for (int index2 = -CollectLength; index2 <= CollectLength; ++index2)
                {
                    Position position = new Position(center.X + index1, center.Y + index2);
                    foreach (EnergyStation station in stations)
                    {
                        if (station.Position == position)
                            Stations.Add(station);
                    }
                    for (int index3 = 0; index3 < robots.Count; ++index3)
                    {
                        if (robots[index3].Position == position && movingRobotIndex > index3)
                            ++RobotCount;
                        else if (robots[index3].Position == position && movingRobotIndex < index3)
                            ++RobotCount;
                    }
                }
            }
        }

        public int GetRobotCount() => RobotCount;

        public int EnergyCanBeCollected()
        {
            int canCollect = 0;
            int allEnergyByRobots = RobotCount * EnergyPerStation;
            foreach (EnergyStation station in Stations)
            {
                int num3 = station.RecoveryRate - allEnergyByRobots;
                if (num3 >= EnergyPerStation)
                    canCollect += EnergyPerStation;
                else if (num3 > 0)
                    canCollect += num3;
            }
            return canCollect;
        }

        public int EnergyToBeCollected()
        {
            int beCollected = 0;
            foreach (EnergyStation station in Stations)
            {
                if (station.Energy >= EnergyPerStation)
                    beCollected += EnergyPerStation;
                else
                    beCollected += station.Energy;
            }
            return beCollected;
        }
    }
}
