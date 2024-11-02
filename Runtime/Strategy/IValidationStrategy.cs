namespace Strangeman.Utils.Strategy
{
    /// <summary>
    /// Defines a strategy for validating a source object against a set of rules.
    /// </summary>
    /// <typeparam name="S">The type of the source object to validate.</typeparam>
    /// <typeparam name="R">The type of the rules used for validation.</typeparam>
    public interface IValidationStrategy<S, R>
    {
        /// <summary>
        /// Validates the source object against the provided rules.
        /// </summary>
        /// <param name="source">The source object to validate.</param>
        /// <param name="rules">The rules to apply for validation.</param>
        /// <returns>True if the source object meets the validation rules; otherwise, false.</returns>
        bool Validate(S source, R rules);
    }
}
