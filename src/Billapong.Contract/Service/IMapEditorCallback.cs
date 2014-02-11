namespace Billapong.Contract.Service
{
    using System.ServiceModel;

    /// <summary>
    /// Callback contract for the map editor.
    /// </summary>
    public interface IMapEditorCallback
    {
        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "AddWindow")]
        void AddWindow(long windowId, int coordX, int coordY);

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "RemoveWindow")]
        void RemoveWindow(long windowId, int coordX, int coordY);

        /// <summary>
        /// Adds the hole.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x.</param>
        /// <param name="windowY">The window y.</param>
        /// <param name="holeId">The hole identifier.</param>
        /// <param name="holeX">The hole x.</param>
        /// <param name="holeY">The hole y.</param>
        [OperationContract(Name = "AddHole")]
        void AddHole(long windowId, int windowX, int windowY, long holeId, int holeX, int holeY);

        /// <summary>
        /// Removes the hole.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x.</param>
        /// <param name="windowY">The window y.</param>
        /// <param name="holeId">The hole identifier.</param>
        [OperationContract(Name = "RemoveHole")]
        void RemoveHole(long windowId, int windowX, int windowY, long holeId);
    }
}
