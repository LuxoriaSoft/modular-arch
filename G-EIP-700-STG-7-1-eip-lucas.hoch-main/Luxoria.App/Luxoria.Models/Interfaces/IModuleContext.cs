namespace Luxoria.Core
{
    public interface IModuleContext
    {
        ImageData GetCurrentImage();
        void UpdateImage(ImageData image);
        void LogMessage(string message);
    }
}
