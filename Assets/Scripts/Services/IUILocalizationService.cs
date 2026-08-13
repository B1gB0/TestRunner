using DataBase.Data;
using UI;

namespace Services
{
    public interface IUILocalizationService : IService
    {
        public UILocalizationData GetLevelTextData(UITextType type);
    }
}