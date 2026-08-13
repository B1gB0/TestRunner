using Cysharp.Threading.Tasks;

namespace Services
{
    public interface IResourceService
    {
        UniTask<T> Load<T>(string assetName);
    }
}