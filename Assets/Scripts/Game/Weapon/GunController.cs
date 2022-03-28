using System.Collections;
using UnityEngine;
using static Lean.Pool.LeanPool;

namespace Game.Weapon
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Animator gunAnimator;
        public FP_Input playerInput;

        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;

        public AudioClip GunShotClip;
        public AudioSource source;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        public float shootRate = 0.15F;
        public float reloadTime = 1.0F;
        public int ammoCount = 15;

        private bool shoot;
        private bool reload;
        public KeyCode shootKey = KeyCode.Mouse0;
        public KeyCode reloadKey = KeyCode.R;

        private static readonly int ReloadProperty = Animator.StringToHash("Reload");
        private static readonly int ShotProperty = Animator.StringToHash("Shot");
        public GameObject projectilePrefab;
        
        private int ammo;
        private float delay;
        private bool reloading;

        void Start()
        {
            ammo = ammoCount;
        }


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
                gunAnimator.SetTrigger(ShotProperty);
                var flash = Instantiate(muzzlePrefab, muzzlePosition.transform);

                // --- Shoot Projectile Object ---
                if (projectilePrefab != null)
                {
                    GameObject newProjectile = Spawn(projectilePrefab, muzzlePosition.transform.position,
                        muzzlePosition.transform.rotation, transform);
                }

                if (source != null)
                {
                    if (source.transform.IsChildOf(transform))
                    {
                        source.Play();
                    }
                    else
                    {
                        AudioSource newAS = Spawn(source);
                        if ((newAS = Spawn(source)) != null && newAS.outputAudioMixerGroup != null &&
                            newAS.outputAudioMixerGroup.audioMixer != null)
                        {
                            newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch",
                                Random.Range(audioPitch.x, audioPitch.y));
                            newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);
                            newAS.PlayOneShot(GunShotClip);
                            Despawn(newAS.gameObject, 4);
                        }
                    }
                }

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
            gunAnimator.SetTrigger(ReloadProperty);
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