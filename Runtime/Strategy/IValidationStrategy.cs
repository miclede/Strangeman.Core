namespace Strangeman.Utils.Strategy
{
    public interface IValidationStrategy<S, R>
    {
        bool Validate(S source, R rules);
    }
}
