using Game.Controllers;
using Game.Weapon;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject CubePrefab;
    [SerializeField] private GameObject EnemyPrefab;

    public override void InstallBindings()
    {
        Container.Bind<ColorController>().AsSingle();
        Container.Bind(typeof(EnemySpawner), typeof(ITickable) ).To<EnemySpawner>().AsSingle();
        Container.Bind<FP_Health_Controller>().AsSingle();
        Container.BindFactory<Vector3, Quaternion, Transform, Bullet, Bullet.Factory>()
            .FromMonoPoolableMemoryPool(
                x => x.WithInitialSize(10).FromComponentInNewPrefab(BulletPrefab).UnderTransformGroup("BulletPool"));
        Container.BindFactory<Vector3, Quaternion, ColorCube, ColorCube.Factory>()
            .FromMonoPoolableMemoryPool(
                x => x.WithInitialSize(10).FromComponentInNewPrefab(CubePrefab).UnderTransformGroup("CubePool"));
        Container.BindFactory<Vector3, EnemyController, EnemyController.Factory>().FromMonoPoolableMemoryPool(
            x => x.WithInitialSize(10).FromComponentInNewPrefab(EnemyPrefab).UnderTransformGroup("EnemyControllerPool"));
    }
}