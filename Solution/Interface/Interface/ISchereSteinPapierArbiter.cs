using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchereSteinPapierInterface
{
    [ServiceContract]
    public interface ISchereSteinPapierArbiter
    {
        [OperationContract]
        bool RegisterPlayer(string name, string connectionString );

        [OperationContract]
        ResultSummary Play(string player1, string player2, int nrOfRepetitions);
    }
}
