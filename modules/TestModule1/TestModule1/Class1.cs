using Luxoria.Modules.Interfaces;
using System.Diagnostics;

namespace TestModule1
{
    public class Class1 : IModule
    {
        public string Name => "TestModule1";
        public string Description => "Basic module for testing purposes";
        public string Version => "1.0.0";
        public void Initialize(IEventBus eventBus, IModuleContext context)
        {
            Debug.WriteLine("TestModule1 initialized");
        }
        public void Execute()
        {
            Debug.WriteLine("TestModule1 executed");
        }
        public void Shutdown()
        {
            Debug.WriteLine("TestModule1 shutdown");
        }
    }
}
