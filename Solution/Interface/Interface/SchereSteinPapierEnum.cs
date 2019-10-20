using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchereSteinPapierInterface
{
    public enum ESchereSteinPapier
    {
        Stein = 0,
        Papier = 1,
        Schere = 2
    }

    public enum EResultStatus
    {
        GameNotStarted,
        GameSuccessfullyCompleted,
        GameError
    }
}
