using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Robot.Common;
using OleksandrKalenikov.RobotChallenge;
using OleksandrKalenikov.RobotChallange.Command;
using OleksandrKalenikov.RobotChallange;
using Moq;

namespace OleksandrKalenikov.RobotChallenge.Test
{

    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Try_MoveCommandReturned()
        {

            int movingRobotIndex = 0;
            var stations = new List<EnergyStation>
            {
                new EnergyStation { Position = new Position(5, 5), RecoveryRate = 40, Energy = 1000 },
                new EnergyStation { Position = new Position(6, 6), RecoveryRate = 40, Energy = 200 },
            };

            var robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(1, 1), Energy = 100 },
                new Robot.Common.Robot { Position = new Position(2, 2), Energy = 100 },
            };
            int roundCount = 20;

            var robotMoving = new RobotMoving(movingRobotIndex, robots, stations, roundCount);

            var result = robotMoving.Try();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MoveCommand));
        }



        [TestMethod]
        public void GetRobotCount_NoRobots_ReturnsZero()
        {
            // Arrange
            var stations = new List<EnergyStation>();
            var robots = new List<Robot.Common.Robot>();
            var center = new Position(0, 0);
            int movingRobotIndex = 0;

            // Create an instance of Cell
            var cell = new Cell(movingRobotIndex, center, stations, robots);

            // Act
            int robotCount = cell.GetRobotCount();

            // Assert
            Assert.AreEqual(0, robotCount);
        }

        [TestMethod]
        public void EnergyCanBeCollected_CalculateEnergy_ReturnsCorrectValue()
        {
            // Arrange
            var stations = new List<EnergyStation>
            {
                new EnergyStation { Position = new Position(1, 1), RecoveryRate = 40, Energy = 100 },
                new EnergyStation { Position = new Position(2, 2), RecoveryRate = 40, Energy = 80 },
            };

            var robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(4, 4), Energy = 100 },
                new Robot.Common.Robot { Position = new Position(5, 5), Energy = 100 },
            };

            var center = new Position(1, 1);
            int movingRobotIndex = 0;

            // Create an instance of Cell
            var cell = new Cell(movingRobotIndex, center, stations, robots);

            // Act
            int energyCanCollect = cell.EnergyCanBeCollected();

            // Assert
            Assert.AreEqual(80, energyCanCollect);
        }

        [TestMethod]
        public void EnergyToBeCollected_CalculateEnergy_ReturnsCorrectValue()
        {
            // Arrange
            var stations = new List<EnergyStation>
            {
                new EnergyStation { Position = new Position(1, 1), Energy = 100 },
                new EnergyStation { Position = new Position(2, 2), Energy = 80 },
            };

            var robots = new List<Robot.Common.Robot>();

            var center = new Position(0, 0);
            int movingRobotIndex = 0;

            // Create an instance of Cell
            var cell = new Cell(movingRobotIndex, center, stations, robots);

            // Act
            int energyToCollect = cell.EnergyToBeCollected();

            // Assert
            Assert.AreEqual(80, energyToCollect); // Adjust this value based on your expectations
        }

        [TestMethod]
        public void EnergyToBeCollected_EmptyStation_ReturnsZero()
        {
            var stations = new List<EnergyStation>();
            var robots = new List<Robot.Common.Robot>();
            var center = new Position(0, 0);
            int movingRobotIndex = 0;

            var cell = new Cell(movingRobotIndex, center, stations, robots);

            int energyToCollect = cell.EnergyToBeCollected();

            Assert.AreEqual(0, energyToCollect);
        }

        [TestMethod]
        public void DoStep_WithNewRobotCreating_ReturnsCreateNewRobotCommand()
        {
            var algorithm = new OleksandrKalenikovAlgorithm();
            var robots = new List<Robot.Common.Robot>();
            var stations = new List<EnergyStation>
            {
                new EnergyStation { Position = new Position(1, 1), Energy = 100 },
                new EnergyStation { Position = new Position(2, 2), Energy = 80 },
            };
            var map = new Map();
            map.Stations = stations;
            int robotToMoveIndex = 0;

            robots.Add(new Robot.Common.Robot()
            {
                Energy = 400,
                Position = new Position(0, 0)
            });

            var result = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOfType(result, typeof(CreateNewRobotCommand));
        }

        [TestMethod]
        public void RobotMoving_Try_Returns_MoveCommand_When_OptimalPositionIsFound()
        {
            // Arrange
            var robotIndex = 0;
            var roundCount = 10;
            var energy = 100;
            var position1 = new Position(1, 1);

            var robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot
                {
                    Energy = energy,
                    Position = position1
                }
            };

            var stations = new List<EnergyStation>
            {
                new EnergyStation
                {
                    Position = new Position(6, 6),
                    Energy = 50,
                    RecoveryRate = 10
                },
                new EnergyStation
                {
                    Position = new Position(4, 4),
                    Energy = 30,
                    RecoveryRate = 5
                }
            };

            var robotMoving = new RobotMoving(robotIndex, robots, stations, roundCount);

            // Act
            var result = robotMoving.Try();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MoveCommand));
        }

        [TestMethod]
        public void RobotMoving_Try_Returns_Null_When_NoOptimalPositionIsFound()
        {
            // Arrange
            var robotIndex = 0;
            var roundCount = 10;
            var energy = 100;
            var position1 = new Position(5, 5);

            var robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot
                {
                    Energy = energy,
                    Position = position1
                }
            };

            var stations = new List<EnergyStation>
            {
                new EnergyStation
                {
                    Position = new Position(7, 7),
                    Energy = 50,
                    RecoveryRate = 10
                }
            };

            var robotMoving = new RobotMoving(robotIndex, robots, stations, roundCount);

            // Act
            var result = robotMoving.Try();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RobotMoving_Try_Returns_Null_When_NotProfitable()
        {
            // Arrange
            var robotIndex = 0;
            var roundCount = 51;
            var energy = 100;
            var position1 = new Position(5, 5);

            var robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot
                {
                    Energy = energy,
                    Position = position1
                }
            };

            var stations = new List<EnergyStation>
            {
                new EnergyStation
                {
                    Position = new Position(6, 6),
                    Energy = 50,
                    RecoveryRate = 10
                }
            };

            var robotMoving = new RobotMoving(robotIndex, robots, stations, roundCount);

            // Act
            var result = robotMoving.Try();

            // Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void DoStep_WithEnergyCollecting_ReturnsCollectEnergyCommand()
        {
            var algorithm = new OleksandrKalenikovAlgorithm();
            var robots = new List<Robot.Common.Robot>();
            var stations = new List<EnergyStation>
            {
                new EnergyStation { Position = new Position(1, 1), Energy = 100 },
                new EnergyStation { Position = new Position(2, 2), Energy = 80 },
            };
            var map = new Map();
            map.Stations = stations;
            int robotToMoveIndex = 0;

            robots.Add(new Robot.Common.Robot()
            {
                Energy = 50,
                Position = new Position(0, 0)
            });

            var result = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOfType(result, typeof(CollectEnergyCommand));
        }
    }
}
