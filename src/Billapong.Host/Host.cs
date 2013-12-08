using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Billapong.Implementation;

namespace Billapong.Host
{
    public class Host
    {
        private const string CommandExit = "exit";
        private const string CommandStatus = "status";
        private const string CommandHelp = "help";
        private const string CommandStart = "start";
        private const string CommandStop = "stop";

        private const string AllServices = "all";

        private readonly Dictionary<string, ServiceHost> serviceHosts;

        public Host()
        {
            this.serviceHosts = new Dictionary<string, ServiceHost>
            {
                {"administration", new ServiceHost(typeof (AdministrationService))},
                {"console", new ServiceHost(typeof (ConsoleService))}
            };
        }
        
        public void Start()
        {
            
            // todo: refactoring
            
            try
            {
                Console.WriteLine("Console host up and running. Type {0} and enter for command list.", CommandHelp);
                this.ManageService(CommandStart, AllServices);
                string userInput;

                do
                {
                    userInput = Console.ReadLine();
                    ParseCommand(userInput);
                } while (userInput != CommandExit);
            }
            catch (Exception ex)
            {
                // todo: log exception
                throw;
            }
            finally
            {
                if (this.serviceHosts != null)
                {
                    foreach (var serviceHost in serviceHosts
                        .Where(serviceHost => serviceHost.Value != null))
                    {
                        serviceHost.Value.Close();
                    }
                }
            }
        }

        private void ParseCommand(string userInput)
        {
            if (userInput == CommandExit)
            {
                return;
            }

            if (userInput == CommandHelp)
            {
                WriteHelp();
            }

            if (userInput == CommandStatus)
            {
                WriteStatus();
            }

            if (userInput.StartsWith(CommandStart) || userInput.StartsWith(CommandStop))
            {
                var input = userInput.Split(' ');
                if (input.Length != 2)
                {
                    Console.WriteLine("Invalid input");
                    return;
                }

                if (input[1] != AllServices && !this.serviceHosts.ContainsKey(input[1]))
                {
                    Console.WriteLine("Service '{0}' does not exists", input[1]);
                    return;
                }

                this.ManageService(input[0], input[1]);
            }
        }

        private void WriteHelp()
        {
            Console.WriteLine("Type one of the command followed by the enter key:");
            Console.WriteLine("\t- {0}: Show this help", CommandHelp);
            Console.WriteLine("\t- {0}: Exit the console service host", CommandExit);
            Console.WriteLine("\t- {0}: Show status of the different services", CommandStatus);
            Console.WriteLine("\t- {0} <{1}>: start the specific service", CommandStart, string.Join("|", this.serviceHosts.Keys.Select(i => i.ToString()).ToArray()));
            Console.WriteLine("\t- {0} <{1}>: stop the specific service", CommandStop, string.Join("|", this.serviceHosts.Keys.Select(i => i.ToString()).ToArray()));
        }

        private void WriteStatus()
        {
            foreach (var serviceHost in this.serviceHosts)
            {
                Console.WriteLine("{0}: {1}", serviceHost.Key, (serviceHost.Value != null ? this.GetStatus(serviceHost.Value.State) : "null"));
            }
        }

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

        private void ManageService(string action, string serviceName)
        {
            if (serviceName == AllServices)
            {
                foreach (var service in this.serviceHosts)
                {
                    this.ManageService(action, service.Value, service.Key);
                }

                return;
            }
            
            this.ManageService(action, this.serviceHosts[serviceName], serviceName);
        }

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

        private void StartService(ServiceHost service, string serviceName)
        {
            if (service.State == CommunicationState.Opened || service.State == CommunicationState.Opening)
            {
                Console.WriteLine("Service '{0}' already running", serviceName);
                return;
            }

            service.Open();
            Console.WriteLine("Service '{0}' started", serviceName);
        }

        private void StopService(ServiceHost service, string serviceName)
        {
            if (service.State == CommunicationState.Closed || service.State == CommunicationState.Closing)
            {
                Console.WriteLine("Service '{0}' not running", serviceName);
                return;
            }

            service.Close();
            Console.WriteLine("Service '{0}' stopped", serviceName);
        }
    }
}
