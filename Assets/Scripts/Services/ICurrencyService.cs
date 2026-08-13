using System;

namespace Services
{
    public interface ICurrencyService : IService
    {
        public event Action<int> OnGoldValueChanged;

        public int Money { get; }
        public int AccumulatedMoney { get; }

        public void AddGold(int gold);
        public void SpendGold(int gold);
        public void ResetAccumulatedGold();
        public void SaveGold();
    }
}