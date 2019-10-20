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
