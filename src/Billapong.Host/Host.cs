namespace Billapong.Host
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel;
    using Core.Server.Services;

    /// <summary>
    /// The services host
    /// </summary>
    public class Host
    {
        /// <summary>
        /// The exit command
        /// </summary>
        private const string CommandExit = "exit";

        /// <summary>
        /// The status command
        /// </summary>
        private const string CommandStatus = "status";

        /// <summary>
        /// The help command
        /// </summary>
        private const string CommandHelp = "help";

        /// <summary>
        /// The start command
        /// </summary>
        private const string CommandStart = "start";

        /// <summary>
        /// The stop command
        /// </summary>
        private const string CommandStop = "stop";

        /// <summary>
        /// All services
        /// </summary>
        private const string AllServices = "all";

        /// <summary>
        /// The service hosts
        /// </summary>
        private readonly Dictionary<string, ServiceHost> serviceHosts;

        /// <summary>
        /// Initializes a new instance of the <see cref="Host"/> class.
        /// </summary>
        public Host()
        {
            this.serviceHosts = new Dictionary<string, ServiceHost>
            {
                { "tracing", new ServiceHost(typeof(TracingService)) },
                { "gameconsole", new ServiceHost(typeof(GameConsoleService)) },
                { "mapeditor", new ServiceHost(typeof(MapEditorService)) },
                { "administration", new ServiceHost(typeof(AdministrationService)) },
                { "session", new ServiceHost(typeof(SessionService)) }
            };
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            try
            {
                this.WriteTitle();
                Console.WriteLine("Console host up and running. Type {0} and enter for command list.", CommandHelp);
                Console.WriteLine(string.Empty);
                this.ManageService(CommandStart, AllServices);
                string userInput;
                
                do
                {
                    userInput = Console.ReadLine();
                    this.ParseCommand(userInput);
                } 
                while (userInput != CommandExit);
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("{0}{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                if (this.serviceHosts != null)
                {
                    foreach (var serviceHost in this.serviceHosts.Where(serviceHost => serviceHost.Value != null))
                    {
                        serviceHost.Value.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Parses the command.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        private void ParseCommand(string userInput)
        {
            Console.WriteLine(string.Empty);
            
            if (userInput == CommandExit)
            {
                return;
            }

            if (userInput == CommandHelp)
            {
                this.WriteHelp();
            }
            else if (userInput == CommandStatus)
            {
                this.WriteStatus();
            }
            else if (userInput.StartsWith(CommandStart) || userInput.StartsWith(CommandStop))
            {
                var input = userInput.Split(' ');
                if (input.Length != 2)
                {
                    Console.WriteLine(" Invalid input :(");
                    Console.WriteLine(string.Empty);
                    return;
                }

                if (input[1] != AllServices && !this.serviceHosts.ContainsKey(input[1]))
                {
                    Console.WriteLine(" Service '{0}' does not exists", input[1]);
                    Console.WriteLine(string.Empty);
                    return;
                }

                this.ManageService(input[0], input[1]);
            }
            else
            {
                Console.WriteLine(" Invalid input :(");
            }

            Console.WriteLine(string.Empty);
        }

        /// <summary>
        /// Writes the help.
        /// </summary>
        private void WriteHelp()
        {
            Console.WriteLine(" Type one of the command followed by the enter key:");
            Console.WriteLine(string.Empty);
            Console.WriteLine("   -> {0}:\t\tShow this help", CommandHelp);
            Console.WriteLine("   -> {0}:\t\tExit the console service host", CommandExit);
            Console.WriteLine("   -> {0}:\t\tShow status of the different services", CommandStatus);
            Console.WriteLine("   -> {0} <service>:\tstart the specific service", CommandStart);
            Console.WriteLine("   -> {0} <service>:\tstop the specific service", CommandStop);
            Console.WriteLine(string.Empty);
            Console.WriteLine(" Available services: {0}", string.Join(", ", this.serviceHosts.Keys.Select(i => i.ToString()).ToArray()));
        }

        /// <summary>
        /// Writes the status.
        /// </summary>
        private void WriteStatus()
        {
            foreach (var serviceHost in this.serviceHosts)
            {
                Console.WriteLine(" Service '{0}': {1}", serviceHost.Key, serviceHost.Value != null ? this.GetStatus(serviceHost.Value.State) : "null");
            }
        }

        /// <summary>
        /// Manages the service.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="serviceName">Name of the service.</param>
        private void ManageService(string action, string serviceName)
        {
            if (serviceName == AllServices)
            {
                foreach (var service in this.serviceHosts)
                {
                    this.ManageService(action, service.Value, service.Key);
                }

                Console.WriteLine(string.Empty);
                return;
            }
            
            this.ManageService(action, this.serviceHosts[serviceName], serviceName);
        }

        /// <summary>
        /// Writes the title.
        /// </summary>
        private void WriteTitle()
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine(@"       ____ ___ _     _        _    ____   ___  _   _  ____");
            Console.WriteLine(@"      | __ )_ _| |   | |      / \  |  _ \ / _ \| \ | |/ ___|");
            Console.WriteLine(@"      |  _ \| || |   | |     / _ \ | |_) | | | |  \| | |  _ ");
            Console.WriteLine(@"      | |_) | || |___| |___ / ___ \|  __/| |_| | |\  | |_| |");
            Console.WriteLine(@"      |____/___|_____|_____/_/   \_\_|    \___/|_| \_|\____|");
            Console.WriteLine(string.Empty);
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>The status</returns>
        private string GetStatus(CommunicationState state)
        {
            switch (state)
            {
                case CommunicationState.Closed:
                case CommunicationState.Closing:
                case CommunicationState.Created:
                    return "stopped";
                case CommunicationState.Opened:
                case CommunicationState.Opening:
                    return "started";
                default:
                    return "failed";
            }
        }

        /// <summary>
        /// Manages the service.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="service">The service.</param>
        /// <param name="serviceName">Name of the service.</param>
        private void ManageService(string action, ServiceHost service, string serviceName)
        {
            if (action == CommandStart)
            {
                this.StartService(service, serviceName);
            }
            else
            {
                this.StopService(service, serviceName);
            }
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceName">Name of the service.</param>
        private void StartService(ServiceHost service, string serviceName)
        {
            if (service.State == CommunicationState.Opened || service.State == CommunicationState.Opening)
            {
                Console.WriteLine(" Service '{0}' already running", serviceName);
                return;
            }

            if (service.State != CommunicationState.Created)
            {
                var type = service.Description.ServiceType;
                service = new ServiceHost(type);
                this.serviceHosts[serviceName] = service;
            }

            service.Open();
            Console.WriteLine(" ... Service '{0}' started", serviceName);
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceName">Name of the service.</param>
        private void StopService(ServiceHost service, string serviceName)
        {
            if (service.State == CommunicationState.Closed || service.State == CommunicationState.Closing)
            {
                Console.WriteLine(" Service '{0}' not running", serviceName);
                return;
            }

            service.Close();
            Console.WriteLine(" ... Service '{0}' stopped", serviceName);
        }
    }
}