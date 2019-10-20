using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SchereSteinPapierInterface;

namespace SchereSteinPapierArbiter
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "net.tcp://localhost:{0}";
            baseAddress = string.Format(baseAddress, 9090);

            var instance = new SchereSteinPapierArbiter();
            var uri = new Uri(baseAddress);

            using (ServiceHost host = new ServiceHost(instance, uri))
            {

                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;  // don't ask for authetication
                binding.ReceiveTimeout = TimeSpan.MaxValue; // do not timeout at all!
                binding.SendTimeout = TimeSpan.MaxValue;
                var endpoint = host.AddServiceEndpoint(
                    typeof(ISchereSteinPapierArbiter),
                    binding,
                    SchereSteinPapierTools.ArbiterService);
                host.Open();
                Console.WriteLine("Schere Stein Papier Arbiter ready - enter quit to stop");

                var cmd = Console.ReadLine();
                while (cmd.Trim() != "quit")
                {
                    if( cmd == "show")
                    {
                        ShowPlayers(instance.RegisteredPlayers);
                    }
                    else if( cmd == "play")
                    {
                        if (GetNonEmptyInput("Enter name of player 1:", out string name1))
                        {
                            if (GetNonEmptyInput("Enter name of player 2:", out string name2))
                            {
                                var summary = instance.Play(name1, name2, 100);
                                Console.WriteLine("**********************************");
                                Console.WriteLine("* Winner: {0} ", summary.Winner);
                                Console.WriteLine("* Winning ratio: {0}", summary.WinnerSuccessRatio);
                                Console.WriteLine("**********************************");
                            }
                        }
                    }
                    else if( cmd == "ping")
                    {
                        Ping(instance);
                    }
                    cmd = Console.ReadLine();
                }
                instance.CloseAllPlayers();
            }
        }

        static void Ping(SchereSteinPapierArbiter arbiter)
        {
            if (GetNonEmptyInput("Enter name of player to ping:", out string name))
            {
                Console.WriteLine(arbiter.PingPlayer(name));
            }
        }

        static void ShowPlayers(IEnumerable<KeyValuePair<string, string>> players)
        {
            Console.WriteLine("****************************");
            Console.WriteLine("*   Registered players:    *");
            Console.WriteLine("****************************");
            foreach (var p in players)
            {
                Console.WriteLine("{0:16} @ {1}", p.Key, p.Value);
            }
        }

        static bool GetNonEmptyInput(string title, out string nameOfPlayer1)
        {
            Console.WriteLine(title);
            var player1 = Console.ReadLine();
            nameOfPlayer1 = player1.Trim();
            return !string.IsNullOrEmpty(nameOfPlayer1);
        }
    }
}
