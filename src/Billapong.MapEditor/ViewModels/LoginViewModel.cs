namespace Billapong.MapEditor.ViewModels
{
    using System;
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Client.Tracing;
    using Billapong.Core.Client.UI;
    using Billapong.MapEditor.Properties;

    /// <summary>
    /// Login view model
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// The authentication service proxy proxy
        /// </summary>
        private readonly AuthenticationServiceClient proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel()
        {
            this.proxy = new AuthenticationServiceClient();
        }
        
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>
        /// The login command.
        /// </value>
        public DelegateCommand LoginCommand
        {
            get
            {
                return new DelegateCommand(this.Login);
            }
        }

        /// <summary>
        /// Logins the user to the server.
        /// </summary>
        private async void Login()
        {
            // check for empty strings
            if (string.IsNullOrWhiteSpace(this.Username) || string.IsNullOrWhiteSpace(this.Password))
            {
                this.Message = Resources.EnterUsernameAndPassword;
                return;
            }
            
            try
            {
                this.Message = Resources.PleaseWait;
                
                // try to login user
                var sessionId = await this.proxy.LoginAsync(this.Username, this.Password, Role.Editor);
                this.LoginSuccessfull(sessionId);
            }
            catch (FaultException<LoginFailedException> ex)
            {
                this.LoginFailed(ex);
            }
        }

        /// <summary>
        /// Login was successfull, open map editor window.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        private void LoginSuccessfull(Guid sessionId)
        {
            this.WindowManager.Open(new MapSelectionViewModel(sessionId));
            this.WindowManager.Close(this);
        }

        /// <summary>
        /// Login failed, show error message.
        /// </summary>
        /// <param name="ex">The exception.</param>
        private async void LoginFailed(Exception ex)
        {
            if (ex != null)
            {
                await Tracer.Warn(ex.Message);
            }
            
            this.Message = Resources.LoginFailed;
        }
    }
}
