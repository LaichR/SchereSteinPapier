using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchereSteinPapierInterface;
using System.ServiceModel;
using RuntimeCheck;

namespace SchereSteinPapierArbiter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, Name = SchereSteinPapierTools.ArbiterService)]
    class SchereSteinPapierArbiter : ISchereSteinPapierArbiter
    {
        Dictionary<string, string> _playerRegistry = new Dictionary<string, string>();

        public ResultSummary Play(string player1, string player2, int nrOfRepetitions)
        {
            Contract.Requires(!string.IsNullOrEmpty(player1), "player1");
            Contract.Requires(!string.IsNullOrEmpty(player2), "player2");
            Contract.Requires<ArgumentOutOfRangeException>(nrOfRepetitions >=  100 && nrOfRepetitions < 1000, "allowed range = [100..1000]", "nrOfRepetitions");
            var summary = new ResultSummary()
            {
                Status = EResultStatus.GameNotStarted,
            };
            if(! _playerRegistry.TryGetValue(player1, out string connectionString1) )
            {
                summary.Status = EResultStatus.GameError;
                return summary;
            }
            if(!_playerRegistry.TryGetValue(player2, out string connectionString2))
            {
                summary.Status = EResultStatus.GameError;
            }

            using (var player1ChannelFactory = GetSchereSteinPapierPlayer(connectionString1))
            {
                using( var player2ChannelFactory = GetSchereSteinPapierPlayer(connectionString2))
                {
                    var p1Service = player1ChannelFactory.CreateChannel();
                    p1Service.Restart();
                    var p2Service = player2ChannelFactory.CreateChannel();
                    p2Service.Restart();
                    var gameStatus = new GameStatus();
                    for ( int i = 0; i < nrOfRepetitions; i++)
                    {
                        gameStatus.Player1Selection = p1Service.SchereSteinPapier(i, gameStatus.Player1Selection, gameStatus.Player2Selection);
                        gameStatus.Player2Selection = p2Service.SchereSteinPapier(i, gameStatus.Player2Selection, gameStatus.Player1Selection);
                        gameStatus.UpdateStats();
                    }
                    summary.Status = EResultStatus.GameSuccessfullyCompleted;
                    summary.Winner = "Unentschieden";
                    if( gameStatus.Player1WinCount > gameStatus.Player2WinCount)
                    {
                        summary.Winner = player1;
                    }
                    else if( gameStatus.Player2WinCount > gameStatus.Player1WinCount)
                    {
                        summary.Winner = player2;
                    }
                    summary.TotalNumberOfGames = nrOfRepetitions;
                    summary.WinnerSuccessRatio = Math.Max(gameStatus.Player1WinCount, gameStatus.Player2WinCount) / (double)nrOfRepetitions;
                }
            }
            return summary;
        }

        public bool RegisterPlayer(string name, string connectionString )
        { 
            Contract.Requires(!string.IsNullOrEmpty(name), "name");
            Contract.Requires(!string.IsNullOrEmpty(connectionString), "connectionString");

            if (_playerRegistry.ContainsKey(name))
            {
                return false;
            }
            _playerRegistry.Add(name, connectionString);
            return true;
        }

        public bool UnregisterPlayer(string name)
        {
            return _playerRegistry.Remove(name);
        }

        ChannelFactory<ISchereSteinPapierPlayer> GetSchereSteinPapierPlayer(string connectionString)
        {
            var address = string.Format("net.tcp://{0}/{1}", connectionString, SchereSteinPapierTools.PlayerService);
            //var instanceContext = new InstanceContext( impl );
            var binding = new NetTcpBinding() { SendTimeout = TimeSpan.MaxValue, ReceiveTimeout = TimeSpan.MaxValue };
            binding.Security.Mode = SecurityMode.None;

            var pf =
                new ChannelFactory<ISchereSteinPapierPlayer>(
                binding,
                new EndpointAddress(
                    address));
            return pf;
        }

        internal string PingPlayer( string name)
        {
            if (!_playerRegistry.TryGetValue(name, out string connectionString))
            {
                return string.Format("no player {0} registered", name);
            }
            try
            {
                using (var channelFactory = GetSchereSteinPapierPlayer(connectionString))
                {
                    var channel = channelFactory.CreateChannel();
                    return channel.Ping();
                }
            }catch(Exception e)
            {
                return string.Format("error during ping: \n{0}", e.Message);
            }
        }

        internal void CloseAllPlayers()
        {
            foreach (var connectionString in _playerRegistry.Values)
            {
                try
                {
                    using (var channelFactory = GetSchereSteinPapierPlayer(connectionString))
                    {
                        var player = channelFactory.CreateChannel();
                        player.Close();
                    }
                }
                catch { }
            }
            _playerRegistry.Clear();
        }

        internal IEnumerable<KeyValuePair<string, string>> RegisteredPlayers
        {
            get
            {
                return _playerRegistry;
            }
        }
    }
}
