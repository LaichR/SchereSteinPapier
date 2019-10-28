using System;
using NUnit.Framework;

namespace SchereSteinPapierTest
{
    [TestFixture]
    public class PlayerTest
    {
        [TestCase("Dummy")]
        [TestCase("MadTeacher")]
        public void TestPlayer(string name)
        {
            var player = new SchereSteinPapierPlayer.SchereSteinPapierPlayer(name, 9095, null);
            player.Restart();
            var ownSelection = SchereSteinPapierInterface.ESchereSteinPapier.Papier;
            for( int i = 0; i< 101; i++)
            {
                ownSelection = player.SchereSteinPapier(i, ownSelection, SchereSteinPapierInterface.ESchereSteinPapier.Papier);
                Console.WriteLine(ownSelection);
            }
        }

        [TestCase("MadTeacher", "Dummy")]
        [TestCase("MadTeacher", "Random")]
        [TestCase("MadTeacher", "RoundRobin")]
        [TestCase("Dummy", "Random")]
        public void TestCompetition(string name1, string name2)
        {
            var player1 = new SchereSteinPapierPlayer.SchereSteinPapierPlayer(name1, 9095, null);
            var player2 = new SchereSteinPapierPlayer.SchereSteinPapierPlayer(name2, 9095, null);
            player1.Restart();
            player2.Restart();
            var selection1 = SchereSteinPapierInterface.ESchereSteinPapier.Papier;
            var selection2 = SchereSteinPapierInterface.ESchereSteinPapier.Papier;
            SchereSteinPapierInterface.ResultSummary summary = new SchereSteinPapierInterface.ResultSummary();
            int[] nrOfWins = { 0, 0 };
            var previousRes = 0;
            for (int i = 0; i < 1001; i++)
            {
                selection1 = player1.SchereSteinPapier(i, selection1, selection2);
                selection2 = player2.SchereSteinPapier(i, selection2, selection1);
                var res = SchereSteinPapierInterface.SchereSteinPapierTools.EvalGame(selection1, selection2);
                if( res > 0)
                {
                    nrOfWins[res - 1]++;
                }
            }
            Console.WriteLine("Player {0} won {1} times", name1, nrOfWins[0]);
            Console.WriteLine("Player {0} won {1} times", name2, nrOfWins[1]);
        }

    }
}
