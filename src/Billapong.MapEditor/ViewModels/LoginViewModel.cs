﻿namespace Billapong.MapEditor.ViewModels
{
    using System;
    using System.Security;
    using System.ServiceModel;
    using System.Windows;
    using System.Windows.Controls;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Client.Tracing;
    using Billapong.Core.Client.UI;
    using Billapong.MapEditor.Properties;

    public class LoginViewModel : ViewModelBase
    {
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

        private readonly AuthenticationServiceClient proxy;

        public DelegateCommand<PasswordBox> LoginCommand
        {
            get
            {
                return new DelegateCommand<PasswordBox>(this.Login);
            }
        }

        public LoginViewModel()
        {
            this.proxy = new AuthenticationServiceClient();
        }

        private async void Login(PasswordBox passwordBox)
        {
            var password = passwordBox.Password;
            
            // check for empty strings
            if (string.IsNullOrWhiteSpace(this.Username) || string.IsNullOrWhiteSpace(password))
            {
                this.Message = Resources.EnterUsernameAndPassword;
                return;
            }
            
            try
            {
                this.Message = Resources.PleaseWait;
                
                // try to login user
                var sessionId = await this.proxy.LoginAsync(this.Username, password, Role.Editor);
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