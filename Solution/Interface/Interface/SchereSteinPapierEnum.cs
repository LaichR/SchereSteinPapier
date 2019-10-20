using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchereSteinPapierInterface
{
    /// <summary>
    /// Enumeration to describe the possible choices of a game
    /// </summary>
    public enum ESchereSteinPapier
    {
        Stein = 0,
        Papier = 1,
        Schere = 2
    }

    /// <summary>
    /// Describes the outcome of the request ISchereSteinPapierArbiter.Play(..)
    /// - GameNotStarted is the state in the beginning of the request
    /// - GameError is the state, when either a Player is not found or a networking problem pops up
    /// </summary>
    public enum EResultStatus
    {
        GameNotStarted,             
        GameSuccessfullyCompleted,
        GameError
    }
}
