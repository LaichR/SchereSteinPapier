using System;

using System.ServiceModel;

namespace SchereSteinPapierInterface
{
    /// <summary>
    /// This interface abstracts one Player that can participate a SchereSteinPapier game
    /// </summary>
    [ServiceContract]
    public interface ISchereSteinPapierPlayer
    {
        /// <summary>
        /// This is the first request by the arbiter before starting a game
        /// It allows the player to prepare for a series games. 
        /// </summary>
        [OperationContract]
        void Restart();

        /// <summary>
        /// Used for diagnosis in order to see if a player is reachable over the network
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string Ping();

        /// <summary>
        /// This operation is called by the arbiter for two players. The player has to make his choice. Of corse he might
        /// consider his privious choice and the previous choice of his opponent.
        /// </summary>
        /// <param name="nrOfInvocations">A play goes over at least 100 repetitions. This parameter indicates the repetition</param>
        /// <param name="ownSelection">Selection of the previous request of this player</param>
        /// <param name="adversarialSelection">Selection of the previous request of the opponent</param>
        /// <returns></returns>
        [OperationContract]
        ESchereSteinPapier SchereSteinPapier(int nrOfInvocations, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection );

        /// <summary>
        /// Close the player. This is called by the arbiter before shutting down.
        /// </summary>
        [OperationContract]
        void Close();
    }
}
