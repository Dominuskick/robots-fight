using Robot.Common;
using System.Collections.Generic;

namespace OleksandrKalenikov.RobotChallange.Command
{
    public class Cell
    {
        private const string OwnerName = "Oleksandr Kalenikov";
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
                        else if (robots[index3].Position == position && movingRobotIndex < index3 && robots[index3].OwnerName == OwnerName)
                            ++RobotCount;
                    }
                }
            }
        }

        public int GetRobotCount() => RobotCount;

        public int EnergyCanBeCollected()
        {
            int num1 = 0;
            int num2 = RobotCount * EnergyPerStation;
            foreach (EnergyStation station in Stations)
            {
                int num3 = station.RecoveryRate - num2;
                if (num3 >= EnergyPerStation)
                    num1 += EnergyPerStation;
                else if (num3 > 0)
                    num1 += num3;
            }
            return num1;
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
