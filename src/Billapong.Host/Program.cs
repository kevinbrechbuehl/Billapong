using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Billapong.Implementation;

namespace Billapong.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost adminHost = null;
            ServiceHost consoleHost = null;
            try
            {
                adminHost = new ServiceHost(typeof(AdministrationService));
                adminHost.Open();

                Console.WriteLine("Administration Service is running");

                consoleHost = new ServiceHost(typeof(ConsoleService));
                consoleHost.Open();

                Console.WriteLine("Console Service is running");

                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (adminHost != null)
                {
                    adminHost.Close();
                }

                if (consoleHost != null)
                {
                    consoleHost.Close();
                }
            }
        }
    }
}
