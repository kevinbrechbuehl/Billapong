namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Windows.Input;
    using Contract.Data.Tracing;
    using Contract.Service;

    public class GameMenuViewModel
    {
        public ICommand OpenGameWindowCommand
        {
            get
            {
                return new DelegateCommand(OpenGameWindow);
            }
        }

        public ICommand LogCommand
        {
            get
            {
                return new DelegateCommand(Log);
            }
        }

        public void OpenGameWindow(object parameter)
        {
            var gameWindow = new GameWindow();
            gameWindow.Show();
        }

        private void Log(object parameter)
        {
            var proxy = ChannelFactory<ITracingService>.CreateChannel(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:4710"));

            Exception exception = null;
            try
            {
                int.Parse("lala");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var messages = new List<LogMessage>();
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Debug, Message = "Debug 1" });
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Debug, Message = "Debug 2" });
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Debug, Message = "Debug 3" });
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Error, Message = exception.Message + exception.StackTrace });

            proxy.Log(messages);
        }
    }
}
