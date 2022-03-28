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
        private ColorController _colorController;

        [Inject]
        public void Construct(GunController gunController, FP_Health_Controller healthController, ColorController colorController)
        {
            _gunController = gunController;
            _healthController = healthController;
            _colorController = colorController;
        }

        void OnGUI()
        {
            GUILayout.Label($"AMMO: {_gunController.AmmoCount}");
            GUILayout.Label($"Health: {_healthController.CurrentHealth}/{_healthController.MaxHealth} ");
            GUILayout.Label($"Red cubes: {_colorController.GetColorPickCount(ColorEnum.Red)} ");
            GUILayout.Label($"Yellow cubes: {_colorController.GetColorPickCount(ColorEnum.Yellow)} ");
            GUILayout.Label($"Green cubes: {_colorController.GetColorPickCount(ColorEnum.Green)} ");
        }
    }
}