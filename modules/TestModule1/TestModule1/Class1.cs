using Luxoria.Core;
using System.Diagnostics;

namespace TestModule1
{
    public class Class1 : IModule
    {
        public string Name => "TestModule1";

        public string Description => "Basic module for testing purposes";
        public void Initialize(IModuleContext context)
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
