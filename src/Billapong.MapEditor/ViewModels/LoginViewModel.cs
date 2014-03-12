namespace Billapong.MapEditor.ViewModels
{
    using System.Windows;
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

        public DelegateCommand LoginCommand
        {
            get
            {
                return new DelegateCommand(this.Login);
            }
        }

        private void Login()
        {
            if (this.Username == "editor" && this.Password == "editor")
            {
                this.LoginSuccessfull();
                return;
            }

            this.LoginFailed();
        }

        private void LoginSuccessfull()
        {
            this.WindowManager.Open(new MapSelectionViewModel());
            //this.WindowManager.Close(this); // todo (breck1): sollte schliessen, geht aber nicht da sonst auch das andere fenster schliesst
        }

        private void LoginFailed()
        {
            MessageBox.Show(Resources.LoginFailed, Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
