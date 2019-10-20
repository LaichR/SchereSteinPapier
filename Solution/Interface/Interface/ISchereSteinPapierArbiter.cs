using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchereSteinPapierInterface
{
    /// <summary>
    /// This interface abstracts the arbiter. The arbiter controls a series of games between two players. 
    /// It guaratees that the two players may not cheat by resetting their internal state at the wrong moment, evaluates
    /// the results and gives out a little statistics of a competition.
    /// In order to do so, each player has to register.
    /// </summary>
    [ServiceContract]
    public interface ISchereSteinPapierArbiter
    {
        /// <summary>
        /// Register a player 
        /// </summary>
        /// <param name="name">Name of the player</param>
        /// <param name="connectionString">String in the form IP-address:Port, this is needed by the arbiter to get into contact with the player, since each player is implemented as a idependent service</param>
        /// <returns></returns>
        [OperationContract]
        bool RegisterPlayer(string name, string connectionString );

        /// <summary>
        /// In case a player wants to leave before the arbiter shuts off, it has to unregister.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        bool UnregisterPlayer(string name);

        /// <summary>
        /// Play a little tournament
        /// </summary>
        /// <param name="player1">name of participant 1</param>
        /// <param name="player2">name of participant 2</param>
        /// <param name="nrOfRepetitions"></param>
        /// <returns>Summary information of the play including winner</returns>
        [OperationContract]
        ResultSummary Play(string player1, string player2, int nrOfRepetitions);
    }
}
