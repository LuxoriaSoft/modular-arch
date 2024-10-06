namespace Luxoria.Core
{
    public interface IModule
    {
        string Name { get; }
        string Description { get; }
        void Initialize();
        void Execute();
        void Shutdown();
    }
}
