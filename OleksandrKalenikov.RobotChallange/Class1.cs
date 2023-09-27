using Robot.Common;
using System.Collections.Generic;
using System;
using OleksandrKalenikov.RobotChallange.Command;

namespace OleksandrKalenikov.RobotChallange
{
    public class OleksandrKalenikovAlgorithm : IRobotAlgorithm
    {
        public OleksandrKalenikovAlgorithm()
        {
            RobotCount = 10;
        }

        public int RoundCount { get; set; }

        public int RobotCount { get; set; }

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            RobotCommand robotCommand = new NewRobotCreating(robotToMoveIndex, RoundCount, RobotCount, robots).Try();
            if (robotCommand == null)
                return new RobotMoving(robotToMoveIndex, robots, map.Stations, RoundCount).Try() ?? new EnergyCollecting(robotToMoveIndex, map.Stations, robots).Try();
            ++RobotCount;
            return robotCommand;
        }

        public string Author => "Oleksandr Kalenikov";

        public class Helper
        {
            public static bool IsCellFree(Position cell, IList<Robot.Common.Robot> robots, int currentRobotIndex)
            {
                foreach (Robot.Common.Robot robot in robots)
                {
                    if ((robot.Position == cell) && robot != robots[currentRobotIndex])
                        return false;
                }
                return true;
            }

            public static int FindDistance(Position a, Position b) => (int)(Math.Pow((a.X - b.X), 2.0) + Math.Pow((a.Y - b.Y), 2.0));

            public static bool IsValid(Position position) => position.X < 100 && position.X >= 0 && position.Y < 100 && position.Y >= 0;
        }

    }
}