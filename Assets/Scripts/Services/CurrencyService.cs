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
        public event Action<int> OnKeysChanged;

        public int Money { get; private set; }
        public int Keys { get; private set; }
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

        public void SetKeys(int keys)
        {
            Keys = keys;
        }

        public void AddKeys(int keys)
        {
            Keys += keys;
            OnKeysChanged?.Invoke(Keys);
        }

        public void AddMoney(int gold)
        {
            Money += gold;
            AccumulatedMoney += gold;
            OnGoldValueChanged?.Invoke(Money);
        }

        public void SetAccumulatedMoney(int accumulatedMoney)
        {
            AccumulatedMoney = accumulatedMoney;
        }

        public void SpendMoney(int gold)
        {
            Money -= gold;
            OnGoldValueChanged?.Invoke(Money);
        }

        public void SpendKeys(int keys)
        {
            Keys -= keys;
            OnKeysChanged?.Invoke(Keys);
        }
        
        public void ResetAccumulatedMoney()
        {
            AccumulatedMoney = MinValue;
        }

        public void SaveMoney()
        {
            YG2.saves.Money = Money;
            YG2.SaveProgress();
        }
    }
}