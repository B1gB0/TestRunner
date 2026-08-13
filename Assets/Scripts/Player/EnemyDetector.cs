using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class EnemyDetector : MonoBehaviour
    {
        private const int MinValue = 0;

        private readonly HashSet<Enemy.Obstacle> _enemiesInRange = new ();

        private float _closestEnemyDistanceSqr;

        public float ClosestEnemyDistance => Mathf.Sqrt(_closestEnemyDistanceSqr);

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!otherCollider.TryGetComponent(out Enemy.Obstacle enemy))
                return;

            _enemiesInRange.Add(enemy);

            enemy.Die += OnEnemyDie;
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            if (!otherCollider.TryGetComponent(out Enemy.Obstacle enemy))
                return;

            _enemiesInRange.Remove(enemy);

            enemy.Die -= OnEnemyDie;
        }

        private void OnDestroy()
        {
            foreach (var enemy in _enemiesInRange.Where(enemyAlienActor => enemyAlienActor != null))
            {
                enemy.Die -= OnEnemyDie;
            }

            _enemiesInRange.Clear();
        }

        public Enemy.Obstacle GetClosestEnemy()
        {
            if (_enemiesInRange.Count <= MinValue)
                return null;

            Enemy.Obstacle bestTarget = null;
            _closestEnemyDistanceSqr = Mathf.Infinity;
            var currentPosition = transform.position;

            foreach (Enemy.Obstacle closestEnemy in _enemiesInRange)
            {
                var directionToTarget = closestEnemy.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;

                if (!(dSqrToTarget < _closestEnemyDistanceSqr))
                    continue;

                _closestEnemyDistanceSqr = dSqrToTarget;
                bestTarget = closestEnemy;
            }

            return bestTarget;
        }

        public HashSet<Enemy.Obstacle> GetEnemiesInRange()
        {
            return _enemiesInRange;
        }

        private void OnEnemyDie(Enemy.Obstacle obstacle)
        {
            _enemiesInRange.Remove(obstacle);
            obstacle.Die -= OnEnemyDie;
        }
    }
}