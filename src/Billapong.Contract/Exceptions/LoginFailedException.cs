namespace Billapong.Contract.Exceptions
{
    using System.Runtime.Serialization;

    [DataContract(Name = "LoginFailedException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class LoginFailedException
    {
        [DataMember(Name = "Username", Order = 1)]
        public string Username { get; set; }

        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }

        public LoginFailedException(string username)
        {
            this.Username = username;
            this.Message = string.Format("Login failed for user '{0}'", username);
        }
    }
}
