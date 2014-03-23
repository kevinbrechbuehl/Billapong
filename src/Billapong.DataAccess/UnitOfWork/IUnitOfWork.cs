namespace Billapong.DataAccess.UnitOfWork
{
    using System;

    /// <summary>
    /// Interface for unit of work pattern implementation.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves changes to the data provider.
        /// </summary>
        void Save();
    }
}
