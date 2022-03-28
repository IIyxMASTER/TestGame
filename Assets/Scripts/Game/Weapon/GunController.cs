using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using static Lean.Pool.LeanPool;

namespace Game.Weapon
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Animator gunAnimator;
        [SerializeField] private  FP_Input playerInput;
        [SerializeField] private Transform spawnPoint;
        [Inject]private Bullet.Factory bulletFactory;
        [SerializeField] private float _bulletLifeTime = 5f;
        [SerializeField] private float _bulletPower;
        [SerializeField] private float _rotationStep;
        [SerializeField] private  GameObject target;
        [SerializeField] private  GameObject muzzlePrefab;
        [SerializeField] private  GameObject muzzlePosition;

        [SerializeField] private  AudioClip GunShotClip;
        [FormerlySerializedAs("source")] [SerializeField] private  AudioSource shootSource;
        [FormerlySerializedAs("source")] [SerializeField] private  AudioSource reloadSource;
        [SerializeField] private  Vector2 audioPitch = new Vector2(.9f, 1.1f);

        [SerializeField] private  float shootRate = 0.15F;
        [SerializeField] private  float reloadTime = 1.0F;
        [SerializeField] private  int ammoCount = 15;
        public int AmmoCount => ammoCount;
        
        private bool shoot;
        private bool reload;
        [SerializeField] private  KeyCode shootKey = KeyCode.Mouse0;
        [SerializeField] private  KeyCode reloadKey = KeyCode.R;

        private static readonly int ReloadProperty = Animator.StringToHash("Reload");
        private static readonly int ShotProperty = Animator.StringToHash("Shot");
        [SerializeField] private  GameObject projectilePrefab;
        
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
            if(Physics.Linecast(transform.parent.position,target.transform.position, out RaycastHit hitInfo))
            {
                var distance = Vector3.Distance(hitInfo.point, transform.position);
                var minDistance = 4;
                
                LookAt(distance > minDistance ? hitInfo.point : target.transform.position);
            }
            
        }

        void LookAt(Vector3 targetPos)
        {
            Vector3 relativePos = targetPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationStep * Time.deltaTime);
            
        }

        void Shoot()
        {
            if (ammoCount > 0)
            {
                gunAnimator.SetTrigger(ShotProperty);
                var flash = Spawn(muzzlePrefab, muzzlePosition.transform);

                if (projectilePrefab != null)
                {
                    var bullet = bulletFactory.Create(spawnPoint.position,
                        muzzlePosition.transform.rotation, transform);
                    bullet.Shot(Vector3.right * _bulletPower, _bulletLifeTime);
                }

                if (shootSource != null)
                {
                    if (shootSource.transform.IsChildOf(transform))
                    {
                        shootSource.Play();
                    }
                    else
                    {
                        AudioSource newAS = Spawn(shootSource);
                        if ((newAS = Spawn(shootSource)) != null && newAS.outputAudioMixerGroup != null &&
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
            gunAnimator.SetTrigger(ReloadProperty);
            if (reloadSource != null)
            {
                if (reloadSource.transform.IsChildOf(transform))
                {
                    reloadSource.Play();
                }
            }

            yield return new WaitForSeconds(reloadTime);
            ammoCount = ammo;
            reloading = false;
        }

        
    }
}