using OleksandrKalenikov.RobotChallange.Command;
using Robot.Common;
using System.Collections.Generic;

namespace OleksandrKalenikov.RobotChallange.Command
{
    public class EnergyCollecting
    {
        private const int MinRobotMustCollect = 0;
        private int RobotIndex;
        private IList<Robot.Common.Robot> Robots;
        private IList<EnergyStation> Stations;

        public EnergyCollecting(int myRobotIndex, IList<EnergyStation> stations, IList<Robot.Common.Robot> robots)
        {
            RobotIndex = myRobotIndex;
            Stations = stations;
            Robots = robots;
        }

        public RobotCommand Try() => new CollectEnergyCommand();
    }
}