namespace Billapong.DataAccess.Model
{
    /// <summary>
    /// The database entity interface
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        long Id { get; set; }
    }
}
