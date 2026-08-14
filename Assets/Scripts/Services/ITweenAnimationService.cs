using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Services
{
    public interface ITweenAnimationService : IService
    {
        public Sequence AnimateRotation(Transform target, float rotationDuration = 2f, float wobbleDuration = 1f);
        public UniTask AnimateScaleAsync(Transform target, bool isDisableTarget = false);
        public void AnimateScale(Transform target, bool isDisableTarget = false);

        public void AnimateMove(
            Transform target,
            Transform showPoint,
            Transform hidePoint,
            bool isDisableTarget = false,
            bool isSetParentToPoint = false);

        public void AnimatePointer(Transform target, Transform pointerPoint);
    }
}