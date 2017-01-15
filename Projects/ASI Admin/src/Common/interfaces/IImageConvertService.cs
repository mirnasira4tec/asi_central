using System.Threading.Tasks;
namespace asi.asicentral.interfaces
{
    public interface IImageConvertService
    {
        Task ConvertImageAsync(string inputFilePath, string targetFilePath, bool isBackgroundTransparent);
    }
}
