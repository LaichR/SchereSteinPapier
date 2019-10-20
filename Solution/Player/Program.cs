using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.ServiceModel;
using RuntimeCheck;

using SchereSteinPapierInterface;

namespace SchereSteinPapierPlayer
{
    /// <summary>
    /// This is the entry point for the process that hosts the WCF service ISchereSteinPapier player. 
    /// The service could be hosted in a thread instead. 
    /// This would be the appropriate solution if a GUI was required.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("usage: <ip address or host name of service>:<port of service>");
            }
            var awaitDone = new AutoResetEvent(false);
            var name = ConfigurationManager.AppSettings.Get("Name");
            Assert.True(!string.IsNullOrEmpty(name), 
                "name needs to be specified");
            Assert.True(int.TryParse(ConfigurationManager.AppSettings.Get("Port"), out int port),
                "no valid port was specified" );
            
            string serviceConnection = args[0];

            var instance = new SchereSteinPapierPlayer(name, port, awaitDone);
 
            string baseAddress = "net.tcp://{0}";
            baseAddress = string.Format(baseAddress, instance.ConnectionString);


            var uri = new Uri(baseAddress);

            using (ServiceHost host = new ServiceHost(instance, uri))
            {
                
                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;
                binding.ReceiveTimeout = TimeSpan.MaxValue; // do not timeout at all!
                binding.SendTimeout = TimeSpan.MaxValue;
                var endpoint = host.AddServiceEndpoint(
                    typeof(ISchereSteinPapierPlayer),
                    binding,
                    SchereSteinPapierTools.PlayerService);
                host.Open();
                Console.WriteLine("Schere Stein Papier Player Started - enter quit to stop");

                var address = string.Format("net.tcp://{0}/{1}", serviceConnection, SchereSteinPapierTools.ArbiterService);
                //var instanceContext = new InstanceContext( impl );
                var clientBinding = new NetTcpBinding() { SendTimeout = TimeSpan.MaxValue, ReceiveTimeout = TimeSpan.MaxValue };
                clientBinding.Security.Mode = SecurityMode.None;

                using (var serviceFactory =
                    new ChannelFactory<ISchereSteinPapierArbiter>(
                    clientBinding,
                    new EndpointAddress(
                        address)))
                {
                    var service = serviceFactory.CreateChannel();
                    if (service.RegisterPlayer(instance.Name, instance.ConnectionString))
                    {
                        Console.WriteLine("Player successfully registered - ready to play");
                        awaitDone.WaitOne();
                    }
                }
            }
        }
    }
}
