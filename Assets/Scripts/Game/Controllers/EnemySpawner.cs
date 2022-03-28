using Game.Weapon;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Controllers
{
    public class EnemySpawner : ITickable
    {
        [Inject] private EnemyController.Factory enemyFactory;
        [Inject] private FP_Character _character;

        [Inject] private ColorCube.Factory cubeFactory;
        private bool isSpawned = true;

        private void SpawnEnemy()
        {
            var testPoint = new Vector3(
                Random.Range(-50f, 50f),
                0,
                Random.Range(-50f, 50f)
            );

            if (!CanSee(testPoint, 60))
            {
                NavMeshHit myNavHit;
                if (NavMesh.SamplePosition(testPoint, out myNavHit, 100, -1))
                {
                    if (!CanSee(myNavHit.position, 60))
                    {
                        var enemy = enemyFactory.Create(myNavHit.position);
                        _lastSpawnTime = Time.realtimeSinceStartupAsDouble;
                        isSpawned = true;
                    }
                }
            }
        }

        bool CanSee(Vector3 point, float angleInDegrees)
        {
            var playerPosition =  _character.transform.position;
            var direction = (point - playerPosition).normalized;
            var playerLook = _character.transform.forward;
            var dot = Vector3.Dot(direction, playerLook );
            float cos = dot;
            float angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
            return angle < angleInDegrees;
        }

        public void SpawnCube(Vector3 transformPosition)
        {
            cubeFactory.Create(transformPosition, Quaternion.identity);
        }

        private double _lastSpawnTime;

        public EnemySpawner()
        {
            _lastSpawnTime = Time.realtimeSinceStartupAsDouble;
        }

        public void Tick()
        {
            if (Time.realtimeSinceStartupAsDouble - _lastSpawnTime >= 3 && isSpawned)
            {
                isSpawned = false;
            }

            if (!isSpawned)
            {
                SpawnEnemy();
            }
        }
    }
}