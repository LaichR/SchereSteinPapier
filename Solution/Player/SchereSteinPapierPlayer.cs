using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SchereSteinPapierInterface;
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;


namespace SchereSteinPapierPlayer
{ 
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, Name = SchereSteinPapierTools.PlayerService)]
    public class SchereSteinPapierPlayer : ISchereSteinPapierPlayer
    {
        int _port = 9095;
        FeedForwardNetwork _network;
        string _name;
        AutoResetEvent _awaitDone;
        Random _rand;
        Dictionary<string, Func<int, ESchereSteinPapier, ESchereSteinPapier, ESchereSteinPapier>> _playStrategies = new Dictionary<string, Func<int, ESchereSteinPapier, ESchereSteinPapier, ESchereSteinPapier>>();

        public SchereSteinPapierPlayer( string name, int port, AutoResetEvent @await )
        {
            _port = port;
            _name = name;
            _awaitDone = @await;
            
            _playStrategies = new Dictionary<string, Func<int, ESchereSteinPapier, ESchereSteinPapier, ESchereSteinPapier>>
            {
                {"Dummy", DummyToss },
                {"Random", RandomToss },
                {"MadTeacher", NeuronalNetworkToss },
                {"RoundRobin", RoundRobinToss }
            };
        }

        public void Restart()
        {
            _rand = new Random();
            
            Console.WriteLine("Preparing for new game");
            /// add your own code here
            /// 
            _network = new FeedForwardNetwork();
        }
        public string Ping()
        {
            var reply = string.Format("{0}@{1} received ping request", Name, ConnectionString);
            Console.WriteLine(reply);
            return reply;
        }

        public ESchereSteinPapier SchereSteinPapier(int nrOfInvocations, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection)
        {
            var attempt = nrOfInvocations + 1;
           
            /// add your own code here
            if(!_playStrategies.TryGetValue(Name, out Func<int, ESchereSteinPapier,ESchereSteinPapier,ESchereSteinPapier> fun))
            {
                fun = DummyToss;
            }
            var toss = fun(nrOfInvocations, ownSelection, adversarialSelection);
            //Console.WriteLine("{0}. attempt -> {1}", attempt, toss);
            return toss; 
        }

        public void Close()
        {
            Console.WriteLine("Closing {0}", ConnectionString );
            _awaitDone.Set();
            
        }
        

        internal string Name
        {
            get
            {
                return _name;
            }
        }

        internal string ConnectionString
        {
            get
            {
                // modify if port 9095 is already in use or if your want ot have multiple clients
                return string.Format("{0}/{1}", GetLocalIPAddress(), _port);
            }
        }

        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private ESchereSteinPapier DummyToss(int nrOfInvocations, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection)
        {
            return ESchereSteinPapier.Papier;
        }

        private ESchereSteinPapier RandomToss(int nrOfInvocations, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection)
        {
            return (ESchereSteinPapier)(int)(_rand.NextDouble() * 3);
        }

        private ESchereSteinPapier NeuronalNetworkToss(int nrOfInvocation, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection)
        {
            return _network.SchereSteinPapier(nrOfInvocation, ownSelection, adversarialSelection);
        }

        private ESchereSteinPapier RoundRobinToss(int nrOfInvocation, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection)
        {
            return (ESchereSteinPapier)((int)(ownSelection + 2) % 3);
        }
    }
}
