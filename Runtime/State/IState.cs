namespace Strangeman.Utils.State
{
    public interface IState
    {
        void OnEnter();
        void PreTick();
        void Tick();
        void PostTick();
        void OnExit();
    }
}