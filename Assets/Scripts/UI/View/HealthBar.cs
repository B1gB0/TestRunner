namespace UI.View
{
    public class HealthBar : Bar
    {
        private Health _health;

        private void OnDestroy()
        {
            _health.Die -= OnDie;
            _health.HealthChanged -= OnChangedValues;
        }

        public void Construct(Health health)
        {
            _health = health;
            _health.Die += OnDie;
            _health.HealthChanged += OnChangedValues;
        }

        private void OnDie()
        {
            Hide();
        }

        public override void Show()
        {
            base.Show();
            OnChangedValues(_health.CurrentHealth, _health.MaxHealth, _health.TargetHealth);
        }

        private void OnChangedValues(float currentHealth, float maxHealth, float targetHealth)
        {
            SetValues(currentHealth, maxHealth, targetHealth);
        }
    }
}