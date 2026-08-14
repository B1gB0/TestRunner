using DG.Tweening;
using Reflex.Attributes;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public abstract class Bar : View
    {
        [SerializeField] protected Slider SmoothSlider;
        [SerializeField] protected Slider Slider;

        [SerializeField] protected TMP_Text TextValue;
        [SerializeField] protected TMP_Text NameText;

        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;
        [SerializeField] private Transform _weaponPanelPoint;

        private ITweenAnimationService _tweenAnimationService;

        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public override void Show()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint);
        }

        public override void Hide()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint, true);
        }

        public void GetPoints(Transform showPoint, Transform hidePoint)
        {
            _showPoint = showPoint;
            _hidePoint = hidePoint;
        }

        public void ChangePlayerCategory(Color color, string name)
        {
            Slider.image.color = color;
            NameText.color = color;
            NameText.text = name;
        }

        protected void SetValues(float currentValue, float maxValue, float targetValue)
        {
            SmoothSlider.value = currentValue / maxValue;

            Slider.value = targetValue / maxValue;

            TextValue.text = (int)targetValue + "/" + (int)maxValue;
        }
    }
}