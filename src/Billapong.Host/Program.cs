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
            ServiceHost host = null;
            try
            {
                host = new ServiceHost(typeof(AdministrationService));
                host.Open();

                Console.WriteLine("Administration Service is running");
                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (host != null)
                {
                    host.Close();
                }
            }
        }
    }
}
