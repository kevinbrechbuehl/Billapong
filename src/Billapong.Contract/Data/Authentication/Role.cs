namespace Billapong.Contract.Data.Authentication
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the different roles.
    /// </summary>
    [DataContract(Name = "Role", Namespace = Globals.DataContractNamespaceName)]
    public enum Role
    {
        /// <summary>
        /// The editor role
        /// </summary>
        [EnumMember]
        Editor = 1,

        /// <summary>
        /// The administrator role
        /// </summary>
        [EnumMember]
        Administrator = 2
    }
}
