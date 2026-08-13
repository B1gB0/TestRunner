using UI.View;
using UnityEngine;

namespace Services
{
    public interface IFloatingTextService
    {
        public void OnSpawnFloatingText(
            string value,
            Transform target,
            FloatingTextViewType floatingTextViewType,
            Color color);

        public void Init(FloatingTextView textView);
    }
}