namespace asi.asicentral.interfaces
{
    public interface IImageConvertService
    {
        void ConvertImage(string inputFilePath, string targetFilePath, bool isBackgroundTransparent);
    }
}
