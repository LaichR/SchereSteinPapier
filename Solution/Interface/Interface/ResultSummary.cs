using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SchereSteinPapierInterface
{
    [DataContract]
    public class ResultSummary
    {
        [DataMember]
        public EResultStatus Status
        {
            get;
            set;
        }

        [DataMember]
        public string Winner
        {
            get;
            set;
        }

        [DataMember] 
        public int NrOfGamesWonByPlayer1
        {
            get;
            set;
        }

        [DataMember]
        public int NrOfGamesWonByPlayer2
        {
            get;
            set;
        }

        [DataMember]
        public int TotalNumberOfGames
        {
            get;
            set;
        }

        [DataMember]
        public double WinnerSuccessRatio
        {
            get;
            set;
        }

    }
}
