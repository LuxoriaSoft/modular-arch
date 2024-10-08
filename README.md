# Luxoria Modular Architecture

## Overview

The **Luxoria** application is designed with a modular architecture, allowing for flexibility and extensibility through separate modules. This architecture separates core functionality and modules into distinct libraries, enabling independent development and deployment.

### Project Structure

The Luxoria solution consists of the following main components:

- **Luxoria.App/**: The main application project containing the user interface and application logic.
- **Luxoria.Core/**: The core library that defines the fundamental functionality and services used throughout the application.
- **Luxoria.Modules/**: A library that manages and provides access to various modules, allowing for extensibility.

### Build Outputs

Upon building the solution, the following outputs are generated:

- **Luxoria.App.dll**: The compiled assembly for the main application.
- **Luxoria.Core.dll**: The compiled assembly for the core services and functionality.
- **Luxoria.Modules.dll**: The compiled assembly that handles module management and interactions.

### Executable

The main executable for the application is:

- **Luxoria.App.exe**

#### Linked DLLs

The executable is linked with the following dynamic-link libraries (DLLs):

- **Luxoria.App.dll**
- **Luxoria.Core.dll**
- **Luxoria.Modules.dll**

### Modules Directory

Modules are organized within a dedicated **modules/** directory. Each module must reference **Luxoria.Modules.dll** to ensure proper integration and functionality. 

#### Module Structure

- A module is a .NET Library project that contains a class that inherits from the **IModule** interface. This design allows for consistent integration and behavior across different modules.

**IModule**:
```csharp
public interface IModule
{
    string Name { get; }
    string Description { get; }
    string Version { get; }
    void Initialize(IEventBus eventBus, IModuleContext context);
    void Execute();
    void Shutdown();
}
```

**EventBus (IEventBus)**:  
The **EventBus** is an essential component that facilitates communication between different modules and the core application. It allows for the publishing and subscribing of events, enabling modules to react to changes and updates effectively.
```csharp
public interface IEventBus
{
    void Publish<TEvent>(TEvent @event);
    void Subscribe<TEvent>(Action<TEvent> handler);
    void Unsubscribe<TEvent>(Action<TEvent> handler);
}
```

**ModuleContext (IModuleContext)**:  
The **ModuleContext** provides modules with essential functionality and services needed to interact with the core application and manage application state. It serves as a communication channel between modules and the main application, ensuring seamless integration and responsiveness.

Key responsibilities of the **IModuleContext** include:

- **Image Management**: The context allows modules to retrieve and update the current image being processed. This functionality is critical for modules that handle image editing, filtering, or any operations requiring access to the image data.

- **Logging**: Modules can log messages for debugging or informational purposes, helping developers track the flow of execution and diagnose issues more effectively.

**IModuleContext**:
```csharp
public interface IModuleContext
{
    ImageData GetCurrentImage();
    void UpdateImage(ImageData image);
    void LogMessage(string message);
}
```

### Example Module: TextModule1

- **Location**: `modules/TextModule1/`
- **Output**: 
  - **TextModule1.dll**: The compiled assembly for the TextModule1 module, enabling specific features related to text processing.

### Summary

The modular architecture of Luxoria promotes a clean separation of concerns, allowing for easier maintenance, scalability, and the addition of new features through modules. This design philosophy ensures that the application can evolve and adapt to changing requirements while maintaining core functionality. 

By leveraging the **EventBus** and **ModuleContext**, modules can interact seamlessly with the core application and with each other, enhancing the overall flexibility and functionality of the Luxoria platform. This modular approach not only simplifies development but also fosters collaboration among developers, allowing them to build and integrate new features efficiently. 

As Luxoria continues to grow, this architecture will facilitate the addition of diverse modules that enhance the application's capabilities, providing users with a rich and customizable experience.
