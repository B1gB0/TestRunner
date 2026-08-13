using System;
using Cysharp.Threading.Tasks;
using YG;

// using YG;

namespace Services
{
    public class CurrencyService : ICurrencyService
    {
        private const int MinValue = 0;

        public event Action<int> OnGoldValueChanged;

        public int Money { get; private set; }
        public int AccumulatedMoney { get; private set; }
        public bool IsInitiated { get; private set; }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            Money = YG2.saves.Money;
            OnGoldValueChanged?.Invoke(Money);

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public void SetMoney(int gold)
        {
            Money = gold;
            OnGoldValueChanged?.Invoke(Money);
        }

        public void AddGold(int gold)
        {
            Money += gold;
            AccumulatedMoney += gold;
            OnGoldValueChanged?.Invoke(Money);
        }

        public void SpendGold(int gold)
        {
            Money -= gold;
            OnGoldValueChanged?.Invoke(Money);
        }
        
        public void ResetAccumulatedGold()
        {
            AccumulatedMoney = MinValue;
        }

        public void SaveGold()
        {
            YG2.saves.Money = Money;
            YG2.SaveProgress();
        }
    }
}