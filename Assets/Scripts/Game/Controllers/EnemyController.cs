using System;
using System.Collections;
using Game.Weapon;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Controllers
{
    public class EnemyController : MonoBehaviour, IPoolable<Vector3,  IMemoryPool>, IDisposable
    {
        [SerializeField] private int _currentHealth;
        [SerializeField] private float _speed;
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _attackDelay = 0.7f;
        [SerializeField] private int _attackDamage;
        [SerializeField] private EnemySpawner _spawner;
        private IMemoryPool _pool;
        
        [SerializeField] private FP_Character _target;
        [SerializeField] private FP_Health_Controller _targetHealthController;
        private IEnumerator _attackCoroutine;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public void DamageEnemy()
        {
            _currentHealth -= 1;
            if (_currentHealth <= 0)
                Death();
        }

        void Death()
        {
            _spawner.SpawnCube(gameObject.transform.position + Vector3.up);
            Destroy(gameObject);
        }

        IEnumerator DamagePlayerAnimation()
        {
            _animator.SetFloat(Speed, 0);
            _animator.SetTrigger(AttackTrigger);
            yield return new WaitForSeconds(_attackDelay);
            if (_currentHealth > 0)
            {
                _targetHealthController.Damage(_attackDamage);
            }

            _attackCoroutine = null;
        }

        void Update()
        {
            if (_target != null)
            {
                if (_agent.destination != _target.transform.position &&
                    Vector3.Distance(transform.position, _target.transform.position) > _attackRadius)
                {
                    _agent.SetDestination(_target.transform.position);
                    _animator.SetFloat(Speed, _speed);
                }

                if (_attackCoroutine == null &&
                    Vector3.Distance(transform.position, _target.transform.position) <= _attackRadius)
                {
                    _attackCoroutine = DamagePlayerAnimation();
                    StartCoroutine(_attackCoroutine);
                }
            }
        }

        [Inject]
        public void Construct(FP_Character target, FP_Health_Controller healthController, EnemySpawner spawner)
        {
            _attackCoroutine = null;
            _target = target;
            _targetHealthController = healthController;
            _spawner = spawner;
        }
        public class Factory : PlaceholderFactory<Vector3, EnemyController>
        {
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(Vector3 position, IMemoryPool pool)
        {
            transform.position = position;
            _pool = pool;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }
    }
}