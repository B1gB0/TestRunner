using DG.Tweening;

namespace UI.View
{
    public class JoystickView : View
    {
        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}