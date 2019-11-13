using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchereSteinPapierInterface;

namespace SchereSteinPapierArbiter
{
    class GameStatus
    {

        int[] _stats = new[] { 0, 0, 0 };
        public ESchereSteinPapier Player1Selection
        {
            get;
            set;
        }

        public ESchereSteinPapier Player2Selection
        {
            get;
            set;
        }

        public int NrOfTies
        {
            get => _stats[0];
        }
        public int Player1WinCount
        {
            get => _stats[1];           
        }

        public int Player2WinCount
        {
            get => _stats[2];
        }

        internal void UpdateStats(ESchereSteinPapier s1, ESchereSteinPapier s2)
        {
            Player1Selection = s1;
            Player2Selection = s2;
            var index = SchereSteinPapierTools.EvalGame(s1, s2);
            Console.WriteLine("Player1: {0}, Player2: {1} => winner = Player {2}", Player1Selection, Player2Selection, index);
            _stats[index]++;
        }
    }
}
