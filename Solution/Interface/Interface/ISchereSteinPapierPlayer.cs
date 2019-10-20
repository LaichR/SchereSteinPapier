using System;

using System.ServiceModel;

namespace SchereSteinPapierInterface
{
    [ServiceContract]
    public interface ISchereSteinPapierPlayer
    {
        [OperationContract]
        void Restart();

        [OperationContract]
        string Ping();

        [OperationContract]
        ESchereSteinPapier SchereSteinPapier(int nrOfInvocations, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection );

        [OperationContract]
        void Close();
    }
}
