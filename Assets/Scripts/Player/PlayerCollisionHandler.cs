using Enemy;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Core.Player))]
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private Core.Player _player;

        private void Awake()
        {
            _player = GetComponent<Core.Player>();
        }

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out Bottle bottle))
            {
                _player.Health.TakeDamage(bottle.Data.Money, true);
                bottle.gameObject.SetActive(false);
            }
            
            if (trigger.TryGetComponent(out Money money))
            {
                _player.Health.AddHealth(money.Data.Money);
                money.gameObject.SetActive(false);
            }
        }
    }
}