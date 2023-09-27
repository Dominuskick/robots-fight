using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Robot.Common;
using OleksandrKalenikov.RobotChallenge;

namespace OleksandrKalenikov.RobotChallenge.Test
{
    [TestClass]
    public class SortedListTest
    {
        [TestMethod]
        public void TestDividedDistanceXPositive()
        {
            Assert.AreEqual(10, OleksandKalenikovAlgorithm.dividedDistanceX(new Position(4, 5), new Position(10, 15), 200));
        }

        [TestMethod]
        public void TestDivideDistanceYPositive()
        {
            Assert.AreEqual(15, OleksandKalenikovAlgorithm.dividedDistanceY(new Position(4, 5), new Position(10, 15), 200));
        }

        [TestMethod]
        public void TestDividedDistanceXNegative()
        {
            Assert.AreNotEqual(109, OleksandKalenikovAlgorithm.dividedDistanceX(new Position(5, 5), new Position(15, 15), 100));
        }

        [TestMethod]
        public void TestDivideDistanceYNegative()
        {
            Assert.AreNotEqual(25, OleksandKalenikovAlgorithm.dividedDistanceX(new Position(5, 5), new Position(15, 15), 100));
        }

        [TestMethod]
        public void TestIsStationFree()
        {
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map() { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            IList<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();

            EnergyStation someStation = new EnergyStation() { Energy = 1000, Position = new Position(30, 30), RecoveryRate = 100 };

            Robot.Common.Robot someRobot = new Robot.Common.Robot() { Energy = 100, Position = new Position(25, 25) };

            mainRobotList.Add(someRobot);

            Assert.IsTrue(mainAlgorithm.IsStationFree(someStation, someRobot, mainRobotList));
        }

        [TestMethod]
        public void TestFindNearestFreeStation()
        {
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map() { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            List<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();

            Robot.Common.EnergyStation energyStation1 = new EnergyStation()
            { Energy = 0, Position = new Position(50, 50), RecoveryRate = 100 };
            Robot.Common.EnergyStation energyStation2 = new EnergyStation()
            { Energy = 0, Position = new Position(8, 8), RecoveryRate = 100 };

            mainMap.Stations.Add(energyStation1);
            mainMap.Stations.Add(energyStation2);

            Robot.Common.Robot currentRobot = new Robot.Common.Robot() { Energy = 100, Position = new Position(20, 20) };

            mainRobotList.Add(currentRobot);

            Assert.AreEqual(energyStation2, mainAlgorithm.FindNearestFreeStation(currentRobot, mainMap, mainRobotList));
        }
        [TestMethod]
        public void TestDoStepCreateNewRobot()
        {
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map() { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            mainMap.Stations.Add(new Robot.Common.EnergyStation() { Energy = 1000, Position = new Position(20, 20), RecoveryRate = 50 });
            mainMap.Stations.Add(new Robot.Common.EnergyStation() { Energy = 1000, Position = new Position(40, 40), RecoveryRate = 50 });

            List<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();

            Robot.Common.Robot currentRobot = new Robot.Common.Robot() { Energy = 500, Position = new Position(20, 20) };

            mainRobotList.Add(currentRobot);

            Assert.IsInstanceOfType(mainAlgorithm.DoStep(mainRobotList, 0, mainMap), typeof(CreateNewRobotCommand));
        }

        [TestMethod]
        public void TestDoStepCreateNewRobotNegative()
        {
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map() { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            mainMap.Stations.Add(new Robot.Common.EnergyStation() { Energy = 1000, Position = new Position(20, 20), RecoveryRate = 50 });
            mainMap.Stations.Add(new Robot.Common.EnergyStation() { Energy = 1000, Position = new Position(40, 40), RecoveryRate = 50 });

            List<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();

            Robot.Common.Robot currentRobot = new Robot.Common.Robot() { Energy = 500, Position = new Position(20, 20) };

            mainRobotList.Add(currentRobot);

            Assert.IsNotInstanceOfType(mainAlgorithm.DoStep(mainRobotList, 0, mainMap), typeof(MoveCommand));
        }

        [TestMethod]
        public void TestDoStepCollectEnergy()
        {
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map()
            { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            mainMap.Stations.Add(new Robot.Common.EnergyStation()
            { Energy = 1000, Position = new Position(20, 20), RecoveryRate = 50 });
            mainMap.Stations.Add(new Robot.Common.EnergyStation()
            { Energy = 1000, Position = new Position(40, 40), RecoveryRate = 50 });

            List<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();

            Robot.Common.Robot currentRobot = new Robot.Common.Robot() { Energy = 100, Position = new Position(20, 20) };

            mainRobotList.Add(currentRobot);

            Assert.IsInstanceOfType(mainAlgorithm.DoStep(mainRobotList, 0, mainMap), typeof(CollectEnergyCommand));
        }

        [TestMethod]
        public void TestDoStepMove()
        {
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map()
            { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            mainMap.Stations.Add(new Robot.Common.EnergyStation()
            { Energy = 1000, Position = new Position(20, 20), RecoveryRate = 50 });
            mainMap.Stations.Add(new Robot.Common.EnergyStation()
            { Energy = 1000, Position = new Position(40, 40), RecoveryRate = 50 });

            List<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();

            Robot.Common.Robot currentRobot = new Robot.Common.Robot() { Energy = 100, Position = new Position(15, 15) };

            mainRobotList.Add(currentRobot);

            Assert.IsInstanceOfType(mainAlgorithm.DoStep(mainRobotList, 0, mainMap), typeof(MoveCommand));
        }

        [TestMethod]
        public void TestDoStepMove2()
        {
            const int count = 5;
            OleksandKalenikovAlgorithm mainAlgorithm = new OleksandKalenikovAlgorithm();

            Robot.Common.Map mainMap = new Robot.Common.Map()
            { MaxPozition = new Position(100, 100), MinPozition = new Position(0, 0) };

            mainMap.Stations.Add(new Robot.Common.EnergyStation()
            { Energy = 1000, Position = new Position(20, 20), RecoveryRate = 50 });
            mainMap.Stations.Add(new Robot.Common.EnergyStation()
            { Energy = 1000, Position = new Position(40, 40), RecoveryRate = 50 });

            List<Robot.Common.Robot> mainRobotList = new List<Robot.Common.Robot>();
            
            for(int i = 0; i < count; i++)
            {
                Robot.Common.Robot currentRobot1 = new Robot.Common.Robot() { Energy = 100, Position = new Position(15 + i, 15 + i) };
                mainRobotList.Add(currentRobot1);
            }
            OleksandKalenikovAlgorithm.myRobots.Clear();
            for(int i = 0; i < count; i++)
            {
                mainAlgorithm.DoStep(mainRobotList, i, mainMap);
            }


            Assert.AreEqual(count, OleksandKalenikovAlgorithm.myRobots.Count);
        }
    }
}
