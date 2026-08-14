using System;

namespace Services
{
    public interface ICurrencyService : IService
    {
        public event Action<int> OnMoneyValueChanged;

        public int Money { get; }
        public int AccumulatedMoney { get; }

        public void AddMoney(int gold);
        public void SpendMoney(int gold);
        public void ResetAccumulatedMoney();
        public void SaveMoney();
        public void SetAccumulatedMoney(int accumulatedMoney);
    }
}