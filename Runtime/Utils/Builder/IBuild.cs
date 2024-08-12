namespace Strangeman.Utils.Builder
{
    public interface IBuild<B>
    {
        IBuild<B> With<T>(string memberName, T value);
        T Get<T>(string memberName);
        B Build();
    }
}