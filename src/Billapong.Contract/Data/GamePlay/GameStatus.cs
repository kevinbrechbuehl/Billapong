namespace Billapong.Contract.Data.GamePlay
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The game status contract
    /// </summary>
    [DataContract(Name = "GameStatus", Namespace = Globals.DataContractNamespaceName)]
    public enum GameStatus
    {
        /// <summary>
        /// The open status
        /// </summary>
        [EnumMember]
        Open = 1,

        /// <summary>
        /// The playing status
        /// </summary>
        [EnumMember]
        Playing = 2,

        /// <summary>
        /// The canceled status
        /// </summary>
        [EnumMember]
        Canceled = 3
    }
}
