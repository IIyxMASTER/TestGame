using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Game.Weapon
{
    public class Bullet : MonoBehaviour, IPoolable<Vector3, Quaternion,Transform, IMemoryPool>, IDisposable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _bulletRenderer;
        [Inject] private ColorController _colorController;
        private IMemoryPool _pool;
        private IEnumerator destroyCoroutine;
        public void Shot(Vector3 velocity, float lifeTime)
        {
            var direction = transform.TransformDirection(velocity);
            transform.SetParent(null);
            _bulletRenderer.material.color = _colorController.Color;
            _rigidbody.AddForce(direction, ForceMode.Impulse);
            destroyCoroutine = DestroyTimer(lifeTime);
            StartCoroutine(destroyCoroutine);
        }

        IEnumerator DestroyTimer(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            Dispose();
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(Vector3 position, Quaternion rotation,Transform parent, IMemoryPool pool)
        {
            _pool = pool;
            transform.SetParent(parent);
            transform.position = position;
            transform.rotation = rotation;
        }

        public void Dispose()
        {
            StopCoroutine(destroyCoroutine);
            destroyCoroutine = null;
            _rigidbody.velocity = Vector3.zero;
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector3, Quaternion,Transform, Bullet>
        {
        }
    }
}