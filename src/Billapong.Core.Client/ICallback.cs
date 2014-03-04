namespace Billapong.Core.Client
{
    /// <summary>
    /// Generic interface for callbacks
    /// </summary>
    /// <typeparam name="TCallback">The type of the callback.</typeparam>
    public interface ICallback<TCallback>
    {
        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        TCallback Callback { get; }
    }
}
