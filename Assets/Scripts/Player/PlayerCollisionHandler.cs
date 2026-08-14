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
                _player.Health.TakeDamage(bottle.Data.Money);
                bottle.gameObject.SetActive(false);
            }
            
            if (trigger.TryGetComponent(out Money money))
            {
                _player.Health.AddHealth(money.Data.Money);
                money.gameObject.SetActive(false);
            }
        }
        
        private void OnTriggerExit(Collider trigger)
        {
            // if (trigger.TryGetComponent(out EntranceTrigger entranceTrigger))
            // {
            //     entranceTrigger.Entrance.CloseGate();
            // }
        }
        //
        // private void OnCollisionEnter(Collision collision)
        // {
        //     if (collision.gameObject.TryGetComponent(out RedCrystal healingCrystal))
        //     {
        //         if (_playerService.PlayerActor.Health.TargetHealth == _playerService.PlayerActor.Health.MaxHealth)
        //             return;
        //
        //         _playerService.PlayerActor.Health.AddHealth(healingCrystal.HealthValue);
        //         healingCrystal.Destroy();
        //     }
        //     else if (collision.gameObject.TryGetComponent(out GoldCrystal goldCrystal))
        //     {
        //         goldCrystal.Destroy();
        //     }
        // }
    }
}