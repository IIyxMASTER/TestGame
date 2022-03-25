using System.Collections;
using UnityEngine;

namespace Game.Weapon
{
    public class GunController : MonoBehaviour
    {
        public FP_Input playerInput;

        public float shootRate = 0.15F;
        public float reloadTime = 1.0F;
        public int ammoCount = 15;

        private int ammo;
        private float delay;
        private bool reloading;

        void Start()
        {
            ammo = ammoCount;
        }

        private bool shoot;
        private bool reload;
        public KeyCode shootKey = KeyCode.Mouse0;
        public KeyCode reloadKey = KeyCode.R;

        void Update()
        {
            switch (playerInput.UseMobileInput)
            {
                case true:
                    reload = playerInput.Reload();
                    shoot = playerInput.Shoot();
                    break;
                case false:
                    reload = Input.GetKey(reloadKey);
                    shoot = Input.GetKey(shootKey);
                    break;
            }

            if (shoot)
                if (Time.time > delay)
                    Shoot();

            if (reload)
                if (!reloading && ammoCount < ammo)
                    StartCoroutine("Reload");
        }

        void Shoot()
        {
            if (ammoCount > 0)
            {
                Debug.Log("Shoot");
                ammoCount--;
            }
            else
                Debug.Log("Empty");

            delay = Time.time + shootRate;
        }

        IEnumerator Reload()
        {
            reloading = true;
            Debug.Log("Reloading");
            yield return new WaitForSeconds(reloadTime);
            ammoCount = ammo;
            Debug.Log("Reloading Complete");
            reloading = false;
        }

        void OnGUI()
        {
            GUILayout.Label("AMMO: " + ammoCount);
        }
    }
}