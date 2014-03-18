namespace Billapong.Contract.Exceptions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for invalid login attemp.
    /// </summary>
    [DataContract(Name = "LoginFailedException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class LoginFailedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedException"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        public LoginFailedException(string username)
        {
            this.Username = username;
            this.Message = string.Format("Login failed for user '{0}'", username);
        }
        
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [DataMember(Name = "Username", Order = 1)]
        public string Username { get; set; }

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
