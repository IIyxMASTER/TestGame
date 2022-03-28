using Game.Controllers;
using Game.Weapon;
using UnityEngine;
using Zenject;

namespace Game
{
    public class LegacyUI : MonoBehaviour
    {
        private GunController _gunController;
        private FP_Health_Controller _healthController;

        [Inject]
        public void Construct(GunController gunController, FP_Health_Controller healthController)
        {
            _gunController = gunController;
            _healthController = healthController;
        }

        void OnGUI()
        {
            GUILayout.Label($"AMMO: {_gunController.AmmoCount}");
            GUILayout.Label($"Health: {_healthController.CurrentHealth}/{_healthController.MaxHealth} ");
        }
    }
}