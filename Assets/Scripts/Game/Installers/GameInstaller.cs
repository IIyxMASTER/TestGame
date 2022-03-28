using Game.Controllers;
using Game.Weapon;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject BulletPrefab;
    public override void InstallBindings()
    {
        Container.Bind<ColorController>().AsSingle();
        Container.Bind<FP_Health_Controller>().AsSingle();
        Container.BindFactory<Vector3, Quaternion, Transform, Bullet, Bullet.Factory>()
            .FromMonoPoolableMemoryPool(
                x => x.WithInitialSize(10).FromComponentInNewPrefab(BulletPrefab).UnderTransformGroup("BulletPool"));
    }
}