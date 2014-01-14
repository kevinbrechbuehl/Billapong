namespace Billapong.Contract.Exceptions
{
    using System;

    public class GameNotOpenException : Exception
    {
        public GameNotOpenException()
        {
        }

        public GameNotOpenException(Guid gameId)
            : base(string.Format("The game with id '{0}' is not in opening state", gameId))
        {
        }
    }
}
