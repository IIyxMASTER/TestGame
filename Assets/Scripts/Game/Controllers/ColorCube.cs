using System;
using UnityEngine;
using Zenject;

namespace Game.Controllers
{
    public class ColorCube : MonoBehaviour, IPoolable<Vector3, Quaternion, IMemoryPool>, IDisposable
    {
        [SerializeField] private MeshRenderer _renderer;
        [Inject] private ColorController _colorController;
        private IMemoryPool _pool;
        private ColorEnum color;

        public void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<FP_Character>();
            if (player != null)
            {
                _colorController.PickColor(color);
                Dispose();
            }
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(Vector3 position, Quaternion rotation, IMemoryPool pool)
        {
            transform.position = position;
            transform.rotation = rotation;
            _pool = pool;
            color = _colorController.GetRandomColor();
            _renderer.material.color = _colorController.GetMaterialColor(color);
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector3, Quaternion, ColorCube>
        {
        }
    }
}