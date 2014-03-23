namespace Billapong.Contract.Exceptions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for an invalid callback.
    /// </summary>
    [DataContract(Name = "CallbackNotValidException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class CallbackNotValidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackNotValidException"/> class.
        /// </summary>
        public CallbackNotValidException()
        {
            this.Message = "The current callback is not valid";
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }
    }
}
