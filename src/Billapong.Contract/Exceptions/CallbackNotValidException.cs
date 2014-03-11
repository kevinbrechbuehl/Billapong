namespace Billapong.Contract.Exceptions
{
    using System.Runtime.Serialization;

    [DataContract(Name = "CallbackNotValidException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class CallbackNotValidException
    {
        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }

        public CallbackNotValidException()
        {
            this.Message = "The current callback is not valid";
        }
    }
}
