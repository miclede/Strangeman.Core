using System;

namespace Strangeman.Utils.Evaluation
{
    public interface IEvaluate
    {
        event Action OnSuccess;
        event Action OnFailure;
        void Evaluate();
    }
}