using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHurtSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var shared = systems.GetShared<SharedData>();

        var bullets = world.Filter<BulletComponent>().End();
        var bulletsPool = world.GetPool<BulletComponent>();
        var deathPool = world.GetPool<DestroyingComponent>();

        foreach (var entity in bullets)
        {
            ref var bullet = ref bulletsPool.Get(entity);
            bullet.LifeTime -= shared.DeltaTime;
            
            if (bullet.LifeTime <= 0)
            {
                deathPool.Add(entity);
            }
        }

        shared.Logic.DespawnDestroyingObjects();
    }
}
