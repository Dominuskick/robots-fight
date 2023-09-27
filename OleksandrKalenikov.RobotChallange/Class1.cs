using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace OleksandrKalenikov.RobotChallenge
{
    public class OleksandKalenikovAlgorithm : IRobotAlgorithm
    {
        public static HashSet<int> myRobots = new HashSet<int>();
        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot currentRobot = robots[robotToMoveIndex];
         
            myRobots.Add(robotToMoveIndex);
            Console.WriteLine(myRobots.Count);

            if (currentRobot.Energy > 400 && myRobots.Count < 100)
            {
                return new CreateNewRobotCommand();
            }

            if (IsCollectedEnergy(FindNearestFreeStation(currentRobot, map, robots).Position, currentRobot.Position))
            {
                return new CollectEnergyCommand();
            }

            List<Robot.Common.EnergyStation> sortedStationList = new List<EnergyStation>();

            foreach (Robot.Common.EnergyStation station in map.Stations)
            {
                if (IsStationFree(station, currentRobot, robots))
                {
                    sortedStationList.Add(station);
                }
            }

            sortedStationList.Sort((x, y) =>
                DistanceCounter.FindDistance(currentRobot.Position, x.Position)
                    .CompareTo(DistanceCounter.FindDistance(currentRobot.Position, y.Position)));

            for (int i = 0; i < sortedStationList.Count; i++)
            {
                if (DistanceCounter.FindDistance(currentRobot.Position, sortedStationList[i].Position) < currentRobot.Energy)
                {
                    return new MoveCommand() { NewPosition = sortedStationList[i].Position };
                }
                else
                {
                    return new MoveCommand()
                    {
                        NewPosition =
                            new Position(
                                dividedDistanceX(currentRobot.Position, sortedStationList[i].Position, currentRobot.Energy),
                                dividedDistanceY(currentRobot.Position, sortedStationList[i].Position, currentRobot.Energy))
                    };
                }
            }

            return null;
        }

        public string Author => "Oleksandr Kalenikov3";

        public static int dividedDistanceX(Position robotPosition, Position stationPosition, int energy)
        {
            int numOfSteps = (int)Math.Ceiling((decimal)DistanceCounter.FindDistance(robotPosition, stationPosition) / energy);
            int dividedX = (stationPosition.X - robotPosition.X) / numOfSteps;
            return robotPosition.X + dividedX;
        }

        public static int dividedDistanceY(Position robotPosition, Position stationPosition, int energy)
        {
            int numOfSteps = (int)Math.Ceiling((decimal)DistanceCounter.FindDistance(robotPosition, stationPosition) / energy);
            int dividedY = (stationPosition.Y - robotPosition.Y) / numOfSteps;
            return robotPosition.Y + dividedY;

        }
        public EnergyStation FindNearestFreeStation(Robot.Common.Robot movingRobot, Map map,
            IList<Robot.Common.Robot> robots)
        {
            EnergyStation nearest = null;
            int minDistance = int.MaxValue;
            foreach (var station in map.Stations)
            {
                if (IsStationFree(station, movingRobot, robots))
                {
                    int d = DistanceCounter.FindDistance(station.Position, movingRobot.Position);
                    if (d < minDistance)

                    {

                        minDistance = d;
                        nearest = station;
                    }
                }
            }
            return nearest == null ? null : nearest;
        }

        public bool IsCollectedEnergy(Position station, Position bot)
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (station.X + i == bot.X && station.Y + j == bot.Y) return true;
                }
            }
            
            return false;
        }
        public bool IsStationFree(EnergyStation station, Robot.Common.Robot movingRobot,
            IList<Robot.Common.Robot> robots)
        {
            return IsCellsFree(station.Position, movingRobot, robots);
        }
        public bool IsCellsFree(Position cell, Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            foreach (var robot in robots)
            {
                if (robot != movingRobot)
                {
                    if (IsCollectedEnergy(robot.Position, cell))
                        return false;
                }
            }
            return true;
        }
    }
    class DistanceCounter
        {
            public static int FindDistance(Position a, Position b)
            {
                return (int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));

            }
        }

}

