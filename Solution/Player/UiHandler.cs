using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RuntimeCheck;
using System.Threading;
using SchereSteinPapierInterface;
using System.Text.RegularExpressions;

namespace SchereSteinPapierPlayer
{
    
    class UiHandler
    {
        const string Stop = @"stop";
        const string Play = @"play\s+(?<player1>\w+)\s+(?<player2>\w+)";

        bool _running = false;
        Regex _cmdStop = new Regex(Stop);
        Regex _cmdPlay = new Regex(Play);
        ISchereSteinPapierArbiter _arbiter;
        AutoResetEvent _awaitDone;
        string _playerName;

        public UiHandler(string name, AutoResetEvent await, ISchereSteinPapierArbiter arbiter)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "name must not be null or empty");
            Contract.Requires<ArgumentNullException>(await != null, "must not be null", "await");
            Contract.Requires<ArgumentNullException>(arbiter != null, "must not be null", "arbiter");

            _arbiter = arbiter;
            _playerName = name;
            _awaitDone = await;
        }

        public void UiHandlerActivity()
        {
            _running = true;
            Console.WriteLine("Starting console input handler..");
            while(_running)
            {
                var cmd = Console.ReadLine().Trim();
                if (_cmdStop.IsMatch(cmd))
                {
                    _arbiter.UnregisterPlayer(_playerName);
                    StopHandler();
                }
                else
                {
                    var match = _cmdPlay.Match(cmd);
                    if (match.Success)
                    {
                        var player1 = match.Groups["player1"].Value;
                        var player2 = match.Groups["player2"].Value;
                        var result = _arbiter.Play(player1, player2, 101);
                    }
                }
            }
        }

        internal void StopHandler()
        {
            _running = false;
            _awaitDone.Set();
        }

    }
}
