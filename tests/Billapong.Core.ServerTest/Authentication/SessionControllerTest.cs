namespace Billapong.Core.ServerTest.Authentication
{
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Server.Authentication;
    using Billapong.Tests.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the session/authentication controller.
    /// </summary>
    [TestClass]
    public class SessionControllerTest : TestBase
    {
        /// <summary>
        /// Test if verifying a valid session with login and logout works.
        /// </summary>
        [TestMethod]
        public void VerifySessionTest()
        {
            // act
            var sessionId = SessionController.Current.Login("editor", "editor", Role.Editor);

            // assert
            Assert.IsTrue(SessionController.Current.IsValidSession(sessionId, Role.Editor), "Session id is not stored in valid sessions");

            // logout
            SessionController.Current.Logout(sessionId);

            // re-assert
            Assert.IsFalse(SessionController.Current.IsValidSession(sessionId, Role.Editor), "Session id was not correctly removed while logout");
        }

        /// <summary>
        /// Tests if the role check is done correct while checking for a valid session.
        /// </summary>
        [TestMethod]
        public void VerifyCorrectRoleTest()
        {
            // act
            var sessionId = SessionController.Current.Login("editor", "editor", Role.Editor);

            // assert
            Assert.IsTrue(SessionController.Current.IsValidSession(sessionId, Role.Editor), "Sessiond id is not stored for editor role");
            Assert.IsFalse(SessionController.Current.IsValidSession(sessionId, Role.Administrator), "Session id was stored in administrator role");
        }

        /// <summary>
        /// Test for an invalid login attemp.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException<LoginFailedException>), "Login failed exception was not thrown.")]
        public void InvalidLoginTest()
        {
            SessionController.Current.Login("user", "password", Role.Editor); 
        }

        /// <summary>
        /// Test that user cannot be logged in with different role he has not assigned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException<LoginFailedException>), "Editor user must not be logged in as administrator.")]
        public void InvalidRoleForLoginTest()
        {
            SessionController.Current.Login("editor", "editor", Role.Administrator);
        }
    }
}
