using Robot.Common;
using System.Collections.Generic;

namespace OleksandrKalenikov.RobotChallange.Command
{
    public class NewRobotCreating
    {
        private const int StopCreatingRound = 40;
        private const int MinProfit = 10;
        private const int EnergyForCreate = 300;
        private const int MaxRobotCount = 100;
        private Robot.Common.Robot FatherRobot;
        private int Round;
        private int RobotCount;
        private IList<Robot.Common.Robot> Robots;

        public NewRobotCreating(int fatherRobotIndex, int round, int robotCount, IList<Robot.Common.Robot> robots)
        {
            this.FatherRobot = robots[fatherRobotIndex];
            this.Round = round;
            this.RobotCount = robotCount;
            this.Robots = robots;
        }

        private bool IsPossible() => FatherRobot.Energy > EnergyForCreate && RobotCount < MaxRobotCount;

        private bool IsProfitable() => Round < StopCreatingRound;

        public RobotCommand Try() => IsPossible() && IsProfitable() ? new CreateNewRobotCommand() : null;
    }
}