using Cysharp.Threading.Tasks;

namespace Services
{
    public interface IService
    {
        public bool IsInitiated { get; }

        public UniTask Init()
        {
            return UniTask.CompletedTask;
        }
    }
}