namespace Strangeman.Utils.Bootstrap
{
    public interface IInitializable
    {
        void Initialize();
    }
    
    public interface IInitializeWith<T>
    {
        void InitializeWith(T arg0);
    }
}
