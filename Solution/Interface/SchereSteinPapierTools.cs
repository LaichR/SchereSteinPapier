using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchereSteinPapierInterface
{
    public static class SchereSteinPapierTools
    {
        public const string ArbiterService = "ShereSteinPapier.ArbiterService";
        public const string PlayerService = "ShereSteinPapier.PlayerService";

        /// <summary>
        /// EvalGame bestimmt, ob Player1 oder Player2 gewonnen hat. Falls
        /// </summary>
        /// <param name="player1Selection">Wahl des Player1</param>
        /// <param name="player2Selection">Wahl des Player2</param>
        /// <returns>Gewinner des Spiels:
        /// - 1 falls Player1 gewonnen hat
        /// - 2 falls Player2 gewonnen hat
        /// - 0 im Fall eines Unentschieden</returns>
        public static int EvalGame(ESchereSteinPapier player1Selection, ESchereSteinPapier player2Selection)
        {
            if (player2Selection == player1Selection)
            {
                return 0;
            }
            else
            {
                switch (player1Selection)
                {
                    case ESchereSteinPapier.Papier:
                        if (player2Selection == ESchereSteinPapier.Stein)
                        {
                            return 1;
                        }
                        break;
                    case ESchereSteinPapier.Schere:
                        if (player2Selection == ESchereSteinPapier.Papier)
                        {
                            return 1;
                        }
                        break;

                    case ESchereSteinPapier.Stein:
                        if (player2Selection == ESchereSteinPapier.Schere)
                        {
                            return 1;
                        }
                        break;
                }
                return 2;
            }
        }
    }
}
