using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.DataAccessTest.GamePlay
{
    using System.Data.Entity;

    using Billapong.DataAccess;
    using Billapong.DataAccess.Initialize;
    using Billapong.Tests.Common;

    using DataAccess.Model.Map;
    using DataAccess.Repository;
    using DataAccess.UnitOfWork;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HighScoreTest : TestBase
    {
        [TestMethod]
        public void AddHighScoreTest()
        {
            // arrange
            var unitOfWork = new UnitOfWork();
            var count = unitOfWork.HighScoreRepository.Get().Count();

            var highscore = new HighScore()
            {
                Map = unitOfWork.MapRepository.GetById(1),
                PlayerName = "Test",
                Score = 1234,
                Timestamp = DateTime.Now
            };

            // act
            unitOfWork.HighScoreRepository.Add(highscore);
            unitOfWork.HighScoreRepository.Save();
            var newCount = unitOfWork.HighScoreRepository.Get().Count();

            // assert
            Assert.IsTrue(count + 1 == newCount, "Highscore was not saved correctly.");
        }

        [TestMethod]
        public void GetMapInstance()
        {
            // arrange
            var unitOfWork = new UnitOfWork();

            var highscore = new HighScore
            {
                Map = unitOfWork.MapRepository.GetById(1),
                PlayerName = "Test",
                Score = 1234,
                Timestamp = DateTime.Now
            };

            // act
            unitOfWork.HighScoreRepository.Add(highscore);
            unitOfWork.HighScoreRepository.Save();
            var savedHighscore = unitOfWork.HighScoreRepository.Get().ToList().Last();

            // assert
            Assert.IsNotNull(savedHighscore.Map, "Map could not be loaded");
            Assert.AreEqual(1, savedHighscore.Map.Id, "Map could not be loaded correctly.");
        }
    }
}
