namespace Billapong.MapEditor.ViewModels
{
    using System;
    using System.ServiceModel;
    using System.Windows;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Client.Tracing;
    using Billapong.Core.Client.UI;
    using Billapong.MapEditor.Properties;

    public class LoginViewModel : ViewModelBase
    {
        private string username;

        public string Username
        {
            get
            {
                return this.username;
            }

            set
            {
                this.username = value;
                OnPropertyChanged();
            }
        }

        private string password;

        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
                OnPropertyChanged();
            }
        }

        private string message;

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnPropertyChanged();
            }
        }

        private readonly AuthenticationServiceClient proxy;

        public DelegateCommand LoginCommand
        {
            get
            {
                return new DelegateCommand(this.Login);
            }
        }

        public LoginViewModel()
        {
            this.proxy = new AuthenticationServiceClient();
        }

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

        private void LoginSuccessfull(Guid sessionId)
        {
            this.WindowManager.Open(new MapSelectionViewModel(sessionId));
            //this.WindowManager.Close(this); // todo (breck1): sollte schliessen, geht aber nicht da sonst auch das andere fenster schliesst
        }

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
