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
            this.RobotIndex = myRobotIndex;
            this.Stations = stations;
            this.Robots = robots;
        }

        private bool IsEnoughToCollect()
        {
            Position position = Robots[RobotIndex].Position;
            return new Cell(RobotIndex, Robots[RobotIndex].Position, Stations, Robots).EnergyToBeCollected() > MinRobotMustCollect;
        }

        public RobotCommand Try() => new CollectEnergyCommand();
    }
}