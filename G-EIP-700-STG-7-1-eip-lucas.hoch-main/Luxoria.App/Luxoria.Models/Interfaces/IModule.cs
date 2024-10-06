namespace Luxoria.Core
{
    public interface IModule
    {
        string Name { get; }
        string Description { get; }
        void Initialize(IModuleContext context);
        void Execute();
        void Shutdown();
    }
}
