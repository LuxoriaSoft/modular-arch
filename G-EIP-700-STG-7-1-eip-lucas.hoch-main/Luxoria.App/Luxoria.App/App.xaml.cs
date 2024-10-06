using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Luxoria.Core;
using Luxoria.Modules;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Luxoria.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly Startup _startup;
        private readonly IHost _host;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            _startup = new Startup();
            _host = CreateHostBuilder(_startup).Build();
        }

        public static IHostBuilder CreateHostBuilder(Startup startup)
        {
            return Host.CreateDefaultBuilder().ConfigureServices((context, services) => startup.ConfigureServices(context, services));
        }

        private void Log(string message)
        {
            Debug.WriteLine(message);
            // You can also log to a file or any other logging mechanism
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Log("Application is starting...");

            // Show splash screen
            var splashScreen = new SplashScreen();
            splashScreen.Activate();

            Log("Modules loaded. Closing splash screen...");
            await Task.Delay(2000);

            // Load modules asynchronously and update the splash screen with the module names
            await LoadModulesAsync(splashScreen);

            // Close the splash screen after loading modules
            splashScreen.DispatcherQueue.TryEnqueue(() =>
            {
                splashScreen.Close();
            });

            m_window = new MainWindow();
            m_window.Activate();
        }

        private async Task LoadModulesAsync(SplashScreen splashScreen)
        {
            using (var scope = _host.Services.CreateScope())
            {
                string modulesPath = Path.Combine(AppContext.BaseDirectory, "modules");

                // Check if the modules directory exists
                if (!Directory.Exists(modulesPath))
                {
                    Debug.WriteLine($"Modules directory not found: {modulesPath}");

                    // Create the modules directory if it doesn't exist
                    Directory.CreateDirectory(modulesPath);

                    Debug.WriteLine($"Modules directory created: {modulesPath}");
                }

                // Get all module DLL files in the modules directory
                string[] moduleFiles = Directory.GetFiles(modulesPath, "*.dll");

                var loader = new ModuleLoader();

                foreach (string moduleFile in moduleFiles)
                {
                    string moduleName = Path.GetFileNameWithoutExtension(moduleFile);

                    Debug.WriteLine("Trying to load : " + moduleName);

                    // Update the splash screen with the module name being loaded
                    splashScreen.DispatcherQueue.TryEnqueue(() =>
                    {
                        splashScreen.CurrentModuleTextBlock.Text = $"Loading {moduleName}...";
                    });

                    // Small delay to ensure the splash screen updates properly
                    await Task.Delay(1000); // 1 second delay

                    try
                    {
                        // Load the module in a background thread
                        await Task.Run(() =>
                        {
                            IModule module = loader.LoadModule(moduleFile);
                            if (module != null)
                            {
                                // Initialize the module
                                module.Initialize(scope.ServiceProvider.GetService<IModuleContext>());

                                Debug.WriteLine($"Module loaded: {moduleName}");
                            }
                            else
                            {
                                Debug.WriteLine($"No valid module found in: {moduleFile}");
                            }
                        });
                    }
                    catch (FileNotFoundException ex)
                    {
                        Debug.WriteLine($"File not found for module [{moduleFile}]: {ex.Message}");
                    }
                    catch (BadImageFormatException ex)
                    {
                        Debug.WriteLine($"Invalid module file format for [{moduleFile}]: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to load module [{moduleFile}]: {ex.Message}");
                    }
                }
            }
        }

        private Window m_window;
    }
}
