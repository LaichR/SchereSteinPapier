using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchereSteinPapierInterface;

namespace SchereSteinPapierPlayer
{
    class FeedForwardNetwork
    {
        ESchereSteinPapier[] _lastTwoAdversarialSelections = new[] { ESchereSteinPapier.Papier, ESchereSteinPapier.Papier };
        
        public FeedForwardNetwork(){}

        public ESchereSteinPapier SchereSteinPapier(int nrOfInvocations, ESchereSteinPapier ownSelection, ESchereSteinPapier adversarialSelection)
        {
            return (ESchereSteinPapier)0;
        }

        static void Reset<T>(T[] array)
        {
            for( int i = 0; i < array.Length; i++)
            {
                array[i] = default(T);
            }
        }
        static void ResetWeights(double[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for(int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = 0;
                }
            }
        }
        
    }
}
