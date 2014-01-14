namespace Billapong.Contract.Exceptions
{
    using System;

    public class GameNotFoundException : Exception
    {
        public GameNotFoundException()
        {
        }

        public GameNotFoundException(Guid gameId)
            : base(string.Format("The game with id '{0}' was not found in the lobby", gameId))
        {
        }
    }
}
