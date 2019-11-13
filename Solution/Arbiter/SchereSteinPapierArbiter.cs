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
        Dictionary<string, int> _victoryStats = new Dictionary<string, int>();

        public ResultSummary Play(string player1, string player2, int nrOfRepetitions)
        {
            Contract.Requires(!string.IsNullOrEmpty(player1), "player1");
            Contract.Requires(!string.IsNullOrEmpty(player2), "player2");
            Contract.Requires<ArgumentOutOfRangeException>(nrOfRepetitions >= 100 && nrOfRepetitions < 1000, "allowed range = [100..1000]", "nrOfRepetitions");
            var summary = new ResultSummary()
            {
                Status = EResultStatus.GameNotStarted,
            };
            if (!_playerRegistry.TryGetValue(player1, out string connectionString1))
            {
                summary.Status = EResultStatus.GameError;
                return summary;
            }
            if (!_playerRegistry.TryGetValue(player2, out string connectionString2))
            {
                summary.Status = EResultStatus.GameError;
                return summary;
            }
            try
            {
                using (var player1ChannelFactory = GetSchereSteinPapierPlayer(connectionString1))
                {
                    ISchereSteinPapierPlayer p1Service = null;
                    try
                    {
                        p1Service = player1ChannelFactory.CreateChannel();
                        p1Service.Restart();
                    }
                    catch
                    {
                        Console.WriteLine("was not able to restart player 1");
                        _playerRegistry.Remove(player1);
                        throw;
                    }
                    using (var player2ChannelFactory = GetSchereSteinPapierPlayer(connectionString2))
                    {
                        ISchereSteinPapierPlayer p2Service = null;
                        try
                        {
                            p2Service = player2ChannelFactory.CreateChannel();
                            p2Service.Restart();
                        }
                        catch
                        {
                            Console.WriteLine("was not able to restart player 2");
                            _playerRegistry.Remove(player2);
                            throw;
                        }
                        Assert.True(p1Service != null, "service for player {0} not available", player1);
                        Assert.True(p1Service != null, "service for player {0} not available", player2);
                        var gameStatus = new GameStatus();
                        for (int i = 0; i < nrOfRepetitions; i++)
                        {
                            var s1 = p1Service.SchereSteinPapier(i, gameStatus.Player1Selection, gameStatus.Player2Selection);
                            var s2 = p2Service.SchereSteinPapier(i, gameStatus.Player2Selection, gameStatus.Player1Selection);
                            gameStatus.UpdateStats(s1, s2);
                        }
                        //p1Service.Close();
                        //p2Service.Close();
                        summary.Status = EResultStatus.GameSuccessfullyCompleted;
                        summary.Winner = "Unentschieden";
                        summary.NrOfGamesWonByPlayer1 = gameStatus.Player1WinCount;
                        summary.NrOfGamesWonByPlayer2 = gameStatus.Player2WinCount;
                        if (gameStatus.Player1WinCount > gameStatus.Player2WinCount)
                        {
                            summary.Winner = player1;
                        }
                        else if (gameStatus.Player2WinCount > gameStatus.Player1WinCount)
                        {
                            summary.Winner = player2;
                        }
                        summary.TotalNumberOfGames = nrOfRepetitions;
                        summary.WinnerSuccessRatio = Math.Max(gameStatus.Player1WinCount, gameStatus.Player2WinCount) / (double)nrOfRepetitions;
                    }

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("an error occurued during the play");
                Console.WriteLine(e.Message);
                summary.Status = EResultStatus.GameError;
            }
            return summary;
        }

        public bool RegisterPlayer(string name, string connectionString )
        { 
            Contract.Requires(!string.IsNullOrEmpty(name), "name");
            Contract.Requires(!string.IsNullOrEmpty(connectionString), "connectionString");

            if (_playerRegistry.ContainsKey(name))
            {
                Console.WriteLine("Player with name {0} already registered", name);
                return false;
            }
            Console.WriteLine("Player {0} successfully registered!", name);
            _playerRegistry.Add(name, connectionString);
            return true;
        }

        public bool UnregisterPlayer(string name)
        {
            Console.WriteLine("Unregistering {0}", name);
            return _playerRegistry.Remove(name);
        }



        ChannelFactory<ISchereSteinPapierPlayer> GetSchereSteinPapierPlayer(string connectionString)
        {
            var address = string.Format("net.tcp://{0}/{1}", connectionString, SchereSteinPapierTools.PlayerService);
            //var instanceContext = new InstanceContext( impl );
            var binding = new NetTcpBinding() { SendTimeout = TimeSpan.FromSeconds(2), 
                ReceiveTimeout = TimeSpan.FromSeconds(2) };
            binding.Security.Mode = SecurityMode.None;

            var pf =
                new ChannelFactory<ISchereSteinPapierPlayer>(
                binding,
                new EndpointAddress(
                    address));
            return pf;
        }

        internal void ResetVictoryStats()
        {
            _victoryStats = new Dictionary<string, int>();
            foreach(var name in _playerRegistry.Keys )
            {
                _victoryStats.Add(name, 0);
            }
        }

        internal void PlayAll(IEnumerable<string> players, int nrOfRep)
        {
            if( players.Count()<2)
            {
                return;
            }
            var others = new List<string>(players.Skip(1));

            PlayAll(others, nrOfRep);

            var player = players.First();
            
            foreach( var other in others)
            {
                var summary = Play(player, other, nrOfRep);
                Console.WriteLine("winner is {0}", summary.Winner);
                if(_victoryStats.TryGetValue(summary.Winner, out int victories))
                {
                    _victoryStats[summary.Winner] = victories + 1;
                }
            }
        }

        internal void PrintVictoryStats()
        {
            var list = new List<KeyValuePair<string, int>>(_victoryStats.OrderByDescending((x) => x.Value));
            Console.WriteLine("ranking name                             #");
            Console.WriteLine("******************************************");

            for( int i = 0; i < list.Count(); i++)
            {
                Console.WriteLine("{0,2}.     {1,-32} #won {2,3}-times", i + 1, list[i].Key, list[i].Value);
            }
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
                _playerRegistry.Remove(name);
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
                catch {}
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
