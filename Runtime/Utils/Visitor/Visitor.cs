namespace Strangeman.Utils.Visitor
{
    /// <summary>
    /// Defines a visitor that can visit objects of type <typeparamref name="T"/> implementing <see cref="IVisitable"/>.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Visits an object of type <typeparamref name="T"/> that implements <see cref="IVisitable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the visitable object, which must implement <see cref="IVisitable"/>.</typeparam>
        /// <param name="visitable">The visitable object to be visited.</param>
        void Visit<T>(T visitable) where T : IVisitable;
    }

    /// <summary>
    /// Defines an object that can accept a visitor.
    /// </summary>
    public interface IVisitable
    {
        /// <summary>
        /// Accepts a visitor and allows it to visit the object.
        /// </summary>
        /// <param name="visitor">The visitor that will visit this object.</param>
        void Accept(IVisitor visitor);
    }
}
