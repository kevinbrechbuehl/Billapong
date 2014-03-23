namespace Billapong.DataAccessTest.GamePlay
{
    using System;
    using System.Linq;
    using Billapong.Tests.Common;
    using DataAccess.Model.Map;
    using DataAccess.UnitOfWork;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test Highscores
    /// </summary>
    [TestClass]
    public class HighScoreTest : TestBase
    {
        /// <summary>
        /// Test adding a highscore to the database.
        /// </summary>
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

        /// <summary>
        /// Test to get the map out of a highscore.
        /// </summary>
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
