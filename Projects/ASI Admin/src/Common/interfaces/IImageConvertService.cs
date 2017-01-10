namespace asi.asicentral.interfaces
{
    public interface IImageConvertService
    {
        bool ConvertImage(string inputFilePath, string targetFilePath, bool isBackgroundTransparent);
    }
}
